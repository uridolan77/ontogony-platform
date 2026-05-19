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

## Closed package — Repo cleaning / documentation / manual QA prep (v1)

**Planning/control package:**

- [`docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/`](repo-cleaning-documentation-manual-qa-prep-v1/)

**Incoming ZIP (preserved):**

- `docs/_incoming/Ontogony-Repo-Cleaning-Documentation-Manual-QA-Prep-v1.zip`

**Status:** **CLOSED / PASS** (2026-05-19) — `DOCS-STANDARD-001`, six-repo RCQ sweeps, `PRODUCT-MANUAL-QA-002R1` manual QA rerun.

Evidence:

- [`RCQ_000_PACKAGE_SETUP_EVIDENCE.md`](../evidence/RCQ_000_PACKAGE_SETUP_EVIDENCE.md)
- [`DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md`](../evidence/DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md)
- [`RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md`](../evidence/RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md)
- [`PRODUCT_MANUAL_QA_002R1_EXECUTION_EVIDENCE.md`](../evidence/PRODUCT_MANUAL_QA_002R1_EXECUTION_EVIDENCE.md)

**Governance (published):** [`docs/operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md`](../operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md) (`DOCS-STANDARD-001`, 2026-05-19).

Manual guided QA package: [`docs/product-hardening/manual-guided-qa/`](manual-guided-qa/).

## Active next package — Real provider validation (v1)

**Fake-provider Docker-local manual QA is closed.** The active next controlled phase is:

- [`docs/product-hardening/real-provider-validation-package-v1/`](real-provider-validation-package-v1/)

**Incoming ZIP (preserved):**

- `docs/_incoming/Ontogony-Real-Provider-Validation-Package-v1.zip`

**Steps:**

| Step | Status | Evidence |
| --- | --- | --- |
| `RP-000` package setup | **DONE** (2026-05-19) | [`RP_000_PACKAGE_SETUP_EVIDENCE.md`](../evidence/RP_000_PACKAGE_SETUP_EVIDENCE.md) |
| `RP-001` secret, budget, safety gates | **DONE** (2026-05-19) | [`RP_001_SECRET_BUDGET_SAFETY_GATES_EVIDENCE.md`](../evidence/RP_001_SECRET_BUDGET_SAFETY_GATES_EVIDENCE.md) |
| `RP-002` Conexus real-provider local mode | **DONE** (2026-05-19) | [`RP_002_CONEXUS_REAL_PROVIDER_LOCAL_MODE_EVIDENCE.md`](../evidence/RP_002_CONEXUS_REAL_PROVIDER_LOCAL_MODE_EVIDENCE.md) |
| `RP-003` Allagma real-provider guided flow | **DONE** (2026-05-19) | [`RP_003_ALLAGMA_REAL_PROVIDER_GUIDED_FLOW_EVIDENCE.md`](../evidence/RP_003_ALLAGMA_REAL_PROVIDER_GUIDED_FLOW_EVIDENCE.md) |
| `RP-003A` live provider completion (Docker TLS) | **DONE** (2026-05-19) | [`RP_003A_LIVE_REAL_PROVIDER_COMPLETION_EVIDENCE.md`](../evidence/RP_003A_LIVE_REAL_PROVIDER_COMPLETION_EVIDENCE.md) |
| `RP-004` frontend operator visibility | **Next** | — |

**Operator policy:** [`docs/operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md`](../operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md)

Start with:

- `real-provider-validation-package-v1/prompts/RP-004_FRONTEND_REAL_PROVIDER_OPERATOR_VISIBILITY.md`
- `conexus-dotnet/docs/development/REAL_PROVIDER_LOCAL_VALIDATION.md`

## Related (not superseded — different scope)

| Area | Path | Notes |
| --- | --- | --- |
| Eval durability → first sanity (prior program) | `docs/planning/eval-durability-to-first-sanity-current/` | Completed baseline path; informs gaps |
| Eval full-sanity alignment snapshot | `docs/alignment/eval-full-sanity-alignment/` | ALIGN-EVAL-001 refresh |
| Backend/frontend sandbox alignment | `docs/alignment/backend-frontend-phase-v2-sandbox-evidence-alignment/` | OpenAPI provenance patterns |
| Docker-local environment | `docs/environments/` | Closed ENV + post-Docker hardening |

## Boundary

Product hardening improves eval product depth, contract alignment, and operator-facing frontend surfaces on the **closed Docker-local foundation**.

The **real provider validation** package (`RP-*`) is a separate controlled phase for **local** real-provider smoke and manual QA after fake-provider manual QA PASS. It is still **not production readiness** — no cloud deployment, no CI real-provider calls, and no secrets in repo.

Production readiness remains **not started** unless explicitly chartered elsewhere.
