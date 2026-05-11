namespace Ontogony.Security;

/// <summary>
/// Rejects replayed nonces for HMAC service identity requests.
/// </summary>
public interface INonceReplayStore
{
    /// <summary>
    /// Attempts to reserve a nonce for a service. Returns false if the nonce was already used.
    /// </summary>
    bool TryReserveNonce(string serviceId, string nonce, DateTimeOffset requestUtc);
}
