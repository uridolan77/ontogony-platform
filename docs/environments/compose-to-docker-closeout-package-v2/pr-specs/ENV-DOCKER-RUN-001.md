# ENV-DOCKER-RUN-001 — Dockerized guided main flow

Repo:

```text
ontogony-platform
```

Optional repo if runner reuses existing internals:

```text
allagma-dotnet
```

## Goal

Prove the main use flow against the Dockerized local working system.

## Must prove

```text
compose stack is up
seed/bootstrap passes against composed services
baseline single_workflow run completes
subject centralized_orchestrator override run completes
subject Kanon topology authorization decision is present
Conexus route/model evidence is present
eval write/list works
baseline comparison create/fetch works
Allagma restart happens
evals/comparison survive Allagma restart
machine report is written
validator passes
```

## Add

```text
docker/local-working-system/scripts/run-docker-guided-main-flow.ps1
docker/local-working-system/scripts/validate-docker-guided-main-flow.ps1
docs/evidence/ENV_DOCKER_RUN_001_GUIDED_MAIN_FLOW_EVIDENCE.md
```

## Acceptance

The report must include:

```text
baselineRunId
subjectRunId
subjectTopologyAuthorizationDecisionId
baselineRouteDecisionId
subjectRouteDecisionId
baselineEvaluationRunId
subjectEvaluationRunId
baselineComparisonId
restartPersistencePassed
verdict=PASS
```

## Boundary

This is Docker-local proof, not production readiness.
