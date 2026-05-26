# Ticket 006 — Tests, docs, and regression hardening

## Goal

Lock in the truthfulness improvements so the page cannot regress into misleading release-readiness language.

## Implementation

Add/update tests for:

- classification helper
- artifact freshness helper
- summary labels
- fixture-only ready route
- unknown source route
- live-with-fallback route
- stale artifact
- missing artifact fields

Add/update docs:

```text
docs/operators/RELEASE_READINESS_TRUTH.md
```

or the closest existing operator-doc location.

Doc should explain:

- generated artifact purpose
- artifact readiness vs release-candidate readiness
- how to regenerate
- why fixture-only rows do not count
- what backend/live validation would require later

## Acceptance

- [ ] Targeted tests pass.
- [ ] Existing tests pass or unrelated failures are documented.
- [ ] Operator doc exists/updated.
- [ ] No broad unrelated rewrites.
