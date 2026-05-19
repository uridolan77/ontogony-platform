# RCQ-000 — Package Setup

```text
We are starting RCQ-000 — Repo cleaning, documentation standard, and manual QA prep package setup.

Repo:
- uridolan77/ontogony-platform

Goal:
Register the repo-cleaning/documentation/manual-QA prep package.

Boundary:
- Docs/package setup only.
- No runtime changes.
- No workflow changes.
- No real provider mode.
- No cloud deployment.
- Not production readiness.
- No secrets.

Tasks:
1. Copy zip to docs/_incoming/Ontogony-Repo-Cleaning-Documentation-Manual-QA-Prep-v1.zip.
2. Unpack to docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/.
3. Add docs/evidence/RCQ_000_PACKAGE_SETUP_EVIDENCE.md.
4. Add pointer from docs/product-hardening/README.md.
5. Do not start code/docs cleanup yet.

Acceptance:
- manifest parses
- all prompts present
- no runtime changes
- no workflow changes
- not production readiness

Suggested commit:
docs(product): add repo cleaning and manual QA prep package
```
