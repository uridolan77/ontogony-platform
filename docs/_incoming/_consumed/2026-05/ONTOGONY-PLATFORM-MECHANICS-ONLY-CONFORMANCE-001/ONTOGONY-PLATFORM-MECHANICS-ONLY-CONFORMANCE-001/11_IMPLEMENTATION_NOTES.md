# Implementation notes

## Recommended implementation posture

Start with docs + scripts + fixtures. Do not immediately refactor packages.

## Script quality rules

- Scripts must be safe by default.
- No real provider calls.
- No secrets.
- No mutation of sibling repos.
- Emit machine-readable JSON.
- Exit codes:
  - `0` PASS
  - `1` FAIL
  - `2` PARTIAL
  - `3` NOT_RUN

## Schema quality rules

- Use JSON Schema 2020-12.
- Include `$id`.
- Keep schemas product-neutral.
- Add valid and invalid fixtures.

## Naming

Suggested package closeout path:

```text
docs/evidence/ONTOGONY_PLATFORM_MECHANICS_ONLY_CONFORMANCE_001_CLOSEOUT.md
```
