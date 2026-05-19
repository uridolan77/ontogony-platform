# Repo Cleaning Principles

## Code tightening

Focus on:

- stale/dead paths
- duplicated helpers/adapters
- hidden fixture fallback
- inconsistent error semantics
- brittle tests/selectors
- OpenAPI/generated-client drift
- redaction gaps
- unsafe logging
- package/build/test command drift

Avoid:

- broad refactors
- mass renames
- changing public contracts without need
- new product features
- formatting-only mega PRs

## Documentation tightening

Focus on:

- docs indexes
- current vs historical separation
- evidence discoverability
- stale status drift
- limitations consolidation
- terminology consistency
- cross-repo links

Avoid:

- mass doc migration
- rewriting history
- moving archives
- mixing documentation cleanup with runtime changes
