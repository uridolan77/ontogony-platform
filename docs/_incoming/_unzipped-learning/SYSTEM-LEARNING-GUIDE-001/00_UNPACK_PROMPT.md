# Cursor unpack prompt — SYSTEM-LEARNING-GUIDE-001

You are implementing SYSTEM-LEARNING-GUIDE-001 across the Ontogony repos.

Goal: create canonical learning guides for understanding, running, using, debugging, and extending Ontogony without adding random parallel docs.

Rules:

1. Verify current files/scripts before writing instructions.
2. Current code and generated artifacts beat stale prose.
3. Keep cross-system learning docs in `ontogony-platform/docs/learn/`.
4. Keep repo-specific implementation detail in the owning repo and link to it.
5. Link to generated artifacts; do not manually copy route inventories or coverage tables.
6. Do not delete docs in this pass. Mark stale docs and prepare archive/delete candidates.
7. Every guide must declare audience and owning repo scope.
8. Every command shown must exist or be explicitly marked as proposed.
9. Do not claim production readiness/security maturity beyond current evidence.
10. Run the acceptance checklist before finalizing.

Expected first action: perform the current-state docs/code audit in `01_DOCS_CURRENT_STATE_AUDIT.md`, updating it with real file paths and command evidence from the local repos.
