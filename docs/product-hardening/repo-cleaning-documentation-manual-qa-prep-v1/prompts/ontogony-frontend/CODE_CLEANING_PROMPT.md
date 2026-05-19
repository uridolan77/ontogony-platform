# ontogony-frontend — Code Cleaning / Tightening Prompt

```text
We are starting RCQ-CODE-001 for ontogony-frontend.

Repo:
- uridolan77/ontogony-frontend

Goal:
Tighten frontend operator surfaces, adapters, mocks, and fixture/live/degraded behavior before manual QA.

Boundary:
- Code hygiene/correctness only.
- No broad refactor.
- No new product feature.
- No production-readiness claim.
- No secrets.

Tasks:
1. Inspect eval dashboard, eval detail/export, baseline workbench, datasets, run detail journey, replay workbench.
2. Inspect API wrappers for error semantics.
3. Inspect generated-client and adapters.
4. Inspect E2E mocks for backend parity.
5. Inspect route catalogs, replay/fixture/config checks.
6. Fix stale copy, false fallback, brittle selectors, or mismatched mocks.
7. Do not add backend semantics.

Validation:
npm run openapi:check; npm run typecheck; npm run fixtures:check; npm run replay:check; focused tests.

Acceptance:
- focused checks pass or pre-existing failures documented
- no secrets
- no broad refactor
- not production readiness

Suggested branch:
chore/rcq-code-001-ontogony-frontend

Suggested commit:
chore(repo): tighten code hygiene before manual QA
```
