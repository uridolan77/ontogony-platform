# Frontend consumption matrix — Allagma eval

**Repo:** `ontogony-frontend`  
**OpenAPI client:** `src/allagma/api/allagmaClient.ts` (generated from `openapi/allagma.v0.json`)  
**Refreshed:** ALIGN-PRODUCT-001 (2026-05-19) after EVAL-PRODUCT-001 global list.

## Route → API → UI

| UI route | Hook / client | Backend call | Adapter | Primary components |
| --- | --- | --- | --- | --- |
| `/allagma/evaluations` | `useAllagmaEvalDashboard` | `listAllagmaEvaluations` (`GET /allagma/v0/evaluations`, limit 100) | `allagmaEvaluationDashboardAdapters` | Overview cards, scenario matrix, trend panel, limitations card |
| `/allagma/evaluations?dashboardFixture=ci-suite` | same (fixture branch) | None (fixture DTOs) | `allagmaEvaluationDashboardFixtures` | Fixture banners, CI-style matrix |
| `/allagma/evaluations/:evaluationRunId` | `useAllagmaEvaluationRun` | `getAllagmaEvaluationRun` | `allagmaEvaluationAdapters` | Quality score detail, raw metrics, fixture/live banners |
| `/allagma/evaluations/baseline-comparisons/:comparisonId` | `useAllagmaBaselineComparison` | `getAllagmaBaselineComparison` | `allagmaEvaluationAdapters` | Baseline comparison detail |
| Run detail (eval section) | `useAllagmaRunEvaluations` | `listAllagmaRunEvaluations` | `allagmaEvaluationAdapters` | `AllagmaRunEvalTopologyEvidenceSection` |
| Run detail → eval dashboard | Link only | — | — | `run-detail-evaluations-link` |
| `/allagma/replay` | `useAllagmaRun`, `useKanonReplayBundles`, `useTraceCorrelation` | run GET + Kanon replay bundles | `buildAllagmaReplayEvidence` | `AllagmaReplayEvidenceWorkbench` |

## Field mapping (evaluation run / summary)

| Backend | View model | UI behavior |
| --- | --- | --- |
| `metadata.sourceKind === "fixture"` | `isFixture`, `sourceKind` | Fixture warning banner |
| `verdict.verdict` | `verdict` (lowercase) | Verdict badge |
| `verdict.qualityScore` | `qualityScore` | Overview average, trend, quality panel |
| `metrics[]` | `metrics[]` | Scenario matrix `replay_kind` via metric name; redacted display |
| `scores[]` | `scores[]` | Quality breakdown; reasons redacted |
| Summary row (`AllagmaEvaluationRunSummaryResponse`) | `mapEvaluationSummaryDto` | Dashboard list, matrix, trend (no full scores on list) |

## Dashboard live mode (post EVAL-PRODUCT-001)

```text
listAllagmaEvaluations(limit=100)
  → mapEvaluationSummaryDtos
  → buildEvalDashboardViewModel(usesGlobalList: true)
```

Honest scope: default limit 100; not full history. Filters (`datasetId`, `baselineComparisonId`, etc.) depend on sparse metadata for manual writes.

## Fixture modes

| Query / path | Purpose |
| --- | --- |
| `?dashboardFixture=ci-suite` | Labelled dashboard fixture (`scenario-dataset-v0` style) |
| Fixture evaluation id routes | `allagmaEvaluationEvidenceFixtures` for detail/baseline when id unknown live |

## Error and empty states

| State | Component | test id |
| --- | --- | --- |
| Loading | `ProductLiveQueryState` | `product-live-query-loading` |
| Error (shows `code`) | `ProductLiveQueryState` | `product-live-query-error` |
| Empty | `ProductLiveQueryState` | `product-live-query-empty` |
| Not configured | `ProductLiveQueryState` | `product-live-query-not-configured` |
| Live list banner | `AllagmaEvalLiveSourceBanner` | `eval-live-sample-banner` |

## Tests

| Layer | Path |
| --- | --- |
| Unit | `allagmaEvaluationAdapters.test.ts`, `allagmaEvaluationDashboardAdapters.test.ts` |
| E2E | `e2e/allagma-eval-dashboards.spec.ts`, `e2e/fixture-live-boundary.spec.ts` |
| OpenAPI drift | `npm run openapi:check` |

## Intentionally not consumed in UI

| Backend capability | Reason |
| --- | --- |
| `POST /runs/{runId}/evaluations` | Operator/harness only; gated `ManualWriteEnabled` + non-production |
| `POST /evaluations/baseline-comparisons` | Harness/smoke writes; no operator create form; **not** gated like manual eval POST |
| `GET /evaluations` filters beyond limit | Dashboard uses default limit only until FE-PRODUCT-001 |
