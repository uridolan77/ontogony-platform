# 02 — Implementation Plan

## Phase 0 — Repo audit

Find actual files before changing anything.

Search terms:

```text
EvidenceSpine
resolveEvidenceSpine
EvidenceGraph
EvidenceNode
EvidenceEdge
EvidenceSourceAttempt
missing link
source failure
routeDecisionId
/admin/v0/route-decisions
/admin/v0/model-calls
/admin/v0/diagnostics/execution-runs/by-request-id
/ontology/v0/semantic-graph
/ontology/v0/decision-records
/allagma/v0/runs
/runs/{runId}
chatcmpl-
executionRun
requestId
not_applicable
```

Expected frontend area from prior work:

```text
ontogony-frontend/src/evidence-spine/
```

But do not assume. Verify.

## Phase 1 — Define stable resolver semantics

Introduce or align these concepts:

```text
EvidenceApplicability = required | optional | not_applicable
EvidenceResolutionState = resolved | partial | unresolved | not_applicable
EvidenceMissingReasonCode =
  not_applicable
  not_recorded
  not_resolved
  backend_missing
  authorization_failed
  fixture_only
  not_implemented
  contract_mismatch
  route_mismatch
  lookup_failed
  upstream_unavailable
  timeout
  redacted
```

Every missing relationship should include:

```text
relationship
from node / root context
to expected kind
source system
applicability
reason code
human message
suggested next step
attempt IDs, when available
```

## Phase 2 — Fix route-decision resolution

Desired behavior:

- If `/admin/v0/model-calls/{modelCallId}/evidence-links` returns `routeDecisionId`, Evidence Spine attempts `/admin/v0/route-decisions/{routeDecisionId}`.
- If route detail exists: add `conexus.routeDecision` node and edge `used_route_decision`.
- If route detail does not exist: add missing relationship with `backend_missing` or `not_recorded`, not generic failure.
- If route endpoint throws: add source failure with structured `lookup_failed`, `contract_mismatch`, or `upstream_unavailable`.

Backend target for Conexus:

- Prefer making the route-decision detail endpoint return `200` for route IDs emitted in model-call evidence links.
- If the route was intentionally not persisted, either stop emitting the ID or return `404` with a typed error body.

## Phase 3 — Fix Kanon applicability

Applicability rules:

| Root kind | Kanon planning decision expected? | Explanation |
|---|---:|---|
| `allagmaRunId` | yes | Governed runtime flow requires Kanon plan/action decisions. |
| `baselineComparisonId` | yes if subject run exists | Comparison links to run; run links to Kanon decision. |
| `allagmaEvaluationRunId` | yes if linked run exists | Evaluation inherits run evidence expectations. |
| `kanonDecisionId` | yes | Root is itself a Kanon decision. |
| `traceId` / `correlationId` | optional/contextual | May include direct Conexus or governed flow. |
| `conexusModelCallId` | no by default | Direct Conexus calls may not be governed. Mark Kanon decision not applicable unless evidence links show one. |
| `conexusRouteDecisionId` | no by default | Routing belongs to Conexus. Kanon is not inherently required. |

## Phase 4 — Canonicalize graph nodes

Merge duplicate nodes by canonical key:

```text
canonicalKey = `${system}:${kind}:${canonicalId}`
```

Examples:

```text
allagma:run:run_fcde...
kanon:decision:decision_f1c...
conexus:modelCall:chatcmpl-...
conexus:request:0HN...
conexus:executionRun:chat-0HN...
platform:trace:52ee...
platform:correlation:1e65...
```

Merge rules:

1. authoritative API node beats placeholder semantic-graph node;
2. preserve all source attempts and source routes;
3. union page links;
4. union aliases/identifiers;
5. preserve warning metadata;
6. if statuses conflict, keep both raw statuses and compute display status by precedence.

Suggested authority precedence:

```text
live_api > semantic_graph > evidence_export > derived > placeholder > fixture
```

## Phase 5 — Normalize IDs

Separate:

```text
Model call ID: chatcmpl-0HNLMJJQFVG3N-00000003
Request ID: 0HNLMJJQFVG3N-00000003
Execution run ID: chat-0HNLMJJQFVG3N-00000003
Route decision ID: rd-0HNLMJJQFVG3N-00000003
```

Do not display one as another. Use aliases internally only for lookup strategy.

## Phase 6 — Normalize Allagma route templates

Every resolver and page link should use versioned API routes:

```text
/allagma/v0/runs/{runId}
/allagma/v0/runs/{runId}/events
/allagma/v0/runs/{runId}/audit
/allagma/v0/runs/{runId}/evaluations
/allagma/v0/evaluations/{evaluationRunId}/evidence
/allagma/v0/evaluations/baseline-comparisons/{comparisonId}
```

No source attempt should show `/runs/{runId}` unless that is truly a frontend route, and then it must be labeled as a page link rather than API lookup.

## Phase 7 — Governed fake-provider E2E

Create a repeatable local proof:

1. start local Conexus/Kanon/Allagma stack;
2. start Allagma run for `summarize-player-risk` using fake Conexus provider;
3. capture run ID;
4. resolve Evidence Spine by run ID;
5. assert required nodes/edges;
6. resolve by model call ID;
7. resolve by Kanon decision ID;
8. resolve by baseline comparison ID if present;
9. export bundle;
10. assert no generic source failures and no duplicate placeholder nodes.

## Phase 8 — UI/operator copy

Replace vague text with exact, typed statuses:

Bad:

```text
An unexpected error occurred.
```

Good:

```text
backend_missing — routeDecisionId was recorded in Conexus model-call evidence links, but no route decision detail row was found.
Suggested next step: open Conexus routing evidence or rerun with route-decision persistence enabled.
```

Bad:

```text
Expected Kanon decision missing.
```

Good for direct Conexus root:

```text
not_applicable — this model call was resolved directly from Conexus and was not known to pass through Allagma/Kanon governance.
```
