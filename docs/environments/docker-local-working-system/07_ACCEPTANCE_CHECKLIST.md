# Acceptance checklist — Docker local working system

Use when closing the **Docker local working system** program (through ENV-DOCKER-CLOSEOUT-001). This is **not** a production readiness gate.

**This package supports the first working local environment and first Dockerized local working system. It is not production readiness.**

## Infrastructure (ENV-COMPOSE-001 onward)

```text
[ ] Docker Compose builds all required services
[ ] Postgres starts and is healthy
[ ] allagma_local DB exists
[ ] kanon_local DB exists
[ ] conexus_local DB exists
[ ] service users exist
[ ] migrations applied on each API
```

## Service connectivity

```text
[ ] Conexus dev project/API key cx-dev-key-change-me works
[ ] Conexus fake provider route works
[ ] Kanon topology policy works
[ ] Allagma can call Kanon by container DNS (kanon-api:8080)
[ ] Allagma can call Conexus by container DNS (conexus-api:8080)
[ ] Host health probes pass on 5081–5083
```

## Governed runs and evidence

```text
[ ] baseline single_workflow run completes
[ ] subject centralized_orchestrator run completes
[ ] subject topologyAuthorizationDecisionId is non-empty
[ ] routeDecisionIds are present (baseline + subject)
[ ] evaluations are written/listed
[ ] baseline comparison created/fetched
[ ] Allagma container restarted
[ ] evaluations still fetch after restart
[ ] baseline comparison still fetches after restart
```

## Frontend

```text
[ ] frontend operator pages open (host browser → localhost APIs)
[ ] fixture/live banners correct where applicable
```

## Safety (required)

```text
[ ] no raw secrets/prompts/completions exposed in UI or exports
[ ] real external execution remains blocked
[ ] no production-readiness claim in evidence or closeout docs
```

## Plan-only (ENV-DOCKER-001)

ENV-DOCKER-001 delivers **this checklist and companion plan docs only**. Checkboxes above are satisfied by later implementation PRs, not by the plan PR.
