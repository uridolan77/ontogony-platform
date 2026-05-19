# ontogony-ui — Documentation Cleaning / Structure Prompt

```text
We are starting RCQ-DOCS-001 for ontogony-ui.

Repo:
- uridolan77/ontogony-ui

Goal:
Align UI package docs around public API, consumer imports, packaging, and component guidance.

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
1. Add/update docs/README.md.
2. Index public API policy, consumer imports, package strategy, packaging status, component docs, evidence.
3. Remove stale conexus-frontend wording.
4. Clarify DevRoot/file-link vs workspace language.
5. Avoid broad historical doc moves.

Validation:
Docs review; package docs/check if available.

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
docs/rcq-docs-001-ontogony-ui

Suggested commit:
docs(repo): align documentation structure before manual QA
```
