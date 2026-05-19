# RP-002 — Conexus Real Provider Local Mode Prompt

```text
We are starting RP-002 — Conexus real provider local mode.

Repos:
- uridolan77/conexus-dotnet
- uridolan77/ontogony-platform

Prerequisite:
- RP-001 is done.

Goal:
Validate a single small real-provider call through Conexus local mode with budget and redaction controls.

Boundary:
- Local validation only.
- No production readiness.
- No cloud deployment.
- No CI real-provider call.
- No secrets committed.
- No broad provider redesign.
- Fake provider remains default.

Tasks:
1. Inspect existing Conexus provider configuration, readiness, fake bootstrap, model alias/project behavior, and route/model-call evidence.
2. Prefer existing Conexus config keys over inventing parallel config.
3. Add/refine local-only validation script if needed.
4. Require explicit enable gate.
5. Classify missing/invalid key/provider errors.
6. Run one tiny real-provider call with small model and output cap.
7. Persist route/model-call evidence.
8. Redact prompt/completion/headers/secrets according to policy.
9. Disable real mode and rerun fake-provider smoke.

Deliver:
- conexus docs/evidence/RP_002_CONEXUS_REAL_PROVIDER_LOCAL_MODE_EVIDENCE.md
- platform docs/evidence/RP_002_CONEXUS_REAL_PROVIDER_LOCAL_MODE_EVIDENCE.md

Acceptance:
- one real call succeeds or external failure is precisely classified
- route/model-call evidence exists
- fake provider regression passes
- no secrets
- not production readiness

Suggested commits:
- feat(conexus): RP-002 validate real provider local mode
- docs(product): RP-002 record Conexus real provider local validation
```
