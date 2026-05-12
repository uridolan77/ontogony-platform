namespace Ontogony.Logging;

/// <summary>
/// Provider-neutral AI/gateway log field names. These are mechanical fields only.
/// </summary>
public static class OntogonyAiLogFields
{
    /// <summary>Logical request id for the AI call.</summary>
    public const string RequestId = "request_id";

    /// <summary>Opaque provider identifier.</summary>
    public const string Provider = "provider";

    /// <summary>Opaque model identifier.</summary>
    public const string Model = "model";

    /// <summary>Gateway model alias when distinct from <see cref="Model"/>.</summary>
    public const string ModelAlias = "model_alias";

    /// <summary>Opaque route or deployment id.</summary>
    public const string RouteId = "route_id";

    /// <summary>Attempt index for retries.</summary>
    public const string Attempt = "attempt";

    /// <summary>Whether a fallback route was used.</summary>
    public const string FallbackUsed = "fallback_used";

    /// <summary>Input token count.</summary>
    public const string InputTokens = "input_tokens";

    /// <summary>Output token count.</summary>
    public const string OutputTokens = "output_tokens";

    /// <summary>Total token count.</summary>
    public const string TotalTokens = "total_tokens";

    /// <summary>Monetary cost amount.</summary>
    public const string CostAmount = "cost_amount";

    /// <summary>ISO currency code for <see cref="CostAmount"/>.</summary>
    public const string CostCurrency = "cost_currency";

    /// <summary>Large payload artifact reference id.</summary>
    public const string ArtifactId = "artifact_id";

    /// <summary>Caller idempotency key.</summary>
    public const string IdempotencyKey = "idempotency_key";
}
