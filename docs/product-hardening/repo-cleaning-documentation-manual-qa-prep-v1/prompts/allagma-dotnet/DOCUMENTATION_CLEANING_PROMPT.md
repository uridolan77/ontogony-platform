# allagma-dotnet — Documentation Cleaning / Structure Prompt

```text
We are starting RCQ-DOCS-001 for allagma-dotnet.

Repo:
- uridolan77/allagma-dotnet

Goal:
Align Allagma docs so API, eval, replay, export, evidence, and release docs are discoverable.

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
2. Index docs/api, development, deployment, evidence, operators, releases, testing.
3. Link platform product-hardening closeout.
4. Clarify no live replay trigger unless OpenAPI route exists.
5. Clarify not production readiness / no real provider by default.
6. Avoid mass moving historical docs.

Validation:
Docs review; links spot-checked.

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
docs/rcq-docs-001-allagma-dotnet

Suggested commit:
docs(repo): align documentation structure before manual QA
```
