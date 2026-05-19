# PFH-001 — Current-state audit evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS** — audit/docs only

**This change is not production readiness.** No real provider keys, secrets, cloud deployment, or unscoped runtime changes.

## Scope

Cross-repo read-only audit per `docs/product-hardening/eval-alignment-frontend-depth/pr-specs/PFH-001_CURRENT_STATE_AUDIT.md`. Updated package matrices `01`, `03`–`08`; no application source changes.

## Six-repo inspection log

| Repo | HEAD | Areas inspected | Docs changed |
| --- | --- | --- | --- |
| `ontogony-platform` | `dc1f37f` | PFH package, alignment, planning, releases, operators, evidence | **yes** (this PR) |
| `allagma-dotnet` | `22a365d` | `Program.cs` eval routes, contracts, OpenAPI, tests, `docs/evidence/*EVAL*`, closeout | no |
| `kanon-dotnet` | `18e83cc` | decision-record routes, topology docs, integration headers, tests | no |
| `conexus-dotnet` | `3bb265e` | model-call/route-decision evidence, journal, bootstrap/readiness docs | no |
| `ontogony-frontend` | `97d543f` | eval pages, hooks, adapters, OpenAPI, Playwright, operator docs | no |
| `ontogony-ui` | `c3fef30` | packaging status, export map, frontend import usage | no |

## Commands run

```powershell
# Repo HEADs
git -C c:\dev\ontogony-platform rev-parse --short HEAD
git -C c:\dev\allagma-dotnet rev-parse --short HEAD
git -C c:\dev\kanon-dotnet rev-parse --short HEAD
git -C c:\dev\conexus-dotnet rev-parse --short HEAD
git -C c:\dev\ontogony-frontend rev-parse --short HEAD
git -C c:\dev\ontogony-ui rev-parse --short HEAD

# Platform evidence inventory
Get-ChildItem c:\dev\ontogony-platform\docs\evidence -Filter '*EVAL*'
Get-ChildItem c:\dev\ontogony-platform\docs\evidence -Filter '*TRACE*'
Get-ChildItem c:\dev\ontogony-platform\docs\evidence -Filter 'FE_*'

# Allagma eval route confirmation (grep/read Program.cs)
# Frontend route/hook mapping (read routes.tsx, allagmaClient.ts, hooks)

# Docs-only guard (platform)
git diff --name-only HEAD -- src/ .github/ docker/ '*.cs' '*.csproj'
```

## Eval route inventory (Allagma)

| Method | Path | Tests | FE client |
| --- | --- | --- | --- |
| GET | `/allagma/v0/runs/{runId}/evaluations` | `AllagmaEvaluationApiTests` | `listAllagmaRunEvaluations` |
| POST | `/allagma/v0/runs/{runId}/evaluations` | same (403 when disabled) | not in operator UI |
| GET | `/allagma/v0/evaluations/{evaluationRunId}` | same | `getAllagmaEvaluationRun` |
| POST | `/allagma/v0/evaluations/baseline-comparisons` | `BaselineComparisonTests` | not in operator UI |
| GET | `/allagma/v0/evaluations/baseline-comparisons/{comparisonId}` | same | `getAllagmaBaselineComparison` |

**Missing:** `GET /allagma/v0/evaluations` (global list/query).

Implementation: `allagma-dotnet/src/Allagma.Api/Program.cs`. OpenAPI: `docs/api/allagma-openapi-v1.snapshot.json`.

## Frontend eval surface inventory

