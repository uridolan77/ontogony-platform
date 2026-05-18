# First working local environment scorecard

**Date:** 2026-05-19  
**Gate:** ENV-CLOSEOUT-001  
**Evidence:** [ENV_CLOSEOUT_001_FIRST_WORKING_ENVIRONMENT.md](../evidence/ENV_CLOSEOUT_001_FIRST_WORKING_ENVIRONMENT.md)

**This is first working local environment, not production readiness.**

Scores reflect what was **demonstrated across the ENV program**, not production SLOs or deploy readiness.

| Area | Score | Notes |
| --- | ---: | --- |
| Workspace layout & settings docs | 9.5/10 | Canonical tree in `local-operator-sanity/`; six-repo layout verified |
| Stack automation | 9.4/10 | Start/check/stop + env template; process ledger for stop |
| Guided main flow | 9.5/10 | `run-guided-main-flow.ps1` + validator; full-sanity report PASS |
| Governed runs | 9.5/10 | Baseline + subject complete; Kanon planning + topology on subject |
| Topology authorization | 9.3/10 | Subject `centralized_orchestrator` decision ID; baseline null documented |
| Conexus route evidence | 9.4/10 | Baseline + subject `routeDecisionId` on live stack |
| Eval write/list/compare | 9.3/10 | Manual eval POST gated; list + baseline comparison fetch |
| Postgres durability (optional) | 9.2/10 | ENV-PG-001: evals + comparison survive Allagma restart |
| Frontend operator surfaces | 8.9/10 | Adapter + Playwright pass; live browser manual |
| ontogony-ui integration | 9.0/10 | ENV-UI-001: file link + CI checkout model documented |
| CI stability | 9.4/10 | CI-STABILIZE-001/002 on main across repos |
| Safety boundaries | 9.8/10 | Fake provider; real execution disabled; no secrets in reports |
| Operator usability | 8.8/10 | Runbook + scripts; ManualWriteEnabled sequencing documented |
| **Overall first environment health** | **9.2/10** | Script-based local path closed; Docker phase next |

## ENV PR acceptance (summary)

| PR | Verdict | Primary evidence |
| --- | --- | --- |
| ENV-SETUP-001 | PASS | `docs/evidence/ENV_SETUP_001_LOCAL_OPERATOR_SANITY_DOCS.md` |
| ENV-SETUP-002 | PASS | `allagma-dotnet/docs/evidence/ENV_SETUP_002_STACK_SCRIPTS_EVIDENCE.md` |
| ENV-RUN-001 | PASS | `allagma-dotnet/docs/evidence/ENV_RUN_001_GUIDED_MAIN_FLOW_EVIDENCE.md` |
| ENV-FE-001 | PASS | `ontogony-frontend/docs/evidence/ENV_FE_001_OPERATOR_WALKTHROUGH_EVIDENCE.md` |
| ENV-PG-001 | PASS | `allagma-dotnet/docs/evidence/ENV_PG_001_POSTGRES_DURABLE_EVIDENCE.md` |
| ENV-UI-001 | PASS | `ontogony-frontend/docs/evidence/ENV_UI_001_INTEGRATION_READINESS.md` |
| ENV-CLOSEOUT-001 | PASS | `docs/evidence/ENV_CLOSEOUT_001_FIRST_WORKING_ENVIRONMENT.md` |

## What the score does not mean

- Not Docker Compose orchestration (next phase: ENV-DOCKER-LOCAL)
- Not multi-tenant production hardening
- Not real external tool execution or real provider keys by default
- Not automated live-browser walkthrough in the sanity runner

See [FIRST_WORKING_ENVIRONMENT_KNOWN_LIMITATIONS.md](./FIRST_WORKING_ENVIRONMENT_KNOWN_LIMITATIONS.md).
