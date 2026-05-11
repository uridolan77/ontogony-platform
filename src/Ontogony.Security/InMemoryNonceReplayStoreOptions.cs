namespace Ontogony.Security;

/// <summary>
/// Bounds for <see cref="InMemoryNonceReplayStore"/> (process-local / test use only).
/// </summary>
public sealed class InMemoryNonceReplayStoreOptions
{
    /// <summary>Entries older than this duration from <see cref="INonceReplayStore.TryReserveNonce"/> time are eligible for eviction.</summary>
    public TimeSpan NonceRetention { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>After pruning by retention, oldest entries are removed until the store is at most this size.</summary>
    public int MaxStoredNonces { get; set; } = 100_000;
}
