# Product hardening planning docs

This folder holds **canonical control packages** for post-infrastructure product depth — eval semantics, backend/frontend alignment, and frontend operator UX.

## Closed package — Eval / alignment / frontend depth (v1)

**Planning/control package:**

- [`docs/product-hardening/eval-alignment-frontend-depth/`](eval-alignment-frontend-depth/)

**Incoming ZIP (preserved):**

- `docs/_incoming/Ontogony-Product-Feature-Hardening-Eval-Alignment-Frontend-Depth-v1.zip`

**Prerequisites (closed):**

- First Dockerized local working system — [`FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md`](../releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md)
- Post-Docker-local hardening — [`POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md`](../releases/POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md)
- `CI-COST-001` across six repos

**Status:** **CLOSED / PASS** (`FE-PRODUCT-CLOSEOUT-001`, 2026-05-19). See [closeout](../releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md).

Start with:

- `eval-alignment-frontend-depth/00_MANIFEST.json`
- `eval-alignment-frontend-depth/02_PRODUCT_HARDENING_SEQUENCE.md`
- `eval-alignment-frontend-depth/pr-specs/`

Evidence:

- [`PFH_000_PACKAGE_SETUP_EVIDENCE.md`](../evidence/PFH_000_PACKAGE_SETUP_EVIDENCE.md)
- [`PFH_001_CURRENT_STATE_AUDIT_EVIDENCE.md`](../evidence/PFH_001_CURRENT_STATE_AUDIT_EVIDENCE.md)
- [`FE_PRODUCT_CLOSEOUT_001_EVIDENCE.md`](../evidence/FE_PRODUCT_CLOSEOUT_001_EVIDENCE.md)

Release pointer: [`PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH.md`](../releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH.md)

## Active next package — Repo cleaning / documentation / manual QA prep (v1)

**Product feature hardening v1 is closed.** The active next preparation package is:

- [`docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/`](repo-cleaning-documentation-manual-qa-prep-v1/)

**Incoming ZIP (preserved):**

- `docs/_incoming/Ontogony-Repo-Cleaning-Documentation-Manual-QA-Prep-v1.zip`

Start with:

- `repo-cleaning-documentation-manual-qa-prep-v1/00_MANIFEST.json`
- `repo-cleaning-documentation-manual-qa-prep-v1/01_EXECUTIVE_PLAN.md`
- `repo-cleaning-documentation-manual-qa-prep-v1/sequences/01_REPO_CLEANING_SEQUENCE.md`

Evidence: [`RCQ_000_PACKAGE_SETUP_EVIDENCE.md`](../evidence/RCQ_000_PACKAGE_SETUP_EVIDENCE.md)

**Next step after `RCQ-000`:** **`DOCS-STANDARD-001`** — unified documentation structure standard (`prompts/00-meta/DOCS-STANDARD-001_UNIFIED_DOCUMENTATION_STANDARD.md`).

## Related (not superseded — different scope)

| Area | Path | Notes |
| --- | --- | --- |
| Eval durability → first sanity (prior program) | `docs/planning/eval-durability-to-first-sanity-current/` | Completed baseline path; informs gaps |
| Eval full-sanity alignment snapshot | `docs/alignment/eval-full-sanity-alignment/` | ALIGN-EVAL-001 refresh |
| Backend/frontend sandbox alignment | `docs/alignment/backend-frontend-phase-v2-sandbox-evidence-alignment/` | OpenAPI provenance patterns |
| Docker-local environment | `docs/environments/` | Closed ENV + post-Docker hardening |

## Boundary

Product hardening improves eval product depth, contract alignment, and operator-facing frontend surfaces on the **closed Docker-local foundation**.

It is **not production readiness**. No real provider mode, cloud deployment, production identity/TLS/secrets, or unscoped runtime refactors belong in this program unless explicitly chartered elsewhere.
