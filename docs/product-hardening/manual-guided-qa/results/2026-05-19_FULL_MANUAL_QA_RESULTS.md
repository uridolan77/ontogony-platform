# PRODUCT-MANUAL-QA-002 — full manual QA results (2026-05-19)

- Session ID: `PRODUCT-MANUAL-QA-002-2026-05-19-A`
- Date/time (UTC): `2026-05-19T16:22:00Z` to `2026-05-19T16:27:30Z`
- Tester: Cursor agent execution
- Environment: Docker-local (`.env.example`, frontend `http://localhost:5175`)
- Mode: Fresh stack + fresh seed/guided flow artifacts only

## Fresh IDs captured

- `subjectRunId`: `run_01ade6e77646494a85cbfb4bf8c5e353`
- `baselineRunId`: `run_2e43ffd852e94ba09220a080679a23e9`
- `subjectEvaluationRunId`: `eval_ecb4b2942c9c48999a89e14664ce8bfa`
- `baselineEvaluationRunId`: `eval_987bfb185f134fdfa73e836e6056562a`
- `baselineComparisonId`: `cmp_2a610fcb16d04607b225562317d50bab`
- `subjectTopologyAuthorizationDecisionId`: `decision_1cfd98615b764bd2bd7bc3fde9b072c4`
- `subjectRouteDecisionId`: `rd-0HNLLOBFKKSIL-00000002`
- `subjectTraceId`: `46cb5621efffc8791105b2aafebd65e4`

## Step verdicts

| Step | Verdict | Route(s) tested | Notes |
| --- | --- | --- | --- |
| 01 Preconditions | PASS | package + boundary + repo/route preflight | `PRODUCT-MANUAL-QA-001` done on `origin/main`; RCQ evidence exists; docs route wording verified as `/evidence` |
| 02 Start stack and seed | PARTIAL | compose + seed scripts | Fresh stack started, seed passed; initial `-Build` attempt failed due NuGet TLS `PartialChain`, fallback used non-build start and manual Allagma migration |
| 03 Guided main flow | PASS | run/eval/baseline journey scripts | `run-docker-guided-main-flow.ps1` PASS with restart durability PASS |
| 04 Eval dashboard | PARTIAL | `/allagma/evaluations`, fixture route | Frontend routes returned SPA shell `200`; backend list API `GET /allagma/v0/evaluations` returned `404` |
| 05 Eval detail and export | PARTIAL | `/allagma/evaluations/{evaluationRunId}` + `/evidence` | Eval detail API returned `200`; evidence export API `GET /allagma/v0/evaluations/{evaluationRunId}/evidence` returned `404` |
| 06 Baseline comparison workbench | PARTIAL | baseline list/detail routes | Detail API `200`; list API `GET /allagma/v0/evaluations/baseline-comparisons` returned `404` |
| 07 Scenario dataset surfaces | FAIL | `/allagma/evaluations/datasets`, dataset API | Frontend route shell `200`; backend list API `GET /allagma/v0/evaluation-datasets` returned `404` |
| 08 Run detail evidence journey | PASS | `/allagma/runs/{runId}` + events/evals | Run detail/events/evaluations APIs returned `200`; guided run links available |
| 09 Replay workbench | PARTIAL | `/allagma/replay` run/trace/decision modes | Frontend replay routes shell `200`; Kanon replay bundle API reachable; no UI export artifact captured in this run |
| 10 Trace / Conexus / Kanon links | PASS | Kanon decision/by-trace/replay + Conexus route decision | Kanon provenance endpoints `200` with trusted actor headers; Conexus route decision endpoint `200` with admin key header |
| 11 Fixture/live/degraded states | PARTIAL | fixture/live route shells + API probe | Fixture/live SPA shells reachable; degraded states inferred from API `404` on expected list/export routes |
| 12 Export and evidence checks | FAIL | eval export and evidence packaging | Required eval evidence export endpoint returned `404`; export artifact set incomplete |

## Failures and classification

