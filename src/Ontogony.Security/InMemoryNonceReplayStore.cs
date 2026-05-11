namespace Ontogony.Security;

/// <summary>
/// Process-local nonce store for tests and single-instance hosts. Not suitable for multi-node clusters.
/// </summary>
public sealed class InMemoryNonceReplayStore : INonceReplayStore
{
    private readonly HashSet<string> _seen = new(StringComparer.Ordinal);
    private readonly object _sync = new();

    public bool TryReserveNonce(string serviceId, string nonce, DateTimeOffset requestUtc)
    {
        ArgumentNullException.ThrowIfNull(serviceId);
        ArgumentNullException.ThrowIfNull(nonce);

        var key = serviceId + "\u001f" + nonce;
        lock (_sync)
        {
            if (_seen.Contains(key))
                return false;

            _seen.Add(key);
            return true;
        }
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
