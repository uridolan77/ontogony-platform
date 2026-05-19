# ontogony-platform — Code Cleaning / Tightening Prompt

```text
We are starting RCQ-CODE-001 for ontogony-platform.

Repo:
- uridolan77/ontogony-platform

Goal:
Tighten platform scripts, schema tests, Docker-local helpers, and package validators before manual QA.

Boundary:
- Code hygiene/correctness only.
- No broad refactor.
- No new product feature.
- No production-readiness claim.
- No secrets.

Tasks:
1. Inspect Docker-local PowerShell scripts for strict mode, deterministic exit codes, and redaction.
2. Inspect product-hardening schema tests and fixtures.
3. Ensure runtime artifacts are not accidentally committed.
4. Check evidence/package validators for stale hardcoded paths.
5. Fix only small correctness/hygiene issues.
6. Do not reorganize docs in this code PR.

Validation:
PowerShell parse checks for touched scripts; relevant .NET schema tests; no runtime artifacts committed.

Acceptance:
- focused checks pass or pre-existing failures documented
- no secrets
- no broad refactor
- not production readiness

Suggested branch:
chore/rcq-code-001-ontogony-platform

Suggested commit:
chore(repo): tighten code hygiene before manual QA
```
