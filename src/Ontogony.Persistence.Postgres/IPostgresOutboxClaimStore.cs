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

    Task ReleaseClaimAsync(string messageId, CancellationToken cancellationToken = default);
}
