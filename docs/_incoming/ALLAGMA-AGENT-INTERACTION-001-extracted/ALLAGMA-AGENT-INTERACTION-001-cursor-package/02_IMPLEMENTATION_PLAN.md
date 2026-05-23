# Implementation plan

## Phase 0 — repo audit and truth map

Before editing, produce a short local audit:

- Where Agent Interaction page lives.
- Where fixture replay data lives.
- Which client adapters already exist for Allagma, Conexus, Kanon, Evidence Spine.
- Which Allagma APIs are available for run detail, events, audit, evaluations, replay/export.
- Which Conexus APIs are available for model-call detail, evidence links, execution runs, provider attempts.
- Which Kanon APIs are available for decision record, provenance, semantic graph, decisions by trace.
- Existing tests for Agent Interaction, Allagma pages, Evidence Spine, run list, Start Run.

Output this audit in the PR notes or a small doc such as:

```text
docs/reviews/ALLAGMA_AGENT_INTERACTION_001_AUDIT.md
```

## Phase 1 — define interaction session contract

Add frontend domain types for an interaction session. Do not pass backend DTOs directly to components.

Key abstractions:

```text
AgentInteractionMode
AgentInteractionSession
AgentInteractionTimelineEvent
AgentInteractionMessage
AgentInteractionSourceAttempt
AgentInteractionMissingReason
AgentInteractionLink
AgentInteractionExportBundle
```

Use the contracts in this package as the target shape.

## Phase 2 — mode model

Implement explicit mode switch:

```text
Live lookup
Fixture replay
Imported JSONL
```

Rules:

- If live Allagma is reachable, default mode is `live_lookup`.
- If live Allagma is unreachable, show live unavailable with reason, then allow fixture replay deliberately.
- Fixture replay always displays the demo badge.
- Imported JSONL always displays imported/offline badge.
- Mode is visible in exported bundles.

## Phase 3 — live Allagma resolution

Live run resolution should call, when available:

```text
GET /allagma/v0/runs/{runId}
GET /allagma/v0/runs/{runId}/events
GET /allagma/v0/runs/{runId}/audit
GET /allagma/v0/runs/{runId}/evaluations
GET /allagma/v0/evaluations/{evaluationRunId}/evidence
```

The resolver should produce:

- session summary
- timeline events
- run state snapshots/deltas, when available
- replay/audit/evaluation links
- source attempts
- missing reasons

## Phase 4 — enrich with Kanon

For planning/action decision IDs found in run detail, events, audit, or Evidence Spine:

```text
GET /ontology/v0/decision-records/{decisionId}
GET /ontology/v0/decision-records/{decisionId}/provenance
GET /ontology/v0/semantic-graph?... when needed
```

Render:

- planning decision
- action-evaluation decision(s)
- ontology version
- policy/gate reason
- provenance status

If action decision is not recorded, show:

```text
Action decision: not recorded by current backend
Reason code: not_recorded
```

Do not show generic `unknown`.

## Phase 5 — enrich with Conexus

For model-call IDs/request IDs found in run detail, events, audit, or Evidence Spine:

```text
GET /admin/v0/model-calls/{modelCallId}
GET /admin/v0/model-calls/{modelCallId}/evidence-links
GET /admin/v0/diagnostics/execution-runs/by-request-id/{requestId}
GET /admin/v0/route-decisions/{routeDecisionId} when available
```

Render:

- model call ID
- request ID
- execution run ID
- provider attempt(s)
- provider mode fake/real/unknown
- selected provider/provider model
- alias/model purpose
- fallback chain
- tokens, cost, latency
- redacted messages if available

## Phase 6 — timeline renderer

Transform raw events into operator-grade event cards.

Event classes:

```text
run_lifecycle
planning
topology
workflow_checkpoint
tool_intent
action_evaluation
human_gate
model_call
evaluation
replay
unresolved
```

Every timeline row should show:

- timestamp
- title
- state/status
- system/source
- evidence links
- compact details
- expandable raw payload if safe
- missing/unresolved reason if enrichment failed

## Phase 7 — run list polish

Fix Allagma overview/run list:

- Replace unlabeled `unknown` with `Task type: unknown` or hide it.
- Hide raw fake provider response from card summary.
- Show purpose, provider mode, ontology, player/context, planning decision ID, model-call ID, run status.
- Structure stream withheld output as:

```text
Output: withheld
Length: 274
Hash: ...
Reason: streaming output redacted from list view
```

- Rename `Failed runs (sample)` to `Failed runs in current list` unless a true time-window metric exists.

## Phase 8 — Start Run workbench

Enhance Start Run:

- request preview before submit
- validate model purpose against runtime config when available
- show idempotency key explanation
- offer:
  - Start run
  - Start and open run detail
  - Start and open Agent Interaction

## Phase 9 — tests

Add unit/component tests and at least one local/manual E2E script target.

Required test categories:

- mode selection defaults to live when reachable
- fixture mode badge and readiness exclusion
- imported JSONL badge
- Allagma event mapping
- Conexus enrichment display
- Kanon decision display
- unresolved reason display
- run list polish
- Start Run preview/actions
- export has no duplicate sections

## Phase 10 — final verification

Run existing frontend test suite plus focused tests. If backend code changed, run targeted dotnet tests in changed repos.
