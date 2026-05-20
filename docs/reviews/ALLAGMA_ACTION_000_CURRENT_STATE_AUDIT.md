# ALLAGMA-ACTION-000 — Current state audit

**Date:** 2026-05-20  
**Scope:** Allagma governed execution — operator action contracts, OpenAPI alignment, frontend wiring  
**Repos reviewed:** `allagma-dotnet`, `ontogony-frontend`, `ontogony-platform` (local stack cross-cut)  
**Evidence index:** [`docs/evidence/ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT_EVIDENCE.md`](../evidence/ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT_EVIDENCE.md)  
**Package intake:** `allagma-dotnet/docs/_incoming/Ontogony-Allagma-Actionability-Workbench-Package-v1/`  
**Mode:** Audit only — no runtime behavior change in this step.

## Executive summary

Allagma already exposes **six operator-actionable HTTP contracts** (start run, resume, manual evaluation write, baseline comparison create, run audit export, eval evidence export) plus extensive **read surfaces** for runs, events, capabilities, and evaluation catalogs. The operator UI is **read-heavy and partially actionable**: resume (run detail + human gates), eval evidence export, and **start run** (`/allagma/runs/start`, ALLAGMA-ACTION-001) are wired; baseline compare create, manual evaluation create, and dedicated run-audit copy/download actions remain **not operator-grade** despite existing backend routes.

**Unsupported as HTTP routes today:** retry run, cancel run, live replay trigger, deny human gate (Kanon-only), bulk export, promote baseline/eval.

**Confirmed next implementation order (roadmap Phase B):**

```text
ALLAGMA-ACTION-001 — Start run workbench
ALLAGMA-ACTION-002 — Human-gate resume workbench (complete)
ALLAGMA-ACTION-003 — Baseline compare actions
ALLAGMA-ACTION-004 — Run audit and evidence actions
```

Phase C (`ALLAGMA-ACTION-005`) designed retry/cancel/replay — see `allagma-dotnet/docs/architecture/RUN_OPERATIONS_CONTRACT_DESIGN.md`. Phase D (`ALLAGMA-ACTION-006`) implements accepted v2 routes.

---

## 1. Backend route inventory

**Authoritative map:** `allagma-dotnet/src/Allagma.Api/Program.cs` (`MapGroup("/allagma/v0")`, all routes require bearer service token auth).

**Committed OpenAPI:** `allagma-dotnet/docs/api/allagma-openapi-v1.snapshot.json` (provenance: `allagma-openapi-v1.provenance.json`, drift tests: `tests/Allagma.Tests/AllagmaOpenApiSnapshotTests.cs`).

| Method | Route | Operation (OpenAPI) | Purpose |
| --- | --- | --- | --- |
| GET | `/allagma/v0/runs` | `listRuns` | Paginated run list; filters: status, `waitingForHumanGate`, traceId, actorId, cursor |
| POST | `/allagma/v0/runs` | `startRun` | Start governed run; optional idempotency header (`AllagmaIdempotencyHeaderReader`) |
| GET | `/allagma/v0/runs/{runId}` | `getRun` | Run detail |
| GET | `/allagma/v0/runs/{runId}/events` | `listRunEvents` | Event stream; optional `includeTopologySummary` |
| GET | `/allagma/v0/capabilities` | `getAllagmaCapabilities` | Sandbox execution + evidence capability metadata (options-driven) |
| GET | `/allagma/v0/runs/{runId}/audit` | `getAgentRunAuditBundle` | Audit bundle export (topology, sandbox evidence, side-effect ledger) |
| POST | `/allagma/v0/runs/{runId}/resume` | `resumeRun` | Resume after human gate; Kanon check; outcomes `resumed` / `denied` / `still_waiting` / `not_waiting` / `not_found` |
| GET | `/allagma/v0/runs/{runId}/evaluations` | `listRunEvaluations` | Evaluations for subject run |
| POST | `/allagma/v0/runs/{runId}/evaluations` | `writeRunEvaluation` | Manual eval write — **403** unless `Allagma:Evaluation:ManualWriteEnabled=true` and non-production |
| GET | `/allagma/v0/evaluations` | `listEvaluationRuns` | Global eval list (filters + cursor) |
| GET | `/allagma/v0/evaluation-datasets` | — | Dataset catalog |
| GET | `/allagma/v0/evaluation-datasets/{datasetId}` | — | Dataset detail |
| GET | `/allagma/v0/evaluations/{evaluationRunId}` | — | Evaluation detail |
| GET | `/allagma/v0/evaluations/{evaluationRunId}/evidence` | `getEvaluationEvidenceExportBundle` | Eval evidence export bundle |
| GET | `/allagma/v0/evaluations/baseline-comparisons` | — | Comparison list (filters include `promotionRecommendation`) |
| GET | `/allagma/v0/evaluations/baseline-comparisons/{comparisonId}` | — | Comparison detail |
| POST | `/allagma/v0/evaluations/baseline-comparisons` | `createBaselineComparison` | Create comparison (baseline + subject run + scenario) |

