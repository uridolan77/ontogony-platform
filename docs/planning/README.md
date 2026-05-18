# Planning docs

This folder contains implementation planning packages for Ontogony.Platform and cross-repo programs that originate from platform planning.

## Active package — Eval Durability → First Full Sanity (current)

Path:

- `docs/planning/eval-durability-to-first-sanity-current/`

Purpose:

This is the current active eval planning package after the completed eval-basing sequence (EVAL-FIX-001 through SYS-OBS-EVAL-001). It defines the path from durable eval/baseline persistence through CI gates, quality/data improvements, polish, alignment refresh, and the first full cross-repo sanity milestone.

Incoming ZIP preserved at:

- `docs/_incoming/ontogony-eval-durability-to-first-sanity-current-package.zip`

Start with:

- `01_CURRENT_STATE.md`
- `03_NEXT_SEQUENCE.md`
- `04_PR_SPECS/`

## Superseded incoming package (historical)

The earlier incoming eval hardening package remains preserved as historical planning input:

- `docs/_incoming/ontogony-eval-hardening-to-full-sanity-package.zip`

That package was useful but predates the completed eval-basing baseline. It is superseded by the current package above—not wrong, just historical relative to the latest repo state.

## Other planning packages

These remain as separate, non-eval planning material:

- `docs/planning/ai-runtime-prs/`
- `docs/planning/conexus-support/`
- `docs/planning/next-phase/`
- `docs/planning/ontogony-platform-next-prs/`
- `docs/planning/robustness/`

Eval-basing foundation (completed baseline context):

- `docs/eval-basing/ontogony-eval-based-cross-repo-development-package/`

Cross-repo alignment (sandbox evidence, pre-eval-basing):

- `docs/alignment/README.md`

Notes:

- Planning packages are not runtime code; implementation PRs belong in the relevant repo.
- Do not start EVAL-DUR-001 from an unpack-only PR.
