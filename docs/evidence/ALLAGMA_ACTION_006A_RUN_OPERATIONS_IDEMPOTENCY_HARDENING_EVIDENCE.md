# ALLAGMA-ACTION-006A — Run operations idempotency and result UX hardening

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS**  
**Parent:** ACTION-006 closeout review findings

## Fixes

| Area | Change |
| --- | --- |
| Cancel/replay fingerprint mismatch | `StartAgentRunIdempotencyConflictException` → HTTP **409** `allagma.idempotency_conflict` (same as retry) |
| Cancel/replay without idempotency key | **Rejected** at API (`400`) and service (`ArgumentException`); no duplicate replay events without a key |
| Idempotency replay | Same key + same payload returns cached response; same key + different payload → **409** |
| Result cards | `RunOperationResultCard` links: open new run (retry), events/detail (cancel), audit/events (replay) |

## Validation

```text
dotnet test --filter FullyQualifiedName~RunOperationsServiceTests → 11 passed
npx vitest run src/allagma/components/AllagmaRunOperationsPanel.test.tsx → 2 passed
```

## Remaining

**ACTION-007A** — browser manual QA checklist still pending operator execution.
