namespace Ontogony.Http;

/// <summary>
/// Mechanical backoff delay calculation shared by the resilient handler and conformance harnesses.
/// </summary>
public static class TransportResilienceBackoff
{
    /// <summary>
    /// Computes delay before the next retry attempt, honoring Retry-After and jitter options.
    /// </summary>
    public static TimeSpan ComputeDelay(
        TransportResilienceOptions options,
        int attempt,
        TimeSpan? retryAfter,
        Func<double>? randomUnit = null)
    {
        var baseMs = options.BackoffPolicy == BackoffPolicy.Exponential
            ? (double)options.BaseDelayMilliseconds * Math.Pow(2, attempt)
            : (double)(options.BaseDelayMilliseconds * (attempt + 1));
        var delayMs = Math.Min(baseMs, options.MaxDelayMilliseconds);
        var delay = TimeSpan.FromMilliseconds(delayMs);

        if (options.RespectRetryAfterHeader
            && retryAfter is { } ra
            && ra > TimeSpan.Zero)
        {
            delay = delay > ra ? delay : ra;
            if (delay.TotalMilliseconds > options.MaxDelayMilliseconds)
            {
                delay = TimeSpan.FromMilliseconds(options.MaxDelayMilliseconds);
            }
        }

        if (options.BackoffJitterFraction > 0)
        {
            var unit = randomUnit?.Invoke() ?? Random.Shared.NextDouble();
            var factor = 1 + (unit * 2 - 1) * options.BackoffJitterFraction;
            delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * factor);
            if (delay.TotalMilliseconds > options.MaxDelayMilliseconds)
            {
                delay = TimeSpan.FromMilliseconds(options.MaxDelayMilliseconds);
            }
        }

        return delay;
    }
}
