# TASK-004 — Frontend Health/Ready Parser

## Goal

Parse `health.v1` and `ready.v1` in `ontogony-frontend`.

## Steps

1. Add typed contracts or generated adapters.
2. Support legacy payloads as degraded/contract-warning during transition.
3. Return structured state:
   - connectivity
   - readiness
   - contractHealth
   - version
   - warnings
   - failures
   - dataSource
4. Add tests.

## Acceptance

No generic "payload format warning" when payload is valid.
