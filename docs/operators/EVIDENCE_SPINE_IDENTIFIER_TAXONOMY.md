# Evidence spine identifier taxonomy

**Status:** v1 contract (EVIDENCE-SPINE-001); system index **SYS-TIGHT-002**  
**Canonical index:** [`SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](./SYSTEM_EVIDENCE_SPINE_CONTRACT.md)  
**Machine routes:** [`system-evidence-spine-resolution.matrix.json`](../system/system-evidence-spine-resolution.matrix.json)  
**Implementation:** `ontogony-frontend/src/evidence-spine/`  
**Related:** [`TRACE_CORRELATION_CONTRACT.md`](./TRACE_CORRELATION_CONTRACT.md), [`docs/reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md`](../reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md)

## Purpose

Operators paste one identifier and expect the console to resolve the governed execution chain across Allagma, Conexus, and Kanon. This document is the **canonical ID taxonomy** for that resolver.

## Identifier kinds

| Kind | Examples | Owner | Primary lookup |
| --- | --- | --- | --- |
| `traceId` | `trace_…`, W3C-style opaque strings | Platform / cross-service | Per-service filters + Kanon `by-trace` |
| `correlationId` | `corr_…` | Platform / cross-service | Allagma run/eval lists; Conexus model-call list |
| `allagmaRunId` | `run_…` | Allagma | `GET /allagma/v0/runs/{runId}` |
| `allagmaEvaluationRunId` | `eval_…` | Allagma | `GET /allagma/v0/evaluations/{evaluationRunId}` |
| `baselineComparisonId` | `baseline_…`, `cmp_…` | Allagma | `GET /allagma/v0/evaluations/baseline-comparisons/{id}` |
| `conexusModelCallId` | `chatcmpl-…` | Conexus | `GET /admin/v0/model-calls/{id}`; execution-run by request id |
| `conexusRouteDecisionId` | `rd-…` | Conexus | `GET /admin/v0/route-decisions/{id}` |
| `kanonDecisionId` | decision record id | Kanon | `GET /ontology/v0/decision-records/{decisionId}` |
| `planningDecisionId` | planning decision id | Kanon / Allagma | Same store as decision records |
| `humanGateId` | `gate_…`, `pause_…` | Allagma / Kanon | Allagma run events (no direct GET by gate id today) |
| `datasetId` | `scenario-dataset-v0` | Allagma eval | `GET /allagma/v0/evaluation-datasets/{datasetId}` |
| `scenarioId` | `scenario-risk-summary-v0` | Allagma eval | Eval list filters |

## Classification rules

1. **Never rely on prefix alone.** `parseEvidenceIdentifier` uses shape hints plus optional manual override (`EvidenceLookupInput.kind`).
2. **Ambiguous values** (for example UUID-shaped strings) yield multiple candidate kinds; the resolver (EVIDENCE-SPINE-002) runs safe lookups in priority order.
3. **Track every lookup** as an `EvidenceSourceAttempt` (`success` | `not_found` | `error` | `skipped`).
4. **Missing data is not failure.** Unresolved relationships become `unresolved_expected_link` edges with a stable reason code.
5. **Hard service errors** surface as source failures; partial graphs remain valid.

## Graph model (summary)

| Layer | Type | Role |
| --- | --- | --- |
| Node | `EvidenceNode` | Service-owned entity (run, eval, model call, decision, …) |
| Edge | `EvidenceEdge` | Relationship with `direct` / `derived` / `weak` / `unresolved` confidence |
| Links | `EvidencePageLink` | Operator navigation targets in the SPA |
| Result | `EvidenceResolutionResult` | Graph + attempts + completeness + warnings |

Node and edge kind enums match `08_EVIDENCE_GRAPH_DATA_MODEL.md` in the evidence-spine package intake.

## Resolver limits (v1)

```text
maxDepth:    3
maxNodes:    50
maxApiCalls: 30
```

## Migration from trace correlation

The existing trace resolver (`src/system/correlation/resolveTraceCorrelation.ts`) accepts only `traceId`, `runId`, `decisionId`, and `modelCallId`. The evidence spine subsumes that behavior in EVIDENCE-SPINE-002 and adds eval, baseline, route-decision, and correlation roots.

Until the unified resolver ships, continue using trace correlation on System overview and Conexus observability; use page-local evidence journeys on Allagma run/eval detail.

## Missing-edge reason codes

Stable codes for operator messaging:

`not_found`, `service_unconfigured`, `service_unreachable`, `browser_blocked`, `actor_forbidden`, `route_not_exposed`, `id_not_present_in_source`, `ambiguous_identifier`, `filtered_lookup_empty`, `capability_not_supported`
