namespace Ontogony.Http;

/// <summary>
/// Backoff curve used between retry attempts in <see cref="ResilientIntegrationDelegatingHandler"/>.
/// </summary>
public enum BackoffPolicy
{
    /// <summary>Delay grows linearly: <c>BaseDelayMilliseconds * (attempt + 1)</c>.</summary>
    Linear = 0,

    /// <summary>Delay grows exponentially: <c>BaseDelayMilliseconds * 2^attempt</c>, capped by <see cref="TransportResilienceOptions.MaxDelayMilliseconds"/>.</summary>
    Exponential = 1
}
