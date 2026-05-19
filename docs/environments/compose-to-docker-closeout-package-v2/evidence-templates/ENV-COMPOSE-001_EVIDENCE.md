# ENV-COMPOSE-001 — Docker Compose orchestration evidence

## Verdict

PASS / FAIL

## Commands

```powershell
docker compose --env-file .env.example -f docker-compose.yml config
.\scripts\start-local-working-system.ps1 -Build -SkipFrontend
.\scripts\wait-local-working-system.ps1 -SkipFrontend
```

## Health results

| Check | Result |
|---|---|
| Postgres healthy | |
| Kanon /health | |
| Conexus /health/live | |
| Conexus /ready before bootstrap | expected strict readiness, may be 503 |
| Allagma /health | |

## Safety

| Check | Status |
|---|---|
| Real provider keys | none |
| Real external execution | blocked |
| Secrets committed | no |
| Production readiness claimed | no |

## Next

ENV-DOCKER-RUN-001.
