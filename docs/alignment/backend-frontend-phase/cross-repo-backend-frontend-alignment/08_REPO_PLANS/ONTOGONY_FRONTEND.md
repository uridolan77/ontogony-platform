# ontogony-frontend Alignment Plan

## Objective

Consume backend alignment work without regressing the Phase I release candidate.

## Rules

- Do not remove limitation banners until backend route + OpenAPI + tests exist.
- Keep `npm run check` and `npm run check:full` green.
- Keep evidence/redaction tests.
- Keep route/e2e coverage matrices synced.

## PRs

### FE-BFA-001 — Backend snapshot refresh
Refresh OpenAPI snapshots and update `docs/openapi/PROVENANCE.md`.

### FE-BFA-002 — Capability-driven limitation cleanup
Replace hardcoded limitation banners with service capability metadata where available.

### FE-BFA-003 — Conexus request search UI
When backend request search exists, add filters/search to observability page.

### FE-BFA-004 — Allagma operation support UI
When backend capability endpoint exists, drive operation support from backend metadata.

### FE-BFA-005 — Typed adapter shrink
Reduce flexible parsing where typed schemas exist.

## Acceptance

- Frontend remains 9+.
- `check:full` passes.
- No fake backend functionality.
