# Prompt 01 — Implement

Implement `EVAL-EVIDENCE-QUALITY-001` according to the package and the repo-grounded plan.

Focus on:

1. Current-page/source-basis labels.
2. Metadata completeness summary.
3. Explicit missing provider/dataset/comparison/token states.
4. Evidence Spine link quality.
5. Baseline comparison empty/absent state.
6. Quality preview wording.
7. Tests for sparse metadata and evidence links.

Rules:

- Do not invent backend fields.
- Do not introduce broad rewrites.
- Do not change scoring semantics.
- Prefer view-model helpers/adapters over component sprawl.
- Keep styling aligned with existing components.
- Add backend follow-ups only as docs/comments where appropriate.
