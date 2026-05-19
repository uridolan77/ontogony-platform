# 01 — Preconditions

Use this checklist before any manual execution session.

## Session metadata

- [ ] Tester name recorded
- [ ] Date/time (UTC) recorded
- [ ] Branch/commit SHAs for six repos recorded
- [ ] Environment mode recorded (`docker-local`)
- [ ] Evidence output folder created for this session

## Scope confirmation

- [ ] Scope includes all files in this package (`01` through `14`)
- [ ] Scope includes fixture, live, and degraded states
- [ ] Scope includes cross-service trace/correlation links
- [ ] Scope includes evidence export paths for eval/run/replay/provenance
- [ ] Scope excludes production readiness claims

## Required repositories present (sibling layout under `C:\dev\`)

- [ ] `ontogony-platform`
- [ ] `allagma-dotnet`
- [ ] `kanon-dotnet`
- [ ] `conexus-dotnet`
- [ ] `ontogony-frontend`
- [ ] `ontogony-ui`

## Safety and boundary confirmations

- [ ] No real provider keys configured for this session
- [ ] No secrets will be pasted into notes or screenshots
- [ ] No runtime/application code changes are required for this package
- [ ] Known limitation behavior is treated as expected when documented
- [ ] This work is labeled as not production readiness

## Source docs loaded

- [ ] `docs/releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md`
- [ ] `docs/releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_KNOWN_LIMITATIONS.md`
- [ ] `ontogony-frontend/docs/README.md`
- [ ] `ontogony-frontend/docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md`
- [ ] `ontogony-frontend/docs/operators/FRONTEND_REPLAY_OPERATOR_CONTRACT.md`
- [ ] `ontogony-frontend/docs/ROUTE_STATE_MATRIX.md`

## Exit criteria for this step

- [ ] All checks above complete
- [ ] If any check fails, execution is blocked and recorded in `13_RESULTS_TEMPLATE.md`
