# PR Spec — PLAT-EVAL-001 — Neutral Evaluation Contracts

## Repo

`ontogony-platform`

## Goal

Add a neutral evaluation contracts package that allows product repos to record eval runs, metrics, scores, and baseline comparisons without placing product semantics in Ontogony.Platform.

## Add

```text
src/Ontogony.Evaluation.Contracts/
tests/Ontogony.Evaluation.Contracts.Tests/
docs/packages/Ontogony.Evaluation.Contracts.md
```

## Public contracts

```csharp
public sealed record EvaluationRunRecord;
public sealed record EvaluationCaseRecord;
public sealed record EvaluationMetricRecord;
public sealed record EvaluationScoreRecord;
public sealed record BaselineComparisonRecord;
public sealed record EvaluationArtifactRef;
public sealed record EvaluationVerdictRecord;
```

## Contract principles

- All semantic values are opaque strings.
- No Allagma, Kanon, Conexus references.
- No model-provider-specific logic.
- No scoring policy; only score fields.
- No persistence provider in this PR.

## Suggested fields

### `EvaluationRunRecord`

```text
EvaluationRunId
SubjectRunId
TraceId
ScenarioId
EvaluationProfileId
StartedAtUtc
CompletedAtUtc
Verdict
Scores
Artifacts
Metadata
```

### `EvaluationScoreRecord`

```text
MetricName
MetricKind
Score
Passed
Threshold
Reason
SourceRef
```

### `BaselineComparisonRecord`

```text
ComparisonId
SubjectRunId
BaselineRunId
ScenarioId
QualityDelta
CostDeltaUsd
LatencyDeltaMs
PolicyEquivalent
PromotionRecommendation
```

## Tests

- JSON round-trip.
- nullable optional fields.
- deterministic canonical JSON hash for sample records.
- public API snapshot.

## Acceptance

- `dotnet build` passes.
- `dotnet test` passes.
- docs package page added.
- changelog updated.
- no product repo references.
