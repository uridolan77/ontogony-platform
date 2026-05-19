# 04 — Backend / Frontend Alignment Gap Matrix

**Audit:** PFH-001 (2026-05-19). **Refreshed:** ALIGN-PRODUCT-001 (2026-05-19) after EVAL-PRODUCT-001.

**Legend:** **aligned** | **partial** | **missing** | **deferred**

| Capability | Backend route | OpenAPI | Generated client | Hook | Adapter | UI route/page | Fixture mode | Tests | Evidence |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Eval dashboard list | **aligned** `GET /allagma/v0/evaluations` | aligned | `listAllagmaEvaluations` | `useAllagmaEvalDashboard` | `mapEvaluationSummaryDto`, dashboard adapters | `/allagma/evaluations` | `dashboardFixture=ci-suite` | `GlobalEvaluationQuery`, adapter tests, e2e | `EVAL_PRODUCT_001_*`, `ALIGN_PRODUCT_001_*` |
| Eval detail | `GET /evaluations/{evaluationRunId}` | aligned | `getAllagmaEvaluationRun` | `useAllagmaEvaluationRun` | `allagmaEvaluationAdapters` | `/allagma/evaluations/:id` | `evalFixture`, `dashboardFixture` | adapter + e2e | `FE_EVAL_002` |
| Per-run evals (run detail) | `GET /runs/{runId}/evaluations` | aligned | `listAllagmaRunEvaluations` | `useAllagmaRunEvaluations` | `allagmaEvaluationAdapters` | `/allagma/runs/:runId` | `evalFixture` | adapter + `e2e/allagma-eval-topology-evidence.spec.ts` | `EVAL_RUN_004` (FE) |
| Baseline comparison read | `GET /evaluations/baseline-comparisons/{id}` | aligned | `getAllagmaBaselineComparison` | `useAllagmaBaselineComparison` | `allagmaEvaluationAdapters` | `/allagma/evaluations/baseline-comparisons/:id` | `dashboardFixture=ci-suite` | e2e + unit | `EVAL_RUN_003`, `FE_EVAL_002` |
| Baseline comparison create | `POST /evaluations/baseline-comparisons` | aligned (no FE form) | not wrapped for UI | — | — | — | harness only | `BaselineComparisonTests` | `EVAL_RUN_003` |
| Baseline comparison list/history | `GET /evaluations/baseline-comparisons` | aligned | `listAllagmaBaselineComparisons` | `useAllagmaBaselineComparisons` | `mapBaselineComparisonSummaryDto` | `/allagma/evaluations/baseline-comparisons` | `dashboardFixture=ci-suite` | `BaselineComparisonQuery`, e2e dashboard spec | `EVAL_PRODUCT_002_*` |
| Scenario dataset index | `GET /evaluation-datasets`, `GET /evaluation-datasets/{datasetId}` | aligned | `listAllagmaEvaluationDatasets`, `getAllagmaEvaluationDataset` | `useAllagmaEvaluationDatasets`, `useAllagmaEvaluationDataset` | dataset metadata adapters + dashboard label lookup | `/allagma/evaluations/datasets` + dashboard links | `dashboardFixture=ci-suite` + live list | backend API tests + FE route/adapter tests | `EVAL_PRODUCT_003_*` |
| Quality scoring display | DTOs on eval run | aligned on eval GET | via eval GET | via eval hooks | `buildEvalQualityBreakdownViewModel` | eval detail | fixture eval ids | `EvalQualityScoringTests`, adapter tests | `EVAL_QUALITY_001` |
| Replay evidence | Kanon replay routes + Allagma run GET | partial (Kanon client separate) | kanon + allagma clients | `useKanonReplayBundles`, `useTraceCorrelation`, `useAllagmaRun` | `kanonProvenanceAdapters`, `buildAllagmaReplayEvidence` | `/allagma/replay` | E2E mock only | `e2e/allagma-replay-evidence.spec.ts` | `FE_TEST_REPLAY_001` |
| Trace correlation | platform + service headers | N/A cross-service | correlation services | `useTraceCorrelation` | `correlationAdapters` | run detail, replay, system | fixture/live e2e | `TRACE_CONTRACT_001` | platform `TRACE_*` |
| Kanon topology decision | `POST .../execution-topologies/evaluate` + `GET /decision-records/{id}` | Kanon snapshot | `kanonClient` | run audit / topology adapters | topology adapters | run detail topology panel | `topologyFixture` | Kanon topology tests, platform scripts | `KANON_OP_001/002` |
| Conexus route evidence | `GET /model-calls/{id}`, admin route-decision | Conexus contracts | conexus client (admin) | observability hooks | — | cross-service links (partial) | live / degraded | `ModelCallRouteEvidenceIntegrationTests` | `EVAL_RUN_005`, `CONEXUS_PERSIST_*` |
| Eval export bundle | **missing** | **missing** | **missing** | **missing** | **missing** | — | — | — | `EVAL-PRODUCT-005` |
| Run retry/cancel/replay | **missing** from Allagma OpenAPI | **missing** | not in client | limitation UI | `backendWaitingContracts` | run triage | limitation banners | unit | deferred — not PFH |

## Manual write gate asymmetry (documented)

| Route | Gate |
| --- | --- |
| `POST /runs/{runId}/evaluations` | `ManualWriteEnabled` + non-production → `403` when disabled |
| `POST /evaluations/baseline-comparisons` | No equivalent gate; harness/smoke only |

Operator UI exposes neither POST. Limitations card lists GET routes only.

## Alignment summary

| Status | Count (major surfaces) |
| --- | --- |
| aligned (read paths + FE consume) | 7 |
| partial (cross-service, POST without UI, filter depth, replay) | 5 |
| missing (export) | 1 |
| deferred (run mutations) | 1 |

A surface is **aligned** only when route, OpenAPI, client, hook, adapter, UI state, fixture/live behavior, tests, and evidence are all mapped and honest.

## Key file paths

| Layer | Path |
| --- | --- |
| Allagma routes | `allagma-dotnet/src/Allagma.Api/Program.cs` |
| Allagma OpenAPI | `allagma-dotnet/docs/api/allagma-openapi-v1.snapshot.json` |
| FE OpenAPI | `ontogony-frontend/openapi/allagma.v0.json` |
| FE client | `ontogony-frontend/src/allagma/api/allagmaClient.ts` |
| Prior matrix | `docs/alignment/eval-full-sanity-alignment/02_FRONTEND_CONSUMPTION_MATRIX.md` |
| PFH package | `docs/product-hardening/eval-alignment-frontend-depth/` |
