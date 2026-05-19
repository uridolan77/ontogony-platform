# EVAL-PRODUCT-003 — Scenario Dataset Surfaces Evidence

Date: 2026-05-19  
Status: **PASS**

## Decision

`EVAL-PRODUCT-003` is accepted as product-hardening scope completion.

Scenario datasets are now surfaced as a read-only product capability across backend and frontend. The implementation intentionally avoids dataset authoring, export bundles, and rich semantic diff UX.

## Backend deliverables (allagma-dotnet)

- Added read-only dataset index/detail routes:
  - `GET /allagma/v0/evaluation-datasets`
  - `GET /allagma/v0/evaluation-datasets/{datasetId}`
- Added dataset DTOs:
  - `AllagmaEvaluationDatasetScenarioSummaryResponse`
  - `AllagmaEvaluationDatasetSummaryResponse`
  - `AllagmaEvaluationDatasetListResponse`
- Added dataset query service and file-system catalog repository over `docs/evals/datasets/*`.
- Added API/auth/not-found coverage in `AllagmaEvaluationApiTests`.
- Updated `docs/api/allagma-openapi-v1.snapshot.json` + provenance sidecar and snapshot assertions.

## Frontend deliverables (ontogony-frontend)

- Synced Allagma OpenAPI + regenerated generated schema (`openapi:sync:allagma`, `openapi:gen`).
- Added dataset API wrappers:
  - `listAllagmaEvaluationDatasets`
  - `getAllagmaEvaluationDataset`
- Added dataset hooks:
  - `useAllagmaEvaluationDatasets`
  - `useAllagmaEvaluationDataset`
- Added read-only dataset surface route/page:
  - `/allagma/evaluations/datasets`
- Dashboard now consumes dataset metadata for operator-facing labels/hints:
  - dataset/suite/scenario filter datalist hints
  - dataset/suite/scenario labels in result/matrix rows where metadata is available
- Added cross-links from dashboard/overview to dataset surface and filtered evaluations.
- Extended E2E mock support for dataset index/detail routes.

## Platform docs refreshed

- `03_EVAL_PRODUCT_GAP_MATRIX.md`
- `04_BACKEND_FRONTEND_ALIGNMENT_GAP_MATRIX.md`
- `05_FRONTEND_OPERATOR_DEPTH_GAP_MATRIX.md`
- `06_CONTRACT_AND_OPENAPI_STATE.md`
- `07_TEST_AND_EVIDENCE_STATE.md`
- `08_KNOWN_LIMITATIONS.md`

## Validation commands

Backend (`allagma-dotnet`):

```powershell
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj --filter "FullyQualifiedName~AllagmaEvaluationApiTests|FullyQualifiedName~AllagmaOpenApiSnapshotTests"
powershell -NoProfile -ExecutionPolicy Bypass -File ./scripts/update-allagma-openapi-provenance.ps1
```

Frontend (`ontogony-frontend`):

```powershell
npm run openapi:sync:allagma
npm run openapi:gen
npm run openapi:check
npm run typecheck
npm run test -- src/app/route-coverage.test.ts src/app/routes.test.ts src/allagma/adapters/allagmaEvaluationDashboardAdapters.test.ts
```

## Remaining limitations (intentional)

- No operator UI for baseline comparison create (`POST /allagma/v0/evaluations/baseline-comparisons`).
- No saved workbench views/bookmarks.
- No rich side-by-side semantic diff visualization.
- Baseline comparison filters remain limited to persisted fields.
- Scenario datasets remain read-only (no authoring flow in this scope).
- Eval export bundle remains deferred to `EVAL-PRODUCT-005`.
