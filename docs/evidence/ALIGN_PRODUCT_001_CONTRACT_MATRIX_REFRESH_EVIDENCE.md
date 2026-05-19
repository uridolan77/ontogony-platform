# ALIGN-PRODUCT-001 — Contract matrix refresh evidence

**Date:** 2026-05-19  
**Verdict:** PASS

## Not production readiness

Product hardening and contract alignment only. Does not assert production readiness, real provider mode, cloud deployment, identity/TLS/secrets, or staging SLOs.

## Goal

Refresh backend/frontend route-to-UI matrix after **EVAL-PRODUCT-001** (`GET /allagma/v0/evaluations`). Fix direct UI/e2e/mock mismatches; document remaining gaps honestly.

## Deliverables

| Artifact | Location |
| --- | --- |
| Alignment gap matrix (refreshed) | `docs/product-hardening/eval-alignment-frontend-depth/04_BACKEND_FRONTEND_ALIGNMENT_GAP_MATRIX.md` |
| Frontend consumption matrix | `docs/alignment/eval-full-sanity-alignment/02_FRONTEND_CONSUMPTION_MATRIX.md` |
| Capability/limitation matrix | `docs/alignment/eval-full-sanity-alignment/05_CAPABILITY_LIMITATION_MATRIX.md` |
| Backend contract snapshot | `docs/alignment/eval-full-sanity-alignment/01_BACKEND_CONTRACT_SNAPSHOT.md` |
| Known limitations / gap matrix updates | `08_KNOWN_LIMITATIONS.md`, `03_EVAL_PRODUCT_GAP_MATRIX.md`, `07_TEST_AND_EVIDENCE_STATE.md` |
| Fixture/live boundary (FE) | `ontogony-frontend/docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md`, `scripts/fixture-live-audit-catalog.json` |

## Direct mismatches fixed (code)

| Area | Change |
| --- | --- |
| Eval dashboard copy | `EvaluationsOverviewPage`, trend/scenario panels — global list messaging (not “no global list API”) |
| E2E mocks | `GET /allagma/v0/evaluations` in `e2e/helpers/mockServices.ts` |
| OpenAPI catalog | `GET /allagma/v0/evaluations` in `openApiSnapshotCatalog.ts` |
| Provenance drift | `allagma-openapi-v1.provenance.json` SHA256 refreshed |

## Alignment summary (post-refresh)

| Status | Surfaces |
| --- | --- |
| **aligned** | Global eval list, eval detail, per-run evals, baseline comparison read, quality on detail |
| **partial** | Replay (cross-service), trace correlation, Conexus route evidence, POST baseline (no UI), list filters |
| **missing** | Baseline list, dataset HTTP index, eval export bundle |
| **deferred** | Run retry/cancel/replay POST mutations |

## Manual write gate (documented)

- `POST /runs/{runId}/evaluations` — gated (`ManualWriteEnabled`, non-production).
- `POST /evaluations/baseline-comparisons` — not gated; harness/smoke only. No operator UI for either.

## Repo heads at refresh

| Repo | HEAD |
| --- | --- |
| `allagma-dotnet` | `3427333be9580d26afacaa353f54245dcb7102c7` |
| `ontogony-frontend` | `c5cab5c9249a515907c449608ac5e4da7394911f` |
| `ontogony-platform` | `99517bf8dadfd54c5e510071c85745b48489e3cb` |
| `kanon-dotnet` | `18e83cc5da5eb6f28c8141da972ba918b9b004bc` |
| `conexus-dotnet` | `3bb265e2519b47e514e3a1ac3a3c5faaab870f46` |

## Commands run

```powershell
cd c:\dev\ontogony-frontend
npm run openapi:check
npm run test -- src/allagma/adapters/allagmaEvaluationAdapters.test.ts src/allagma/adapters/allagmaEvaluationDashboardAdapters.test.ts src/shared/capability/openApiSnapshotCatalog.test.ts
npm run fixtures:audit

cd c:\dev\allagma-dotnet
.\scripts\update-allagma-openapi-provenance.ps1
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter "FullyQualifiedName~AllagmaOpenApiSnapshot|FullyQualifiedName~AllagmaEvaluationApi"
```

## Results

| Gate | Result |
| --- | --- |
| `openapi:check` (frontend) | **pass** |
| Allagma OpenAPI + eval API tests | **17 passed** |
| Frontend eval adapter + catalog tests | **12 passed** |
| `fixtures:audit` | **pass** (regenerated `FE_FIXTURE_LIVE_BOUNDARY_AUDIT.md`) |

## Intentional remaining gaps (next PRs)

- `FE-PRODUCT-001` — dashboard v2 filters, suite/dataset dimensions
- `EVAL-PRODUCT-002` — baseline comparison list/history
- `EVAL-PRODUCT-003` — scenario dataset HTTP/UI index
- `EVAL-PRODUCT-005` — eval evidence export bundle
- `FE-PRODUCT-003` — replay workbench fixture depth

## Next step

**FE-PRODUCT-001** — eval dashboard v2 (see `FE_PRODUCT_001_EVAL_DASHBOARD_V2_EVIDENCE.md`).
