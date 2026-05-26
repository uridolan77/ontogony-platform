namespace Ontogony.Http;

/// <summary>
/// Mechanical context for a single retry decision (timeouts, attempt index, elapsed time).
/// Passed to <see cref="IRetryClassifierV2"/>; not product-specific.
/// </summary>
/// <param name="AttemptNumber">Zero-based attempt index for the decision.</param>
/// <param name="MaxRetries">Configured maximum retries after the first attempt.</param>
/// <param name="TotalElapsed">Elapsed time since the operation started.</param>
/// <param name="AttemptTimeout">Configured per-attempt timeout, if any.</param>
/// <param name="TotalTimeout">Configured total operation timeout, if any.</param>
/// <param name="IsCallerCancellation">True when the caller's token requested cancellation.</param>
/// <param name="IsAttemptTimeout">True when the failure is classified as a per-attempt timeout.</param>
/// <param name="IsTotalTimeout">True when the failure is classified as total-timeout exhaustion.</param>
public sealed record RetryExceptionContext(
    int AttemptNumber,
    int MaxRetries,
    TimeSpan TotalElapsed,
    TimeSpan? AttemptTimeout,
    TimeSpan? TotalTimeout,
    bool IsCallerCancellation,
    bool IsAttemptTimeout,
    bool IsTotalTimeout);
