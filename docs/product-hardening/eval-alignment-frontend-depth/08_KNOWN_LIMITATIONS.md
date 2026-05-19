# 08 — Known Limitations

## Program boundary

- Not production readiness.
- Not real provider mode.
- Not cloud deployment.
- Not identity/TLS/secrets/DR.
- Not a replacement for staging or load testing.

## Eval/product limitations to audit

- Eval dashboard may still rely on per-run sampling rather than a global eval query/list route.
- Replay trigger routes may be missing or explicitly unsupported.
- Baseline comparison may lack history/filter UX.
- Scenario datasets may be represented in docs/catalogs but not first-class UI.
- Judge/scoring metadata may not expose confidence/calibration deeply enough.
- Export bundle may not exist yet as a coherent product artifact.

## Accepted stance

Document limitations honestly. Do not mask missing capabilities with fake success states.
