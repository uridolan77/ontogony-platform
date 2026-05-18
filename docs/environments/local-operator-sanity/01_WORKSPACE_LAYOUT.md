# Workspace layout — local operator sanity

This program assumes six sibling repositories under a single dev root. Paths below use `C:\dev\`; adjust `ONTOGONY_DEV_ROOT` if your root differs.

## Directory tree

```text
C:\dev\
  ontogony-platform\     # Shared mechanics, planning, environment docs (this tree)
  allagma-dotnet\        # Governed execution API (port 5083)
  kanon-dotnet\           # Meaning / topology authorization API (port 5081)
  conexus-dotnet\         # Model gateway API (port 5082)
  ontogony-frontend\      # Allagma operator UI (Vite)
  ontogony-ui\            # Shared UI package + dev laboratory (not the product frontend)
```

## Role of each repo

| Repo | Role in local sanity |
| --- | --- |
| `ontogony-platform` | Platform packages, cross-repo planning, canonical environment docs |
| `allagma-dotnet` | Runs, evaluations, baseline comparison, sanity scripts (from ENV-SETUP-002 onward) |
| `kanon-dotnet` | Ontology, decision records, topology authorization |
| `conexus-dotnet` | Model routing; **fake provider** in Development by default |
| `ontogony-frontend` | Operator routes for runs, evaluations, baseline comparisons |
| `ontogony-ui` | `@ontogony/ui` primitives; Storybook/dev lab — integrated in ENV-UI-001, not required to start APIs |

## Environment variables (workspace roots)

Set in each PowerShell session (or via ENV-SETUP-002 template script):

```powershell
$env:ONTOGONY_DEV_ROOT = "C:\dev"
$env:ONTOGONY_PLATFORM_ROOT = "C:\dev\ontogony-platform"
$env:ALLAGMA_ROOT = "C:\dev\allagma-dotnet"
$env:KANON_ROOT = "C:\dev\kanon-dotnet"
$env:CONEXUS_ROOT = "C:\dev\conexus-dotnet"
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
  "ontogony-frontend",
  "ontogony-ui"
) | ForEach-Object {
  $p = Join-Path $root $_
  [PSCustomObject]@{ Repo = $_; Exists = (Test-Path $p) }
}
```

All six `Exists` values should be `True` before running the stack.

## Related docs

- Exact ports and app settings: `02_EXACT_SETTINGS.md`
- How to start services: `04_STARTUP_MODES.md` (scripts land in ENV-SETUP-002)
- Planning package (PR specs, prompts, stubs): `../local-operator-sanity-package/`
