# Post-Docker-local hardening — next options

**Date:** 2026-05-19  
**After:** `POST-DOCKER-HARDENING-CLOSEOUT-001` (hardening package **CLOSED / PASS**, including `CI-COST-001`)

**Prerequisite closed:** [FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md](./FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md)

**This is strategic planning after local hardening, not production readiness.**

## Current posture

```text
First Dockerized local working system      CLOSED / PASS  (ENV-DOCKER-CLOSEOUT-001)
Post-Docker-local hardening                CLOSED / PASS  (POST-DOCKER-HARDENING-CLOSEOUT-001)
Product feature hardening v1 (PFH)         CLOSED / PASS  (FE-PRODUCT-CLOSEOUT-001)
CI-COST-001 (6 repos)                      DONE
Production readiness                       NOT STARTED
```

## Completed in this package (not optional tracks)

| Item | Status |
| --- | --- |
| `CONEXUS-PERSIST-001` … `TERMINOLOGY-CLEANUP-001` | **DONE** |
| `CI-COST-001` | **DONE** — path filters, aggregate checks, operator guide |
| `POST-DOCKER-HARDENING-CLOSEOUT-001` | **DONE** — this closeout |

**Optional manual follow-up for CI:** Configure branch protection when ready — require aggregate checks only (see [POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md](./POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md#ci-cost-001-summary)).

## Remaining optional tracks

Pick based on team priority. None are required to use the closed Docker-local stack.

### Option 1 — `ENV-REAL-PROVIDER-001` (optional backend smoke)

**When:** You need confidence that a **real** Conexus provider path works with explicit keys — still not production.

| Aspect | Notes |
| --- | --- |
| Scope | Backend-only; explicit opt-in keys; separate evidence |
| Risk | Cost, credential handling, non-determinism |
| Does not replace | Fake/local default for daily Docker-local dev |

### Option 2 — `PROD-READINESS-001+` (separate program)

**When:** Deploy, identity, TLS, DR, and provider policy are first-class — **not** an extension of Docker-local.

| Aspect | Notes |
| --- | --- |
| Prerequisite | Closed local + hardening packages (satisfied) |
| Out of scope for ENV | Cloud manifests, staging/prod identity, SLOs |
| Start | New program charter + acceptance matrix — do not relabel ENV closeout as prod |

### Option 3 — Product feature hardening v1 (**closed**)

**Status:** **CLOSED / PASS** (`FE-PRODUCT-CLOSEOUT-001`, 2026-05-19).

**Closeout:**

- [`PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md`](./PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md)
- [`PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_NEXT_OPTIONS.md`](./PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_NEXT_OPTIONS.md) — v1 follow-ups

**For new product depth work**, start a new charter or pick items from the PFH next-options doc (saved views, bulk export, `ALIGN-PRODUCT-002`–`004`, replay fixture catalog, etc.).

| Track | Example entry points |
| --- | --- |
| PFH v1 follow-ups | `PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_NEXT_OPTIONS.md` |
| Prior eval durability | `docs/planning/eval-durability-to-first-sanity-current/` |
| Alignment baseline | `docs/alignment/eval-full-sanity-alignment/`, `docs/alignment/backend-frontend-phase-v2-sandbox-evidence-alignment/` |
| Allagma eval closeout | `allagma-dotnet/docs/releases/FIRST_FULL_SANITY_CLOSEOUT.md` |

### Option 4 — Cloud / deployment program

**When:** You need staging/prod topology, IaC, and release pipelines beyond compose.

| Aspect | Notes |
| --- | --- |
| Scope | Manifests, environments, secrets management, deploy runbooks |
| Relationship | Builds on closed local system; does not retroactively change ENV acceptance |
| Deferred from ENV | Runtime Vite config injection in frontend image; full compose-in-CI |

## Recommended default (no single mandate)

Most teams should **keep using the closed Docker-local stack daily** and choose **one** optional track:

1. **Product polish** → Option 3 follow-ups ([PFH next options](./PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_NEXT_OPTIONS.md))  
2. **Provider confidence** → Option 1 (`ENV-REAL-PROVIDER-001`)  
3. **Deploy timeline** → Option 2 (`PROD-READINESS-*`)  
4. **Hosted environments** → Option 4 (cloud/deployment program)

## Active preparation — repo cleaning / manual QA (RCQ)

**Status:** Package registered (`RCQ-000`); unified documentation standard published (`DOCS-STANDARD-001`, 2026-05-19).

| Need | Doc |
| --- | --- |
| RCQ control package | [`docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/`](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/) |
| Documentation structure (six repos) | [`docs/operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md`](../operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md) |
| Platform docs index | [`docs/README.md`](../README.md) |
| RCQ / DOCS-STANDARD evidence | [`docs/evidence/RCQ_000_PACKAGE_SETUP_EVIDENCE.md`](../evidence/RCQ_000_PACKAGE_SETUP_EVIDENCE.md), [`docs/evidence/DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md`](../evidence/DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md) |

**Done:** `DOCS-STANDARD-001`, six-repo `RCQ-CODE-001` + `RCQ-DOCS-001`, `RCQ-DOCS-FINAL-001` closeout — [`RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md`](../evidence/RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md).

**Next:** `PRODUCT-MANUAL-QA-002` execution. `PRODUCT-MANUAL-QA-001` package is **done** (2026-05-19) — see [`../evidence/PRODUCT_MANUAL_QA_001_PACKAGE_EVIDENCE.md`](../evidence/PRODUCT_MANUAL_QA_001_PACKAGE_EVIDENCE.md). **Not production readiness.**

## Operator quick reference (unchanged)

| Need | Doc |
| --- | --- |
| Glossary | `docs/operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md` |
| Compose stack | `docker/local-working-system/README.md` |
| Hardening closeout | `docs/releases/POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md` |
| Docker-local closeout | `docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md` |
| Frontend `check` | `ontogony-frontend`: `npm run check` |
| CI cost / protection | `docs/operators/CI_COST_CONTROL.md` |

## Required statement

```text
The first Dockerized local working system and post-Docker-local hardening are closed.
CI-COST-001 is done. Neither milestone is production readiness.
```
