# ontogony-platform — Documentation Cleaning / Structure Prompt

```text
We are starting RCQ-DOCS-001 for ontogony-platform.

Repo:
- uridolan77/ontogony-platform

Goal:
Align platform documentation as the canonical cross-repo index without mass-moving historical docs.

Boundary:
- Documentation cleanup only.
- Keep reorganization manageable.
- Do not rewrite historical archives.
- Do not move large doc trees.
- No runtime changes.
- No workflow changes.
- No secrets.
- Not production readiness.

Use:
- ontogony-platform/docs/operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md

Tasks:
1. Update docs/product-hardening/README.md and/or docs/README.md.
2. Index current closeouts, evidence, product-hardening packages, and manual QA prep.
3. Correct stale next/in-progress statuses.
4. Link the unified documentation standard.
5. Mark historical packages where needed.
6. Avoid moving archives or _incoming content broadly.

Validation:
Docs link review; no runtime changes.

Acceptance:
- docs index updated or created
- current docs discoverable
- evidence docs discoverable
- stale status drift fixed
- limitations consolidated
- no mass reorg
- no secrets
- not production readiness

Suggested branch:
docs/rcq-docs-001-ontogony-platform

Suggested commit:
docs(repo): align documentation structure before manual QA
```