| ID | Classification | Severity | Route/Area | Expected | Actual |
| --- | --- | --- | --- | --- | --- |
| PMQA002-001 | blocking defect | high | stack build (`start-local-working-system.ps1 -Build`) | Build from current main should complete | Build failed during `dotnet restore` in Docker with `NU1301` TLS `PartialChain` to NuGet |
| PMQA002-002 | inconclusive (possible version-skew) | high | `GET /allagma/v0/evaluations` | Route should exist and return list | `404 Not Found` in running stack after rebuild failure |
| PMQA002-003 | docs/runtime mismatch (inconclusive until rebuild succeeds) | high | `GET /allagma/v0/evaluations/{evaluationRunId}/evidence` | Route should exist per docs and preflight wording | `404 Not Found` in running stack after rebuild failure |
| PMQA002-004 | inconclusive (possible version-skew) | medium | `GET /allagma/v0/evaluations/baseline-comparisons` | List route should return comparisons | `404 Not Found` in running stack after rebuild failure |
| PMQA002-005 | inconclusive (possible version-skew) | medium | `GET /allagma/v0/evaluation-datasets` | Dataset list route should return data/empty | `404 Not Found` in running stack after rebuild failure |
| PMQA002-006 | operator usability improvement | low | manual QA automation | Single probe should capture all checklist states including export artifacts | Extra script-level probing needed due mixed route availability |

## Artifacts used (fresh)

- `docker/local-working-system/artifacts/env-seed-001-report.json`
- `docker/local-working-system/artifacts/docker-guided-main-flow-report.json`
- `docker/local-working-system/artifacts/fe-harden-001-frontend-evidence-report.json`
- `docker/local-working-system/artifacts/trace-contract-001-evidence-report.json`
- `docker/local-working-system/artifacts/kanon-op-001-topology-evidence-report.json`
- `docker/local-working-system/artifacts/manual-qa/2026-05-19/product-manual-qa-002-endpoint-probe.json`

## Boundary

```text
This execution validates Docker-local product-hardening behavior only.
It is not production readiness and does not authorize real provider mode, cloud deployment,
or production identity/TLS/secrets posture.
```

## Overall verdict

- Overall verdict: **PARTIAL / BLOCKED**
- Acceptance status: **not met** (every checklist executed, but export/list route failures and build blocker prevent PASS)
- Next required actions:
  1. Fix Docker build certificate chain trust for NuGet (or provide approved internal feed/trust configuration).
  2. Rebuild current images successfully, then re-verify Allagma route availability for `evaluations list`, `evidence export`, `baseline list`, and `dataset list`.
  3. Re-run `PRODUCT-MANUAL-QA-002` from fresh stack after fixes.

## Addendum — PMQA002-001 follow-up execution (2026-05-19)

This addendum records post-run defect follow-up only. Original run history above remains unchanged.

- `PMQA002-001` status: **implemented / backend rebuild path fixed**
- Evidence: `docs/evidence/PMQA002_001_DOCKER_REBUILD_TLS_FIX_EVIDENCE.md`

### Rebuild outcome

- Reproduced `NU1301` / `PartialChain` during Docker restore.
- Root cause isolated to local TLS interception trust gap (Windows trusted root CA not present in Linux Docker build stages).
- Applied narrow fix: optional CA injection build arg + automatic local trusted-root export/injection from Windows cert store at build time.
- No TLS verification bypass introduced.
- Backend compose build from current sources succeeded (`allagma-api`, `kanon-api`, `conexus-api`).

### Route re-check on rebuilt images

After rebuild and seed/migration setup:

- `GET /allagma/v0/evaluations` → `200`
- `GET /allagma/v0/evaluations/{evaluationRunId}/evidence` → `200`
- `GET /allagma/v0/evaluations/baseline-comparisons` → `200`
- `GET /allagma/v0/evaluation-datasets` → `200`

### Reclassification of prior 404 findings

