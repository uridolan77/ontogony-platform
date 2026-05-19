# kanon-dotnet — Documentation Cleaning / Structure Prompt

```text
We are starting RCQ-DOCS-001 for kanon-dotnet.

Repo:
- uridolan77/kanon-dotnet

Goal:
Align Kanon docs and make topology/decision/provenance operator docs easy to find.

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
2. Index topology evidence, topology diagnostics, trace/idempotency, provenance docs.
3. Link platform closeouts and product-hardening evidence.
4. Preserve historical docs in place.
5. Clarify not production readiness.

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
docs/rcq-docs-001-kanon-dotnet

Suggested commit:
docs(repo): align documentation structure before manual QA
```
