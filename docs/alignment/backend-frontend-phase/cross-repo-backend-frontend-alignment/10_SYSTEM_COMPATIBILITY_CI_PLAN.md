# System Compatibility CI Plan

## Goal

Create a cross-repo confidence gate that proves the Ontogony system works together.

## Suggested home

Start in `ontogony-platform`. If it grows too large, split into a future `ontogony-system` repo.

## Jobs

### compatibility-openapi
- generate backend OpenAPI
- compare with frontend snapshots
- output diff summary
- fail if frontend snapshot does not match selected backend SHAs

### compatibility-runtime
- start Conexus, Kanon, Allagma with test providers/mocks
- call `/health`
- call OpenAPI endpoints
- execute a minimal governed flow:
  1. Allagma run fixture/start
  2. Kanon decision/provenance
  3. Conexus model call fixture
  4. Allagma event/replay evidence

### compatibility-frontend
- build ontogony-ui at pinned ref
- build frontend with selected backend OpenAPI snapshots
- run `npm run check:full`
- archive frontend provenance

## Artifacts

- `system-compatibility-report.json`
- backend OpenAPI artifacts
- frontend `dist/provenance.json`
- service health snapshots
- trace chain sample
- logs on failure
