# EVAL-PRODUCT-002 — Baseline comparison workbench evidence

**Date:** 2026-05-19  
**Verdict:** PASS

## Not production readiness

Product hardening scope only. This evidence does **not** claim production readiness, real provider mode, cloud deployment, or identity/TLS hardening.

## Goal

Deliver baseline comparison history depth with a real list API and operator workbench UX (filters, cursor pagination, drilldown links, fixture/live honesty).

## Backend deliverables (`allagma-dotnet`)

| Area | Path |
| --- | --- |
| Baseline list route | `src/Allagma.Api/Program.cs` (`GET /allagma/v0/evaluations/baseline-comparisons`) |
| Query service + cursor | `src/Allagma.Application/Evaluation/QueryBaselineComparisonsService.cs` |
| Query/cursor models | `src/Allagma.Application/BaselineComparisonListQuery.cs`, `BaselineComparisonListCursorCodec.cs` |
| Repository ports | `src/Allagma.Application/Ports.cs` (`IBaselineComparisonListRepository`) |
| In-memory + harness list repos | `src/Allagma.Infrastructure/InMemoryBaselineComparisonRepository.cs`, `src/Allagma.Application/Evaluation/EvalHarnessStores.cs` |
| Postgres list repo | `src/Allagma.Infrastructure/Persistence/PostgresBaselineComparisonRepository.cs` |
| HTTP query binding | `src/Allagma.Api/EvaluationQueryParameters.cs` |
| Contract DTOs | `src/Allagma.Contracts/EvaluationQueryContracts.cs` |
| API tests | `tests/Allagma.Tests/AllagmaEvaluationApiTests.cs` (`BaselineComparisonQuery`) |
| OpenAPI drift guards | `tests/Allagma.Tests/AllagmaOpenApiSnapshotTests.cs`, `docs/api/allagma-openapi-v1.snapshot.json` |

## Frontend deliverables (`ontogony-frontend`)

| Area | Path |
| --- | --- |
| New workbench page | `src/allagma/pages/BaselineComparisonWorkbenchPage.tsx` |
| New list hook | `src/allagma/api/useAllagmaBaselineComparisons.ts` |
| API client list method | `src/allagma/api/allagmaClient.ts` (`listAllagmaBaselineComparisons`) |
| Filter model + URL sync | `src/allagma/filters/allagmaBaselineComparisonFilters.ts` |
| Adapter mapping | `src/allagma/adapters/allagmaEvaluationAdapters.ts` (`mapBaselineComparisonSummaryDto`) |
| Routing | `src/app/routes.tsx` (`/allagma/evaluations/baseline-comparisons`) |
| Dashboard entry-point updates | `src/allagma/components/AllagmaEvalComparisonEntryPanel.tsx`, `src/allagma/pages/EvaluationsOverviewPage.tsx` |
| Detail page cross-link | `src/allagma/pages/BaselineComparisonDetailPage.tsx` |
| E2E mock support | `e2e/helpers/mockServices.ts` |
| Unit + e2e tests | `src/allagma/filters/allagmaBaselineComparisonFilters.test.ts`, `e2e/allagma-eval-dashboards.spec.ts` |

## Validation commands and results

```powershell
cd c:\dev\allagma-dotnet
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj
```

- Result: **662 passed, 0 failed**

```powershell
cd c:\dev\ontogony-frontend
npm run openapi:sync:allagma
npm run openapi:gen
npm run build
npm run test -- src/allagma/filters/allagmaBaselineComparisonFilters.test.ts src/app/routes.test.ts src/app/route-coverage.test.ts
npm run test:e2e -- e2e/allagma-eval-dashboards.spec.ts
npm run format:check
```

- Result: build succeeded; targeted unit tests **9 passed**; dashboard e2e **9 passed**; formatting check passed.

## Remaining limitations (intentional)

- Baseline comparison create remains harness/smoke (`POST /evaluations/baseline-comparisons`) with no operator form.
- Workbench supports filters + cursor, but no saved views/favorites.
- Rich side-by-side diff visualization remains a follow-up depth enhancement.
