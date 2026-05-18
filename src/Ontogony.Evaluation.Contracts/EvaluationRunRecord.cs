namespace Ontogony.Evaluation.Contracts;

/// <summary>Evaluation run boundary record (contracts only; temporal values use <see cref="DateTimeOffset"/>).</summary>
/// <remarks>
/// <see cref="EvaluationProfileId"/> and verdict strings are opaque; Ontogony does not define scoring policy.
/// </remarks>
public sealed record EvaluationRunRecord(
    string EvaluationRunId,
    string? SubjectRunId = null,
    string? TraceId = null,
    string? ScenarioId = null,
    string? EvaluationProfileId = null,
    DateTimeOffset? StartedAtUtc = null,
    DateTimeOffset? CompletedAtUtc = null,
    EvaluationVerdictRecord? Verdict = null,
    IReadOnlyList<EvaluationScoreRecord>? Scores = null,
    IReadOnlyList<EvaluationCaseRecord>? Cases = null,
    IReadOnlyList<EvaluationArtifactRef>? Artifacts = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
