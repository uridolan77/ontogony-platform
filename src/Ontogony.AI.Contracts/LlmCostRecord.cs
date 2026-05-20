namespace Ontogony.AI.Contracts;

/// <summary>
/// Cost line item: supports estimated vs actual without prescribing billing policy.
/// When <see cref="CostUnknownReason"/> is set, cost could not be computed (for example streaming usage missing)
/// and cost amount fields should be treated as absent.
/// </summary>
public sealed record LlmCostRecord(
    string? Currency,
    decimal? EstimatedCost,
    decimal? ActualCost,
    string? PriceCatalogVersion,
    string? CostUnknownReason = null);
