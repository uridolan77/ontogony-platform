namespace Ontogony.Evaluation.Contracts;

/// <summary>Scored metric outcome (opaque names; callers own thresholds and rubrics).</summary>
public sealed record EvaluationScoreRecord(
    string MetricName,
    string? MetricKind = null,
    decimal? Score = null,
    bool? Passed = null,
    decimal? Threshold = null,
    string? Reason = null,
    string? SourceRef = null);
