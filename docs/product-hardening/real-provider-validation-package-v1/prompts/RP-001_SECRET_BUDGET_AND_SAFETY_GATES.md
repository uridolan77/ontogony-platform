# RP-001 — Secret, Budget, and Safety Gates Prompt

```text
We are starting RP-001 — Secret, budget, and safety gates.

Repos:
- uridolan77/ontogony-platform
- uridolan77/conexus-dotnet

Goal:
Define and enforce the local-only safety posture before any real provider call is possible.

Boundary:
- Safety/docs/config guardrails only.
- No real provider calls in this PR.
- No provider keys committed.
- No CI real-provider tests.
- Fake provider remains default.
- Not production readiness.

Tasks:
1. Document real-provider disabled-by-default policy.
2. Document local-only secret handling.
3. Document budget caps and kill switch.
4. Document failure classes.
5. Add `.env.example` placeholders only if needed.
6. Add config validation only if it prevents accidental real-provider mode.
7. Add evidence in both platform and Conexus.

Deliver:
- platform: docs/operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md
- platform: docs/evidence/RP_001_SECRET_BUDGET_SAFETY_GATES_EVIDENCE.md
- conexus: docs/development/REAL_PROVIDER_LOCAL_VALIDATION.md
- conexus: docs/evidence/RP_001_SECRET_BUDGET_SAFETY_GATES_EVIDENCE.md

Acceptance:
- fake provider remains default
- real-provider requires explicit opt-in
- no CI real calls
- no secrets
- not production readiness

Suggested commits:
- docs(product): RP-001 define real provider validation safety gates
- docs(conexus): RP-001 document real provider local safety gates
```
