# 02 — Reference AG-UI Model for Ontogony

## External reference model

AG-UI is an event-based protocol for connecting user-facing applications to agentic backends. The important concepts for Ontogony are:

- bidirectional agent ↔ UI communication
- run lifecycle events
- text/message streaming
- tool-call lifecycle events
- state snapshots and deltas
- human interrupts and resumes
- frontend-defined tools
- generative UI intents
- custom events for framework-specific needs

The lesson is not that Ontogony must immediately depend on CopilotKit. The lesson is that Ontogony needs a durable contract for agent-user interaction.

## Ontogony translation

| Reference capability | Ontogony interpretation |
| --- | --- |
| Streaming chat | Live run transcript with user/agent/operator messages |
| Run events | Allagma run and operation lifecycle |
| Tool calls | Backend model calls, frontend actions, governed external actions |
| Shared state | Session state + active evidence graph + selected run context |
| Interrupts | Kanon human gate / approval / structured input request |
| Resume | Allagma resume endpoint with Kanon-validated payload |
| Generative UI | Typed UI intents rendered by trusted @ontogony/ui components |
| Reasoning | Sanitized milestone/evidence events, not raw hidden reasoning |
| Tool output streaming | Conexus model-call/provider-attempt progress and Allagma operation logs |
| Custom events | Route decisions, semantic decisions, evidence edges, audit links |

## Compatibility stance

Use three layers:

```text
Layer 1 — Ontogony internal events
  Canonical source of truth for the system.

Layer 2 — AG-UI adapter
  Maps Ontogony events to AG-UI-compatible events and maps AG-UI input/resume shapes back into Ontogony.

Layer 3 — CopilotKit or other clients
  Optional integration point for external UI clients or demos.
```

This avoids blocking on SDK maturity and protects Ontogony-specific semantics.

## Minimum useful event classes

1. `RUN_*` — run lifecycle.
2. `STEP_*` — subtask/operation progress.
3. `MESSAGE_*` — user/agent/operator messages.
4. `TOOL_CALL_*` — tool/model/action proposals and results.
5. `STATE_*` — state snapshot/delta.
6. `INTERRUPT_*` — human gate / approval / structured input request.
7. `RESUME_*` — user response to interrupt.
8. `EVIDENCE_*` — evidence graph node/edge/link/source-attempt events.
9. `DECISION_*` — Kanon semantic/governance decisions.
10. `MODEL_CALL_*` — Conexus model-call lifecycle and route details.
11. `UI_INTENT_*` — trusted renderable component intents.
12. `ERROR_*` — recoverable and terminal errors.

## Non-negotiable design rules

- Every event must be traceable to `runId`, `threadId`, `traceId`, or `correlationId` when available.
- Events must be replayable from JSONL fixtures.
- Hidden chain-of-thought must never be serialized. Use public reasoning summaries, decision reasons, source attempts, or evidence milestones.
- UI intents must be validated against a safe registry. No arbitrary component execution.
- Human interrupts must be durable, resumable, and idempotent.
- Evidence links must not be ornamental; they must navigate to real Allagma/Kanon/Conexus/Ontogony surfaces or explicitly declare missing links.
