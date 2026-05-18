# SYS-FULL-SANITY-001 — First full sanity test

## Goal

Run and document the first full cross-repo sanity test of the eval-governed loop.

## Flow

1. Start Kanon, Conexus, Allagma, frontend.
2. Run baseline Allagma run: `single_workflow`.
3. Run subject Allagma run: `centralized_orchestrator` override.
4. Confirm Kanon topology authorization.
5. Confirm Conexus route/model evidence.
6. Write evaluation records.
7. Create baseline comparison.
8. Confirm frontend run detail and eval dashboard show evidence.
9. Export full sanity report.

## Output

```text
artifacts/full-sanity/full-sanity-report.json
docs/evidence/SYS_FULL_SANITY_001_REPORT.md
```
