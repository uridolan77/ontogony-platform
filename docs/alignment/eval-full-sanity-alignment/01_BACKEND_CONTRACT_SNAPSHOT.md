# Backend contract snapshot — Allagma eval (v0)

**Aligned at:** 2026-05-18  
**Source of truth:** `allagma-dotnet/docs/api/allagma-openapi-v1.snapshot.json`  
**API prefix:** `/allagma/v0`

## Evaluation HTTP routes

| Method | Path | operationId | Auth | Notes |
| --- | --- | --- | --- | --- |
| GET | `/runs/{runId}/evaluations` | `listRunEvaluations` | Bearer | `404` if run missing (`allagma.run_not_found`) |
| POST | `/runs/{runId}/evaluations` | `writeRunEvaluation` | Bearer | Dev-only manual write; `403` when disabled |
| GET | `/evaluations/{evaluationRunId}` | `getEvaluationRun` | Bearer | `404` → `allagma.evaluation_not_found` |
| POST | `/evaluations/baseline-comparisons` | `createBaselineComparison` | Bearer | Harness/operator write path |
| GET | `/evaluations/baseline-comparisons/{comparisonId}` | `getBaselineComparison` | Bearer | `404` → `allagma.baseline_comparison_not_found` |

There is **no** `GET /evaluations` global list route.

## Core DTOs

### `AllagmaEvaluationRunResponse`

| Field | Type | Frontend use |
| --- | --- | --- |
| `evaluationRunId` | string (required) | Primary key, deep links |
| `subjectRunId` | string? | Run linkage, dashboard sampling |
| `traceId` | string? | Correlation (not shown raw in eval UI) |
| `scenarioId` | string? | Scenario matrix row key |
| `evaluationProfileId` | string? | Profile display |
| `startedAtUtc` / `completedAtUtc` | date-time? | Trend ordering |
| `verdict` | `AllagmaEvaluationVerdictResponse`? | Pass/fail/inconclusive + `qualityScore` |
| `scores` | array? | Quality detail panel |
| `metrics` | array? | Raw metrics (redacted in UI) |
| `metadata` | string map? | `sourceKind`, `replay_kind`, harness fields |

### `AllagmaBaselineComparisonResponse`

| Field | Type | Frontend use |
| --- | --- | --- |
| `comparisonId` | string (required) | Baseline detail route |
| `subjectRunId` | string (required) | Subject run link |
| `baselineRunId` | string? | Baseline run link |
| `baselineMode` / `subjectMode` | string? | Mode labels |
| `qualityDelta`, `costDeltaUsd`, `latencyDeltaMs` | number? | Comparison cards |
| `policyEquivalent`, `sideEffectSafe` | bool? | Safety summary |
| `promotionRecommendation` | string? | Promotion hint |

### `WriteEvaluationRunRequest` (POST manual write)

Required: `scenarioId`, `evaluationProfileId`. Optional: `sourceKind` (fixture harness uses `fixture`).

### `CreateBaselineComparisonRequest`

Required: `baselineRunId`, `subjectRunId`, `scenarioId`.

## Error envelope

Eval routes return `CrossServiceErrorEnvelope` on `404`/`403`:

| HTTP | Code | When |
| --- | --- | --- |
| 404 | `allagma.run_not_found` | List evals for missing run |
| 404 | `allagma.evaluation_not_found` | Get evaluation by id |
| 404 | `allagma.baseline_comparison_not_found` | Get comparison by id |
| 403 | `allagma.evaluation_manual_write_disabled` | POST eval when manual write off or production |

## Persistence

| Mode | Eval runs | Baseline comparisons |
| --- | --- | --- |
| `InMemory` | `InMemoryEvaluationRunRepository` | In-memory store |
| `Postgres` | `allagma_evaluation_runs` | `allagma_baseline_comparisons` |

See `allagma-dotnet/docs/evidence/EVAL_DUR_001_DURABLE_EVAL_PERSISTENCE.md`.

## Harness and CI (backend-only)

| Artifact | Location |
| --- | --- |
| Eval profile | `docs/evals/profiles/eval-profile-v0.json` |
| Scenario dataset | `docs/evals/datasets/scenario-dataset-v0/` |
| CI suite script | `scripts/run-eval-ci-suite.ps1` |
| Cross-repo smoke | `scripts/run-sys-eval-smoke.ps1` |

## Related run evidence (not eval routes)

Run detail and eval topology panels also consume:

- `GET /runs/{runId}/events` — topology + governed events
- Run audit / route evidence metrics (no raw prompts in DTOs)

Kanon topology authorization and Conexus route evidence are verified in full sanity via their respective services (see `06_FULL_SANITY_TEST_PLAN.md`).
