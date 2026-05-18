namespace Ontogony.Evaluation.Contracts;

/// <summary>Baseline comparison between a subject run and a reference run (opaque promotion vocabulary).</summary>
public sealed record BaselineComparisonRecord(
    string ComparisonId,
    string SubjectRunId,
    string? BaselineRunId = null,
    string? ScenarioId = null,
    string? BaselineMode = null,
    decimal? QualityDelta = null,
    decimal? CostDeltaUsd = null,
    long? LatencyDeltaMs = null,
    bool? PolicyEquivalent = null,
    bool? SideEffectSafe = null,
    string? PromotionRecommendation = null);
