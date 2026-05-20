# EVIDENCE-SPINE-000 — Cross-service evidence spine current state audit

**Date:** 2026-05-20  
**Scope:** Frontend correlation resolver, page-local evidence journeys, backend lookup routes (Allagma, Conexus, Kanon), test coverage, gap matrix, first implementation slice  
**Evidence index:** [`docs/evidence/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT_EVIDENCE.md`](../evidence/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT_EVIDENCE.md)  
**Package intake:** `docs/_incoming/Ontogony-Cross-Service-Evidence-Spine-Package-v1/`  
**Mode:** Audit only — no product code changes in this step.

## Executive summary

The operator console already has a **trace-centric cross-service resolver** (`resolveTraceCorrelation`), shared UI (`CrossServiceLinksCard`, `TraceCorrelationLookupBar`), and **page-local evidence journeys** (Allagma run/eval/replay, Conexus observability, Kanon provenance). What is missing is a **unified evidence spine**: one paste-any-ID entry point, canonical ID taxonomy, normalized graph, recorded source attempts, and completeness/missing-edge semantics.

**First implementation slice (confirmed):**

1. **EVIDENCE-SPINE-001** — Canonical ID kinds, graph model, parser (`parseEvidenceIdentifier`), graph helpers; platform taxonomy doc. No graph UI yet.
2. **EVIDENCE-SPINE-002** — `src/evidence-spine/resolveEvidenceSpine.ts` wrapping existing clients; extend beyond trace-only inputs (eval ID, baseline comparison ID, route decision ID, correlation ID).

No new backend aggregator is required for v1; existing GET routes cover most lookups. Gaps are primarily **frontend resolver inputs**, **indirect lookups** (human gate by ID, correlation-only roots), and **operator UX consolidation**.

---

## 1. Frontend resolver audit

### 1.1 Core module (`ontogony-frontend/src/system/correlation/`)

| File | Role |
| --- | --- |
| `correlationTypes.ts` | `TraceCorrelationView`, `TraceLookupInput` (4 fields), identifier slots |
| `resolveTraceCorrelation.ts` | Async resolver; calls Allagma, Kanon, Conexus clients |
| `useTraceCorrelation.ts` | React Query wrapper; key `queryKeys.system.trace(...)` |
| `correlationAdapters.ts` | Page links, `TraceCorrelationItem` mapping, identifier slot hrefs |
| `enrichTraceCorrelationView.ts` | Merge operator-known hints into resolved view (run detail, etc.) |
| `extractHumanGateId.ts` | Parse `humanGateId` from Allagma run events |
| `buildCorrelationSourceEvidence.ts` | Endpoint/confidence metadata for replay/diagnostics UI |

### 1.2 Lookup inputs (today)

`TraceLookupInput` accepts **only**:

- `traceId`
- `runId` (Allagma run)
- `decisionId` (Kanon decision)
- `modelCallId` (Conexus request / OpenAI-style id)

**Not accepted as resolver roots:** evaluation run ID, baseline comparison ID, route decision ID, correlation ID, human gate ID, dataset/scenario ID.

`TraceCorrelationLookupBar` syncs the same four fields to URL query params (`traceId`, `runId`, `decisionId`, `modelCallId`; legacy alias `requestId` → `modelCallId`).

### 1.3 Resolution algorithm (actual order)

```text
1. modelCallId (no trace yet)
   → GET /admin/v0/diagnostics/execution-runs/by-request-id/{requestId}
   → may set traceId only (does not fetch model call detail or route decision)

2. runId
   → GET /allagma/v0/runs/{runId}
   → sets traceId, modelCallId, planningDecisionId, kanonDecisionId
   → GET /allagma/v0/runs/{runId}/events → humanGateId from payload

3. traceId (if no decisionId yet)
   → GET /ontology/v0/decision-records/by-trace/{traceId} (first decision only)

4. decisionId
   → GET /ontology/v0/decision-records/{decisionId}
   → may set traceId, correlationId, allagmaRunId, planningDecisionId

5. traceId (if no runId yet)
   → GET /allagma/v0/runs?traceId= (limit 5, prefer exact trace match)
   → GET /allagma/v0/runs/{runId} + events if match

6. planningDecisionId fallback
   → GET /ontology/v0/decision-records/{planningDecisionId}

7. buildCorrelationLinks + detail message
```

