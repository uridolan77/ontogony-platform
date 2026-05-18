# Exact Settings

## Workspace env

```powershell
$env:ONTOGONY_DEV_ROOT="C:\dev"
$env:ONTOGONY_PLATFORM_ROOT="C:\dev\ontogony-platform"
$env:ALLAGMA_ROOT="C:\dev\allagma-dotnet"
$env:KANON_ROOT="C:\dev\kanon-dotnet"
$env:CONEXUS_ROOT="C:\dev\conexus-dotnet"
$env:ONTOGONY_FRONTEND_ROOT="C:\dev\ontogony-frontend"
$env:ONTOGONY_UI_ROOT="C:\dev\ontogony-ui"
```

## Ports

```powershell
$env:KANON_BASE_URL="http://localhost:5081"
$env:CONEXUS_BASE_URL="http://localhost:5082"
$env:ALLAGMA_BASE_URL="http://localhost:5083"
```

## Common

```powershell
$env:ASPNETCORE_ENVIRONMENT="Development"
```

## Allagma

```powershell
$env:Allagma__Evaluation__ManualWriteEnabled="true"
$env:Kanon__BaseUrl="http://localhost:5081"
$env:Conexus__BaseUrl="http://localhost:5082"
$env:Conexus__ProjectApiKey="cx-dev-key-change-me"
```

## Conexus

Fake/local provider first:

```powershell
$env:ASPNETCORE_ENVIRONMENT="Development"
$env:CONEXUS_DEV_PROJECT_API_KEY="cx-dev-key-change-me"
```

Optional real provider only later:

```powershell
$env:CONEXUS_PROVIDER_OPENAI_API_KEY="sk-..."
```

Never place real provider keys in frontend `.env.local`, appsettings JSON, browser storage, screenshots, reports, or database rows.

## Frontend

Suggested local `.env.local` values only if the app needs them:

```env
VITE_ALLAGMA_BASE_URL=http://localhost:5083
VITE_KANON_BASE_URL=http://localhost:5081
VITE_CONEXUS_BASE_URL=http://localhost:5082
```

No provider secrets.
