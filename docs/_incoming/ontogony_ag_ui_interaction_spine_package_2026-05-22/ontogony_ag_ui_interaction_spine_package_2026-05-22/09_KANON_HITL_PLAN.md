# 09 — Kanon HITL / Interrupt Plan

**Repo:** `kanon-dotnet`

## Goal

Map Kanon’s semantic governance and human-gate flows into Ontogony interaction interrupts.

## Current foundation

Kanon already exposes decision records, provenance, semantic graph anchors, Conexus assistance review, topology policy decisions, and human gate check/resolve routes. It also defines that human gate evidence can be read via:

```http
GET /ontology/v0/semantic-graph?humanGateId={humanGateId}
```

and resolved through:

```http
POST /ontology/v0/actions/human-gates/{humanGateId}/resolve
```

## Interrupt shape

```ts
interface OntogonyInterrupt {
  id: string;
  reason:
    | "tool_call"
    | "confirmation"
    | "input_required"
    | "kanon:human_gate"
    | "kanon:policy_escalation"
    | "kanon:semantic_review";
  message: string;
  humanGateId?: string;
  kanonDecisionId?: string;
  toolCallId?: string;
  responseSchema?: JsonSchema;
  expiresAt?: string;
  metadata?: Record<string, unknown>;
}
```

## Standard response schemas

### Approval

```json
{
  "type": "object",
  "properties": {
    "approved": { "type": "boolean" },
    "reason": { "type": "string" }
  },
  "required": ["approved"]
}
```

### Approval with edits

```json
{
  "type": "object",
  "properties": {
    "approved": { "type": "boolean" },
    "edits": { "type": "object", "additionalProperties": true },
    "reason": { "type": "string" }
  },
  "required": ["approved"]
}
```

### Structured input

Use a domain-specific schema supplied by Kanon policy or Allagma run step.

## Event mapping

| Kanon fact | Interaction event |
| --- | --- |
| human gate created/opened | `INTERRUPT_CREATED`, `HUMAN_GATE_OPENED` |
| human gate check performed | `HUMAN_GATE_CHECKED` |
| gate approved | `INTERRUPT_RESOLVED`, `HUMAN_GATE_RESOLVED`, `DECISION_RECORDED` |
| gate rejected | `INTERRUPT_RESOLVED`, `HUMAN_GATE_RESOLVED`, `DECISION_RECORDED` |
| policy escalation | `INTERRUPT_CREATED` with `kanon:policy_escalation` |
| provenance available | `DECISION_PROVENANCE_LINKED` |

## Idempotency

A resume should be idempotent on:

```text
(threadId, runId, humanGateId, interruptId, payload hash)
```

Kanon should record enough provenance to distinguish:

- approval
- rejection
- approval with edits
- cancelled/no-response
- expired/stale response
- invalid schema payload

## Frontend behavior

- Render pending gate as `AgentInterruptCard`.
- Validate response payload client-side when `responseSchema` is present.
- Submit resume through Allagma when the gate is attached to a run.
- Link to Kanon semantic graph and decision/provenance pages.

## Tests

- human gate graph maps to pending interrupt
- approval maps to resolved interrupt + decision
- rejection maps to resolved interrupt + decision
- edits are preserved in resume payload
- invalid payload is rejected
- stale/expired interrupt produces explicit error event

## Non-goals

- Kanon does not own Allagma run continuation.
- Kanon does not hydrate Conexus model-call evidence.
- Kanon does not render UI components directly.
