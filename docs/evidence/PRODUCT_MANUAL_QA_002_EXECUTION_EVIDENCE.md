# PRODUCT-MANUAL-QA-002 — execution evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PARTIAL / BLOCKED**

## Purpose

Execute the full manual guided QA package from `docs/product-hardening/manual-guided-qa/` against a fresh Docker-local stack, record route-by-route outcomes, and classify all failures.

## Inputs used

- `docs/product-hardening/manual-guided-qa/`
- `docs/product-hardening/manual-guided-qa/13_RESULTS_TEMPLATE.md`
- `docs/releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_CLOSEOUT.md`
- `docs/evidence/FE_PRODUCT_CLOSEOUT_001_EVIDENCE.md`

## Preflight status

| Check | Result |
| --- | --- |
| Pull latest main in six repos | PASS |
| PFH closeout + RCQ/docs standard + `PRODUCT-MANUAL-QA-001` closed | PASS |
| Route wording check (`/evaluations/{evaluationRunId}/evidence`) | PASS in docs preflight |
| `RCQ_CODE_001_ONTOGONY_PLATFORM_EVIDENCE.md` exists | PASS |

## Execution summary

1. Attempted clean rebuild flow (`reset -> start -Build -> wait -> seed -> guided`)  
   - **Blocked initially** by Docker build restore TLS trust issue (`NU1301`, certificate `PartialChain`).
2. Started stack without rebuild (fresh containers/volume), then ran seed:
   - Seed initially failed because Allagma tables were missing (`relation "allagma_runs" does not exist`).
3. Applied Allagma EF migrations to compose Postgres (`Host=localhost;Port=55433;...`) using:
   - `allagma-dotnet/scripts/apply-allagma-postgres-migration.ps1`
4. Re-ran guided flow:
   - `run-docker-guided-main-flow.ps1` **PASS** (including Allagma restart durability PASS).
5. Ran supporting evidence scripts:
   - frontend HTTP shell/safety evidence PASS
   - trace correlation evidence PASS
   - Kanon topology evidence PASS
6. Ran explicit endpoint probe across Allagma/Kanon/Conexus/frontend routes:
   - Found multiple expected Allagma list/export routes returning `404` in the running stack.
   - Because rebuild failed earlier, these `404`s are treated as inconclusive until rebuilt current images are verified.

## Key route outcomes

| Route/API | Expected | Actual |
| --- | --- | --- |
| `GET /allagma/v0/evaluations` | global evaluation list | `404` |
| `GET /allagma/v0/evaluations/{evaluationRunId}` | evaluation detail | `200` |
| `GET /allagma/v0/evaluations/{evaluationRunId}/evidence` | eval evidence export | `404` |
| `GET /allagma/v0/evaluations/baseline-comparisons` | baseline list | `404` |
| `GET /allagma/v0/evaluations/baseline-comparisons/{comparisonId}` | baseline detail | `200` |
| `GET /allagma/v0/evaluation-datasets` | dataset list | `404` |
| `GET /allagma/v0/runs/{runId}` + `/events` + `/evaluations` | run journey | `200` |
| Kanon decision/by-trace/replay-bundles (trusted actor headers) | provenance/replay links | `200` |
| Conexus admin route decision (`X-Conexus-Admin-Key`) | route evidence | `200` |

## Failure classification

| ID | Classification | Severity | Summary |
| --- | --- | --- | --- |
| PMQA002-001 | blocking defect | high | Docker rebuild from current main fails due NuGet TLS trust (`PartialChain`) inside build containers |
| PMQA002-002 | inconclusive (possible version-skew) | high | Allagma eval list route returned `404` in running stack after rebuild failure |
| PMQA002-003 | docs/runtime mismatch (inconclusive until rebuild succeeds) | high | Documented eval evidence export route returned `404` in running stack after rebuild failure |
| PMQA002-004 | inconclusive (possible version-skew) | medium | Allagma baseline list route returned `404` in running stack after rebuild failure |
| PMQA002-005 | inconclusive (possible version-skew) | medium | Allagma dataset list route returned `404` in running stack after rebuild failure |

## Deliverables produced

- Results: `docs/product-hardening/manual-guided-qa/results/2026-05-19_FULL_MANUAL_QA_RESULTS.md`
- Fresh artifacts:
  - `docker/local-working-system/artifacts/env-seed-001-report.json`
  - `docker/local-working-system/artifacts/docker-guided-main-flow-report.json`
  - `docker/local-working-system/artifacts/fe-harden-001-frontend-evidence-report.json`
  - `docker/local-working-system/artifacts/trace-contract-001-evidence-report.json`
  - `docker/local-working-system/artifacts/kanon-op-001-topology-evidence-report.json`
  - `docker/local-working-system/artifacts/manual-qa/2026-05-19/product-manual-qa-002-endpoint-probe.json`

## Acceptance check

| Acceptance condition | Result |
| --- | --- |
| Every checklist executed or explicitly skipped with reason | PASS |
| All failures classified | PASS |
| Known limitations preserved | PASS |
| No secrets in results | PASS |
| Not production readiness | PASS |
| Overall package PASS | **NO** (blocked/partial due defects above) |

## Boundary

```text
This work is NOT production readiness. It does not authorize real provider mode by default,
cloud deployment, production identity/TLS/secrets, or unscoped runtime refactors.
```
