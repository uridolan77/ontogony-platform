namespace Ontogony.Errors;

/// <summary>Neutral machine codes shared across Ontogony runtime services.</summary>
public static class CrossServiceErrorCodes
{
    /// <summary>Transport authentication is missing or invalid.</summary>
    public const string AuthMissing = "auth.missing";

    /// <summary>Caller is authenticated but lacks permission.</summary>
    public const string AuthForbidden = "auth.forbidden";

    /// <summary>Idempotency key reused with a different payload.</summary>
    public const string IdempotencyConflict = "idempotency.conflict";

    /// <summary>Idempotency reservation is still in progress.</summary>
    public const string IdempotencyInProgress = "idempotency.in_progress";

    /// <summary>Service is not ready to accept traffic.</summary>
    public const string ReadinessUnavailable = "readiness.unavailable";

    /// <summary>Generic downstream call failure.</summary>
    public const string DownstreamFailure = "downstream.failure";

    /// <summary>Downstream dependency is temporarily unavailable.</summary>
    public const string DownstreamUnavailable = "downstream.unavailable";

    /// <summary>Operation timed out or was canceled.</summary>
    public const string Timeout = "timeout";

    /// <summary>Request validation failed.</summary>
    public const string ValidationFailed = "validation.failed";
}
