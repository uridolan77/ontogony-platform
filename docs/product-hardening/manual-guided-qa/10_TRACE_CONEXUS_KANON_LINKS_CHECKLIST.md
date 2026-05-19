# 10 — Trace, Conexus, and Kanon links checklist

Goal: verify cross-service observability journey integrity from Allagma product routes.

## Source route

- [ ] Open `/allagma/runs/{subjectRunId}`
- [ ] Confirm trace and correlation identifiers are visible (where expected)

## Conexus link checks

- [ ] If request ID exists, link resolves to Conexus diagnostics detail/summary surface
- [ ] Conexus target page renders diagnostic context for the same operation
- [ ] Any missing Conexus data yields explicit degraded/unavailable state

## Kanon link checks

- [ ] Decision link resolves to Kanon decision/provenance route
- [ ] Kanon route reflects same decision/trace context as originating run
- [ ] Replay bundle linkage from Kanon to Allagma replay is coherent

## Correlation consistency checks

- [ ] Trace ID is stable across Allagma -> Kanon navigation
- [ ] Decision/run references do not cross-wire to unrelated entities
- [ ] Broken link behavior is explicit and non-crashing

## Security/redaction checks

- [ ] Correlation evidence panels do not expose secrets
- [ ] Payload previews use redaction markers where sensitive fields appear

## Evidence

- [ ] Screenshot: run detail with trace/correlation card
- [ ] Screenshot: Conexus linked surface
- [ ] Screenshot: Kanon linked surface
- [ ] Note: ID continuity result (`PASS`/`FAIL` with IDs)
