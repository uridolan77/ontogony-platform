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
| PMQA002-002 | product bug | high | `GET /allagma/v0/evaluations` | Route should exist and return list | `404 Not Found` |
| PMQA002-003 | docs mismatch | high | `GET /allagma/v0/evaluations/{evaluationRunId}/evidence` | Route should exist per docs and preflight wording | `404 Not Found` in running stack |
| PMQA002-004 | product bug | medium | `GET /allagma/v0/evaluations/baseline-comparisons` | List route should return comparisons | `404 Not Found` |
| PMQA002-005 | product bug | medium | `GET /allagma/v0/evaluation-datasets` | Dataset list route should return data/empty | `404 Not Found` |
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
  2. Resolve Allagma route availability mismatch for `evaluations list`, `evidence export`, `baseline list`, and `dataset list`.
  3. Re-run `PRODUCT-MANUAL-QA-002` from fresh stack after fixes.
