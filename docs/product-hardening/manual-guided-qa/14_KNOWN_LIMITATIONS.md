# 14 — Known limitations (manual QA context)

Manual QA must treat the following as expected unless a separate change explicitly closes them.

Source-of-truth references:

- `docs/releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_KNOWN_LIMITATIONS.md`
- `ontogony-frontend/docs/operators/FRONTEND_FIXTURE_LIVE_BOUNDARY.md`
- `ontogony-frontend/docs/operators/FRONTEND_REPLAY_OPERATOR_CONTRACT.md`

## Accepted limitations for this package

- Replay trigger mutation is not exposed by default OpenAPI snapshot; replay is lookup/evidence-focused.
- Dashboard pagination is bounded; full history UX is deferred.
- Dataset and baseline filters depend on sparse metadata in some evaluation records.
- Baseline creation operator form is not provided (harness/smoke posture).
- Scenario dataset surfaces are read-only (no dataset authoring workflow).
- Eval export is per-evaluation and operator-oriented, not bulk/compliance archive.
- Rich semantic side-by-side diff viewer is not part of current baseline detail.
- In-memory data durability constraints remain unless Postgres mode is explicitly used.

## Non-negotiable safety rules

- No fake success states for missing backend operations.
- No silent fallback from live failure to fixture data.
- No secret/token leakage in UI or evidence artifacts.
- No production readiness claims in manual QA output.

## Required statement for reports

```text
This work is NOT production readiness. It does not authorize real provider mode by default,
cloud deployment, production identity/TLS/secrets, or unscoped runtime refactors.
```
