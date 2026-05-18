# Exact settings — Docker local working system

**Boundary:** Development Docker operator workflows only. Credentials below are **local dev placeholders** — not for staging or production.

## Host vs container URLs

| Consumer | Kanon | Conexus | Allagma |
| --- | --- | --- | --- |
| Browser / frontend on host | `http://localhost:5081` | `http://localhost:5082` | `http://localhost:5083` |
| Allagma container → peers | `http://kanon-api:8080` | `http://conexus-api:8080` | — |
| API containers → Postgres | `Host=postgres;Port=5432;...` | same | same |

## Shared backend container settings

```env
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:8080
```

## Allagma (`allagma-api` service)

Verified keys (see `Allagma.Infrastructure` persistence resolver):

```env
Allagma__Persistence__Mode=Postgres
Allagma__Evaluation__ManualWriteEnabled=true
Kanon__BaseUrl=http://kanon-api:8080
Conexus__BaseUrl=http://conexus-api:8080
Conexus__ProjectApiKey=cx-dev-key-change-me
ConnectionStrings__Allagma=Host=postgres;Port=5432;Database=allagma_local;Username=allagma_local;Password=allagma_local_pw
```

Alternate (equivalent): `Allagma__Persistence__Postgres__ConnectionString` with the same connection string value.

## Kanon (`kanon-api` service)

Verified keys (`Kanon:Persistence` section — `Kanon.Infrastructure/Persistence/KanonPersistenceOptions.cs`):

```env
Kanon__Persistence__Mode=Postgres
Kanon__Persistence__Postgres__ConnectionString=Host=postgres;Port=5432;Database=kanon_local;Username=kanon_local;Password=kanon_local_pw
Kanon__Persistence__Postgres__ApplyMigrationsOnStartup=true
```

**Note:** `ConnectionStrings__Kanon` is **not** read by Kanon today; use `Kanon__Persistence__Postgres__ConnectionString` in compose.

## Conexus (`conexus-api` service)

Verified keys (`Conexus.Persistence.DependencyInjection.ResolvePostgresConnectionString`):

```env
ConnectionStrings__ConexusPostgres=Host=postgres;Port=5432;Database=conexus_local;Username=conexus_local;Password=conexus_local_pw
CONEXUS_DEV_PROJECT_API_KEY=cx-dev-key-change-me
```

**Note:** Conexus enables Postgres persistence when a non-empty connection string is configured. There is no `Conexus__Persistence__Mode` toggle — omitting the connection string keeps in-memory stores.

Optional section form: `Conexus__Persistence__ConnectionString` (same value as above).

## Frontend (`ontogony-frontend` service or host dev)

Browser must reach APIs on **host** ports:

```env
VITE_ALLAGMA_BASE_URL=http://localhost:5083
VITE_KANON_BASE_URL=http://localhost:5081
VITE_CONEXUS_BASE_URL=http://localhost:5082
```

No provider secrets in frontend env files.

## Postgres (container)

Planned admin bootstrap (see `04_DATABASES_AND_SEEDS.md`):

```env
POSTGRES_USER=ontogony_admin
POSTGRES_PASSWORD=ontogony_admin_pw
POSTGRES_DB=postgres
```

Per-service databases/users created by init SQL (ENV-DB-001).

## Safety

- **Fake/local** Conexus provider in Development — no real API keys in compose.
- **Real external execution** remains disabled on Allagma.
- **Manual eval POST** only when `ManualWriteEnabled=true` and non-production host.

## Port conflicts

If host port `5432` is already in use (local Postgres, ENV-PG-001 `55432` container, etc.), map postgres to an alternate host port in compose (e.g. `55433:5432`) and document the mapping in operator runbooks (ENV-COMPOSE-001).
