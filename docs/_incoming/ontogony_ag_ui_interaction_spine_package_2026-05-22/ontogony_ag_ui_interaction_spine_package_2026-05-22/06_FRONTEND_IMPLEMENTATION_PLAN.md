# 06 — Frontend Implementation Plan

**Repo:** `ontogony-frontend`

## New module

```text
src/agent-interaction/
  adapters/
    agUiAdapter.ts
    allagmaRunInteractionAdapter.ts
    conexusModelCallInteractionAdapter.ts
    evidenceSpineInteractionAdapter.ts
    jsonlFixtureAdapter.ts
    kanonDecisionInteractionAdapter.ts
    traceCorrelationInteractionAdapter.ts
  components/
    AgentInteractionWorkbench.tsx
    AgentInteractionLookupBar.tsx
    AgentInteractionPanels.tsx
  fixtures/
    sample-run.jsonl
    sample-human-gate-interrupt.jsonl
  pages/
    AgentInteractionWorkbenchPage.tsx
  reducers/
    interactionReducer.ts
    interactionSelectors.ts
  tests/
    interactionReducer.test.ts
    jsonlFixtureAdapter.test.ts
    evidenceSpineInteractionAdapter.test.ts
    allagmaRunInteractionAdapter.test.ts
  types.ts
  index.ts
```

## Workbench route

Add:

```text
/system/agent-interaction
```

Supported query params:

```text
?runId=
?traceId=
?correlationId=
?modelCallId=
?decisionId=
?humanGateId=
?fixture=sample-run
```

## Store model

Keep this module local unless/until it must become global.

```ts
interface AgentInteractionWorkbenchState {
  status: "idle" | "loading" | "ready" | "error";
  source: "fixture" | "api-synthesized" | "stream";
  lookup: {
    runId?: string;
    traceId?: string;
    correlationId?: string;
    modelCallId?: string;
    decisionId?: string;
    humanGateId?: string;
  };
  session: OntogonyAgentSession | null;
  warnings: Array<{ code: string; message: string }>;
}
```

## Reducer rules

The reducer must be deterministic and side-effect free.

Required reducer behaviors:

- append events idempotently by `eventId`
- update run status from lifecycle events
- accumulate message deltas by message id
- accumulate tool-call argument deltas by tool-call id
- track open interrupts
- apply resolved/cancelled/expired interrupt states
- merge state snapshots and deltas
- collect evidence links and graph snapshots
- preserve unknown custom events in timeline

## Client-side synthesized adapter flow

### From Allagma run id

1. Fetch run detail.
2. Fetch run events.
3. Fetch operations if available.
4. Fetch audit bundle when available.
5. Use existing `resolveEvidenceSpine` for graph and source attempts.
6. Emit ordered interaction events.

### From model-call id

1. Use Conexus model-call detail/list/evidence routes.
2. Use trace correlation to find Allagma/Kanon ids.
3. Emit model-call events plus evidence links.

### From Kanon decision id

1. Fetch decision record.
2. Fetch provenance and semantic graph if available.
3. Resolve trace/correlation links.
4. Emit decision and evidence events.

### From human gate id

1. Fetch semantic graph by humanGateId.
2. Resolve related decision/run ids where available.
3. Emit `INTERRUPT_CREATED`/`HUMAN_GATE_*` events.

## Embedded surfaces

Add compact panels to:

- Allagma run detail: “Interaction Spine” tab/card.
- Conexus observability: model-call interaction timeline card.
- Kanon decisions/provenance: decision interaction card.
- Evidence Spine page: “Open in Agent Interaction Workbench”.

## Tests

Minimum tests:

- JSONL fixture parse/serialize round-trip.
- reducer reconstructs session from sample run.
- reducer preserves unknown custom events.
- human gate fixture produces one open interrupt.
- resume event resolves open interrupt.
- evidence-spine adapter creates `EVIDENCE_SOURCE_ATTEMPTED` events.
- trace-correlation adapter carries links into state.

## UX constraints

- The UI must make it clear whether events are live, synthesized, or fixture replay.
- Use explicit unresolved/missing states, never blank gaps.
- User approvals should require deliberate action.
- State diffs should default to collapsed for noisy payloads.
- Evidence links should display service ownership.
