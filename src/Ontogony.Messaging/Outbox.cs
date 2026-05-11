namespace Ontogony.Messaging;

public interface IOutboxStore
{
    Task AppendAsync(OutboxMessage message, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OutboxMessage>> ClaimBatchAsync(int batchSize, CancellationToken cancellationToken = default);
    Task MarkDispatchedAsync(string messageId, CancellationToken cancellationToken = default);
    Task MarkFailedAsync(string messageId, string reason, CancellationToken cancellationToken = default);
}

public sealed record OutboxMessage(
    string MessageId,
    string EventType,
    string TraceId,
    string PayloadJson,
    DateTimeOffset CreatedAt,
    int AttemptCount = 0);

public sealed class NoOpOutboxStore : IOutboxStore
{
    public Task AppendAsync(OutboxMessage message, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task<IReadOnlyList<OutboxMessage>> ClaimBatchAsync(int batchSize, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<OutboxMessage>>([]);
    public Task MarkDispatchedAsync(string messageId, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task MarkFailedAsync(string messageId, string reason, CancellationToken cancellationToken = default) => Task.CompletedTask;
}
