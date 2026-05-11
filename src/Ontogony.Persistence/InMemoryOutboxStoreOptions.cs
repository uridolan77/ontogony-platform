namespace Ontogony.Persistence;

/// <summary>
/// Options for <see cref="InMemoryOutboxStore"/> (reference implementation and tests).
/// </summary>
public sealed class InMemoryOutboxStoreOptions
{
    /// <summary>
    /// When set together with a non-null <see cref="IDeadLetterWriter"/> on the store,
    /// a <see cref="IOutboxDispatcher.MarkFailedAsync"/> call that raises <see cref="OutboxMessage.AttemptCount"/>
    /// to this value or higher will emit a <see cref="DeadLetterMessage"/> and exclude the message from future reads.
    /// </summary>
    public int? MoveToDeadLetterAfterAttempts { get; set; }
}
