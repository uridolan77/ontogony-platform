# conexus-dotnet — Documentation Cleaning / Structure Prompt

```text
We are starting RCQ-DOCS-001 for conexus-dotnet.

Repo:
- uridolan77/conexus-dotnet

Goal:
Align Conexus docs around persistence, readiness, bootstrap, provider, and evidence.

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
2. Index startup/readiness, Docker-local, migration runbook, provider/bootstrap, evidence docs.
3. Make /health/live vs /ready prominent.
4. Link platform Conexus evidence.
5. Keep real provider / production readiness out of scope.

Validation:
Docs review; no runtime changes.

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
docs/rcq-docs-001-conexus-dotnet

Suggested commit:
docs(repo): align documentation structure before manual QA
```
