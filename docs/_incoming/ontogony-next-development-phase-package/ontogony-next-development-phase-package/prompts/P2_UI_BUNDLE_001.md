# P2 — UI-BUNDLE-001

## Objective

Track `@ontogony/ui` subpath dependency and bundle impact.

## Tasks

1. Generate dependency graph per public subpath.
2. Identify heavy dependencies.
3. Ensure Monaco/JSON/editor/table deps stay isolated.
4. Guard root barrel against accidental heavy exports.
5. Add report and evidence.

## Acceptance

- `docs/reports/SUBPATH_BUNDLE_IMPACT.md` exists.
- CI/report command exists.
- Root barrel additions are reviewed.
