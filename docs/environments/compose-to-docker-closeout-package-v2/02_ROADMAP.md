# Roadmap

## Phase A — Compose startup

`ENV-COMPOSE-001`

Deliver:

```text
docker-compose.yml
.env.example
start/stop/wait/reset scripts
compose config validation
backend startup validation
Conexus liveness/readiness boundary evidence
```

Acceptance:

```text
postgres healthy
Kanon /health pass
Conexus /health/live pass
Allagma /health pass
Conexus /ready strict before bootstrap
```

## Phase B — Dockerized guided flow

`ENV-DOCKER-RUN-001`

Deliver:

```text
Dockerized guided main flow runner
machine report
validator
Allagma restart durability check
```

Acceptance:

```text
seed passes against composed services
baseline run completes
subject topology override run completes
Kanon topology auth present for subject
Conexus route/model evidence present
eval write/list pass
baseline comparison create/fetch pass
Allagma restart preserves evidence
```

## Phase C — Frontend walkthrough

`ENV-DOCKER-FE-001`

Deliver:

```text
frontend operator walkthrough
page opener
optional Playwright fixture/live smoke if practical
```

Acceptance:

```text
dashboard opens
run detail opens
evaluation detail opens
baseline comparison detail opens
fixture mode still available
live backend degradation states are honest
```

## Phase D — Docker-local closeout

`ENV-DOCKER-CLOSEOUT-001`

Deliver:

```text
closeout report
scorecard
known limitations
next-step hardening backlog
```

Acceptance:

```text
explicit not-production-readiness statement
all evidence linked
post-closeout hardening separated
```
