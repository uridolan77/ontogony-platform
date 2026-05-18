# Frontend consumption matrix — Allagma eval

**Repo:** `ontogony-frontend`  
**OpenAPI client:** `src/allagma/api/allagmaClient.ts` (generated from `openapi/allagma.v0.json`)

## Route → API → UI

| UI route | Hook / client | Backend call | Adapter | Primary components |
| --- | --- | --- | --- | --- |
| `/allagma/evaluations` | `useAllagmaEvalDashboard` | `listAllagmaRuns` + per-run `listAllagmaRunEvaluations` | `allagmaEvaluationDashboardAdapters` | Overview cards, scenario matrix, trend panel, limitations card |
| `/allagma/evaluations?dashboardFixture=ci-suite` | same (fixture branch) | None (fixture DTOs) | `allagmaEvaluationDashboardFixtures` | Fixture banners, CI-style matrix |
| `/allagma/evaluations/:evaluationRunId` | `useAllagmaEvaluationRun` | `getAllagmaEvaluationRun` | `allagmaEvaluationAdapters` | Quality score detail, raw metrics, fixture/live banners |
| `/allagma/evaluations/baseline-comparisons/:comparisonId` | `useAllagmaBaselineComparison` | `getAllagmaBaselineComparison` | `allagmaEvaluationAdapters` | Baseline comparison detail |
| Run detail (eval section) | `useAllagmaRunEvaluations` | `listAllagmaRunEvaluations` | `allagmaEvaluationAdapters` | `AllagmaRunEvalTopologyEvidenceSection` |
| Run detail → eval dashboard | Link only | — | — | `run-detail-evaluations-link` |

## Field mapping (evaluation run)

| Backend | View model | UI behavior |
| --- | --- | --- |
| `metadata.sourceKind === "fixture"` | `isFixture`, `sourceKind` | Fixture warning banner |
| `verdict.verdict` | `verdict` (lowercase) | Verdict badge |
| `verdict.qualityScore` | `qualityScore` | Overview average, trend, quality panel |
| `metrics[]` | `metrics[]` | Scenario matrix `replay_kind` via metric name; redacted display |
| `scores[]` | `scores[]` | Quality breakdown; reasons redacted |

## Dashboard sampling (live)

```text
listAllagmaRuns(limit=12)
  → for each run: listAllagmaRunEvaluations(runId)
  → flatten → buildEvalDashboardViewModel
```

Honest scope message: `EVAL_DASHBOARD_NO_GLOBAL_LIST_MESSAGE` — no global list API.

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
| Live sample banner | `AllagmaEvalLiveSourceBanner` | `eval-live-sample-banner` |

## Tests

| Layer | Path |
| --- | --- |
| Unit | `allagmaEvaluationAdapters.test.ts`, `allagmaEvaluationDashboardAdapters.test.ts` |
| E2E | `e2e/allagma-eval-dashboards.spec.ts` |
| OpenAPI drift | `npm run openapi:check` |

## Intentionally not consumed in UI

| Backend capability | Reason |
| --- | --- |
| `POST /runs/{runId}/evaluations` | Operator/harness only; gated `ManualWriteEnabled` |
| `POST /evaluations/baseline-comparisons` | Harness/smoke writes; no operator create form in FE-EVAL-002 |
