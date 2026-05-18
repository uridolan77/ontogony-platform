namespace Ontogony.Evaluation.Contracts;

/// <summary>Raw metric observation before scoring (opaque <see cref="MetricName"/> and <see cref="MetricKind"/>).</summary>
public sealed record EvaluationMetricRecord(
    string MetricName,
    string? MetricKind = null,
    decimal? NumericValue = null,
    string? TextValue = null,
    string? Unit = null,
    string? SourceRef = null,
    DateTimeOffset? ObservedAtUtc = null);
