namespace Ontogony.Evaluation.Contracts;

/// <summary>Aggregate evaluation verdict (opaque <see cref="Verdict"/> vocabulary).</summary>
public sealed record EvaluationVerdictRecord(
    string Verdict,
    decimal? QualityScore = null,
    string? Summary = null,
    IReadOnlyList<string>? Reasons = null);
