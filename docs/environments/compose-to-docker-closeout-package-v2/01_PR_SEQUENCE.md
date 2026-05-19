# PR Sequence

## Minimal Docker-local closeout path

| Order | PR | Repo(s) | Purpose |
|---:|---|---|---|
| 1 | `ENV-COMPOSE-001` | `ontogony-platform` | Compose orchestration and startup/wait/reset scripts |
| 2 | `ENV-DOCKER-RUN-001` | `ontogony-platform`, optionally `allagma-dotnet` | Dockerized guided main flow with restart persistence proof |
| 3 | `ENV-DOCKER-FE-001` | `ontogony-frontend`, `ontogony-platform` | Frontend walkthrough against Docker-local APIs |
| 4 | `ENV-DOCKER-CLOSEOUT-001` | `ontogony-platform` | Close first Docker-local working system |

## Post-closeout hardening backlog

These are useful but must not block the first Docker-local closeout:

```text
KANON-OP-001
KANON-OP-002
CONEXUS-PERSIST-001
CONEXUS-PERSIST-002
CONEXUS-PERSIST-003
FE-HARDEN-001
FE-AUDIT-FIXTURES-001
FE-TEST-REPLAY-001
FE-HYGIENE-CONFIG-001
UI-PACKAGING-STATUS-001
TERMINOLOGY-CLEANUP-001
TRACE-CONTRACT-001
```

## Sequence rule

Do not move broad hardening ahead of `ENV-DOCKER-CLOSEOUT-001` unless a hard blocker prevents the first Dockerized local working system from functioning.
