# Workspace layout — Docker local working system

Same eight-repo dev root as the closed script-based program. Docker Compose lives under **ontogony-platform** with build contexts pointing at sibling repos.

## Directory tree

```text
C:\dev\
  ontogony-platform\
    docker\local-working-system\     # Planned compose root (ENV-COMPOSE-001)
    docs\environments\docker-local-working-system\   # This plan (ENV-DOCKER-001)
  allagma-dotnet\
  kanon-dotnet\
  conexus-dotnet\
  metabole-dotnet\
  aisthesis-dotnet\
  ontogony-frontend\
  ontogony-ui\
```

## Role of each repo

| Repo | Role in Docker local working system |
| --- | --- |
| `ontogony-platform` | Compose orchestration root, environment plan docs |
| `allagma-dotnet` | Governed execution API image (`allagma-api`) |
| `kanon-dotnet` | Meaning / topology authorization API image (`kanon-api`) |
| `conexus-dotnet` | Model gateway API image (`conexus-api`); fake provider in Development |
| `metabole-dotnet` | Data transformation API image (`metabole-api`) |
| `aisthesis-dotnet` | Evidence spine API image (`aisthesis-api`) |
| `ontogony-frontend` | Operator UI image (`ontogony-frontend`) |
| `ontogony-ui` | `@ontogony/ui` package — build dependency for frontend image, not a compose service |

## Environment variables (workspace roots)

```powershell
$env:ONTOGONY_DEV_ROOT = "C:\dev"
$env:ONTOGONY_PLATFORM_ROOT = "C:\dev\ontogony-platform"
$env:ALLAGMA_ROOT = "C:\dev\allagma-dotnet"
$env:KANON_ROOT = "C:\dev\kanon-dotnet"
$env:CONEXUS_ROOT = "C:\dev\conexus-dotnet"
$env:METABOLE_ROOT = "C:\dev\metabole-dotnet"
$env:AISTHESIS_ROOT = "C:\dev\aisthesis-dotnet"
$env:ONTOGONY_FRONTEND_ROOT = "C:\dev\ontogony-frontend"
$env:ONTOGONY_UI_ROOT = "C:\dev\ontogony-ui"
```

## Verify layout

```powershell
$root = "C:\dev"
@(
  "ontogony-platform",
  "allagma-dotnet",
  "kanon-dotnet",
  "conexus-dotnet",
  "metabole-dotnet",
  "aisthesis-dotnet",
  "ontogony-frontend",
  "ontogony-ui"
) | ForEach-Object {
  $p = Join-Path $root $_
  [PSCustomObject]@{ Repo = $_; Exists = (Test-Path $p) }
}
```

All eight `Exists` values should be `True` before building images or running compose.

## Related docs

- Architecture: `02_TARGET_ARCHITECTURE.md`
- Compose plan: `05_DOCKER_COMPOSE_PLAN.md`
- Script-based predecessor: `../local-operator-sanity/01_WORKSPACE_LAYOUT.md`
