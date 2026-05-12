namespace Ontogony.Quotas;

/// <summary>
/// Common <see cref="QuotaLimit.Metric"/> string constants (opaque; hosts may define others).
/// </summary>
public static class QuotaMetric
{
    /// <summary>HTTP or logical request count.</summary>
    public const string Requests = "requests";

    /// <summary>LLM input tokens.</summary>
    public const string InputTokens = "input_tokens";

    /// <summary>LLM output tokens.</summary>
    public const string OutputTokens = "output_tokens";

    /// <summary>Combined token count.</summary>
    public const string TotalTokens = "total_tokens";

    /// <summary>Monetary cost in an agreed currency unit.</summary>
    public const string Cost = "cost";

    /// <summary>Concurrent in-flight requests.</summary>
    public const string ConcurrentRequests = "concurrent_requests";
}
