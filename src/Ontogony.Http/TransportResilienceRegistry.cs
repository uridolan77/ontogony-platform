using System.Collections.Concurrent;
using System.Net;
using Ontogony.Primitives;

namespace Ontogony.Http;

/// <summary>
/// Per-named-client circuit and retry-budget state for <see cref="ResilientIntegrationDelegatingHandler"/>.
/// </summary>
public sealed class TransportResilienceRegistry : ICircuitBreakerRegistry
{
    private readonly ConcurrentDictionary<string, ClientCircuitState> _byClient = new(StringComparer.Ordinal);
    private readonly RetryBudgetTracker _budgetTracker;
    private readonly IClock _clock;

    /// <summary>Creates the registry with optional clock override.</summary>
    public TransportResilienceRegistry(IClock? clock = null)
    {
        _clock = clock ?? new SystemClock();
        _budgetTracker = new RetryBudgetTracker(_clock);
    }

    /// <summary>Returns a synthetic 503 when the circuit is open, otherwise null.</summary>
    public HttpResponseMessage? TryGetCircuitOpenSyntheticResponse(string clientName, TransportResilienceOptions options)
    {
        if (!options.Enabled)
        {
            return null;
        }

        var state = _byClient.GetOrAdd(clientName, _ => new ClientCircuitState());
        lock (state.Sync)
        {
            if (state.CircuitOpenUntilUtc is { } openUntil && _clock.UtcNow < openUntil)
            {
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    ReasonPhrase = "ontogony_circuit_open"
                };
            }
        }

        return null;
    }

    /// <summary>
    /// Attempt to consume a retry from the budget for the given client.
    /// </summary>
    /// <returns>True if allowed; false if budget exhausted.</returns>
    public bool TryConsumeRetryBudget(string clientName, TransportResilienceOptions options)
    {
        return _budgetTracker.TryConsumeRetry(clientName, options.RetryBudgetPerMinute);
    }

    /// <summary>Resets failure counters when a successful response is observed.</summary>
    public void RecordSuccess(string clientName, TransportResilienceOptions options)
    {
        if (!options.Enabled)
        {
            return;
        }

        var state = _byClient.GetOrAdd(clientName, _ => new ClientCircuitState());
        lock (state.Sync)
        {
            state.ConsecutiveFailures = 0;
            state.CircuitOpenUntilUtc = null;
        }
    }

    /// <summary>Increments failure counters and may open the circuit.</summary>
    public void RecordFailure(string clientName, TransportResilienceOptions options)
    {
        if (!options.Enabled)
        {
            return;
        }

        var state = _byClient.GetOrAdd(clientName, _ => new ClientCircuitState());
        lock (state.Sync)
        {
            state.ConsecutiveFailures++;
            if (state.ConsecutiveFailures >= options.CircuitFailureThreshold)
            {
                state.CircuitOpenUntilUtc = _clock.UtcNow.AddSeconds(options.CircuitOpenDurationSeconds);
                state.ConsecutiveFailures = 0;
            }
        }
    }

    /// <summary>Returns a snapshot of circuit state per registered client.</summary>
    public IReadOnlyDictionary<string, TransportResilienceSnapshot> GetSnapshot()
    {
        var dict = new Dictionary<string, TransportResilienceSnapshot>(StringComparer.OrdinalIgnoreCase);
        foreach (var kv in _byClient)
        {
            lock (kv.Value.Sync)
            {
                var open = kv.Value.CircuitOpenUntilUtc is { } u && _clock.UtcNow < u;
                dict[kv.Key] = new TransportResilienceSnapshot(open, kv.Value.ConsecutiveFailures, kv.Value.CircuitOpenUntilUtc);
            }
        }

        return dict;
    }

    private sealed class ClientCircuitState
    {
        public object Sync { get; } = new();

        public int ConsecutiveFailures { get; set; }

        public DateTimeOffset? CircuitOpenUntilUtc { get; set; }
    }
}

/// <summary>
/// Point-in-time circuit snapshot for a single HTTP client name.
/// </summary>
/// <param name="CircuitOpen">True when synthetic open-circuit responses would be returned.</param>
/// <param name="ConsecutiveFailures">Current failure streak when the circuit is not open.</param>
/// <param name="CircuitOpenUntilUtc">UTC instant until which the circuit remains open, if applicable.</param>
public sealed record TransportResilienceSnapshot(bool CircuitOpen, int ConsecutiveFailures, DateTimeOffset? CircuitOpenUntilUtc);