**Semantics:**

- `404` from Allagma/Kanon/Conexus (where wrapped in `safe()`) → **unresolved**, not thrown.
- `500` and other errors → **propagate** to UI.
- **Model-call-only lookup** does not resolve run or decision unless Conexus execution run returns `traceId` and downstream steps succeed; unit test explicitly guards against false “resolved” claims.
- `actionEvaluationDecisionId` slot exists in types but is **always empty**; `buildCorrelationSourceEvidence` marks it unavailable (no OpenAPI lookup).
- `correlationId` is populated from Kanon decision when found; there is **no correlationId-first lookup path**.

### 1.4 Cross-service UI

| Component | Location | Behavior |
| --- | --- | --- |
| `CrossServiceLinksCard` | `src/shared/components/CrossServiceLinksCard.tsx` | Renders identifier slots with links; used on run detail and similar |
| `TraceCorrelationLookupBar` | `src/shared/components/TraceCorrelationLookupBar.tsx` | Four-field lookup + URL sync |
| `LiveTraceCorrelationPanel` | `src/shared/components/LiveTraceCorrelationPanel.tsx` | Wraps `useTraceCorrelation` for workbenches |
| System overview | `src/system/pages/SystemOverviewPage.tsx` | Global correlation lookup + health “trace bridge” |
| Conexus observability | `src/conexus/components/ConexusRequestObservabilityWorkbench.tsx` | Lookup bar + model call detail + route decision panel (`routeDecisionId` query param only) + correlation panel |
| Run detail | `src/allagma/pages/RunDetailPage.tsx` | `enrichTraceCorrelationView` + `CrossServiceLinksCard` |
| Replay evidence | `src/allagma/pages/ReplayEvidencePage.tsx` | Correlation source evidence + journey section |

### 1.5 Page-local evidence journeys (not unified)

| Builder | Path | Scope |
| --- | --- | --- |
| `buildRunEvidenceJourneyLinks` | `src/allagma/adapters/buildRunEvidenceJourneyLinks.ts` | Run → eval dashboard, eval detail, baseline, dataset, Conexus observability, route decision (inferred), Kanon decision, replay |
| `buildEvalRunEvidenceJourneyLinks` | same file | Eval → subject run, baseline, dataset, Conexus, Kanon, replay |
| `buildReplayEvidenceJourneyLinks` | `src/allagma/adapters/buildReplayEvidenceJourneyLinks.ts` | Run journey minus replay slot |
| `mapEvidenceLinksToSpineView` | `src/conexus/adapters/conexusEvidenceSpineAdapters.ts` | Conexus `evidence-links` API + correlation merge |
| `buildKanonProvenanceEvidence` | `src/kanon/diagnostics/buildKanonProvenanceEvidence.ts` | Redacted provenance export bundle |

**Route decision linking:** `inferRouteDecisionIdFromModelCallId` only works for `chatcmpl-{requestId}` shaped model call IDs; otherwise journey slot is unavailable.

**Eval/baseline roots:** Journey links are built **from page context** (run loaded, eval list), not from a shared resolver entry point.

### 1.6 Export / diagnostics (today)

| Export | Mechanism |
| --- | --- |
| Operator diagnostic bundle | `buildOperatorDiagnosticBundle` — includes correlation fields |
| Allagma eval evidence | `GET /allagma/v0/evaluations/{id}/evidence` via `getAllagmaEvalEvidenceExportBundle` |
| Allagma run audit | `GET /allagma/v0/runs/{id}/audit` |
| Kanon provenance | `buildKanonProvenanceEvidence` + replay bundle APIs |
| Conexus | `DiagnosticExportButton` on observability workbench; model-call evidence-links API |

**Gap:** No unified cross-service evidence export bundle (package EVIDENCE-SPINE-007).

---

## 2. Backend lookup route audit

### 2.1 Allagma (`/allagma/v0`)

