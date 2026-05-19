# ENV-DOCKER-CLOSEOUT-001 — First Docker-local working system closeout

**Date:** 2026-05-19  
**Program:** Docker local working system (compose + Postgres + guided flow)  
**Gate:** ENV-DOCKER-CLOSEOUT-001  
**Planning source:** `docs/environments/docker-local-working-system/` and `docs/environments/compose-to-docker-closeout-package-v2/`

**This is first Dockerized local working system, not production readiness.**

## Milestone summary

The **Docker/Postgres** local working system is **closed**. Operators can start the compose stack, seed/bootstrap, run the dockerized guided main flow (baseline + subject runs, evals, baseline comparison), prove Allagma restart durability, and verify frontend operator surfaces against live report IDs.

| Field | Value |
| --- | --- |
| Closeout PR | ENV-DOCKER-CLOSEOUT-001 |
| Verdict | **PASS** |
| Closeout evidence | [ENV_DOCKER_CLOSEOUT_001_EVIDENCE.md](../evidence/ENV_DOCKER_CLOSEOUT_001_EVIDENCE.md) |
| Compose root | `docker/local-working-system/` |
| Prior program | [FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md](./FIRST_WORKING_ENVIRONMENT_CLOSEOUT.md) (script-based, **closed**) |

## Repository SHAs (closeout validation)

| Repo | SHA |
| --- | --- |
| ontogony-platform | `032de3d56d584ebb121e496804a0a31bc2d5fadf` |
| allagma-dotnet | `4fdb3c797a1f2e32376bdbbdcaa810c421f60170` |
| kanon-dotnet | `4275f7f2c11ae32cee5bb1460afb00b5e4f6abd5` |
| conexus-dotnet | `b14820c535f7ca12c9ac7d47971156427cad30cf` |
| ontogony-frontend | `1eed7738d52f3fe9cbf4a08370cbc9c5b17dbca1` |
| ontogony-ui | `10f8a02665c17390ea60836afee2d12e3097a2d5` |

## Ports and settings (default)

| Service | URL | Notes |
| --- | --- | --- |
| Postgres (host) | `localhost:55433` | Default in `.env.example`; override if taken |
| Kanon | `http://localhost:5081` | `/health` |
| Conexus | `http://localhost:5082` | `/health/live` liveness; `/ready` strict |
| Allagma | `http://localhost:5083` | `ManualWriteEnabled` in Development |
| Frontend (compose) | `http://localhost:5175` | nginx SPA; `VITE_*` → host APIs |
| Project API key | `cx-dev-key-change-me` | Dev placeholder only |

## Program sequence status

| PR | Repo | Status |
| --- | --- | --- |
| ENV-DOCKER-001 | ontogony-platform | **DONE** — plan |
| ENV-DOCKER-002 | service repos | **DONE** — Dockerfiles |
| ENV-DB-001 | ontogony-platform | **DONE** — Postgres bootstrap |
| ENV-SEED-001 | ontogony-platform | **DONE** — host-local + compose seed |
| ENV-COMPOSE-001 | ontogony-platform | **DONE** — compose orchestration |
| ENV-DOCKER-RUN-001 | ontogony-platform | **DONE** — guided flow + restart durability |
| ENV-DOCKER-FE-001 | ontogony-frontend | **DONE** — Docker walkthrough |
| ENV-DOCKER-CLOSEOUT-001 | ontogony-platform | **DONE** (this closeout) |

## Evidence IDs (latest docker guided report)

From `docker/local-working-system/artifacts/docker-guided-main-flow-report.json` (2026-05-19 UTC):

| Role | ID |
| --- | --- |
| Baseline run (`single_workflow`) | `run_926c1317fbe94ce9ac2fd7832840552e` |
| Subject run (`centralized_orchestrator`) | `run_33c73fabdbba46e980a0b9315e6a14c8` |
| Subject topology authorization | `decision_c1bea0d2f88044deabc2c42015f0d8b9` |
| Baseline route decision | `rd-0HNLL7I92B7KF-00000001` |
| Subject route decision | `rd-0HNLL7I92B7KF-00000002` |
| Baseline evaluation | `eval_7a44e977930343d180e7732d99e0fb13` |
| Subject evaluation | `eval_2f6af9867cb64dee8bdced67ac1f39b7` |
| Baseline comparison | `cmp_caffacdb61864198a89ce393616fc200` |
| Restart persistence | **PASS** |

Baseline `topologyAuthorizationDecisionId` is **null by design** on `single_workflow`.

## What passed

- Compose builds and starts postgres + three APIs + frontend.
- Postgres healthy; three logical DBs; migrations on startup.
- ENV-SEED-001 seed/bootstrap against composed APIs.
- Container DNS: Allagma → Kanon/Conexus internal URLs.
- Dockerized guided main flow + validator **PASS**.
- Evaluations and baseline comparison survive `allagma-api` restart.
- Frontend operator routes return SPA HTML on required paths (ENV-DOCKER-FE-001).
- Safety: fake provider; no real external execution; no production-readiness claim.

## Health boundary (preserved)

```text
/health, /health/live → liveness (compose wait scripts)
/ready → strict readiness (Conexus may be 503 pre-bootstrap)
```

## Repeatable validation (closeout)

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\validate-docker-guided-main-flow.ps1
```

With stack running:

```powershell
.\docker\local-working-system\scripts\wait-local-working-system.ps1
cd C:\dev\ontogony-frontend
.\scripts\docker\open-docker-local-operator-pages.ps1 -NoBrowser
```

## Related artifacts

| Document | Path |
| --- | --- |
| Scorecard | [FIRST_DOCKER_LOCAL_WORKING_SYSTEM_SCORECARD.md](./FIRST_DOCKER_LOCAL_WORKING_SYSTEM_SCORECARD.md) |
| Known limitations | [FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md](./FIRST_DOCKER_LOCAL_WORKING_SYSTEM_KNOWN_LIMITATIONS.md) |
| Post-closeout backlog | [FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md](./FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md) |

## Sign-off

| Field | Value |
| --- | --- |
| Closeout date | 2026-05-19 |
| Environment verdict | **PASS** |
| Production readiness | **Not claimed** |
| Signed off by | Operator (ENV Docker sequence evidence + closeout validation) |

**Status: CLOSED** — first Dockerized local working system accepted. Post-closeout hardening is a **separate** backlog (see next-steps doc).
