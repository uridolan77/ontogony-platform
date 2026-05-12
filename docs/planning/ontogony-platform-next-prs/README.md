# Ontogony Platform — Next Infrastructure PR Package

This package is a planning and execution kit for continuing `ontogony-platform` after PR25.

Current assumption:

- PR25 has landed or is staged as `Unreleased` in `CHANGELOG.md`.
- Agentor and Athanor should be paused except for critical compatibility fixes.
- The next phase should make `ontogony-platform` more robust, extensive, documented, and production-shaped without leaking product/domain semantics.

Core rule:

```text
Share mechanics. Do not share meaning.
```

## Recommended execution order

1. PR26 — `Ontogony.Hosting` service defaults.
2. PR27 — Postgres outbox provider package.
3. PR28 — protocol ingress normalization layer.
4. PR29 — production service-identity security hardening.
5. PR30 — observability operations pack.
6. PR31 — schema governance and compatibility tests.
7. PR32 — advanced HTTP resilience policies.
8. PR33 — conformance testing package.
9. PR34 — release automation and quality gates.
10. PR35 — documentation-first developer experience.

## Package contents

- `docs/00_CURRENT_STATUS_AFTER_PR25.md`
- `docs/01_ROADMAP_PR26_TO_PR35.md`
- `docs/02_PACKAGE_BOUNDARY_MATRIX.md`
- `docs/03_DECISIONS_TO_LOCK.md`
- `pr-specs/PR26_...md` through `PR35_...md`
- `cursor-prompts/PR26_...md` through `PR35_...md`
- `checklists/PLATFORM_INVARIANTS.md`
- `checklists/PR_ACCEPTANCE_CHECKLIST.md`
- `templates/PR_SPEC_TEMPLATE.md`
- `templates/CURSOR_IMPLEMENTATION_PROMPT_TEMPLATE.md`
