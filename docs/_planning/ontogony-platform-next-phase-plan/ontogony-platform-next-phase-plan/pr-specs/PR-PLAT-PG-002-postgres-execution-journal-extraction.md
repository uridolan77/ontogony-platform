# PR-PLAT-PG-002 — Shared Postgres Execution Journal Provider

## Status

Later / conditional.

## Trigger

Do only when multiple services need durable execution journal storage.

## Scope

Generic durable provider for:

```text
runs
steps
attempts
transitions
checkpoints
trace/run lookup
```

## Non-goals

No agent plan semantics. No Kanon decision semantics. No Conexus provider routing semantics.