- `PMQA002-002` (`/allagma/v0/evaluations`): stale-image/version-skew, resolved on rebuilt image
- `PMQA002-003` (`/allagma/v0/evaluations/{evaluationRunId}/evidence`): stale-image/version-skew, resolved on rebuilt image
- `PMQA002-004` (`/allagma/v0/evaluations/baseline-comparisons`): stale-image/version-skew, resolved on rebuilt image
- `PMQA002-005` (`/allagma/v0/evaluation-datasets`): stale-image/version-skew, resolved on rebuilt image

### Remaining non-PMQA002-001 blocker

Full-stack `start-local-working-system.ps1 -Build` still fails on `ontogony-frontend` image build (`tsc: not found`), which is separate from the NuGet TLS trust defect.

## Addendum — PMQA002-002 follow-up execution (2026-05-19)

This addendum records the frontend Docker build blocker resolution. Prior history remains unchanged.

- `PMQA002-002` status: **implemented / fixed**
- Evidence:
  - `docs/evidence/PMQA002_002_FRONTEND_DOCKER_BUILD_FIX_EVIDENCE.md`
  - `ontogony-frontend/docs/evidence/PMQA002_002_FRONTEND_DOCKER_BUILD_FIX_EVIDENCE.md`

### Frontend blocker resolution

- Reproduced failing frontend stage: `ontogony-frontend build-ui` at `RUN npm run build`.
- Error confirmed: `tsc: not found`.
- Applied narrow frontend build-stage fix:
  - deterministic build-time dev tool installation (`npm ci --include=dev`),
  - Node build-stage TLS trust alignment via optional CA injection and npm `cafile`,
  - compose wiring for frontend `EXTRA_CA_CERT_BASE64`,
  - startup probe expansion to include npm registry TLS.

### Full-stack rebuild status

- `start-local-working-system.ps1 -Build` now succeeds for all services including `ontogony-frontend`.
- Docker-local stack reached healthy state after rebuild.

### Frontend route smoke on rebuilt image

- `GET http://localhost:5175/` → `200`
- `GET http://localhost:5175/allagma/evaluations` → `200`
- `GET http://localhost:5175/allagma/evaluations/baseline-comparisons` → `200`
- `GET http://localhost:5175/allagma/evaluations/datasets` → `200`
- `GET http://localhost:5175/allagma/replay` → `200`

### Reclassification

- Previous frontend blocker (`tsc not found`) is resolved.
- Manual QA remains pending rerun (`PRODUCT-MANUAL-QA-002R1`) to produce a fresh end-to-end execution package on rebuilt images.

## Addendum — PMQA002-003 rebuilt-stack smoke verification (2026-05-19)

This addendum records rebuilt-stack smoke execution prior to full checklist rerun.

- `PMQA002-003` status: **implemented / PASS**
- Evidence: `docs/evidence/PMQA002_003_FULL_REBUILT_STACK_SMOKE_EVIDENCE.md`

### Smoke execution verdict

- Fresh Docker-local reset + full rebuild succeeded:
  - `reset-local-working-system.ps1 -Force`
  - `start-local-working-system.ps1 -Build`
  - `wait-local-working-system.ps1`
- Seed/bootstrap and guided main flow both passed on rebuilt stack:
  - `seed-and-verify-local-working-system.ps1` -> PASS
  - `run-docker-guided-main-flow.ps1` -> PASS
  - `validate-docker-guided-main-flow.ps1` -> PASS
- Guided-flow restart durability remained PASS after `allagma-api` restart.

### Required route/UI probes

Backend (with required Allagma Bearer token):

- `GET http://localhost:5083/health` -> `200`
- `GET http://localhost:5083/allagma/v0/evaluations` -> `200`
- `GET http://localhost:5083/allagma/v0/evaluations/{evaluationRunId}/evidence` -> `200`
- `GET http://localhost:5083/allagma/v0/evaluations/baseline-comparisons` -> `200`
- `GET http://localhost:5083/allagma/v0/evaluation-datasets` -> `200`
- `GET http://localhost:5081/health` -> `200`
- `GET http://localhost:5082/health/live` -> `200`

