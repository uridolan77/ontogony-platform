# 13 — Acceptance Checklist

## Platform

- [ ] `AGENT_INTERACTION_SPINE_CONTRACT.md` published.
- [ ] Event and session schemas published.
- [ ] Matrix maps event families to service owners.
- [ ] Validation scripts pass.
- [ ] Fixtures validate.
- [ ] Redaction / hidden reasoning rules explicit.

## @ontogony/ui

- [ ] `./agent` export added.
- [ ] Timeline component implemented.
- [ ] Interrupt card implemented.
- [ ] Tool-call card implemented.
- [ ] Evidence links panel implemented.
- [ ] State diff panel implemented.
- [ ] Storybook examples added.
- [ ] Export smoke tests pass.

## ontogony-frontend

- [ ] `src/agent-interaction` module added.
- [ ] JSONL fixture adapter added.
- [ ] Deterministic reducer added.
- [ ] Workbench route added.
- [ ] Evidence Spine adapter added.
- [ ] Allagma/Kanon/Conexus client-side adapters added.
- [ ] Lookup by run/model-call/decision/trace/humanGate works.
- [ ] JSONL import/export works.

## Allagma

- [ ] Projection service added.
- [ ] Run lifecycle maps to event schema.
- [ ] Run events/operations map to steps/tool/message/events.
- [ ] Human gate references map to interrupts.
- [ ] JSONL export validates.
- [ ] Live stream deferred or implemented only after UI baseline.

## Kanon

- [ ] Human gate interrupt mapping documented.
- [ ] Response-schema conventions documented.
- [ ] Approval/reject/edit payloads covered by tests.
- [ ] Decision/provenance links emitted as evidence events.

## Conexus

- [ ] Model-call projection documented.
- [ ] Fake provider event sequence fixture validates.
- [ ] Route decision and provider attempts mapped.
- [ ] Usage/cost event redacts raw content.
- [ ] Evidence links and bundle mapping covered.

## System-level acceptance

- [ ] Operator can open `/system/agent-interaction?runId=...` and see a timeline.
- [ ] Operator can open by `modelCallId`, `decisionId`, `traceId`, or `humanGateId`.
- [ ] Missing links are displayed as unresolved, not blank.
- [ ] Exported JSONL can be replayed offline.
- [ ] AG-UI adapter tests pass against sample fixtures.
