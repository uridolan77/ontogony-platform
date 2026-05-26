using System.Net;

namespace Ontogony.Http;

/// <summary>
/// Adapter boundary for per-client circuit breaker state (mechanical; no product trip rules).
/// </summary>
public interface ICircuitBreakerRegistry
{
    /// <summary>Returns a synthetic response when the circuit is open, otherwise null.</summary>
    HttpResponseMessage? TryGetCircuitOpenSyntheticResponse(string clientName, TransportResilienceOptions options);

    /// <summary>Resets failure counters after a successful response.</summary>
    void RecordSuccess(string clientName, TransportResilienceOptions options);

    /// <summary>Increments failure counters and may open the circuit.</summary>
    void RecordFailure(string clientName, TransportResilienceOptions options);

    /// <summary>Returns point-in-time circuit snapshots per registered client name.</summary>
    IReadOnlyDictionary<string, TransportResilienceSnapshot> GetSnapshot();
}
