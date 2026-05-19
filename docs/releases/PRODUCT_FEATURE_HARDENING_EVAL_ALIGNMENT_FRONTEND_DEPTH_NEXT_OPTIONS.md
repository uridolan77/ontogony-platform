# Product feature hardening v1 — next options

**Date:** 2026-05-19  
**After:** `FE-PRODUCT-CLOSEOUT-001` (product hardening package **CLOSED / PASS**)

**Prerequisites closed:** [FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md](./FIRST_DOCKER_LOCAL_WORKING_SYSTEM_CLOSEOUT.md), [POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md](./POST_DOCKER_LOCAL_HARDENING_CLOSEOUT.md)

**This is strategic planning after product hardening v1, not production readiness.**

## Current posture

```text
First Dockerized local working system      CLOSED / PASS
Post-Docker-local hardening                CLOSED / PASS
Product feature hardening v1 (PFH)         CLOSED / PASS
Repo cleaning + manual QA prep (RCQ)       CLOSED / PASS
PRODUCT-MANUAL-QA-002R1 (fake provider)    PASS
Real provider validation v1 (RP-*)       ACTIVE (RP-002 Conexus gate done)
CI-COST-001 (6 repos)                      DONE
Production readiness                       NOT STARTED
```

## Completed in product hardening v1 (not optional tracks)

| Item | Status |
| --- | --- |
| `PFH-000` … `EVAL-PRODUCT-005` | **DONE** |
| `FE-PRODUCT-CLOSEOUT-001` | **DONE** — this closeout |
| `EVAL-POLISH-001` | **DONE** — cross-repo polish (parity + error semantics) |

## Active phase — real provider validation (RP v1)

After fake-provider `PRODUCT-MANUAL-QA-002R1` **PASS**, the next controlled phase is **real provider validation** (still not production readiness):

| Need | Doc |
| --- | --- |
| RP package | [`docs/product-hardening/real-provider-validation-package-v1/`](../product-hardening/real-provider-validation-package-v1/) |
| RP-000 setup evidence | [`docs/evidence/RP_000_PACKAGE_SETUP_EVIDENCE.md`](../evidence/RP_000_PACKAGE_SETUP_EVIDENCE.md) |
| Product hardening index | [`docs/product-hardening/README.md`](../product-hardening/README.md) |

**Done:** `RP-000`–`RP-002` (2026-05-19). **Next:** `RP-003` Allagma guided flow. Policy: [`REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md`](../operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md). Conexus evidence: [`RP_002_CONEXUS_REAL_PROVIDER_LOCAL_MODE_EVIDENCE.md`](../evidence/RP_002_CONEXUS_REAL_PROVIDER_LOCAL_MODE_EVIDENCE.md).

**Closed (prerequisite):** RCQ + `PRODUCT-MANUAL-QA-002R1` — see [`RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md`](../evidence/RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md), [`PRODUCT_MANUAL_QA_002R1_EXECUTION_EVIDENCE.md`](../evidence/PRODUCT_MANUAL_QA_002R1_EXECUTION_EVIDENCE.md).

## Remaining optional tracks

Pick based on team priority. None are required to use the closed Docker-local stack with current eval operator surfaces.

### Option 1 — PFH v1 follow-ups (product depth)

**When:** Operator UX polish is still the bottleneck after v1 closeout.

| Track | Example | Package reference |
| --- | --- | --- |
| Saved views / bookmarks | Dashboard and dataset operator presets | Gap matrix P2 rows |
| Rich baseline diff | Side-by-side semantic outcome viewer | `05_FRONTEND_OPERATOR_DEPTH_GAP_MATRIX.md` |
| Bulk eval export | Multi-eval or compliance-oriented archive | Post-`EVAL-PRODUCT-005` |
| Replay fixture catalog | `?replayFixture=` on replay route | `FE-PRODUCT-003` follow-up |
| Live replay trigger | Requires Allagma OpenAPI route first | Backend + FE |
| `ALIGN-PRODUCT-002`–`004` | Capability registry, OpenAPI discipline, cross-service id map | `pr-specs/ALIGN-PRODUCT-*.md` |

### Option 2 — Real provider validation v1 (`RP-*`) — **ACTIVE**

**When:** Fake-provider manual QA is PASS and you need controlled **local** real-provider validation through Conexus/Allagma/frontend — still not production.

Package: [`docs/product-hardening/real-provider-validation-package-v1/`](../product-hardening/real-provider-validation-package-v1/). Legacy pointer: [POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md](./POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md#option-1--env-real-provider-001-optional-backend-smoke).

### Option 3 — `PROD-READINESS-001+` (separate program)

**When:** Deploy, identity, TLS, DR, and provider policy are first-class.

Do not relabel PFH closeout as production readiness. Start a new program charter.

### Option 4 — Cloud / deployment program

**When:** Staging/prod topology, IaC, and release pipelines beyond compose.

Deferred from ENV: runtime Vite injection in frontend image; full compose-in-CI.

## Recommended default (no single mandate)

Most teams should **keep using the closed Docker-local stack daily** and choose **one** optional track:

1. **Real provider validation** → Option 2 (`RP-*`, active after manual QA PASS)  
2. **Product polish** → Option 1 (PFH v1 follow-ups above)  
3. **Deploy timeline** → Option 3 (`PROD-READINESS-*`)  
4. **Hosted environments** → Option 4 (cloud/deployment program)

## Operator quick reference

| Need | Doc |
| --- | --- |
| PFH closeout | [PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md](./PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md) |
| PFH scorecard | [PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_SCORECARD.md](./PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_SCORECARD.md) |
| PFH limitations | [PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_KNOWN_LIMITATIONS.md](./PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_KNOWN_LIMITATIONS.md) |
| Package control | [eval-alignment-frontend-depth/](../product-hardening/eval-alignment-frontend-depth/) |
| Compose stack | `docker/local-working-system/README.md` |
| Frontend `check` | `ontogony-frontend`: `npm run check` |

## Required statement

```text
Product feature hardening v1 is closed.
It does not constitute production readiness, real provider mode, or cloud deployment.
```
