using System.ComponentModel.DataAnnotations;

namespace Ontogony.Http;

/// <summary>
/// Tunable limits for outbound HTTP retries, backoff, circuit breaking, and idempotency-aware retry policy.
/// Used by <see cref="ResilientIntegrationDelegatingHandler"/> and <see cref="TransportResilienceRegistry"/>.
/// </summary>
public sealed class TransportResilienceOptions
{
    public bool Enabled { get; set; } = true;

    [Range(0, 10)]
    public int MaxRetries { get; set; } = 2;

    [Range(10, 60_000)]
    public int BaseDelayMilliseconds { get; set; } = 200;

    // Legacy naming compatibility from donor packages.
    [Range(10, 60_000)]
    public int BaseBackoffMilliseconds
    {
        get => BaseDelayMilliseconds;
        set => BaseDelayMilliseconds = value;
    }

    [Range(100, 600_000)]
    public int MaxDelayMilliseconds { get; set; } = 5_000;

    [Range(1, 100)]
    public int CircuitFailureThreshold { get; set; } = 5;

    [Range(1, 3600)]
    public int CircuitOpenDurationSeconds { get; set; } = 30;

    public int[] RetryableStatusCodes { get; set; } = [408, 425, 429, 500, 502, 503, 504];

    public bool CountOnlyRetryableResponsesAsCircuitFailures { get; set; } = true;

    public bool RetryUnsafeMethodsOnlyWithIdempotencyKey { get; set; } = true;

    [Range(1, 100_000_000)]
    public int MaxBufferedContentBytes { get; set; } = 1_000_000;

    public string IdempotencyKeyHeaderName { get; set; } = "Idempotency-Key";

    /// <summary>
    /// When true and the failing response includes a <c>Retry-After</c> header, the wait before the next attempt is at least that duration (capped by <see cref="MaxDelayMilliseconds"/>).
    /// </summary>
    public bool RespectRetryAfterHeader { get; set; } = true;

    /// <summary>
    /// Randomized jitter applied symmetrically around the computed delay: multiplier in <c>[1 - f, 1 + f]</c> where <c>f</c> is this value. Zero disables jitter.
    /// </summary>
    [Range(0, 1)]
    public double BackoffJitterFraction { get; set; }

    /// <summary>
    /// Maximum number of retries allowed per minute per client. Zero disables retry budgeting. Once exhausted, retries are refused until the next minute window.
    /// </summary>
    [Range(0, 10_000)]
    public int RetryBudgetPerMinute { get; set; } = 0;

    /// <summary>
    /// Total time limit for the entire request lifecycle (all attempts + backoff delays). Null disables total timeout. When exceeded, the in-progress attempt is cancelled.
    /// </summary>
    public TimeSpan? TotalTimeout { get; set; }

    /// <summary>
    /// Time limit for a single HTTP attempt. Null disables per-attempt timeout. When exceeded, the attempt is cancelled as a transient error.
    /// </summary>
    public TimeSpan? AttemptTimeout { get; set; }

    /// <summary>
    /// When true, emit metrics for each attempt, retry, and circuit state change. Used for observability into retry behavior.
    /// </summary>
    public bool EmitAttemptMetrics { get; set; } = true;
}
