# Startup modes — local operator sanity

Modes for the first working local environment. **Postgres and real provider are optional later PRs** — not required to pass first fake-provider sanity.

## Mode summary

| Mode | Persistence | Conexus provider | When to use |
| --- | --- | --- | --- |
| **default-in-memory** | Allagma InMemory (Development) | Fake | First sanity, daily operator work |
| **manual-process** | Same as default | Fake | Until ENV-SETUP-002 scripts ship |
| **postgres-durable** | Postgres (Allagma + Conexus) | Fake first | ENV-PG-001 — prove records survive restart |
| **real-provider** | Per base mode | OpenAI-compatible (backend only) | ENV-REAL-PROVIDER-001 — explicit opt-in |

## default-in-memory (recommended first)

- `ASPNETCORE_ENVIRONMENT=Development`
- Allagma `Persistence.Mode`: `InMemory` (`appsettings.Development.json`)
- Conexus: fake provider only (no outbound model API)
- Ports: Kanon `5081`, Conexus `5082`, Allagma `5083`

## manual-process (current until ENV-SETUP-002)

Start each API in its own terminal from the repo root, with env vars from `02_EXACT_SETTINGS.md`:

```powershell
# Terminal 1 — Kanon
cd C:\dev\kanon-dotnet\src\Kanon.Api
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --urls http://localhost:5081

# Terminal 2 — Conexus
cd C:\dev\conexus-dotnet\src\Conexus.Api
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:CONEXUS_DEV_PROJECT_API_KEY = "cx-dev-key-change-me"
dotnet run --urls http://localhost:5082

# Terminal 3 — Allagma
cd C:\dev\allagma-dotnet\src\Allagma.Api
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:Allagma__Evaluation__ManualWriteEnabled = "true"
$env:Kanon__BaseUrl = "http://localhost:5081"
$env:Conexus__BaseUrl = "http://localhost:5082"
$env:Conexus__ProjectApiKey = "cx-dev-key-change-me"
dotnet run --urls http://localhost:5083

# Terminal 4 — Frontend
cd C:\dev\ontogony-frontend
npm run dev
```

Quick health probe:

```powershell
Invoke-WebRequest http://localhost:5081/health -UseBasicParsing
Invoke-WebRequest http://localhost:5082/health -UseBasicParsing
Invoke-WebRequest http://localhost:5083/health -UseBasicParsing
```

Existing Allagma helpers (overlap with this program; use one style per session):

- `allagma-dotnet/scripts/run-local-stack.ps1` — desktop-oriented stack
- `allagma-dotnet/scripts/run-local-stack-headless.ps1` — CI/headless stack

## scripted-stack (ENV-SETUP-002, next)

Planned in `allagma-dotnet`:

```text
scripts/env/start-local-operator-sanity.ps1
scripts/env/check-local-operator-sanity.ps1
scripts/env/stop-local-operator-sanity.ps1
scripts/env/write-local-operator-sanity-env-template.ps1
```

`start-local-operator-sanity.ps1` will verify six repos, start APIs on fixed ports, optionally start frontend, and write `artifacts/env/local-operator-sanity/processes.json`. **Not implemented in ENV-SETUP-001.**

## postgres-durable (ENV-PG-001, optional)

- Local Postgres + migrations
- Restart Allagma and verify eval + baseline comparison still fetch
- **Does not** replace fake provider for first sanity sign-off

## real-provider (ENV-REAL-PROVIDER-001, optional)

- Backend-only API keys via environment
- Never in frontend `.env.local` or operator reports
- Real external execution remains **disabled** on Allagma for this program unless explicitly scoped otherwise in that PR

## Port conflicts

If `5081`–`5083` are in use:

```powershell
netstat -ano | findstr :5081
netstat -ano | findstr :5082
netstat -ano | findstr :5083
```

See `allagma-dotnet/docs/operations/SYSTEM_OBSERVABILITY_PORT_PREFLIGHT.md` and `-RuntimeProfile alternate` (`5181`–`5183`) in `LOCAL_RUNTIME_PROFILES.md`. If using alternate ports, update **all** base URL env vars and frontend `.env.local` consistently.
