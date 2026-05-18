# EVAL-CLEAN-001 — Final eval-sequence cleanup before durable persistence

## Repos

- `allagma-dotnet`
- optional: `ontogony-frontend`
- optional: `ontogony-platform`

## Goal

Perform a final cleanup/readiness pass after:

```text
EVAL-FIX-001
CX-ROUTE-EVIDENCE-001
AGM-EVAL-001
SYS-EVAL-001
SYS-OBS-EVAL-001
```

Do not add new runtime functionality.

## Tasks

1. Normalize evidence docs.
2. Add `docs/evidence/EVAL_SEQUENCE_STATUS_INDEX.md`.
3. Validate committed SYS-EVAL report.
4. Validate latest eval harness summary.
5. Add `docs/evidence/EVAL_SEQUENCE_SAFETY_GATES.md`.
6. Cross-check observability/operator docs.
7. Add `docs/planning/EVAL_DUR_001_HANDOFF.md`.

## Required scripts

If missing, add:

```text
scripts/validate-sys-eval-smoke-report.ps1
scripts/validate-latest-eval-summary.ps1
```

## Acceptance

- evidence docs are non-contradictory
- committed SYS-EVAL report validates
- latest eval summary validates
- safety gate doc exists
- EVAL-DUR-001 handoff exists
- no real external execution enabled
- no fixture/live confusion
- no raw prompt/completion/secret exposure

## Evidence

Add:

```text
docs/evidence/EVAL_CLEAN_001_FINAL_CLEANUP_EVIDENCE.md
```
