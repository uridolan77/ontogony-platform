# Validation Matrix

| Validation | Command / Path | Expected |
|---|---|---|
| Compose config | `docker compose --env-file .env.example -f docker-compose.yml config` | PASS |
| Postgres health | Docker healthcheck | healthy |
| Kanon startup | `http://localhost:5081/health` | 2xx |
| Conexus liveness | `http://localhost:5082/health/live` | 2xx |
| Conexus strict readiness before bootstrap | `http://localhost:5082/ready` | may be 503; expected |
| Allagma startup | `http://localhost:5083/health` | 2xx |
| Seed proof | `seed-and-verify-local-working-system.ps1` | PASS |
| Conexus fake route | seed report | providerKey `fake`, routeDecisionIds present |
| Eval write/list | seed/guided report | PASS |
| Baseline comparison | seed/guided report | create/fetch PASS |
| Allagma restart durability | Docker guided main flow | evidence survives restart |
| Frontend root | `http://localhost:5175/` | 2xx HTML |
| Frontend SPA route | `/allagma/evaluations` | index.html fallback works |

## Health contract

Do not use Conexus `/ready` for Compose startup health.

Use:

```text
/health/live
```

for startup.

Use:

```text
/ready
```

for strict provider/store/alias readiness.