**Host routes (outside `/allagma/v0`):** `GET /health`, `GET /ready` (Ontogony service defaults).

### 1.1 Prompt checklist routes — contract notes

| Route | OpenAPI | Implementation notes |
| --- | --- | --- |
| POST `/runs` | Yes | Body: `StartAgentRunRequest`; 409 idempotency conflict / in-progress |
| GET `/runs` | Yes | Returns `RunListPage` |
| GET `/runs/{runId}` | Yes | 404 `CrossServiceErrorEnvelope` |
| GET `/runs/{runId}/events` | Yes | 404 if run missing |
| GET `/capabilities` | Yes | Reflects kill switch, sandbox flags, production guard |
| GET `/runs/{runId}/audit` | Yes | 404 if run missing |
| POST `/runs/{runId}/resume` | Yes | 409 `not_waiting`; 502 if Kanon unreachable |
| POST `/runs/{runId}/evaluations` | Yes | Policy-gated manual write |
| POST `/evaluations/baseline-comparisons` | Yes | 400 validation errors |
| GET `/evaluations/{evaluationRunId}/evidence` | Yes | 404 if eval missing |

**OpenAPI ↔ Program alignment:** All 15 `/allagma/v0` path templates in the snapshot match `Program.cs` mappings. No undocumented operator routes found in code.

---

## 2. Unsupported operation inventory

Searched `Allagma.Api/Program.cs`, OpenAPI `paths`, and frontend `allagmaRunOperationsCapability.ts` expected paths.

| Operation | HTTP route | Verdict | Notes |
| --- | --- | --- | --- |
| **Retry run** | `POST /allagma/v0/runs/{runId}/retry` | **Absent** | Not in OpenAPI or Program |
| **Cancel run** | `POST /allagma/v0/runs/{runId}/cancel` | **Absent** | `RunStatus.Cancelled` exists in domain (`RunStatus.cs`) but no transition API |
| **Replay trigger** | `POST /allagma/v0/runs/{runId}/replay` (or similar) | **Absent** | Internal replay-safe ledger + events (`SandboxExecuteReplaySkipped`, Kanon replay bundle recorder); no operator trigger route |
| **Deny human gate** | Allagma deny route | **Absent** | Deny is **Kanon** `resolveKanonHumanGate` then optional `POST .../resume`; Allagma `ResumeAgentRunService` applies deny when Kanon returns deny on resume |
| **Bulk export** | e.g. `POST /allagma/v0/exports` | **Absent** | Per-run audit and per-eval evidence only |
| **Promote baseline/eval** | promote mutation | **Absent** | `promotionRecommendation` is a **computed field** on baseline comparison records and a **list filter** only (`BaselineComparisonService`) |

---

## 3. Frontend action surface inventory

**Router:** `ontogony-frontend/src/app/routes.tsx`  
**Workflow catalog:** `ontogony-frontend/src/app/route-workflow-catalog.json`  
**HTTP client:** `ontogony-frontend/src/allagma/api/allagmaClient.ts`  
**Mutations:** `ontogony-frontend/src/allagma/api/allagmaMutations.ts` (`resumeAllagmaRun`, `useResolveHumanGateAndResume` → Kanon + resume)

