namespace Ontogony.Security;

/// <summary>
/// Process-local nonce store for tests and single-instance hosts. Not suitable for multi-node clusters.
/// Evicts entries by age (<see cref="InMemoryNonceReplayStoreOptions.NonceRetention"/>) and caps total entries
/// (<see cref="InMemoryNonceReplayStoreOptions.MaxStoredNonces"/>). Production clusters must use a distributed <see cref="INonceReplayStore"/>.
/// </summary>
public sealed class InMemoryNonceReplayStore : INonceReplayStore
{
    private readonly InMemoryNonceReplayStoreOptions _options;
    private readonly Func<DateTimeOffset> _utcNow;
    private readonly Dictionary<string, DateTimeOffset> _seen = new(StringComparer.Ordinal);
    private readonly object _sync = new();

    public InMemoryNonceReplayStore(
        InMemoryNonceReplayStoreOptions? options = null,
        Func<DateTimeOffset>? utcNow = null)
    {
        _options = options ?? new InMemoryNonceReplayStoreOptions();
        _utcNow = utcNow ?? (() => DateTimeOffset.UtcNow);
    }

    public bool TryReserveNonce(string serviceId, string nonce, DateTimeOffset requestUtc)
    {
        ArgumentNullException.ThrowIfNull(serviceId);
        ArgumentNullException.ThrowIfNull(nonce);

        var key = serviceId + "\u001f" + nonce;
        var now = _utcNow();
        lock (_sync)
        {
            PruneLocked(now);

            if (_seen.ContainsKey(key))
                return false;

            if (_seen.Count >= _options.MaxStoredNonces)
                EvictOldestLocked();

            _seen[key] = requestUtc;
            return true;
        }
    }

    private void PruneLocked(DateTimeOffset now)
    {
        var cutoff = now - _options.NonceRetention;
        List<string>? toRemove = null;
        foreach (var kv in _seen)
        {
            if (kv.Value < cutoff)
                (toRemove ??= new List<string>()).Add(kv.Key);
        }

        if (toRemove is null)
            return;

        foreach (var k in toRemove)
            _seen.Remove(k);
    }

    private void EvictOldestLocked()
    {
        string? oldestKey = null;
        var oldestUtc = DateTimeOffset.MaxValue;
        foreach (var kv in _seen)
        {
            if (kv.Value < oldestUtc)
            {
                oldestUtc = kv.Value;
                oldestKey = kv.Key;
            }
        }

        if (oldestKey is not null)
            _seen.Remove(oldestKey);
    }

    /// <summary>Clears all reserved nonces (for tests).</summary>
    public void Clear()
    {
        lock (_sync)
        {
            _seen.Clear();
        }
    }
}
