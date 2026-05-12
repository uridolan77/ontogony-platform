using Ontogony.Persistence;

namespace Ontogony.Testing;

/// <summary>
/// Conformance test harness for <see cref="IOutboxWriter"/>, <see cref="IOutboxReader"/>, and
/// <see cref="IOutboxDispatcher"/>. Provides assertion helpers that work against any
/// <see cref="InMemoryOutboxStore"/> (or any store implementing the contracts) without
/// coupling tests to a specific database provider.
/// </summary>
public static class OutboxConformanceHarness
{
    /// <summary>
    /// Builds a minimal valid <see cref="OutboxMessage"/> for use in conformance tests.
    /// Callers can override any property after construction.
    /// </summary>
    public static OutboxMessage BuildMessage(
        string messageId,
        string eventType = "ontogony.test.event",
        string source = "ontogony://test/conformance",
        string traceId = "trace-conformance",
        DateTimeOffset? occurredAt = null,
        DateTimeOffset? availableAt = null,
        string payloadJson = "{}")
    {
        if (string.IsNullOrWhiteSpace(messageId))
            throw new ArgumentException("messageId cannot be empty.", nameof(messageId));

        var now = occurredAt ?? DateTimeOffset.UtcNow;
        return new OutboxMessage(
            MessageId: messageId,
            EventId: Guid.NewGuid().ToString("n"),
            EventType: eventType,
            Source: source,
            TraceId: traceId,
            OccurredAt: now,
            AvailableAt: availableAt ?? now,
            AttemptCount: 0,
            LastError: null,
            PayloadJson: payloadJson,
            PayloadHash: "conformance-hash",
            MetadataJson: "{}");
    }

    /// <summary>
    /// Verifies that a message written to <paramref name="writer"/> can be read back
    /// via <paramref name="reader"/> and includes the expected MessageId.
    /// </summary>
    public static async Task AssertWriteThenReadAsync(
        IOutboxWriter writer,
        IOutboxReader reader,
        OutboxMessage message)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(message);

        await writer.WriteAsync(message);

        var batch = await reader.ReadAvailableAsync(
            asOfUtc: DateTimeOffset.UtcNow.AddSeconds(1),
            maxBatchSize: 10);

        var found = batch.Any(m => string.Equals(m.MessageId, message.MessageId, StringComparison.Ordinal));
        if (!found)
        {
            throw new InvalidOperationException(
                $"Message '{message.MessageId}' was written but not returned by ReadAvailableAsync.");
        }
    }

    /// <summary>
    /// Verifies that after <see cref="IOutboxDispatcher.MarkDispatchedAsync"/>, the message is
    /// no longer returned by a subsequent <see cref="IOutboxReader.ReadAvailableAsync"/> call.
    /// </summary>
    public static async Task AssertMarkDispatchedRemovesFromQueueAsync(
        IOutboxWriter writer,
        IOutboxReader reader,
        IOutboxDispatcher dispatcher,
        OutboxMessage message)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(message);

        await writer.WriteAsync(message);
        await dispatcher.MarkDispatchedAsync(message.MessageId, DateTimeOffset.UtcNow);

        var batch = await reader.ReadAvailableAsync(
            asOfUtc: DateTimeOffset.UtcNow.AddSeconds(1),
            maxBatchSize: 10);

        var found = batch.Any(m => string.Equals(m.MessageId, message.MessageId, StringComparison.Ordinal));
        if (found)
        {
            throw new InvalidOperationException(
                $"Message '{message.MessageId}' was marked dispatched but is still returned by ReadAvailableAsync.");
        }
    }

    /// <summary>
    /// Verifies that writing a message with a duplicate <see cref="OutboxMessage.MessageId"/>
    /// throws an <see cref="InvalidOperationException"/>.
    /// </summary>
    public static async Task AssertDuplicateWriteThrowsAsync(IOutboxWriter writer, OutboxMessage message)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(message);

        await writer.WriteAsync(message);

        bool threw = false;
        try
        {
            await writer.WriteAsync(message);
        }
        catch (InvalidOperationException)
        {
            threw = true;
        }

        if (!threw)
        {
            throw new InvalidOperationException(
                $"Expected a duplicate write for MessageId '{message.MessageId}' to throw InvalidOperationException, but it did not.");
        }
    }

    /// <summary>
    /// Verifies that <see cref="IOutboxDispatcher.MarkFailedAsync"/> is idempotent:
    /// calling it twice does not throw.
    /// </summary>
    public static async Task AssertMarkFailedIsIdempotentAsync(
        IOutboxWriter writer,
        IOutboxDispatcher dispatcher,
        OutboxMessage message)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(dispatcher);
        ArgumentNullException.ThrowIfNull(message);

        await writer.WriteAsync(message);

        var nextAvailable = DateTimeOffset.UtcNow.AddSeconds(30);

        await dispatcher.MarkFailedAsync(message.MessageId, "first error", nextAvailable);
        await dispatcher.MarkFailedAsync(message.MessageId, "second error", nextAvailable); // must not throw
    }
}
