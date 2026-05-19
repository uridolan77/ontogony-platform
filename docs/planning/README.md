# Planning docs

This folder contains implementation planning packages for Ontogony.Platform and cross-repo programs that originate from platform planning.

**Documentation map:** [`docs/README.md`](../README.md)  
**Active preparation program (not production readiness):** [`docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/`](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/)

## Closed — Feature hardening v1 (eval / alignment / frontend depth)

> **Historical** — Program **CLOSED / PASS** (2026-05-19). Current operator guidance: closeout and evidence links below; active prep is RCQ (repo cleaning / manual QA).

Path:

- [`docs/product-hardening/eval-alignment-frontend-depth/`](../product-hardening/eval-alignment-frontend-depth/)

Closeout: [`docs/releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md`](../releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md)

Incoming ZIP (preserved): `docs/_incoming/Ontogony-Product-Feature-Hardening-Eval-Alignment-Frontend-Depth-v1.zip`

**Not production readiness.**

## Prior package — Eval Durability → First Full Sanity

Path:

- `docs/planning/eval-durability-to-first-sanity-current/`

Purpose:

Completed eval planning path after the eval-basing sequence (EVAL-FIX-001 through SYS-OBS-EVAL-001). Retained as baseline context for the product-hardening gap matrices; superseded as the **active** program by `docs/product-hardening/`.

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
