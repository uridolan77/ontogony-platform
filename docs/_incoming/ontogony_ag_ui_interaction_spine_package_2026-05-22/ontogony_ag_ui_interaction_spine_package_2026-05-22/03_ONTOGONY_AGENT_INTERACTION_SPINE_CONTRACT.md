# 03 — Ontogony Agent Interaction Spine Contract

## Contract name

`ontogony-agent-interaction-v0`

## Purpose

Provide a canonical, replayable, event-sourced contract for agent-user interaction across Ontogony services and UIs.

This contract standardizes how Allagma, Kanon, Conexus, ontogony-frontend, and @ontogony/ui represent live agent runs, user interventions, semantic decisions, model calls, and evidence links.

## Authority boundaries

| Boundary | Owner | Responsibility |
| --- | --- | --- |
| Run lifecycle | Allagma | Run, operation, resume, retry, cancel, replay |
| Semantic authority | Kanon | Decisions, human gates, provenance, graph anchors |
| Model-call authority | Conexus | Model calls, route decisions, provider attempts, usage/cost |
| Cross-service contract | ontogony-platform | Schemas, mapping docs, drift tests |
| UI rendering | @ontogony/ui | Trusted UI primitives and view models |
| Operator orchestration | ontogony-frontend | Event ingestion, replay, interaction workbench, adapters |

## Core identifiers

Every event should carry as many of these as are known:

```ts
interface OntogonyInteractionIds {
  threadId?: string;
  runId?: string;
  parentRunId?: string;
  stepId?: string;
  operationId?: string;
  traceId?: string;
  correlationId?: string;
  modelCallId?: string;
  routeDecisionId?: string;
  kanonDecisionId?: string;
  planningDecisionId?: string;
  humanGateId?: string;
  evidenceNodeId?: string;
  evidenceEdgeId?: string;
}
```

## Base event shape

```ts
interface OntogonyAgentEventBase {
  schema: "ontogony-agent-interaction-event-v0";
  eventId: string;
  type: OntogonyAgentEventType;
  timestamp: string;
  source: "allagma" | "kanon" | "conexus" | "ontogony-frontend" | "ontogony-ui" | "ontogony-platform" | "adapter";
  severity?: "debug" | "info" | "warning" | "error";
  ids: OntogonyInteractionIds;
  redaction?: {
    containsRedactedFields?: boolean;
    redactionReasonCodes?: string[];
  };
  evidence?: OntogonyEvidenceLink[];
  payload: unknown;
}
```

## Event families

```text
RUN_STARTED
RUN_FINISHED
RUN_ERROR
RUN_CANCELLED
RUN_RETRIED
RUN_REPLAYED

STEP_STARTED
STEP_PROGRESS
STEP_FINISHED
STEP_ERROR

MESSAGE_STARTED
MESSAGE_DELTA
MESSAGE_FINISHED
MESSAGE_SNAPSHOT

TOOL_CALL_STARTED
TOOL_CALL_ARGS_DELTA
TOOL_CALL_READY
TOOL_CALL_RESULT
TOOL_CALL_ERROR

STATE_SNAPSHOT
STATE_DELTA

INTERRUPT_CREATED
INTERRUPT_EXPIRED
INTERRUPT_CANCELLED
INTERRUPT_RESOLVED
RESUME_SUBMITTED
RESUME_REJECTED

EVIDENCE_NODE_ADDED
EVIDENCE_EDGE_ADDED
EVIDENCE_SOURCE_ATTEMPTED
EVIDENCE_LINK_ADDED
EVIDENCE_GRAPH_SNAPSHOT

DECISION_RECORDED
DECISION_PROVENANCE_LINKED
HUMAN_GATE_OPENED
HUMAN_GATE_CHECKED
HUMAN_GATE_RESOLVED

MODEL_CALL_STARTED
MODEL_CALL_ROUTE_DECIDED
MODEL_CALL_PROVIDER_ATTEMPTED
MODEL_CALL_COMPLETED
MODEL_CALL_FAILED
USAGE_COST_RECORDED

UI_INTENT_PROPOSED
UI_INTENT_ACCEPTED
UI_INTENT_REJECTED
UI_INTENT_RENDERED
```

## Event stream semantics

### Replayable append-only log

An interaction session is an append-only event stream. Derived UI state must be reconstructible from events and deterministic reducers.

### Terminal lifecycle

A run terminates with one of:

- `RUN_FINISHED` with success outcome
- `RUN_ERROR`
- `RUN_CANCELLED`
- `RUN_FINISHED` with interrupt outcome, paired with one or more `INTERRUPT_CREATED` events

### Interrupt/resume rule

A pending interrupt blocks new normal user input for the same `threadId` unless the input includes a `resume` payload addressing all open interrupts.

### Evidence rule

Any event that asserts a material state transition should carry at least one evidence link or a reason code explaining why the link is unavailable.

### Redaction rule

Events may contain summarized, redacted, or structured payloads. They must not contain raw secrets, raw credentials, hidden chain-of-thought, or unredacted model prompts/completions unless explicitly allowed by a future secured evidence contract.

## Interaction session shape

```ts
interface OntogonyAgentSession {
  schema: "ontogony-agent-interaction-session-v0";
  threadId: string;
  activeRunId?: string;
  status: "idle" | "running" | "interrupted" | "completed" | "failed" | "cancelled";
  ids: OntogonyInteractionIds;
  messages: OntogonyInteractionMessage[];
  openInterrupts: OntogonyInterrupt[];
  evidenceGraph?: unknown;
  state?: Record<string, unknown>;
  events: OntogonyAgentEventBase[];
}
```

## Transport

Support three transport modes:

1. **Fixture JSONL** for deterministic tests and design work.
2. **HTTP polling / existing APIs** for client-side synthesized events from current endpoints.
3. **SSE or WebSocket** for backend-produced live events in the later milestone.

Do not begin with WebSocket unless there is a specific need for bidirectional low-latency transport. SSE plus POST resume/cancel/retry is enough for v0.
