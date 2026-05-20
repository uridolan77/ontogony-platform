# P1 — CONEXUS-RETENTION-002

## Objective

Add durable maintenance-run history for retention cleanup.

## Context

`CONEXUS-RETENTION-001` correctly documents that `lastRun` is process-local and clears on restart. This is acceptable for Sprint 4 but not for production-like operations.

## Tasks

1. Add Postgres table for retention maintenance runs.
2. Persist trigger, dryRun, asOfUtc, effective cutoffs, counters, status/failure reason, actor/admin audit correlation.
3. Update status endpoint to read latest durable record when persistence enabled.
4. Keep in-memory fallback for non-Postgres mode.
5. Add restart/multi-instance semantics docs.
6. Add tests.

## Acceptance

- Last run survives restart in Postgres mode.
- Status endpoint returns durable last run.
- Admin audit and structured logs remain counts-only.
- No prompts/completions/secrets are stored.
