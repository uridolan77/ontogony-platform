using System.ComponentModel.DataAnnotations;

namespace Ontogony.Http;

/// <summary>
/// Tunable limits for outbound HTTP retries, backoff, circuit breaking, and idempotency-aware retry policy.
/// Used by <see cref="ResilientIntegrationDelegatingHandler"/> and <see cref="TransportResilienceRegistry"/>.
/// </summary>
public sealed class TransportResilienceOptions
{
    /// <summary>Master switch for resilience behavior on the outbound pipeline.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Maximum retry attempts after the first try.</summary>
    [Range(0, 10)]
    public int MaxRetries { get; set; } = 2;

    /// <summary>Base linear backoff component in milliseconds.</summary>
    [Range(10, 60_000)]
    public int BaseDelayMilliseconds { get; set; } = 200;

    // Legacy naming compatibility from donor packages.
    /// <summary>Alias for <see cref="BaseDelayMilliseconds"/>.</summary>
    [Range(10, 60_000)]
    public int BaseBackoffMilliseconds
    {
        get => BaseDelayMilliseconds;
        set => BaseDelayMilliseconds = value;
    }

    /// <summary>Upper cap for computed delay between attempts.</summary>
    [Range(100, 600_000)]
    public int MaxDelayMilliseconds { get; set; } = 5_000;

    /// <summary>Consecutive failures before opening the circuit.</summary>
    [Range(1, 100)]
    public int CircuitFailureThreshold { get; set; } = 5;

    /// <summary>How long the circuit stays open after tripping.</summary>
    [Range(1, 3600)]
    public int CircuitOpenDurationSeconds { get; set; } = 30;

    /// <summary>HTTP status codes eligible for retry.</summary>
    public int[] RetryableStatusCodes { get; set; } = [408, 425, 429, 500, 502, 503, 504];

    /// <summary>When true, only retryable responses increment circuit failure counters.</summary>
    public bool CountOnlyRetryableResponsesAsCircuitFailures { get; set; } = true;

    /// <summary>When true, unsafe methods retry only when an idempotency key header is present.</summary>
    public bool RetryUnsafeMethodsOnlyWithIdempotencyKey { get; set; } = true;

    /// <summary>Maximum bytes buffered to allow body replay on retry.</summary>
    [Range(1, 100_000_000)]
    public int MaxBufferedContentBytes { get; set; } = 1_000_000;

    /// <summary>Primary header name examined for idempotency-aware retries.</summary>
    public string IdempotencyKeyHeaderName { get; set; } = OntogonyIntegrationHeaders.IdempotencyKey;

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
