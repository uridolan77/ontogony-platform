# ENV-DOCKER-RUN-001 — Dockerized guided main flow evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **ACCEPTED** — DONE / PASS  
**Statement:** This package targets the first Dockerized local working system. It is not production readiness.

## Scope

`ontogony-platform` — Docker guided main flow scripts and machine report against the compose stack.

**Cross-repo prerequisite (fresh volume):** `conexus-dotnet` dev bootstrap issues `CONEXUS_DEV_PROJECT_API_KEY` when set (not a random key) so Allagma compose env and Conexus Postgres keys stay aligned.

## Delivered

```text
docker/local-working-system/scripts/run-docker-guided-main-flow.ps1
docker/local-working-system/scripts/validate-docker-guided-main-flow.ps1
docker/local-working-system/artifacts/docker-guided-main-flow-report.json
docs/evidence/ENV_DOCKER_RUN_001_GUIDED_MAIN_FLOW_EVIDENCE.md
```

## Commands

```powershell
cd C:\dev\ontogony-platform

.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1 -SkipFrontend
.\docker\local-working-system\scripts\validate-docker-guided-main-flow.ps1
```

Fresh-volume proof (includes Conexus rebuild with dev-key bootstrap fix):

```powershell
.\docker\local-working-system\scripts\reset-local-working-system.ps1 -Force
cd docker\local-working-system
docker compose --env-file .env.example -f docker-compose.yml up -d --build
cd ..\..
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1 -SkipFrontend
```

## Machine report

`docker/local-working-system/artifacts/docker-guided-main-flow-report.json`

| Field | Value (2026-05-19 run) |
| --- | --- |
| baselineRunId | `run_926c1317fbe94ce9ac2fd7832840552e` |
| subjectRunId | `run_33c73fabdbba46e980a0b9315e6a14c8` |
| subjectTopologyAuthorizationDecisionId | `decision_c1bea0d2f88044deabc2c42015f0d8b9` |
| baselineRouteDecisionId | `rd-0HNLL7I92B7KF-00000001` |
| subjectRouteDecisionId | `rd-0HNLL7I92B7KF-00000002` |
| baselineEvaluationRunId | `eval_7a44e977930343d180e7732d99e0fb13` |
| subjectEvaluationRunId | `eval_2f6af9867cb64dee8bdced67ac1f39b7` |
| baselineComparisonId | `cmp_caffacdb61864198a89ce393616fc200` |
| restartPersistencePassed | **true** |
| verdict | **PASS** |

## Proof summary

| Check | Result |
| --- | --- |
| Compose stack healthy (wait script) | **PASS** |
| ENV-SEED-001 seed/bootstrap | **PASS** |
| Baseline `single_workflow` run | **PASS** |
| Subject `centralized_orchestrator` + Kanon topology auth | **PASS** |
| Conexus route/model evidence | **PASS** |
| Eval write/list | **PASS** |
| Baseline comparison create/fetch | **PASS** |
| `allagma-api` container restart | **PASS** |
| Evaluations + comparison survive restart | **PASS** |
| Report validator | **PASS** |
| Real provider keys | **no** |
| Production readiness claim | **no** |

## Safety

| Check | Status |
| --- | --- |
| Real provider keys committed | **no** |
| Real external execution enabled | **no** |
| Production readiness claim | **no** |

## Next step

**ENV-DOCKER-FE-001** — done. **ENV-DOCKER-CLOSEOUT-001** — first Docker local system closeout.
