using System.Collections.Concurrent;

namespace Ontogony.Idempotency;

/// <summary>
/// In-memory <see cref="IIdempotencyLedger"/> for <b>tests and single-process</b> hosts; not shared across nodes.
/// </summary>
public sealed class InMemoryIdempotencyLedger : IIdempotencyLedger
{
    private readonly ConcurrentDictionary<string, IdempotencyRecord> _records = new(StringComparer.Ordinal);

    public Task<bool> TryBeginAsync(string key, CancellationToken cancellationToken = default)
    {
        var record = new IdempotencyRecord(key, IdempotencyStatus.InProgress, DateTimeOffset.UtcNow);
        return Task.FromResult(_records.TryAdd(key, record));
    }

    public Task MarkSucceededAsync(string key, string? resultReference = null, CancellationToken cancellationToken = default)
    {
        _records.AddOrUpdate(
            key,
            _ => new IdempotencyRecord(key, IdempotencyStatus.Succeeded, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, resultReference),
            (_, existing) => existing with { Status = IdempotencyStatus.Succeeded, UpdatedAt = DateTimeOffset.UtcNow, ResultReference = resultReference });
        return Task.CompletedTask;
    }

    public Task MarkFailedAsync(string key, string? reason = null, CancellationToken cancellationToken = default)
    {
        _records.AddOrUpdate(
            key,
            _ => new IdempotencyRecord(key, IdempotencyStatus.Failed, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, FailureReason: reason),
            (_, existing) => existing with { Status = IdempotencyStatus.Failed, UpdatedAt = DateTimeOffset.UtcNow, FailureReason = reason });
        return Task.CompletedTask;
    }

    public Task<IdempotencyRecord?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        _records.TryGetValue(key, out var record);
        return Task.FromResult(record);
    }
}
