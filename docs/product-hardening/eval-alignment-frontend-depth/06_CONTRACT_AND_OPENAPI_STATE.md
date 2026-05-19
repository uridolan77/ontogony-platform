# 06 — Contract and OpenAPI State

**Audit:** PFH-001 (2026-05-19).

## Contract-first rule

```text
Route/DTO semantics → OpenAPI snapshot → generated client → product wrapper/hook → adapter contract → UI → tests/evidence
```

## Per-service snapshot status

| Service | Snapshot path | Provenance | Frontend client | Drift check | PFH relevance |
| --- | --- | --- | --- | --- | --- |
| **Allagma** | `allagma-dotnet/docs/api/allagma-openapi-v1.snapshot.json` | `allagma-openapi-v1.provenance.json` | `ontogony-frontend/openapi/allagma.v0.json` + `src/allagma/api/generated/schema.ts` | FE: `npm run openapi:check`; BE: `AllagmaOpenApiSnapshotTests` | **Primary** — eval routes L182–299 approx. |
| **Kanon** | `kanon-dotnet/docs/api/kanon-openapi-v1.json` | (document name v1; routes `/ontology/v0`) | `ontogony-frontend` kanon client + generated types | Kanon `OpenApiDocumentTests` | Decision records, replay bundles, topology evaluate |
| **Conexus** | OpenAPI in Conexus repo/tests | Provider contracts | Conexus client in frontend | Conexus API tests | Model-call evidence, admin route-decision |

## Allagma eval paths in snapshot (confirmed)

| Method | Path | In FE client |
| --- | --- | --- |
| GET | `/allagma/v0/evaluations` | `listAllagmaEvaluations` (EVAL-PRODUCT-001) |
| GET | `/allagma/v0/runs/{runId}/evaluations` | `listAllagmaRunEvaluations` |
| GET | `/allagma/v0/evaluations/{evaluationRunId}` | `getAllagmaEvaluationRun` |
| POST | `/allagma/v0/runs/{runId}/evaluations` | not in operator UI |
| POST | `/allagma/v0/evaluations/baseline-comparisons` | not in operator UI |
| GET | `/allagma/v0/evaluations/baseline-comparisons/{comparisonId}` | `getAllagmaBaselineComparison` |

**Added (EVAL-PRODUCT-001):** `GET /allagma/v0/evaluations` — `AllagmaEvaluationRunSummaryResponse`, `AllagmaEvaluationRunListPageResponse`.

## Provenance cross-repo (baseline)

| Artifact | Location |
| --- | --- |
| Backend snapshot SHA | `allagma-dotnet/docs/api/allagma-openapi-v1.provenance.json` |
| FE provenance sidecar | `ontogony-frontend/docs/openapi/ALLAGMA_SNAPSHOT_PROVENANCE.json` |
| Alignment doc | `docs/alignment/eval-full-sanity-alignment/04_OPENAPI_PROVENANCE.md` |
| Sandbox alignment | `docs/alignment/backend-frontend-phase-v2-sandbox-evidence-alignment/05_compatibility_and_ci/01_OPENAPI_PROVENANCE_PLAN.md` |

## Commands

```powershell
# Frontend (from ontogony-frontend)
npm run openapi:check
npm run openapi:gen    # after snapshot sync

# Allagma (from allagma-dotnet)
dotnet test tests/Allagma.Tests/AllagmaOpenApiSnapshotTests.cs
```

## Missing route policy

If a product capability is desired but no backend route exists, the PR must choose one:

- add backend route and OpenAPI snapshot
- explicitly mark as missing capability and expose limitation state
- defer the capability and remove UI affordance

**Current honest limitations:** baseline list, eval export, sparse metadata filters on global eval list, Allagma replay/retry/cancel POST mutations.

No hidden placeholder success states.
