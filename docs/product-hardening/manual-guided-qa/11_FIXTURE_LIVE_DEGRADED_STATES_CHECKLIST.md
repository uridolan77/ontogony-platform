# 11 — Fixture, live, and degraded states checklist

Validate route-state integrity using `ontogony-frontend/docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md`
and `ontogony-frontend/docs/ROUTE_STATE_MATRIX.md`.

## Fixture-state checks

- [ ] `/allagma/evaluations?dashboardFixture=ci-suite` shows fixture banner
- [ ] `/allagma/evaluations/{id}?dashboardFixture=ci-suite` shows fixture banner
- [ ] `/allagma/runs/{id}?evalFixture=*` shows eval fixture banner
- [ ] `/allagma/runs/{id}?topologyFixture=*` shows topology fixture banner
- [ ] `/allagma/runs/{id}?sandboxFixture=*` shows sandbox fixture banner
- [ ] Unknown fixture ID is ignored (falls back to live behavior)

## Live-state checks

- [ ] `/allagma/evaluations` uses live contract when no fixture param
- [ ] `/allagma/runs/{id}` shows no fixture banners in live mode
- [ ] `/allagma/replay` performs live lookup/evidence behavior

## Degraded/error-state checks

- [ ] Unauthorized state is explicit (401 conditions)
- [ ] Network error state is explicit with retry/remediation path
- [ ] Empty state is explicit and not mis-labeled as success
- [ ] Replay unavailable bundle state is explicit (`unavailable`)

## Honesty checks

- [ ] No automatic fixture fallback on live failures
- [ ] No fake success buttons for missing backend actions
- [ ] Limitation banners align with OpenAPI-backed capabilities

## Evidence

- [ ] Screenshot: fixture banner example
- [ ] Screenshot: live-state example
- [ ] Screenshot: degraded/unauthorized/empty-state example
- [ ] Note: fixture-live boundary verdict (`PASS`/`FAIL`)
