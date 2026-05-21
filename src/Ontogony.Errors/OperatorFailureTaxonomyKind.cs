namespace Ontogony.Errors;

/// <summary>Stable operator-facing failure categories (SYS-TIGHT-006). Does not replace service-specific public error contracts.</summary>
public static class OperatorFailureTaxonomyKind
{
    /// <summary>Transport authentication is missing or invalid.</summary>
    public const string AuthFailed = "auth_failed";

    /// <summary>Caller is authenticated but lacks permission or policy denied.</summary>
    public const string Forbidden = "forbidden";

    /// <summary>Request validation failed.</summary>
    public const string ValidationFailed = "validation_failed";

    /// <summary>Target resource does not exist.</summary>
    public const string NotFound = "not_found";

    /// <summary>State conflict (non-idempotency).</summary>
    public const string Conflict = "conflict";

    /// <summary>Idempotency key reuse or unsupported streaming idempotency.</summary>
    public const string IdempotencyConflict = "idempotency_conflict";

    /// <summary>Downstream dependency temporarily unavailable.</summary>
    public const string DownstreamUnavailable = "downstream_unavailable";

    /// <summary>Model/provider failure that may succeed on retry.</summary>
    public const string ProviderFailedRetryable = "provider_failed_retryable";

    /// <summary>Model/provider failure requiring input or policy change.</summary>
    public const string ProviderFailedTerminal = "provider_failed_terminal";

    /// <summary>Gateway quota exhausted.</summary>
    public const string QuotaExceeded = "quota_exceeded";

    /// <summary>Operation timed out or was canceled.</summary>
    public const string Timeout = "timeout";

    /// <summary>Unclassified failure.</summary>
    public const string Unknown = "unknown";
}
