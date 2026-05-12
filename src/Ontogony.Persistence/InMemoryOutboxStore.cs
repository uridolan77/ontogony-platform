namespace Ontogony.Persistence;

/// <summary>
/// Thread-safe in-memory reference implementation of outbox write/read/dispatch and processed-message tracking.
/// Intended for tests and single-process validation; not a multi-node or durable queue.
/// </summary>
public sealed class InMemoryOutboxStore : IOutboxWriter, IOutboxReader, IOutboxDispatcher, IProcessedMessageStore
{
    private readonly InMemoryOutboxStoreOptions _options;
    private readonly IDeadLetterWriter? _deadLetterWriter;
    private readonly Ontogony.Primitives.IClock _clock;
    private readonly Dictionary<string, OutboxEntry> _outbox = new(StringComparer.Ordinal);
    private readonly Dictionary<string, ProcessedMessage> _processed = new(StringComparer.Ordinal);
    private readonly object _sync = new();

    public InMemoryOutboxStore(
        InMemoryOutboxStoreOptions? options = null,
        IDeadLetterWriter? deadLetterWriter = null,
        Ontogony.Primitives.IClock? clock = null)
    {
        _options = options ?? new InMemoryOutboxStoreOptions();
        _deadLetterWriter = deadLetterWriter;
        _clock = clock ?? new Ontogony.Primitives.SystemClock();
    }

    /// <inheritdoc />
    public Task WriteAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        OutboxContracts.Validate(message);

        lock (_sync)
        {
            if (_outbox.ContainsKey(message.MessageId))
            {
                throw new InvalidOperationException($"An outbox message with MessageId '{message.MessageId}' already exists.");
            }

            _outbox[message.MessageId] = new OutboxEntry { Message = message };
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<OutboxMessage>> ReadAvailableAsync(
        DateTimeOffset asOfUtc,
        int maxBatchSize,
        CancellationToken cancellationToken = default)
    {
        if (maxBatchSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxBatchSize));
        }

        lock (_sync)
        {
            var batch = _outbox.Values
                .Where(e => !e.Dispatched && !e.DeadLettered && e.Message.AvailableAt <= asOfUtc)
                .OrderBy(static e => e.Message.AvailableAt)
                .ThenBy(static e => e.Message.OccurredAt)
                .Take(maxBatchSize)
                .Select(static e => e.Message)
                .ToArray();

            return Task.FromResult<IReadOnlyList<OutboxMessage>>(batch);
        }
    }

    /// <inheritdoc />
    public Task MarkDispatchedAsync(string messageId, DateTimeOffset dispatchedAtUtc, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(messageId));
        }

        lock (_sync)
        {
            if (!_outbox.TryGetValue(messageId, out var entry))
            {
                return Task.CompletedTask;
            }

            if (entry.Dispatched || entry.DeadLettered)
            {
                return Task.CompletedTask;
            }

            entry.Dispatched = true;
            entry.DispatchedAtUtc = dispatchedAtUtc;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task MarkFailedAsync(
        string messageId,
        string lastError,
        DateTimeOffset nextAvailableAtUtc,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(messageId));
        }

        ArgumentNullException.ThrowIfNull(lastError);

        OutboxMessage? snapshotForDeadLetter = null;
        var deadLetterAt = _clock.UtcNow;

        lock (_sync)
        {
            if (!_outbox.TryGetValue(messageId, out var entry))
            {
                return;
            }

            if (entry.Dispatched || entry.DeadLettered)
            {
                return;
            }

            var incremented = entry.Message with
            {
                AttemptCount = entry.Message.AttemptCount + 1,
                LastError = lastError,
                AvailableAt = nextAvailableAtUtc
            };

            entry.Message = incremented;

            var threshold = _options.MoveToDeadLetterAfterAttempts;
            if (threshold is { } t && incremented.AttemptCount >= t && _deadLetterWriter is not null)
            {
                snapshotForDeadLetter = incremented;
                entry.DeadLettered = true;
            }
        }

        if (snapshotForDeadLetter is { } snap && _deadLetterWriter is { } writer)
        {
            var dl = new DeadLetterMessage(
                snap.MessageId,
                snap.EventId,
                snap.EventType,
                snap.Source,
                snap.TraceId,
                snap.OccurredAt,
                deadLetterAt,
                snap.AttemptCount,
                lastError,
                snap.PayloadJson,
                snap.PayloadHash,
                snap.MetadataJson);

            await writer.WriteAsync(dl, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public Task<bool> HasProcessedAsync(string consumerName, string messageId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(consumerName))
        {
            throw new ArgumentException("ConsumerName is required.", nameof(consumerName));
        }

        if (string.IsNullOrWhiteSpace(messageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(messageId));
        }

        var key = OutboxContracts.BuildProcessedMessageKey(consumerName, messageId);
        lock (_sync)
        {
            return Task.FromResult(_processed.ContainsKey(key));
        }
    }

    /// <inheritdoc />
    public Task MarkProcessedAsync(ProcessedMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        if (string.IsNullOrWhiteSpace(message.ConsumerName))
        {
            throw new ArgumentException("ConsumerName is required.", nameof(message));
        }

        if (string.IsNullOrWhiteSpace(message.MessageId))
        {
            throw new ArgumentException("MessageId is required.", nameof(message));
        }

        var key = OutboxContracts.BuildProcessedMessageKey(message.ConsumerName, message.MessageId);
        lock (_sync)
        {
            _processed.TryAdd(key, message);
        }

        return Task.CompletedTask;
    }

    /// <summary>Number of outbox rows (including dispatched and dead-lettered).</summary>
    public int OutboxRowCount
    {
        get
        {
            lock (_sync)
            {
                return _outbox.Count;
            }
        }
    }

    private sealed class OutboxEntry
    {
        public required OutboxMessage Message { get; set; }
        public bool Dispatched { get; set; }
        public DateTimeOffset? DispatchedAtUtc { get; set; }
        public bool DeadLettered { get; set; }
    }
}
