namespace Ontogony.Idempotency;

public interface IIdempotencyLedger
{
    Task<bool> TryBeginAsync(string key, CancellationToken cancellationToken = default);
    Task MarkSucceededAsync(string key, string? resultReference = null, CancellationToken cancellationToken = default);
    Task MarkFailedAsync(string key, string? reason = null, CancellationToken cancellationToken = default);
    Task<IdempotencyRecord?> GetAsync(string key, CancellationToken cancellationToken = default);
}

public sealed record IdempotencyRecord(
    string Key,
    IdempotencyStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt = null,
    string? ResultReference = null,
    string? FailureReason = null);

public enum IdempotencyStatus
{
    InProgress = 0,
    Succeeded = 1,
    Failed = 2
}
