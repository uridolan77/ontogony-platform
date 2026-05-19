# Manual guided QA package

This folder is the canonical operator package for `PRODUCT-MANUAL-QA-001`.

Purpose:

- provide repeatable, route-by-route manual QA checklists for Docker-local product-hardening surfaces
- make expected degraded/fixture/limitation behavior explicit
- standardize evidence capture for `PRODUCT-MANUAL-QA-002` execution

Boundary:

- This package is **not production readiness**.
- No real provider mode, cloud deployment, production identity/TLS/secrets, or unscoped runtime refactors.
- No runtime code changes are defined here; this is operator verification guidance only.

## Files

- `00_MANIFEST.json`
- `01_PRECONDITIONS.md`
- `02_START_STACK_AND_SEED.md`
- `03_GUIDED_MAIN_FLOW.md`
- `04_EVAL_DASHBOARD_CHECKLIST.md`
- `05_EVAL_DETAIL_AND_EXPORT_CHECKLIST.md`
- `06_BASELINE_COMPARISON_CHECKLIST.md`
- `07_SCENARIO_DATASET_CHECKLIST.md`
- `08_RUN_DETAIL_EVIDENCE_JOURNEY_CHECKLIST.md`
- `09_REPLAY_WORKBENCH_CHECKLIST.md`
- `10_TRACE_CONEXUS_KANON_LINKS_CHECKLIST.md`
- `11_FIXTURE_LIVE_DEGRADED_STATES_CHECKLIST.md`
- `12_EXPORT_AND_EVIDENCE_CHECKLIST.md`
- `13_RESULTS_TEMPLATE.md`
- `14_KNOWN_LIMITATIONS.md`

## Related references

- Control package: `docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/`
- PFH closeout: `docs/releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md`
- Frontend docs index: `ontogony-frontend/docs/README.md`
- Frontend fixture/live contract: `ontogony-frontend/docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md`
- Frontend replay contract: `ontogony-frontend/docs/operators/FRONTEND_REPLAY_OPERATOR_CONTRACT.md`
- Allagma docs index: `allagma-dotnet/docs/README.md`
