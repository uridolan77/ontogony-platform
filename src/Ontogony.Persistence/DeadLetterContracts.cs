namespace Ontogony.Persistence;

/// <summary>
/// Mechanical snapshot written when a message is moved to a dead-letter sink.
/// Storage format is intentionally unspecified; hosts map this to queues, tables, or blobs.
/// </summary>
public sealed record DeadLetterMessage(
    string MessageId,
    string EventId,
    string EventType,
    string Source,
    string TraceId,
    DateTimeOffset OccurredAt,
    DateTimeOffset DeadLetteredAtUtc,
    int FinalAttemptCount,
    string Reason,
    string PayloadJson,
    string PayloadHash,
    string MetadataJson);

/// <summary>
/// Writes dead-lettered outbox messages. Implementations are host-specific.
/// </summary>
public interface IDeadLetterWriter
{
    /// <summary>Writes a dead-letter message to the configured sink.</summary>
    Task WriteAsync(DeadLetterMessage deadLetter, CancellationToken cancellationToken = default);
}