| ID kind | Direct lookup | Indirect lookup | Notes |
| --- | --- | --- | --- |
| `allagmaRunId` | `GET /runs/{runId}` | — | Also list filters: status, actor, `waitingForHumanGate`, `traceId`, `correlationId` |
| `allagmaEvaluationRunId` | `GET /evaluations/{evaluationRunId}` | `GET /runs/{runId}/evaluations`, `GET /evaluations?runId=` | List also: `traceId`, `baselineComparisonId`, `correlationId` |
| `baselineComparisonId` | `GET /evaluations/baseline-comparisons/{comparisonId}` | `GET /evaluations?baselineComparisonId=`, list comparisons | |
| `traceId` | `GET /runs?traceId=` | Eval list `?traceId=` | Run list summaries omit `traceId` in response — detail GET needed |
| `correlationId` | `GET /runs?correlationId=` | `GET /evaluations?correlationId=` | Not exposed on run GET body in all paths; often from Kanon decision |
| `humanGateId` | **None** | `GET /runs/{runId}/events` (payload scan) | `waitingForHumanGate=true` on run list for gate **workbench**, not by gate ID |
| `conexusModelCallId` | — | On run detail: `modelCallId` field | |
| `kanonDecisionId` / `planningDecisionId` | — | On run detail + audit references | |

Additional: `GET /runs/{runId}/audit`, `GET /evaluations/{id}/evidence`, `POST /runs/{runId}/resume`.

### 2.2 Conexus (`/admin/v0`, `/v1`)

| ID kind | Direct lookup | Indirect lookup | Notes |
| --- | --- | --- | --- |
| `conexusModelCallId` | `GET /model-calls/{modelCallId}` | `GET /model-calls?modelCallId=` | Admin scope |
| Request id (OpenAI id) | `GET /diagnostics/execution-runs/by-request-id/{requestId}` | Journal id `chat-{requestId}` | Used by trace resolver |
| `conexusRouteDecisionId` | `GET /route-decisions/{routeDecisionId}` | `GET /model-calls?routeDecisionId=` | Frontend client: `readConexusRouteDecisionDetail` |
| `traceId` | `GET /model-calls?traceId=` | Execution run metadata | |
| `correlationId` | `GET /model-calls?correlationId=` | Execution journal metadata | |

Additional: `GET /model-calls/{id}/evidence-links`, `GET /diagnostics/summary`. Project-scoped `GET /conexus/v0/model-calls/{modelCallId}` exists but operator UI uses admin routes.

### 2.3 Kanon (`/ontology/v0`)

| ID kind | Direct lookup | Indirect lookup | Notes |
| --- | --- | --- | --- |
| `kanonDecisionId` | `GET /decision-records/{decisionId}` | — | |
| `traceId` | `GET /decision-records/by-trace/{traceId}` | Returns **list**; resolver uses **first** only | |
| Entity ref | `GET /decision-records/by-entity/{entityRef}` | Provenance workbench | Not used in trace resolver |
| Provenance | `GET /decision-records/{id}/provenance` | — | |
| Replay bundles | `GET /decision-records/{id}/replay-bundles` (+ export) | — | Surfaced in correlation source evidence |
| `humanGateId` | `POST /actions/human-gates/{id}/resolve` | **Mutation**, not evidence lookup | Gates page uses Kanon resolve + Allagma resume |

### 2.4 Platform

| ID kind | Direct lookup | Notes |
| --- | --- | --- |
| `traceId` / `correlationId` | Propagation only (`Ontogony.Observability`, HTTP middleware) | No `GET /ontogony/v0/evidence/resolve` |

---

## 3. Gap matrix

