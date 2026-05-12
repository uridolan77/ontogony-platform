using Ontogony.Persistence;

namespace Ontogony.Persistence.Postgres;

/// <summary>
/// Provider-specific claim lease controls for PostgreSQL outbox workers.
/// </summary>
public interface IPostgresOutboxClaimStore
{
    Task<IReadOnlyList<OutboxMessage>> ClaimAvailableAsync(
        DateTimeOffset asOfUtc,
        int maxBatchSize,
        TimeSpan? leaseDuration = null,
        CancellationToken cancellationToken = default);

    Task<bool> TryClaimAsync(
        string messageId,
        DateTimeOffset asOfUtc,
        TimeSpan? leaseDuration = null,
        CancellationToken cancellationToken = default);

    Task<bool> RenewClaimAsync(
        string messageId,
        TimeSpan? leaseDuration = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a message as dispatched only when this worker currently owns the claim lease.
    /// </summary>
    Task<bool> MarkDispatchedIfOwnedAsync(
        string messageId,
        DateTimeOffset dispatchedAtUtc,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a message as failed only when this worker currently owns the claim lease.
    /// </summary>
    Task<bool> MarkFailedIfOwnedAsync(
        string messageId,
        string lastError,
        DateTimeOffset nextAvailableAtUtc,
        CancellationToken cancellationToken = default);

    Task ReleaseClaimAsync(string messageId, CancellationToken cancellationToken = default);
}
