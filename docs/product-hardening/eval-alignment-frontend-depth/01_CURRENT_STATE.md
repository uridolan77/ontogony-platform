# 01 — Current State

**Audit:** `PFH-001` (2026-05-19). Evidence: `docs/evidence/PFH_001_CURRENT_STATE_AUDIT_EVIDENCE.md`.

> **Post-closeout (2026-05-19):** Product hardening v1 is **CLOSED**. This file is the pre-implementation audit baseline. For current posture see [`PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md`](../../releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md) and matrices `03`–`08` (refreshed at closeout).

## Closed prerequisites

```text
First Dockerized local working system      CLOSED / PASS
Post-Docker-local hardening                CLOSED / PASS
CI-COST-001                                DONE across six repos
Production readiness                       NOT STARTED
```

Docker-local foundation, trace/correlation proof, Conexus persistence validation, Kanon topology diagnostics, frontend fixture/live/replay/config gates, UI packaging documentation, and path-filtered CI aggregates are in place.

## Repo HEADs at audit (short)

| Repo | HEAD |
| --- | --- |
| `ontogony-platform` | `dc1f37f` |
| `allagma-dotnet` | `22a365d` |
| `kanon-dotnet` | `18e83cc` |
| `conexus-dotnet` | `3bb265e` |
| `ontogony-frontend` | `97d543f` |
| `ontogony-ui` | `c3fef30` |

## What changed since prior alignment packages

| Prior baseline | Current finding |
| --- | --- |
| `docs/alignment/eval-full-sanity-alignment/` (ALIGN-EVAL-001, 2026-05-18) | Still valid for route→client mapping; dashboard still uses run sampling |
| `docs/planning/eval-durability-to-first-sanity-current/` | **Closed** — `FIRST_FULL_SANITY_CLOSEOUT` PASS in Allagma; durable eval persistence + baseline comparison shipped |
| Post-Docker hardening (`FE-HARDEN-001`, `FE-TEST-REPLAY-001`, `TRACE-CONTRACT-001`, etc.) | Frontend operator contracts and cross-service trace proof exist; product depth gaps remain |
| This package (PFH-000) | Registered 2026-05-19; matrices below refreshed by PFH-001 |

## Backend eval surface (Allagma)

Five eval-related routes under `/allagma/v0` (minimal APIs in `Allagma.Api/Program.cs`):

| Method | Path | Status |
| --- | --- | --- |
| GET | `/runs/{runId}/evaluations` | **Exists** — full list per run, no pagination |
| GET | `/evaluations/{evaluationRunId}` | **Exists** |
| POST | `/runs/{runId}/evaluations` | **Exists** — harness/manual; gated `ManualWriteEnabled` + non-prod |
| POST | `/evaluations/baseline-comparisons` | **Exists** — not gated like manual write |
| GET | `/evaluations/baseline-comparisons/{comparisonId}` | **Exists** |

**Missing:** `GET /evaluations` (global list/query), eval export bundle route, cross-run trend API, baseline comparison list/history.

Scenario dataset v0 lives under `allagma-dotnet/docs/evals/datasets/scenario-dataset-v0/` with harness/CI scripts; not a first-class HTTP dataset index.

## Frontend eval surface

| Route | Live data | Fixture |
| --- | --- | --- |
| `/allagma/evaluations` | Samples recent runs + per-run eval lists | `?dashboardFixture=ci-suite` |
| `/allagma/evaluations/:evaluationRunId` | `getAllagmaEvaluationRun` | `dashboardFixture` / `evalFixture` |
| `/allagma/evaluations/baseline-comparisons/:id` | `getAllagmaBaselineComparison` | `dashboardFixture=ci-suite` |
| `/allagma/runs/:runId` (eval panel) | `listAllagmaRunEvaluations` + audit | `evalFixture`, `topologyFixture` |
| `/allagma/replay` | Run + trace + Kanon replay bundles | E2E mocks only (no fixture query param) |

OpenAPI client: `ontogony-frontend/openapi/allagma.v0.json` + `src/allagma/api/generated/schema.ts`; drift gate `npm run openapi:check`.

## Cross-service evidence (Kanon / Conexus)

- **Kanon:** Decision-record + replay-bundle routes under `/ontology/v0/decision-records/*`; topology evaluate emits `decisionId` (Allagma stores as `topologyAuthorizationDecisionId`). Operator docs: `kanon-dotnet/docs/operators/TOPOLOGY_*`.
- **Conexus:** `GET /conexus/v0/model-calls/{modelCallId}`, `GET /admin/v0/route-decisions/{routeDecisionId}`; journal metadata includes `correlation_id`, `allagma_run_id`. Bootstrap/readiness: `docs/deployment/STARTUP_AND_READINESS.md`.

## Ontogony UI

`@ontogony/ui` `0.1.0-alpha.0` — packaging PASS (`UI-PACKAGING-STATUS-001`). Eval-specific widgets (scenario matrix, quality breakdown, baseline cards) live in **ontogony-frontend**; UI package supplies layout, execution, and replay chrome.

## Product-level bottleneck (confirmed)

Infrastructure is not the bottleneck. Gaps are **product depth and contract alignment**:

1. **No global eval list/query** — dashboard samples `GET /runs` then hydrates per-run evals.
2. **Dashboard data model** — tied to sampling; filters/suite/dataset dimensions thin.
3. **Baseline comparison** — GET-by-id only in UI; no history/list/filter product surface.
4. **Scenario datasets** — filesystem/CI dataset strong; HTTP index and operator UI weak.
5. **Quality scoring** — backend scaffold + FE breakdown partial; calibration metadata shallow.
6. **Run detail / replay** — panels exist; not yet integrated evidence workbenches with cross-links.
7. **Export bundle** — no coherent eval evidence export artifact.

## Production-readiness separation

Deferred to separate programs (not PFH):

| Item | Program |
| --- | --- |
| Real provider mode | `ENV-REAL-PROVIDER-001` (optional) |
| Cloud deploy, identity, TLS, DR | `PROD-READINESS-*` |
| Runtime Vite/nginx env injection | Cloud/deployment program |
| Allagma retry/cancel/replay POST mutations | Backend not in OpenAPI snapshot; FE shows limitation |

## Boundary

Do not mix this product-hardening package with production readiness.

## First implementation PR (historical)

**`EVAL-PRODUCT-001`** — Eval query/list contract and dashboard data model (completed; see closeout).