| ID kind | Owner | Direct API | Indirect API | Frontend client | UI link | Export | Gap | Owner (fix) |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| `traceId` | Platform | Partial (per-service filters) | Via resolver chain | Yes (`resolveTraceCorrelation`) | Yes | Partial | No single graph; Kanon returns many decisions, resolver takes first | frontend EVIDENCE-SPINE-002 |
| `correlationId` | Platform | Allagma runs/evals list; Conexus model-calls list | Kanon decision field | Partial (eval list only; **not** in `listAllagmaRuns` client) | Slot display only | Partial | **No correlation-first resolver**; run list client omits `correlationId` query param | frontend 002; optional client fix |
| `allagmaRunId` | Allagma | Yes | — | Yes | Yes | Audit bundle | Not a root in unified spine yet | frontend 001–002 |
| `allagmaEvaluationRunId` | Allagma | Yes | By run | Yes | Journey pages | Eval evidence GET | **Not in trace resolver** | frontend 002 |
| `baselineComparisonId` | Allagma | Yes | Eval list filter | Yes | Journey | — | **Not in trace resolver** | frontend 002 |
| `conexusModelCallId` | Conexus | Yes | Run detail, execution-run | Yes | Observability URL | Diagnostics | Model-call-only weak chain | frontend 002 |
| `conexusRouteDecisionId` | Conexus | Yes | Model call list filter | Yes | URL param + journey infer | — | **Not in trace resolver**; infer only `chatcmpl-*` | frontend 002 |
| `kanonDecisionId` | Kanon | Yes | By trace (first) | Yes | Yes | Provenance export | Multiple decisions per trace collapsed | frontend 002 |
| `planningDecisionId` | Kanon/Allagma | Same as decision | Run detail | Yes (slot) | Yes | Replay bundles | Often aliased to `kanonDecisionId` in resolver | frontend 001 taxonomy |
| `humanGateId` | Allagma/Kanon | **No GET by gate ID** | Run events | Extract only | Gates list (filtered runs) | — | **No reverse lookup** from gate ID alone | frontend 002 best-effort; optional Allagma API later |
| `actionEvaluationDecisionId` | Kanon | **No** | — | Slot only | — | — | Documented unavailable | Kanon contract / frontend when exposed |
| `datasetId` / `scenarioId` | Allagma eval | Dataset GET; eval filters | — | Yes (eval APIs) | Journey | Eval export | Not resolver roots | frontend 002+ (low priority) |
| Unified export | — | **No** | — | — | — | — | High gap | EVIDENCE-SPINE-007 |

---

## 4. Test audit

### 4.1 Unit tests (present)

| Area | File | Coverage notes |
| --- | --- | --- |
| Resolver | `resolveTraceCorrelation.test.ts` | Run, trace, decision, human gate via events, model-call-only, 404 swallow, 500 propagate |
| Adapters | `correlationAdapters.test.ts` | `TraceCorrelationItem`, `hasResolvedCorrelation` |
| Enrich | `enrichTraceCorrelationView.test.ts` | Hint merge |
| Source evidence | `buildCorrelationSourceEvidence.test.ts` | Endpoint/confidence labeling |
| Human gate extract | `extractHumanGateId.test.ts` | Event payload |
| Evidence journey | `buildRunEvidenceJourneyLinks.test.ts` | Slot availability |
| Cross-service card | `CrossServiceLinksCard.test.tsx` | Slot rendering |
| Conexus spine adapter | `conexusEvidenceSpineAdapters.test.ts` | Evidence-links mapping |
| Kanon provenance export | `buildKanonProvenanceEvidence.test.ts` | Redaction bundle |
| Diagnostics | `buildOperatorDiagnosticBundle` (via shared) | Correlation fields |

### 4.2 E2E tests (present)

| Spec | Covers |
| --- | --- |
| `e2e/correlation.spec.ts` | System overview run/trace lookup, trace bridge health |
| `e2e/trace-url-sync.spec.ts` | URL ↔ lookup field sync |
| `e2e/keyboard.spec.ts` | Enter submits correlation lookup |
| `e2e/allagma-run-detail-correlation.spec.ts` | Cross-service card on run detail |
| `e2e/allagma-replay-evidence.spec.ts` | Replay correlation source + journey slots |
| `e2e/allagma-run-detail-evidence-journey.spec.ts` | Journey card navigation |
| `e2e/conexus-observability.spec.ts` | Model call detail + trace display |
| `e2e/diagnostic-export.spec.ts` | Export action (system) |

### 4.3 E2E / browser gaps

