# Exact settings — Docker local working system

**Boundary:** Development Docker operator workflows only. Credentials below are **local dev placeholders** — not for staging or production.

## Host vs container URLs

| Consumer | Kanon | Conexus | Allagma | Aisthesis | Metabole |
| --- | --- | --- | --- | --- | --- |
| Browser / frontend on host | `http://localhost:5081` | `http://localhost:5082` | `http://localhost:5083` | `http://localhost:5084` | `http://localhost:5085` |
| Allagma container → peers | `http://kanon-api:8080` | `http://conexus-api:8080` | — | — | — |
| Metabole container → peers | `http://kanon-api:8080` | `http://conexus-api:8080` | — | — | — |
| API containers → Postgres | `Host=postgres;Port=5432;...` | same | same | same | same |

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

## Aisthesis (`aisthesis-api` service)

Verified keys (`Aisthesis.Api/Program.cs`):

```env
ConnectionStrings__Aisthesis=Host=postgres;Port=5432;Database=aisthesis_local;Username=aisthesis_local;Password=aisthesis_local_pw
AISTHESIS_RequireServiceToken=false
```

When `ConnectionStrings__Aisthesis` is set, EF migrations apply on startup via `DatabaseMigrationService`.

## Metabole (`metabole-api` service)

Verified keys (`Metabole.Api/ServiceCollectionExtensions.cs`):

```env
Metabole__Storage=Postgres
Metabole__Postgres__Enabled=true
Metabole__Postgres__MigrationMode=Apply
Metabole__Postgres__Credentials__local-dev-metabole-postgres=Host=postgres;Port=5432;Database=metabole_local;Username=metabole_local;Password=metabole_local_pw
Metabole__Kanon__Enabled=true
Metabole__Kanon__Mode=Http
Metabole__Kanon__BaseUrl=http://kanon-api:8080
Metabole__Kanon__ServiceToken=kanon-dev-service-token-change-in-production
Metabole__Conexus__Enabled=true
Metabole__Conexus__Mode=Http
Metabole__Conexus__BaseUrl=http://conexus-api:8080
Metabole__Conexus__ApiKey=cx-dev-key-change-me
Metabole__Auth__Enabled=false
Metabole__Auth__DevelopmentAllowAnonymous=true
```

**Note:** When running Metabole standalone via `dotnet run`, the default dev port is **5084**. In the full Docker five-service stack, Metabole is mapped to host **5085** so Aisthesis can use **5084**.

## Frontend (`ontogony-frontend` service or host dev)

Browser must reach APIs on **host** ports:

```env
VITE_ALLAGMA_BASE_URL=http://localhost:5083
VITE_AISTHESIS_BASE_URL=http://localhost:5084
VITE_METABOLE_BASE_URL=http://localhost:5085
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
