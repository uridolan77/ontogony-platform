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
CI-COST-001 (6 repos)                      DONE
Production readiness                       NOT STARTED
```

## Completed in product hardening v1 (not optional tracks)

| Item | Status |
| --- | --- |
| `PFH-000` … `EVAL-PRODUCT-005` | **DONE** |
| `FE-PRODUCT-CLOSEOUT-001` | **DONE** — this closeout |
| `EVAL-POLISH-001` | **DONE** — cross-repo polish (parity + error semantics) |

## Active preparation — repo cleaning / manual QA (RCQ)

Before full manual guided QA, the six repos follow the **repo cleaning + documentation + manual QA prep** program (not production readiness):

| Need | Doc |
| --- | --- |
| RCQ package | [`docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/`](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/) |
| Unified documentation standard | [`docs/operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md`](../operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md) |
| Platform docs index | [`docs/README.md`](../README.md) |

`DOCS-STANDARD-001` is **published** (2026-05-19). Next: `RCQ-CODE-001` per repo, then `RCQ-DOCS-001`, then `PRODUCT-MANUAL-QA-001` / `002`.

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

### Option 2 — `ENV-REAL-PROVIDER-001` (optional backend smoke)

**When:** You need confidence that a **real** Conexus provider path works with explicit keys — still not production.

See [POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md](./POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md#option-1--env-real-provider-001-optional-backend-smoke).

### Option 3 — `PROD-READINESS-001+` (separate program)

**When:** Deploy, identity, TLS, DR, and provider policy are first-class.

Do not relabel PFH closeout as production readiness. Start a new program charter.

### Option 4 — Cloud / deployment program

**When:** Staging/prod topology, IaC, and release pipelines beyond compose.

Deferred from ENV: runtime Vite injection in frontend image; full compose-in-CI.

## Recommended default (no single mandate)

Most teams should **keep using the closed Docker-local stack daily** and choose **one** optional track:

1. **Product polish** → Option 1 (PFH v1 follow-ups above)  
2. **Provider confidence** → Option 2 (`ENV-REAL-PROVIDER-001`)  
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
