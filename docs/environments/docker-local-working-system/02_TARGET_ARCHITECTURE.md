# Target architecture — Docker local working system

**Boundary:** first Dockerized local working system, not production readiness.

## Service topology

```text
docker compose (ontogony-platform/docker/local-working-system/)
│
├─ postgres
│   ├─ allagma_local
│   ├─ kanon_local
│   └─ conexus_local
│
├─ kanon-api       host http://localhost:5081  →  container http://kanon-api:8080
├─ conexus-api     host http://localhost:5082  →  container http://conexus-api:8080
├─ allagma-api     host http://localhost:5083  →  container http://allagma-api:8080
└─ ontogony-frontend   host http://localhost:5175 or :5080  →  container :8080
```

## Design choices (v1)

| Choice | Rationale |
| --- | --- |
| One PostgreSQL container | Three logical databases (`allagma_local`, `kanon_local`, `conexus_local`) — simpler operator setup |
| Container port 8080 for APIs | Aligns with ASP.NET Core `ASPNETCORE_URLS=http://+:8080` in images |
| Host ports 5081–5083 | Matches closed script-based program — reuse operator muscle memory |
| In-container DNS for Allagma → Kanon/Conexus | `http://kanon-api:8080`, `http://conexus-api:8080` |
| Browser → host URLs | Frontend and operator curl use `localhost:5081`–`5083` |
| Fake/local Conexus provider | No external model API keys for first Docker sanity |
| Real external execution blocked | Allagma safety boundary unchanged |

## Health and startup order

1. `postgres` healthy (`pg_isready`)
2. `kanon-api` and `conexus-api` started (migrations on startup)
3. `allagma-api` started after postgres + peer APIs
4. `ontogony-frontend` after APIs (ENV-COMPOSE-001)

Per-service probes: `GET /health` (and `/ready` where applicable).

## What this architecture does not include

- TLS / certificates
- Production identity (OIDC, mTLS)
- Separate Postgres instances per service
- Cloud deployment or Kubernetes
- Real provider routing by default

See `09_KNOWN_LIMITATIONS.md`.
