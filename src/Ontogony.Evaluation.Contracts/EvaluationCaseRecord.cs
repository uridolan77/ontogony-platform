namespace Ontogony.Evaluation.Contracts;

/// <summary>Per-scenario evaluation case within a run (opaque scenario and verdict strings).</summary>
public sealed record EvaluationCaseRecord(
    string CaseId,
    string ScenarioId,
    string? EvaluationRunId = null,
    string? SubjectRunId = null,
    string? TraceId = null,
    EvaluationVerdictRecord? Verdict = null,
    IReadOnlyList<EvaluationScoreRecord>? Scores = null,
    IReadOnlyList<EvaluationMetricRecord>? Metrics = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