| UI route | Hook(s) | Live API pattern | Fixture |
| --- | --- | --- | --- |
| `/allagma/evaluations` | `useAllagmaEvalDashboard` | `listAllagmaRuns` + per-run eval lists | `dashboardFixture=ci-suite` |
| `/allagma/evaluations/:id` | `useAllagmaEvaluationRun` | `getAllagmaEvaluationRun` | `evalFixture`, `dashboardFixture` |
| `/allagma/evaluations/baseline-comparisons/:id` | `useAllagmaBaselineComparison` | `getAllagmaBaselineComparison` | `dashboardFixture=ci-suite` |
| `/allagma/runs/:runId` | `useAllagmaRunEvaluations` | per-run eval list | `evalFixture` |
| `/allagma/replay` | `useAllagmaRun`, `useTraceCorrelation`, `useKanonReplayBundles` | cross-service | E2E mocks |

Pages: `ontogony-frontend/src/allagma/pages/`. OpenAPI: `openapi/allagma.v0.json`.

## Product gap priority list

| Priority | Gap | Next PR |
| --- | --- | --- |
| P0 | No global eval list/query; dashboard sampling | `EVAL-PRODUCT-001` |
| P0 | Contract-first dashboard data model | `EVAL-PRODUCT-001` |
| P1 | Baseline history/list/filter | `EVAL-PRODUCT-002` |
| P1 | Scenario dataset HTTP/UI index | `EVAL-PRODUCT-003` |
| P1 | Quality scoring / calibration depth | `EVAL-PRODUCT-004` |
| P1 | Run detail evidence workbench | `FE-PRODUCT-002` |
| P2 | Replay workbench cross-links | `FE-PRODUCT-003` |
| P2 | Eval evidence export bundle | `EVAL-PRODUCT-005` |

## Production-readiness separation

| Item | Disposition |
| --- | --- |
| Real provider keys / cost | `ENV-REAL-PROVIDER-001` — out of PFH |
| Cloud deploy, identity, TLS, DR | `PROD-READINESS-*` — out of PFH |
| Runtime Vite/nginx injection | deployment program — out of PFH |
| Allagma retry/cancel/replay POST | not in OpenAPI; FE limitation — deferred |
| Branch protection for aggregate CI | optional ops follow-up — out of PFH |

## First implementation PR (confirmed)

```text
EVAL-PRODUCT-001 — Eval query/list contract and dashboard data model
```

Rationale: `02_PRODUCT_HARDENING_SEQUENCE.md` — backend semantics first; audit confirms global list is the blocking product gap.

## Deliverables updated

| File | Status |
| --- | --- |
| `01_CURRENT_STATE.md` | updated |
| `03_EVAL_PRODUCT_GAP_MATRIX.md` | updated |
| `04_BACKEND_FRONTEND_ALIGNMENT_GAP_MATRIX.md` | updated |
| `05_FRONTEND_OPERATOR_DEPTH_GAP_MATRIX.md` | updated |
| `06_CONTRACT_AND_OPENAPI_STATE.md` | updated |
| `07_TEST_AND_EVIDENCE_STATE.md` | updated |
| `08_KNOWN_LIMITATIONS.md` | updated |
| `PFH_001_CURRENT_STATE_AUDIT_EVIDENCE.md` | added (this file) |

## Validation checklist

| Check | Result |
| --- | --- |
| All six repos inspected | **PASS** |
| Eval routes inventoried | **PASS** |
| Frontend eval surfaces inventoried | **PASS** |
| Product vs production gaps separated | **PASS** |
| First PR = `EVAL-PRODUCT-001` | **PASS** |
| Runtime source changes | **none** |
| Workflow changes | **none** |
| Secrets committed | **none** |

## Limitations

- No `npm run openapi:check` or `dotnet test` executed in this audit PR (read-only doc pass).
- HEAD pins are point-in-time; not re-verified on merge.
- Cross-repo code paths cited from inspection; implementers should re-read before `EVAL-PRODUCT-001`.

## Required statement

```text
PFH-001 records current product-hardening state across six repos.
This is not production readiness, real provider mode, or cloud deployment.
```

## Next implementation item

**EVAL-PRODUCT-001** — `pr-specs/EVAL-PRODUCT-001_EVAL_QUERY_LIST_CONTRACT.md`.
