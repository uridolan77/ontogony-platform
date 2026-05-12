namespace Ontogony.Logging;

/// <summary>
/// Provider-neutral AI/gateway log field names. These are mechanical fields only.
/// </summary>
public static class OntogonyAiLogFields
{
    public const string RequestId = "request_id";
    public const string Provider = "provider";
    public const string Model = "model";
    public const string ModelAlias = "model_alias";
    public const string RouteId = "route_id";
    public const string Attempt = "attempt";
    public const string FallbackUsed = "fallback_used";

    public const string InputTokens = "input_tokens";
    public const string OutputTokens = "output_tokens";
    public const string TotalTokens = "total_tokens";
    public const string CostAmount = "cost_amount";
    public const string CostCurrency = "cost_currency";

    public const string ArtifactId = "artifact_id";
    public const string IdempotencyKey = "idempotency_key";
}
