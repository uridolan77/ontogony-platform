# ENV-CLOSEOUT-001 — First working local environment closeout

**Date:** 2026-05-19  
**Program:** Local operator sanity (script-based first working environment)  
**Gate:** ENV-CLOSEOUT-001  
**Planning source:** `docs/environments/local-operator-sanity-package/`

**This is first working local environment, not production readiness.**

## Milestone summary

The **script-based** local operator environment is **closed**. Operators can start Kanon, Conexus, and Allagma on fixed localhost ports with fake/local Conexus provider, run the guided main flow (baseline + subject runs, evals, baseline comparison), optionally prove Postgres durability, and verify frontend operator surfaces with documented walkthroughs.

| Field | Value |
| --- | --- |
| Closeout PR | ENV-CLOSEOUT-001 |
| Verdict | **PASS** |
| Closeout evidence | [ENV_CLOSEOUT_001_FIRST_WORKING_ENVIRONMENT.md](../evidence/ENV_CLOSEOUT_001_FIRST_WORKING_ENVIRONMENT.md) |
| Canonical operator docs | `docs/environments/local-operator-sanity/` |
| Allagma runbook | `allagma-dotnet/docs/development/LOCAL_OPERATOR_SANITY_RUNBOOK.md` |

## Repository SHAs (closeout validation)

| Repo | SHA |
| --- | --- |
| ontogony-platform | `a9e317f` |
| allagma-dotnet | `d39a98d` |
| kanon-dotnet | `b4e1d34` |
| conexus-dotnet | `b5d5ae0` |
| ontogony-frontend | `fb5a640` |
| ontogony-ui | `10f8a02` |

## Ports and settings (default)

| Service | URL | Notes |
| --- | --- | --- |
| Kanon | `http://localhost:5081` | `/health`, `/ready` |
| Conexus | `http://localhost:5082` | Fake/local provider in Development |
| Allagma | `http://localhost:5083` | `Allagma__Evaluation__ManualWriteEnabled=true` |
| Frontend | Vite **5175** (typical) | `ontogony-frontend` dev server |
| Project API key | `cx-dev-key-change-me` | Allagma + Conexus aligned |

## Program sequence status

| PR | Repo | Status |
| --- | --- | --- |
| ENV-SETUP-001 | ontogony-platform | **DONE** — canonical operator docs |
| ENV-SETUP-002 | allagma-dotnet | **DONE** — start/check/stop scripts |
| ENV-RUN-001 | allagma-dotnet | **DONE** — guided main flow + validator |
| ENV-FE-001 | ontogony-frontend | **DONE** — operator walkthrough + page opener |
| ENV-PG-001 | allagma-dotnet | **DONE** — optional Postgres durable sanity |
| ENV-UI-001 | ontogony-frontend | **DONE** — `@ontogony/ui` integration readiness |
| CI-STABILIZE-001 / 002 | cross-repo | **DONE** — CI green on main |
| ENV-CLOSEOUT-001 | ontogony-platform | **DONE** (this closeout) |
| ENV-REAL-PROVIDER-001 | — | **Deferred** (optional; not blocking) |

## Evidence IDs (latest guided / Postgres durable run)

From `allagma-dotnet/artifacts/full-sanity/full-sanity-report.json` (ENV-PG-001, 2026-05-18 UTC):

| Role | ID |
| --- | --- |
| Baseline run (`single_workflow`) | `run_605fbc0654af469785f07c13ced0cf25` |
| Subject run (`centralized_orchestrator`) | `run_ef16456834ee4bb3997ebb2912406b0b` |
| Subject topology authorization | `decision_3af24ca0a94749f6bb03eb997b2ff6b0` |
| Baseline route decision | `rd-0HNLKV6BPTQG3-00000001` |
| Subject route decision | `rd-0HNLKV6BPTQG3-00000002` |
| Baseline evaluation | `eval_aca6d1b39d6c4368b3ab0676249a9e60` |
| Subject evaluation | `eval_200bbfe3c76744a89c82740a902eb1f3` |
| Baseline comparison | `cmp_3518f4a7041e494491e16911d300cc86` |

Baseline `topologyAuthorizationDecisionId` is **null by design** (`single_workflow` / low-risk path).

Prior ENV-RUN-001 guided run (InMemory, same acceptance shape): see `allagma-dotnet/docs/evidence/ENV_RUN_001_FULL_SANITY_REPORT.md`.

## What passed

- Six-repo workspace layout under `C:\dev\` (including `ontogony-ui`).
- Stack scripts: start, check, stop, env template (`allagma-dotnet/scripts/env/`).
- Guided main flow runner + validator; full-sanity report PASS.
- Postgres durable mode: eval records and baseline comparison survive Allagma restart.
- Frontend: OpenAPI check, adapter tests, Playwright eval dashboards; operator walkthrough doc + `open-local-operator-pages.ps1`.
- `ontogony-ui` consumption documented and verified (ENV-UI-001).
- Safety: fake provider first; real external execution disabled; no production-readiness claim.

## Frontend walkthrough status

| Surface | Status |
| --- | --- |
| Automated adapter + Playwright gates | **PASS** (ENV-FE-001) |
| `open-local-operator-pages.ps1` URL generation | **PASS** |
| Live browser walkthrough | **Manual** (documented in `LOCAL_OPERATOR_WALKTHROUGH.md`) |
| Full-sanity runner live browser | **Skipped** by design in guided operator runs (`-SkipFrontendChecks`) |

## Repeatable validation (closeout)

```powershell
cd C:\dev\allagma-dotnet
.\scripts\env\check-local-operator-sanity.ps1 -DevRoot C:\dev -SkipHealthCheck
.\scripts\env\validate-guided-main-flow.ps1 -AllowSkippedFrontend
```

With stack running, operators may additionally run:

```powershell
.\scripts\env\run-guided-main-flow.ps1 -UseExistingServices -SkipFrontendChecks
```

## Related artifacts

| Document | Path |
| --- | --- |
| Scorecard | [FIRST_WORKING_ENVIRONMENT_SCORECARD.md](./FIRST_WORKING_ENVIRONMENT_SCORECARD.md) |
| Known limitations | [FIRST_WORKING_ENVIRONMENT_KNOWN_LIMITATIONS.md](./FIRST_WORKING_ENVIRONMENT_KNOWN_LIMITATIONS.md) |
| Next phase | [FIRST_WORKING_ENVIRONMENT_NEXT_STEPS.md](./FIRST_WORKING_ENVIRONMENT_NEXT_STEPS.md) |

## Sign-off

| Field | Value |
| --- | --- |
| Closeout date | 2026-05-19 |
| Environment verdict | **PASS** |
| Production readiness | **Not claimed** |
| Signed off by | Operator (ENV sequence evidence + closeout validation) |

**Status: CLOSED** — first script-based working local environment accepted. Next major phase: **ENV-DOCKER-LOCAL** (Docker/Postgres local working system per `allagma-dotnet/docs/environments/working-local-environment-complete-package/`).
