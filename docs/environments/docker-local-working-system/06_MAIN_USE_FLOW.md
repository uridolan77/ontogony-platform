# Main use flow — Docker local working system

Twelve-step proof target for the Docker phase (parity with script-based program, container-backed persistence).

**Boundary:** first Dockerized local working system, not production readiness.

## Flow

1. **Start Docker stack** — `docker compose up` from `ontogony-platform/docker/local-working-system/` (ENV-COMPOSE-001).
2. **Wait for health** — postgres, kanon-api, conexus-api, allagma-api healthy.
3. **Run baseline** `single_workflow` governed run (Allagma).
4. **Run subject** `centralized_orchestrator` topology override run.
5. **Verify Kanon topology authorization** for subject (`topologyAuthorizationDecisionId` non-empty).
6. **Verify Conexus route/model evidence** (`routeDecisionId` on baseline + subject model calls).
7. **Write/list evaluations** per run (manual eval POST gated).
8. **Create/fetch baseline comparison**.
9. **Restart Allagma container** — prove Postgres-backed eval persistence.
10. **Verify evaluations and comparison still fetch** after restart.
11. **Open frontend operator pages** (host browser → `localhost` API ports).
12. **Export and validate evidence** — machine report + closeout docs (ENV-DOCKER-CLOSEOUT-001).

## Expected frontend routes

```text
/allagma/evaluations
/allagma/evaluations?dashboardFixture=ci-suite
/allagma/runs/{subjectRunId}
/allagma/evaluations/{subjectEvaluationRunId}
/allagma/evaluations/baseline-comparisons/{comparisonId}
```

Reuse walkthrough patterns from `ontogony-frontend/docs/development/LOCAL_OPERATOR_WALKTHROUGH.md` with Docker host URLs.

## Scenario IDs (harness)

Same eval harness as script-based sanity (see `allagma-dotnet/scripts/run-full-sanity.ps1`):

| Role | Topology | Case |
| --- | --- | --- |
| baseline | `single_workflow` | `case-single-workflow-summary` |
| subject | `centralized_orchestrator` | `case-topology-override-centralized` |

## Expected nulls

- Baseline `topologyAuthorizationDecisionId` is **null by design** on `single_workflow` / low-risk path.

## Automation (later)

ENV-DOCKER-RUN-001 adds a Docker-aware guided runner (analogous to `scripts/env/run-guided-main-flow.ps1`). Until then, operators follow this doc and existing sanity scripts adapted for compose.

## Prior art

Closed script-based flow: `docs/environments/local-operator-sanity/03_MAIN_USE_FLOW.md` and `allagma-dotnet/docs/development/LOCAL_OPERATOR_SANITY_RUNBOOK.md`.
