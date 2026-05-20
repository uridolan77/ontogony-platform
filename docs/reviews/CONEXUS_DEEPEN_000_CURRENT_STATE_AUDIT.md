# CONEXUS-DEEPEN-000 — Platform cross-cut (Conexus deepening)

**Date:** 2026-05-20  
**Scope:** Cross-service evidence, trace/correlation, local operator stack, and platform mechanics used by Conexus observability deepening  
**Evidence index:** [`docs/evidence/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT_EVIDENCE.md`](../evidence/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT_EVIDENCE.md)  
**Primary backend audit:** `conexus-dotnet/docs/reviews/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT.md`  
**Mode:** Audit only — no product code changes in this step.

## Executive summary

Platform mechanics for Conexus deepening are **already in place** (execution journal, trace middleware, correlation headers, retention, cohesion scripts). The gap is **product contracts and operator UX** in Conexus + frontend, not new generic infrastructure in `ontogony-platform`.

**CONEXUS-DEEPEN-001 platform impact:** minimal — optional OpenAPI snapshot / backend-waiting contract alignment in `ontogony-frontend`; no new platform package required for the list API itself.

---

## 1. Cross-service identifier spine (current)

| Identifier | Owner | Conexus storage / API | Allagma | Kanon | Frontend resolver |
|---|---|---|---|---|---|
| `modelCallId` | Conexus | `conexus_llm_response.response_id`; `GET /conexus/v0/model-calls/{id}` | Run + eval fields; Conexus client | — | `resolveTraceCorrelation`, observability URLs |
| `routeDecisionId` | Conexus | `conexus_route_decision`; `GET /admin/v0/route-decisions/{id}` | Eval enricher | — | Not on Conexus pages yet |
| `traceId` | Platform propagation | LLM rows + journal | Run + events | Decision records | Primary lookup key |
| `correlationId` | Platform propagation | Journal metadata `correlation_id` | Run context JSON | Decision records | Secondary lookup |
| `allagmaRunId` | Allagma | Journal metadata `allagma_run_id` | Run id | Decision `allagmaRunId` | Links to `/allagma/runs/...` |
| Kanon `decisionId` | Kanon | — | `planningDecisionId` on run | API | Links to `/kanon/decisions` |

**Proven locally:** `docs/evidence/TRACE_CONTRACT_001_EVIDENCE.md`, `docs/operators/TRACE_CORRELATION_CONTRACT.md`, `docker/local-working-system/scripts/inspect-trace-correlation-evidence.ps1`.

---

## 2. Platform packages Conexus already consumes (do not duplicate)

| Package / concern | Conexus usage | Deepening note |
|---|---|---|
| `Ontogony.Observability` | Request tracing middleware | Keep; list/detail APIs log traceId |
| `Ontogony.Execution` | `IExecutionJournal`, run/step/attempt records | Provider attempts for DEEPEN-002 |
| `Ontogony.Idempotency` | Ledger (Postgres or in-memory) | Not operator-facing |
| `Ontogony.Quotas` | Quota ledger rows | Aggregate only today |
| `Ontogony.Artifacts` | Optional raw payload store | Policy-gated; not default UI |
| `Ontogony.Redaction` / `Ontogony.Secrets` | Logging + provider keys | Boundary for DEEPEN exports |

Per `AGENTS.md`: **do not** add generic list/pagination/hash helpers to Conexus when they belong here — only add platform primitives if multiple services need the same **mechanical** contract.

---

## 3. Local working system (operator stack)

| Asset | Relevance to deepening |
|---|---|
| `docker/local-working-system/README.md` | Six-repo compose; Conexus Postgres volume for durable telemetry |
| `POST /admin/v0/dev/bootstrap` | Fake provider path for list/detail manual QA |
| Route/model evidence acceptance | Non-empty `routeDecisionId` on model-calls + admin route-decision fetch |
| Trace scripts | Pre-deepening proof; reuse in CONEXUS-DEEPEN-007 manual QA |

**Postgres vs in-memory:** Docker-local should use Postgres for realistic list pagination; host-only Conexus without `CONEXUS_POSTGRES_CONNECTION_STRING` remains single-process in-memory (see conexus audit §2.2).

---

## 4. Frontend ↔ platform integration points

| Mechanism | Location | Gap for deepening |
|---|---|---|
| Trace correlation resolver | `ontogony-frontend/src/system/correlation/` | Works when ids known; weak for Conexus-first browse |
| Backend-waiting gates | `shared/backend-waiting/backendWaitingContracts.ts` | `conexus-request-list` blocks client until OpenAPI lists route |
| Diagnostic export | `DiagnosticExportButton` | Not Conexus model-call bundle schema (package `07_DATA_RETENTION`) |
| Cohesion / E2E | `e2e/conexus-observability.spec.ts` | Mocked ids; extend after list API |

**UI-heavy findings:** documented in conexus-dotnet audit §3; optional frontend evidence note not duplicated here.

---

## 5. Missing link directions (CONEXUS-DEEPEN-005 preview)

```text
Today:
  Allagma run ──link──► /conexus/observability?modelCallId=
  Kanon decision ──link──► Conexus (trace/modelCall)
  Conexus workbench ──partial──► Allagma/Kanon via correlation panel

Needed:
  Conexus recent list row ──► model-call detail ──► route decision ──► Allagma run + Kanon decision
  Allagma eval panel ──► route decision id (already partially via evidence client)
  Kanon ──► Conexus model call when decision metadata includes gateway ids
```

No platform repo change required for link **URLs** (frontend routes exist). May need **admin detail DTO fields** for stable ids (Conexus repo).

---

## 6. Gap matrix (platform lens)

| Capability | Platform state | Gap | Owner repo |
|---|---|---|---|
| Trace/correlation propagation | Documented + scripted | — | platform (docs/scripts) |
| Execution journal abstraction | Shipped | Expose attempts in **Conexus admin DTO** | conexus-dotnet |
| Recent model-call list | N/A (product) | **Missing** | conexus-dotnet + frontend |
| Evidence bundle schema | Partial (generic export) | Conexus-specific bundle v1 | conexus-dotnet + frontend |
| Cohesion proof | Exists | Re-run after DEEPEN-007 | platform scripts + all repos |

---

## 7. Recommended sequence (platform touchpoints)

| Item | Platform work |
|---|---|
| CONEXUS-DEEPEN-001 | None unless shared pagination DTO is extracted (unlikely needed) |
| CONEXUS-DEEPEN-002 | Confirm journal attempt metadata keys stable for UI |
| CONEXUS-DEEPEN-005 | Update `TRACE_CORRELATION_CONTRACT.md` with Conexus list/detail links |
| CONEXUS-DEEPEN-007 | Run `inspect-trace-correlation-evidence.ps1` + manual checklist `09_MANUAL_QA_CHECKLIST.md` (package) |

---

## 8. Stale platform docs to refresh later

| Doc | Action |
|---|---|
| `docs/product-hardening/eval-alignment-frontend-depth/01_CURRENT_STATE.md` | Add list/detail routes when shipped |
| `docs/operators/TRACE_CORRELATION_CONTRACT.md` | Add Conexus observability list-first flow |
| Incoming cross-service evidence package | Align with CONEXUS-DEEPEN-005 outcomes |

---

## 9. Validation (this audit)

- No runtime code changes in `ontogony-platform`.
- Cross-referenced live Conexus endpoint inventory and frontend correlation resolver.

**Acceptance:** Platform does **not** block CONEXUS-DEEPEN-001; implement new admin routes in `conexus-dotnet` and consume from `ontogony-frontend`.