| Gap | Risk |
| --- | --- |
| Eval ID as sole lookup input | High — common operator id |
| Baseline comparison ID lookup | Medium |
| Route decision ID → model call / run / decision chain | Medium |
| Correlation ID lookup | Medium |
| Model-call-only → full cross-service resolution | Medium (known weak; needs E2E when 002 improves) |
| Human gate ID paste → run | Medium |
| Unified evidence spine workbench page | High (post-002) |
| Live Docker six-service resolution | Deferred to EVIDENCE-SPINE-008 |

---

## 5. Target vs current (package alignment)

Package target (`01_EXECUTIVE_BRIEF.md`): paste any ID → graph, completeness, page links, export, missing-edge reasons, source attempt log.

| Layer | Current | Next |
| --- | --- | --- |
| Identifier taxonomy | Implicit in `correlationTypes` + package draft `04_IDENTIFIER_TAXONOMY.md` | EVIDENCE-SPINE-001 |
| Resolver contract | `resolveTraceCorrelation` (4 inputs, flat view) | `resolveEvidenceSpine` + source attempts |
| Service adapters | Inline in resolver + page builders | `evidenceSpineAdapters/*` |
| Graph UI | None | EVIDENCE-SPINE-006 |
| Unified export | Per-service only | EVIDENCE-SPINE-007 |

Recommended module layout already matches package `03_TARGET_ARCHITECTURE.md` (`src/evidence-spine/`).

---

## 6. First implementation slice (confirmed)

### Phase A — EVIDENCE-SPINE-001 (contract only)

- Add `EvidenceIdentifierKind`, `EvidenceGraph`, `EvidenceNode`, `EvidenceEdge`, `EvidenceSourceAttempt`, `EvidenceResolutionResult`.
- Implement `parseEvidenceIdentifier(raw, override?)` without prefix-only dependence.
- Graph helpers: `addNode`, `addEdge`, `mergeIdentifiers`, `markUnresolvedEdge`, `buildPageLinks`.
- Platform doc: `docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`.
- Unit tests for parser and graph merge.

**Repos:** `ontogony-frontend`, `ontogony-platform` (docs).

### Phase B — EVIDENCE-SPINE-002 (resolver v1)

- Implement `resolveEvidenceSpine.ts` using existing clients.
- **Reuse** `resolveTraceCorrelation` for trace/run/decision/model-call subgraph, but add parallel roots:
  - `getAllagmaEvaluationRun`, `getAllagmaBaselineComparison`
  - `readConexusModelCallDetail`, `readConexusRouteDecisionDetail`, `listConexusModelCalls` (filtered)
  - `listAllagmaRuns` / `listAllagmaEvaluations` with `correlationId` when added to client
- Record every `EvidenceSourceAttempt` (success / not_found / error / skipped).
- Do **not** require new backend routes unless audit during 002 proves a blocker (none identified in 000).

**Repos:** `ontogony-frontend`, evidence docs in both frontend and platform.

### Explicitly defer

- Graph workbench UI (006), unified export bundle (007), E2E Docker proof (008), Allagma evidence normalization pass (003) can follow 002.

---

## 7. Related audits and contracts

| Doc | Relevance |
| --- | --- |
| `docs/operators/TRACE_CORRELATION_CONTRACT.md` | Prior trace/correlation operator contract |
| `docs/evidence/TRACE_CONTRACT_001_EVIDENCE.md` | Local stack trace proof |
| `docs/reviews/CONEXUS_DEEPEN_000_CURRENT_STATE_AUDIT.md` | Conexus list/detail gaps (partially addressed since audit) |
| Package `04_IDENTIFIER_TAXONOMY.md`, `06_API_CONTRACT_GAP_MATRIX.md` | Intake baseline; this audit validates against live code |

---

## 8. Acceptance checklist (EVIDENCE-SPINE-000)

- [x] Current resolver behavior documented (algorithm, inputs, error semantics)
- [x] ID taxonomy gaps listed with owners
- [x] Backend routes inventoried per service
- [x] Gap matrix completed
- [x] Test coverage and E2E gaps identified
- [x] First implementation slice confirmed: **001 → 002**