| Page | Route | Read clients | Mutations / actions today | Action gaps |
| --- | --- | --- | --- | --- |
| Overview | `/allagma` | `listAllagmaRuns` | Links only; `startRunEnabled: false` | No start-run workbench |
| Runs | `/allagma/runs` | `listAllagmaRuns` | Resume via row/detail patterns (capability-gated) | No start; no cancel/retry |
| Run detail | `/allagma/runs/:runId` | run, events, correlation | `AllagmaRunOperationsPanel` → resume; triage export (client-built JSON) | Audit loaded in sections but no top-level audit **export action**; no start eval/compare |
| Human gates | `/allagma/gates` | waiting runs, events | Approve/deny → Kanon resolve + Allagma resume | Workbench UX partial; deny is not an Allagma route |
| Evaluations | `/allagma/evaluations` | global eval list | Navigation only | No create-eval action |
| Eval detail | `/allagma/evaluations/:evaluationRunId` | eval + evidence bundle | `AllagmaEvalEvidenceExportPanel` copy/download | Strongest export UX |
| Datasets | `/allagma/evaluations/datasets` | dataset list/detail | Read-only | — |
| Baseline workbench | `/allagma/evaluations/baseline-comparisons` | comparison list | Read + drilldown | **No POST** client; copy says harness-only create |
| Baseline detail | `/allagma/evaluations/baseline-comparisons/:comparisonId` | comparison get | Read-only | No promote action (no backend route) |
| Replay & evidence | `/allagma/replay` | run lookup, Kanon replay | Client-side replay evidence export | **No live replay trigger**; lookup/export only |

**Stale doc:** `ontogony-frontend/docs/phase-j-frontend-ui-tightening/backend-waiting/allagma-start-run.md` still says backend-waiting; OpenAPI and snapshot **already document** `POST /allagma/v0/runs`. Gap is **UI + client**, not backend contract.

**Capability modules:**

- `allagmaRunOperationsCapability.ts` — OpenAPI-gated start/resume/retry/cancel/replay metadata; only resume wired in UI.
- `allagmaEvidenceCapability.ts` — audit + events documented; audit consumed by topology/sandbox sections, not a single export CTA on run detail.

---

## 4. Action capability matrix

| Operation | Backend route | OpenAPI | Frontend client | UI entry | Run-state gating | Idempotency | Next slice |
| --- | --- | --- | --- | --- | --- | --- | --- |
| **Start run** | `POST /allagma/v0/runs` | Documented | **Missing** (`startRun` in generated schema only) | Overview limitation banner | N/A (creates run) | Header supported on backend | **ACTION-001** |
| **List / get runs** | GET runs, GET run | Yes | `listAllagmaRuns`, `getAllagmaRun` | Overview, runs, detail | — | — | Existing (add action entry points in 001–004) |
| **List run events** | GET `.../events` | Yes | `listAllagmaRunEvents` | Run detail, gates | — | — | Existing |
| **Resume run** | `POST .../resume` | Yes | `resumeAllagmaRun` | Run detail ops panel; gates page (after Kanon) | Backend: `WaitingForHumanGate` + active pause; UI: `waiting_for_human`, `blocked` | Resume ledger replay per pause id | **ACTION-002** polish |
| **Deny human gate** | Kanon resolve API | N/A (Kanon) | `resolveKanonHumanGate` | Human gates approve/deny | Gate must be open | Kanon decision idempotency | **ACTION-002** (not Allagma route) |
| **Export run audit** | GET `.../audit` | Yes | `getAllagmaRunAuditBundle` | Topology/sandbox sections (implicit) | Any run that exists | Safe GET | **ACTION-004** |
| **Create evaluation** | POST `.../evaluations` | Yes | **Missing** | None | Completed runs typical; backend 403 if disabled/prod | Not on this route | **ACTION-004** (if manual write enabled) |
| **Export eval evidence** | GET `.../evidence` | Yes | `getAllagmaEvalEvidenceExportBundle` | Eval detail panel | Eval must exist | Safe GET | **ACTION-004** (placement on run detail) |
| **Create baseline comparison** | POST `.../baseline-comparisons` | Yes | **Missing** | Workbench copy only | Subject/baseline runs must exist | Not documented on route | **ACTION-003** |
| **Promote baseline/eval** | — | **Absent** | — | — | — | — | **ACTION-005/006** design only |
| **Retry run** | — | **Absent** | — | Limitation list | — | — | **ACTION-005/006** |
| **Cancel run** | — | **Absent** | Domain enum only | Limitation list | — | — | **ACTION-005/006** |
| **Replay trigger** | — | **Absent** | — | Replay page = lookup only | — | Internal ledger replay | **ACTION-005/006** |
| **Bulk export** | — | **Absent** | — | — | — | — | Out of scope v1 |
| **Read capabilities** | GET `/capabilities` | Yes | `getAllagmaCapabilities` | Sandbox sections (indirect) | Environment/options | — | Use for ACTION-001/004 gating |

