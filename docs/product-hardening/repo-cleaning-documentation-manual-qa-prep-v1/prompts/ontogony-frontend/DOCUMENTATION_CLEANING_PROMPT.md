# ontogony-frontend — Documentation Cleaning / Structure Prompt

```text
We are starting RCQ-DOCS-001 for ontogony-frontend.

Repo:
- uridolan77/ontogony-frontend

Goal:
Align frontend docs around operator contracts, fixture/live rules, test catalogs, and evidence.

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
2. Index operator contracts, Docker-local walkthrough, fixture/live boundary, config contract, replay/catalog docs, evidence.
3. Correct stale route/capability wording.
4. Clarify no fake replay trigger and no fixture fallback on live failure.
5. Avoid broad doc moves.

Validation:
fixtures:check/replay:check if catalogs touched; docs review.

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
docs/rcq-docs-001-ontogony-frontend

Suggested commit:
docs(repo): align documentation structure before manual QA
```
