# 12 — Testing and Fixture Harness

## Goal

Create deterministic tests for agentic UI flows without depending on live LLMs, live providers, or complete backend integration.

## Fixture format

Use JSONL: one event per line.

Example:

```jsonl
{"schema":"ontogony-agent-interaction-event-v0","eventId":"evt-001","type":"RUN_STARTED",...}
{"schema":"ontogony-agent-interaction-event-v0","eventId":"evt-002","type":"MESSAGE_STARTED",...}
```

## Fixture categories

```text
fixtures/events/
  sample-run.jsonl
  sample-human-gate-interrupt.jsonl
  sample-model-call-route.jsonl
  sample-missing-evidence.jsonl
  sample-replay.jsonl
```

## Frontend tests

- parse JSONL fixtures
- validate fixtures against schema
- reducer reconstructs session
- interrupted run blocks new input
- resume resolves interrupt
- unknown custom event remains visible
- evidence links preserve service ownership
- redaction flags render correctly

## UI package tests

- timeline sorting/grouping
- accessible approval actions
- collapsed payloads
- missing evidence state
- tool-call lifecycle display
- state diff display
- generative UI fallback

## Backend tests

### Allagma

- run projection validates against schema
- event projection preserves ids
- human gate run event projects interrupt
- resume projects resume/resolved events

### Kanon

- human gate maps to interrupt
- approval/rejection decision maps to resolved event
- response schema validation docs/tests

### Conexus

- fake provider call maps to model-call event sequence
- route-decision detail links model call
- usage/cost event redacts raw content

## Golden test rule

Every adapter must have at least one golden JSONL file. If the adapter output changes, the diff must be reviewed intentionally.

## Why this matters

AG-UI-style applications are nondeterministic at runtime, but their UI/event reducers must be deterministic. Golden JSONL fixtures are the fastest way to keep the interaction layer reliable.
