# Exact settings — local operator sanity

**Boundary:** first working **local** environment. These values are for Development operator workflows, not production deployment.

## Service ports

| Service | Base URL | Health |
| --- | --- | --- |
| Kanon | `http://localhost:5081` | `GET /health`, `GET /ready` |
| Conexus | `http://localhost:5082` | `GET /health`, `GET /ready` |
| Allagma | `http://localhost:5083` | `GET /health`, `GET /ready` |

Alternate ports (`5181`–`5183`) exist in Allagma runtime profiles for port conflicts; the **default** local-operator sanity path uses `5081`–`5083`. See `allagma-dotnet/docs/system/LOCAL_RUNTIME_PROFILES.md`.

## Session environment (PowerShell)

### Workspace

```powershell
$env:ONTOGONY_DEV_ROOT = "C:\dev"
$env:ONTOGONY_PLATFORM_ROOT = "C:\dev\ontogony-platform"
$env:ALLAGMA_ROOT = "C:\dev\allagma-dotnet"
$env:KANON_ROOT = "C:\dev\kanon-dotnet"
$env:CONEXUS_ROOT = "C:\dev\conexus-dotnet"
$env:ONTOGONY_FRONTEND_ROOT = "C:\dev\ontogony-frontend"
$env:ONTOGONY_UI_ROOT = "C:\dev\ontogony-ui"
```

### Base URLs (scripts and probes)

```powershell
$env:KANON_BASE_URL = "http://localhost:5081"
$env:CONEXUS_BASE_URL = "http://localhost:5082"
$env:ALLAGMA_BASE_URL = "http://localhost:5083"
```

### Common

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
```

## Allagma (required for eval operator path)

```powershell
$env:Allagma__Evaluation__ManualWriteEnabled = "true"
$env:Kanon__BaseUrl = "http://localhost:5081"
$env:Conexus__BaseUrl = "http://localhost:5082"
$env:Conexus__ProjectApiKey = "cx-dev-key-change-me"
```

Defaults also appear in `allagma-dotnet/src/Allagma.Api/appsettings.Development.json` (`Persistence.Mode`: `InMemory` for first sanity).

**Manual evaluation POST** is allowed only when `ManualWriteEnabled=true` **and** the host is non-production. Do not enable this in staging or production configs.

## Conexus (fake/local provider first)

Development registers the deterministic **fake** provider (`FakeProviderChatClient`). No external model API calls in this mode.

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:CONEXUS_DEV_PROJECT_API_KEY = "cx-dev-key-change-me"
```

Use the same project API key on Allagma (`Conexus__ProjectApiKey`) and Conexus dev bootstrap. **Do not** commit real provider keys.

Optional real provider (later PR **ENV-REAL-PROVIDER-001** only):

```powershell
# Backend-only, never in frontend .env.local or reports
$env:CONEXUS_PROVIDER_OPENAI_API_KEY = "sk-..."
```

Never place provider secrets in frontend `.env.local`, `appsettings` committed to git, browser storage, screenshots, reports, or database rows.

## Kanon

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
# Default listen: http://localhost:5081 (launchSettings / Kestrel)
```

## Frontend (`ontogony-frontend`)

Suggested `.env.local` (only if the app needs explicit bases):

```env
VITE_ALLAGMA_BASE_URL=http://localhost:5083
VITE_KANON_BASE_URL=http://localhost:5081
VITE_CONEXUS_BASE_URL=http://localhost:5082
```

See `ontogony-frontend/.env.example`. No provider secrets in the frontend.

## Safety settings (do not relax for first sanity)

| Setting | Expected |
| --- | --- |
| Conexus provider | Fake/local in Development |
| Allagma real external execution | **Disabled** (capabilities API reports `RealExternalExecution.Enabled: false`) |
| Manual eval POST | Gated (`ManualWriteEnabled` + non-production) |
| Raw prompts/completions in UI or reports | **Forbidden** |

## `ontogony-ui`

Runs its own Vite dev lab (`npm run dev`) for component work. It is **not** a substitute for `ontogony-frontend` during the main operator sanity flow. ENV-UI-001 covers integration readiness.