Frontend:

- `GET http://localhost:5175/` -> `200`
- `GET http://localhost:5175/allagma/evaluations` -> `200`
- `GET http://localhost:5175/allagma/evaluations/baseline-comparisons` -> `200`
- `GET http://localhost:5175/allagma/evaluations/datasets` -> `200`
- `GET http://localhost:5175/allagma/replay` -> `200`

Existing frontend inspect script:

- `ontogony-frontend/scripts/docker/inspect-docker-local-operator-frontend.ps1` -> PASS

### Reclassification / gate outcome

- No stale-image/version-skew evidence remains in rebuilt-stack smoke.
- Rebuild coherence gate is now satisfied.
- Next step remains unchanged: run `PRODUCT-MANUAL-QA-002R1` full manual QA rerun from fresh rebuilt stack.

## Addendum — PRODUCT-MANUAL-QA-002R1 rerun execution (2026-05-19)

This addendum records the full rerun after PMQA002-001/002/003 completion.

- Rerun status: **PASS**
- Evidence:
  - `docs/evidence/PRODUCT_MANUAL_QA_002R1_EXECUTION_EVIDENCE.md`
  - `docker/local-working-system/artifacts/manual-qa/2026-05-19/product-manual-qa-002r1-endpoint-probe.json`

### Fresh IDs captured (rerun)

- `subjectRunId`: `run_51349d9077ac4c48a8fd6b44bf5fecb5`
- `baselineRunId`: `run_be59dc3bf70646c19a91497441e14dd6`
- `subjectEvaluationRunId`: `eval_cb125f2cd98c4d2ba31e205b9aafc8d2`
- `baselineEvaluationRunId`: `eval_5218f5d3d28f481994a263be1008537a`
- `baselineComparisonId`: `cmp_5f44bf81529c4a47a8ce3dae8d9094ed`
- `subjectTopologyAuthorizationDecisionId`: `decision_172b9caad8b545be97298553192f05b3`
- `subjectRouteDecisionId`: `rd-0HNLLPVKSUP2G-00000004`
- `subjectTraceId`: `1fbf0978f83fc5cad6e97e4dabb7bbe8`

### Rerun step verdicts

| Step | Verdict | Route(s) tested | Notes |
| --- | --- | --- | --- |
| 01 Preconditions | PASS | repo and package checks | Executed from fresh rebuilt stack context |
| 02 Start stack and seed | PASS | reset + build + wait + seed | Build/wait PASS; seed PASS after Allagma local migration apply |
| 03 Guided main flow | PASS | run/eval/baseline flow | Guided main flow + validation PASS |
| 04 Eval dashboard | PASS | `/allagma/evaluations` + API list | Frontend route `200`; backend list route `200` |
| 05 Eval detail and export | PASS | eval detail + `/evidence` | Both backend routes returned `200`; frontend detail route `200` |
| 06 Baseline comparison workbench | PASS | baseline list/detail routes | Backend list/detail `200`; frontend list/detail `200` |
| 07 Scenario dataset surfaces | PASS | dataset list route/page | Backend dataset route `200`; frontend route `200` |
| 08 Run detail evidence journey | PASS | run/events/evaluations routes | All run journey APIs returned `200` |
| 09 Replay workbench | PASS | replay run/trace/decision modes | All replay SPA routes returned `200` |
| 10 Trace / Conexus / Kanon links | PASS | Kanon + Conexus evidence routes | Kanon provenance and Conexus route decision probes `200` |
| 11 Fixture/live/degraded states | PASS | fixture/live pages + inspect | Fixture/live pages `200`; frontend inspect script PASS |
| 12 Export and evidence checks | PASS | artifact/report generation | Required rerun artifacts generated and validated |

### Overall rerun verdict

- `PRODUCT-MANUAL-QA-002R1`: **PASS**
- Manual QA rerun acceptance conditions met on fresh rebuilt stack.
- Boundary preserved: Docker-local hardening verification only, not production readiness.
