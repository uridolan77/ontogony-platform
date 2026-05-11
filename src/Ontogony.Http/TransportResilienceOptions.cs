using System.ComponentModel.DataAnnotations;

namespace Ontogony.Http;

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
}
