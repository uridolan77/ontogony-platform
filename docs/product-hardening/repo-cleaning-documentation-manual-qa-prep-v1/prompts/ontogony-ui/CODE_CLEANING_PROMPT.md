# ontogony-ui — Code Cleaning / Tightening Prompt

```text
We are starting RCQ-CODE-001 for ontogony-ui.

Repo:
- uridolan77/ontogony-ui

Goal:
Tighten shared UI package exports, consumer contract, and package smoke checks.

Boundary:
- Code hygiene/correctness only.
- No broad refactor.
- No new product feature.
- No production-readiness claim.
- No secrets.

Tasks:
1. Inspect public exports and consumer import rules.
2. Inspect build/typecheck/pack/smoke/export checks.
3. Inspect primitives used by evidence/export/workbench surfaces.
4. Inspect Storybook/Vitest browser workspace needs.
5. Add/adjust consumer contract tests if needed.
6. Preserve frontend compatibility.

Validation:
npm run build; npm run test:run; check:exports; check:smoke-dist where available.

Acceptance:
- focused checks pass or pre-existing failures documented
- no secrets
- no broad refactor
- not production readiness

Suggested branch:
chore/rcq-code-001-ontogony-ui

Suggested commit:
chore(repo): tighten code hygiene before manual QA
```
