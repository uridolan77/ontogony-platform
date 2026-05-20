# CONEXUS-IDEMP-001 — Make implicit idempotency multi-node durable

**Priority:** P1  
**Repo:** conexus-dotnet  
**Theme:** Durability

## Problem

Explicit non-streaming replay exists, but implicit internal idempotency should not remain in-memory for durable multi-node operation.

## Scope

Add Postgres-backed ledger implementation or formally constrain implicit idempotency to single-node/local mode with readiness signal.

## Acceptance criteria

In Postgres mode, duplicate implicit requests are coordinated durably across process restart or multiple service instances.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
