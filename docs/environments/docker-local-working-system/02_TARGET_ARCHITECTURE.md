# Target architecture — Docker local working system

**Boundary:** first Dockerized local working system, not production readiness.

## Service topology

```text
docker compose (ontogony-platform/docker/local-working-system/)
│
├─ postgres
│   ├─ allagma_local
│   ├─ kanon_local
│   ├─ conexus_local
│   ├─ aisthesis_local
│   └─ metabole_local
│
├─ kanon-api       host http://localhost:5081  →  container http://kanon-api:8080
├─ conexus-api     host http://localhost:5082  →  container http://conexus-api:8080
├─ allagma-api     host http://localhost:5083  →  container http://allagma-api:8080
├─ aisthesis-api   host http://localhost:5084  →  container http://aisthesis-api:8080
├─ metabole-api    host http://localhost:5085  →  container http://metabole-api:8080
└─ ontogony-frontend   host http://localhost:5175 or :5080  →  container :8080
```

## Design choices (v1)

| Choice | Rationale |
| --- | --- |
| One PostgreSQL container | Five logical databases (`allagma_local`, `kanon_local`, `conexus_local`, `aisthesis_local`, `metabole_local`) — simpler operator setup |
| Container port 8080 for APIs | Aligns with ASP.NET Core `ASPNETCORE_URLS=http://+:8080` in images |
| Host ports 5081–5085 | Matches five-service local stack — Kanon/Conexus/Allagma muscle memory plus Aisthesis (5084) and Metabole (5085) |
| In-container DNS for Allagma → Kanon/Conexus | `http://kanon-api:8080`, `http://conexus-api:8080` |
| In-container DNS for Metabole → Kanon/Conexus | `http://kanon-api:8080`, `http://conexus-api:8080` |
| Browser → host URLs | Frontend and operator curl use `localhost:5081`–`5085` |
| Fake/local Conexus provider | No external model API keys for first Docker sanity |
| Real external execution blocked | Allagma safety boundary unchanged |

## Health and startup order

1. `postgres` healthy (`pg_isready`)
2. `kanon-api` and `conexus-api` started (migrations on startup)
3. `aisthesis-api` and `metabole-api` started after postgres (and Metabole after Kanon/Conexus started)
4. `allagma-api` started after postgres + peer APIs
5. `ontogony-frontend` after APIs (ENV-COMPOSE-001)

Per-service probes: `GET /health` (and `/ready` where applicable).

## What this architecture does not include

- TLS / certificates
- Production identity (OIDC, mTLS)
- Separate Postgres instances per service
- Cloud deployment or Kubernetes
- Real provider routing by default

See `09_KNOWN_LIMITATIONS.md`.