### 4.1 Run-state gating (backend truth)

Domain statuses (`allagma-dotnet/src/Allagma.Domain/RunStatus.cs`):

```text
Created, Planning, Running,
WaitingForKanon, WaitingForConexus, WaitingForFramework,  (reserved)
WaitingForHumanGate,
Completed, Failed, Denied, Cancelled
```

| Status | Resume (`POST .../resume`) | Manual eval POST | Baseline compare POST | Audit GET |
| --- | --- | --- | --- | --- |
| `WaitingForHumanGate` | Yes (after Kanon allow on resume) | No | No | Yes |
| `Running` / `Planning` | No (`not_waiting`) | No | No | Yes |
| `Completed` | No | Yes (if config) | Yes (harness) | Yes |
| `Failed` / `Denied` | No | Policy-dependent | Yes (harness) | Yes |
| `Cancelled` | No | No | Maybe | Yes |

Frontend maps `WaitingForHumanGate` → `waiting_for_human` (`allagmaExecutionAdapters.ts`).

---

## 5. OpenAPI comparison summary

| Check | Result |
| --- | --- |
| Snapshot path count (`/allagma/v0/*`) | 12 path templates, 15 operations (runs has GET+POST; run evaluations GET+POST; baseline-comparisons GET+POST) |
| Matches `Program.cs` | Yes |
| Frontend generated schema (`ontogony-frontend/src/allagma/api/generated/schema.ts`) | Same path set; mutations `startRun`, `writeRunEvaluation`, `createBaselineComparison`, `resumeRun` declared |
| Drift CI | `AllagmaOpenApiSnapshotTests` asserts audit, eval, baseline, capabilities schemas |
| Missing from snapshot (intentionally) | retry, cancel, replay trigger, promote, bulk export |

---

## 6. Recommended implementation sequence

| Slice | Goal | Primary repos |
| --- | --- | --- |
| **ACTION-001** | `startAllagmaRun` client + operator workbench (form, presets, idempotency key) | `ontogony-frontend`; refresh backend-waiting doc |
| **ACTION-002** | Human-gate workbench polish (queue, deny copy, partial-failure recovery) | `ontogony-frontend` + Kanon client |
| **ACTION-003** | Baseline compare create from run/eval context | `ontogony-frontend` |
| **ACTION-004** | Run audit export CTA; eval create (non-prod); evidence links on run detail | `ontogony-frontend` |
| **ACTION-005** | Design doc for retry/cancel/replay + capability endpoint proposal | `allagma-dotnet` + platform evidence |
| **ACTION-006** | Implement only accepted v2 contracts | `allagma-dotnet` + frontend |
| **ACTION-007** | Manual QA walkthrough | platform evidence + release notes |

**Platform touchpoints for Phase B:** local stack (`docker/local-working-system`) for manual QA; no new generic platform packages required unless shared action-dialog primitives are extracted (prefer `ontogony-ui` / existing `OperatorConfirmDialog`).

---

## 7. Acceptance checklist (ACTION-000)

- [x] Backend route inventory complete  
- [x] OpenAPI comparison documented  
- [x] Frontend action surface inventory complete  
- [x] Action capability matrix complete  
- [x] Unsupported operations explicit  
- [x] Next implementation order confirmed (001 → 004, then 005–007)
