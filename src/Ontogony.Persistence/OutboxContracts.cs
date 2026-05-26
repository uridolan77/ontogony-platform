using System.Text.Json;

namespace Ontogony.Persistence;

/// <summary>Writes outbox messages for later dispatch.</summary>
public interface IOutboxWriter
{
    /// <summary>Appends an outbox message.</summary>
    Task WriteAsync(OutboxMessage message, CancellationToken cancellationToken = default);
}

/// <summary>Reads outbox messages available for dispatch.</summary>
public interface IOutboxReader
{
    /// <summary>Returns messages available at the given UTC instant, up to the batch size.</summary>
    Task<IReadOnlyList<OutboxMessage>> ReadAvailableAsync(
        DateTimeOffset asOfUtc,
        int maxBatchSize,
        CancellationToken cancellationToken = default);
}

/// <summary>Updates outbox dispatch state after handler attempts.</summary>
public interface IOutboxDispatcher
{
    /// <summary>Marks a message as successfully dispatched.</summary>
    Task MarkDispatchedAsync(string messageId, DateTimeOffset dispatchedAtUtc, CancellationToken cancellationToken = default);

    /// <summary>Marks a message as failed and schedules the next availability time.</summary>
    Task MarkFailedAsync(string messageId, string lastError, DateTimeOffset nextAvailableAtUtc, CancellationToken cancellationToken = default);
}

/// <summary>Tracks idempotent consumer processing of outbox messages.</summary>
public interface IProcessedMessageStore
{
    /// <summary>Returns whether the consumer has already processed the message.</summary>
    Task<bool> HasProcessedAsync(string consumerName, string messageId, CancellationToken cancellationToken = default);

    /// <summary>Records that the consumer processed the message.</summary>
    Task MarkProcessedAsync(ProcessedMessage message, CancellationToken cancellationToken = default);
}

/// <summary>Executes work inside a transactional boundary.</summary>
public interface IUnitOfWorkBoundary
{
    /// <summary>Executes an operation inside a unit of work.</summary>
    Task ExecuteAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default);

    /// <summary>Executes an operation inside a unit of work and returns a result.</summary>
    Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> operation, CancellationToken cancellationToken = default);
}

/// <summary>
/// Mechanical DTO for a single outbox row (SQL-agnostic). Hosts map this to storage; this package does not ship a database implementation.
/// </summary>
/// <remarks>
/// <para>All string fields are <b>opaque</b> to Ontogony: the platform does not interpret event types, sources, or JSON payloads beyond non-whitespace checks in <see cref="OutboxContracts.Validate"/>.</para>
/// <list type="bullet">
/// <item><description><see cref="MessageId"/>, <see cref="EventId"/> — caller-defined identifiers (dedupe key and correlation id).</description></item>
/// <item><description><see cref="EventType"/>, <see cref="Source"/> — integration metadata; no registry in this package.</description></item>
/// <item><description><see cref="TraceId"/> — correlation string, often aligned with HTTP trace headers.</description></item>
/// <item><description><see cref="PayloadJson"/> — canonical or raw JSON bytes as text; semantic validation is a host concern.</description></item>
/// <item><description><see cref="PayloadHash"/> — opaque fingerprint (for example SHA-256 hex of canonical JSON).</description></item>
/// <item><description><see cref="MetadataJson"/> — opaque JSON object text (sorted key serialization is a caller convention).</description></item>
/// <item><description><see cref="LastError"/> — opaque diagnostic text from the last dispatch attempt.</description></item>
/// </list>
/// </remarks>
public sealed record OutboxMessage(
    string MessageId,
    string EventId,
    string EventType,
    string Source,
    string TraceId,
    DateTimeOffset OccurredAt,
    DateTimeOffset AvailableAt,
    int AttemptCount,
    string? LastError,
    string PayloadJson,
    string PayloadHash,
    string MetadataJson);

/// <summary>Idempotent consumer processing record for an outbox message.</summary>
public sealed record ProcessedMessage(
    string ConsumerName,
    string MessageId,
    DateTimeOffset ProcessedAt,
    string? TraceId = null);

/// <summary>Outcome of dispatching a single outbox message.</summary>
public sealed record OutboxDispatchResult(
    string MessageId,
    bool Dispatched,
    int AttemptCount,
    DateTimeOffset NextAvailableAt,
    string? LastError = null);

/// <summary>
/// Shared validation and helper routines for outbox contracts. Does not perform I/O.
/// </summary>
public static class OutboxContracts
{
    /// <summary>Validates required outbox message fields.</summary>
    public static void Validate(OutboxMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        Require(message.MessageId, nameof(message.MessageId));
        Require(message.EventId, nameof(message.EventId));
        Require(message.EventType, nameof(message.EventType));
        Require(message.Source, nameof(message.Source));
        Require(message.TraceId, nameof(message.TraceId));
        Require(message.PayloadJson, nameof(message.PayloadJson));
        Require(message.PayloadHash, nameof(message.PayloadHash));
        Require(message.MetadataJson, nameof(message.MetadataJson));

        if (message.AttemptCount < 0)
        {
            throw new ArgumentException("AttemptCount cannot be negative.", nameof(message));
        }
    }

    /// <summary>Builds the composite key for processed-message deduplication.</summary>
    public static string BuildProcessedMessageKey(string consumerName, string messageId)
    {
        Require(consumerName, nameof(consumerName));
        Require(messageId, nameof(messageId));
        return $"{consumerName}:{messageId}";
    }

    /// <summary>Calculates the next availability instant using exponential backoff.</summary>
    public static DateTimeOffset CalculateNextAvailableAt(
        DateTimeOffset nowUtc,
        int attemptCount,
        TimeSpan? baseDelay = null,
        TimeSpan? maxDelay = null)
    {
        if (attemptCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(attemptCount), "attemptCount cannot be negative.");
        }

        var initial = baseDelay ?? TimeSpan.FromSeconds(2);
        var cap = maxDelay ?? TimeSpan.FromMinutes(5);

        var exponent = Math.Min(attemptCount, 16);
        var multiplier = Math.Pow(2, exponent);
        var delay = TimeSpan.FromMilliseconds(initial.TotalMilliseconds * multiplier);
        if (delay > cap)
        {
            delay = cap;
        }

        return nowUtc.Add(delay);
    }

    /// <summary>Serializes metadata to canonical sorted JSON object text.</summary>
    public static string SerializeMetadata(IReadOnlyDictionary<string, string>? metadata)
    {
        if (metadata is null || metadata.Count == 0)
        {
            return "{}";
        }

        var ordered = metadata
            .OrderBy(kvp => kvp.Key, StringComparer.Ordinal)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.Ordinal);
        return JsonSerializer.Serialize(ordered);
    }

    private static void Require(string value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{name} cannot be null or whitespace.", name);
        }
    }
}