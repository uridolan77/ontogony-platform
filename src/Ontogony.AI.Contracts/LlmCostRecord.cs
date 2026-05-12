namespace Ontogony.AI.Contracts;

/// <summary>
/// Cost line item: supports estimated vs actual without prescribing billing policy.
/// </summary>
public sealed record LlmCostRecord(string? Currency, decimal? EstimatedCost, decimal? ActualCost, string? PriceCatalogVersion);
