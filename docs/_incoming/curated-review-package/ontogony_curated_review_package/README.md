# Ontogony curated review implementation package

Generated: 2026-05-20

This package consolidates the four uploaded reviews into an implementation-ready planning set. It preserves only current, relevant issues and suggestions after checking the most important disputed points against the current repositories.

## Package contents

- `00_EXECUTIVE_BRIEF.md` — concise synthesis and ordering.
- `01_CURATED_BACKLOG.md` — prioritized issues grouped by system and repo.
- `02_SPRINT_PLAN.md` — recommended PR sequence.
- `03_ACCEPTANCE_MATRIX.md` — acceptance criteria by issue.
- `04_VERIFICATION_CHECKLIST.md` — manual/CI validation checklist.
- `05_CURRENT_STATE_EVIDENCE.md` — evidence notes used for curation.
- `repo-plans/` — repo-specific plans.
- `issue-cards/` — one PR-sized issue card per retained item.
- `data/curated_backlog.json` and `data/curated_backlog.csv` — machine-readable backlog.

## Curation rule

The original reviews were used as input, but this package is not a restatement of them. Items that no longer match current repo state were not carried forward. The goal is an actionable implementation package, not another review report.
