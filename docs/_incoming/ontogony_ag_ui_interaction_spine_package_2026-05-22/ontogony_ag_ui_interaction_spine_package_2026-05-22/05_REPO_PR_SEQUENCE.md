# 05 — Repo PR Sequence

## Milestone name

`ONTOGONY-AGUI-000 — Agent Interaction Spine Baseline`

## Sequence principles

- Start with contracts and static fixtures.
- Build UI primitives before backend streaming.
- Reuse existing evidence/correlation APIs first.
- Keep every PR small enough to test independently.
- Avoid security/prod-hardening scope unless needed for contract correctness.

## PR sequence

### 1. `PLAT-AGUI-000` — Contract and schemas

**Repo:** `ontogony-platform`  
**Purpose:** Publish the canonical event/session schema, mapping docs, and drift gates.

Deliverables:

- `docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md`
- `docs/system/agent-interaction-event.matrix.json`
- `docs/schemas/ontogony-agent-interaction-event-v0.schema.json`
- `docs/schemas/ontogony-agent-interaction-session-v0.schema.json`
- `docs/evidence/PLAT_AGUI_000_EVIDENCE.md`
- schema validation tests/scripts

Acceptance:

- Event schema validates package fixtures.
- Mapping matrix covers Allagma, Kanon, Conexus, Frontend, UI.
- Contract explicitly states hidden reasoning / raw secret non-goals.

### 2. `OUI-AGENT-001` — Agent UI primitive subpath

**Repo:** `ontogony-ui`  
**Purpose:** Add a product-neutral `./agent` export with reusable components.

Deliverables:

- `src/components/agent/types.ts`
- `AgentEventTimeline`
- `AgentRunStatusHeader`
- `AgentMessageStream`
- `AgentToolCallCard`
- `AgentInterruptCard`
- `AgentStateDiffPanel`
- `AgentEvidenceLinksPanel`
- `AgentUiIntentRenderer`
- Storybook stories and tests
- package export `./agent`

Acceptance:

- No dependency on product-specific API clients.
- Components consume view models only.
- Empty/error/loading states covered.
- Keyboard and screen-reader semantics for approvals.

### 3. `OFE-AGUI-001` — Frontend interaction module + fixture replay

**Repo:** `ontogony-frontend`  
**Purpose:** Add the `src/agent-interaction/` module and a replayable workbench using fixtures.

Deliverables:

- `src/agent-interaction/types.ts`
- `src/agent-interaction/reducers/interactionReducer.ts`
- `src/agent-interaction/adapters/jsonlFixtureAdapter.ts`
- `src/agent-interaction/adapters/evidenceSpineAdapter.ts`
- `src/agent-interaction/pages/AgentInteractionWorkbenchPage.tsx`
- route `/system/agent-interaction`
- tests using JSONL fixtures

Acceptance:

- Workbench can replay sample run and sample interrupt fixtures.
- Reducer is deterministic.
- Evidence links resolve to existing system pages where available.

### 4. `OFE-AGUI-002` — Client-side adapters over existing APIs

**Repo:** `ontogony-frontend`  
**Purpose:** Synthesize interaction events from existing Allagma/Kanon/Conexus APIs.

Deliverables:

- `allagmaRunInteractionAdapter.ts`
- `kanonDecisionInteractionAdapter.ts`
- `conexusModelCallInteractionAdapter.ts`
- `traceCorrelationInteractionAdapter.ts`
- embedded panels on Allagma run detail, Conexus observability, Kanon decisions

Acceptance:

- A known Allagma run can generate a timeline of run, step, evidence, decision, and model-call events.
- Missing downstream data becomes explicit unresolved evidence events.
- No backend changes required.

### 5. `ALLAGMA-AGUI-001` — Canonical backend event projection

**Repo:** `allagma-dotnet`  
**Purpose:** Add an application-layer projection from Allagma run events to Ontogony agent interaction events.

Deliverables:

- `OntogonyAgentEvent` C# records or DTOs
- `IAgentInteractionEventProjector`
- projection from run, events, operations, audit, resume/retry/cancel
- fixture output endpoint or export command, not necessarily live streaming yet
- tests for run lifecycle and interrupt projection

Acceptance:

- Existing run-event data can be projected to schema-valid interaction JSONL.
- Resume/cancel/retry are represented without breaking current endpoints.

### 6. `KANON-HITL-AGUI-001` — Human-gate interrupt contract

**Repo:** `kanon-dotnet`  
**Purpose:** Define and expose the mapping from human gates / semantic approval decisions to interaction interrupts.

Deliverables:

- human-gate interrupt DTOs
- response-schema conventions for approval/edit/reject payloads
- mapping docs and tests
- optional evidence enrichment from decision/provenance routes

Acceptance:

- `humanGateId` can map to an `INTERRUPT_CREATED` event.
- approval with edits maps to `RESUME_SUBMITTED` + `HUMAN_GATE_RESOLVED`.
- idempotency and audit fields are documented.

### 7. `CONEXUS-AGUI-001` — Model-call event projection

**Repo:** `conexus-dotnet`  
**Purpose:** Project existing model-call evidence into interaction events.

Deliverables:

- model-call event DTOs or adapter tests
- model-call → route-decision → provider-attempt → cost event mapping
- JSONL fixture export for one fake provider call
- docs linked from `MODEL_CALL_EVIDENCE_FLOW.md`

Acceptance:

- Fake provider model call emits schema-valid model-call events.
- Route decision and provider attempts are linked by ids.
- Raw prompts/completions remain excluded by default.

### 8. `OFE-AGUI-003` — Integrated Agent Interaction Workbench

**Repo:** `ontogony-frontend`  
**Purpose:** Turn fixture/client-side adapters into a unified operator surface.

Deliverables:

- lookup by run/model-call/decision/trace/humanGate id
- timeline + messages + tool calls + approvals + evidence graph snapshot
- export/import JSONL
- redaction display
- route links back to Evidence Spine, Conexus, Kanon, Allagma pages

Acceptance:

- Operator can start from any major identifier and inspect the interaction spine.
- JSONL export can be replayed locally.
- Workbench does not crash on missing links.

### 9. `ALLAGMA-AGUI-002` — Live stream transport

**Repo:** `allagma-dotnet`  
**Purpose:** Add SSE or WebSocket stream for interaction events after schemas and UI are stable.

Recommended v0:

```http
GET /allagma/v0/runs/{runId}/interaction-events/stream
Accept: text/event-stream
```

Acceptance:

- Stream resumes from `Last-Event-ID` or cursor if feasible.
- Same events can be exported as JSONL for tests.
- Frontend gracefully falls back to polling/current adapters.

### 10. `ADAPTER-AGUI-001` — External AG-UI compatibility adapter

**Repo:** likely `ontogony-frontend` first, optionally platform package later  
**Purpose:** Map Ontogony internal event streams to AG-UI-compatible event streams.

Acceptance:

- Internal events remain canonical.
- Adapter is pure and tested with fixtures.
- Unknown custom events are preserved under `ontogony.*` names.
