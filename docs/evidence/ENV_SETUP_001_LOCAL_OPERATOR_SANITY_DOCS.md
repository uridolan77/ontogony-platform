# ENV-SETUP-001 — Local operator sanity environment docs

**Recorded at (UTC):** 2026-05-18  
**Verdict:** PASS

## Scope

Add canonical operator documentation under `docs/environments/local-operator-sanity/` per `local-operator-sanity-package/04_PR_SPECS/ENV-SETUP-001.md`. Documentation only — no runtime source, no stack scripts (ENV-SETUP-002).

## Delivered

```text
docs/environments/local-operator-sanity/
  00_MANIFEST.json
  01_WORKSPACE_LAYOUT.md
  02_EXACT_SETTINGS.md
  03_MAIN_USE_FLOW.md
  04_STARTUP_MODES.md
  05_ACCEPTANCE_CHECKLIST.md
  06_TROUBLESHOOTING.md
  07_KNOWN_LIMITATIONS.md
docs/environments/README.md (updated — canonical path + ENV-SETUP-001 complete)
```

## Repo heads at delivery

| Repo | HEAD |
| --- | --- |
| `ontogony-platform` | `e2b9d066acc1d85081fd3f7ef6040e045f67b505` |
| `allagma-dotnet` | `afc1daa00a89fb868b3b54ec1c739a5f849b8a4b` |
| `kanon-dotnet` | `b4e1d34123534fcdd29dbfcb9aa294ab57ad9791` |
| `conexus-dotnet` | `cb7860694e53b0abe8fe607ec5532d6734de73ae` |
| `ontogony-frontend` | `61b111c57e30cf9dd2921cda6f197bc159a5ed0e` |
| `ontogony-ui` | `10f8a02665c17390ea60836afee2d12e3097a2d5` |

## Commands run

```powershell
cd c:\dev\ontogony-platform
$base = "docs\environments\local-operator-sanity"
@(
  "00_MANIFEST.json",
  "01_WORKSPACE_LAYOUT.md",
  "02_EXACT_SETTINGS.md",
  "03_MAIN_USE_FLOW.md",
  "04_STARTUP_MODES.md",
  "05_ACCEPTANCE_CHECKLIST.md",
  "06_TROUBLESHOOTING.md",
  "07_KNOWN_LIMITATIONS.md"
) | ForEach-Object { Test-Path (Join-Path $base $_) }

$root = "C:\dev"
@("ontogony-platform","allagma-dotnet","kanon-dotnet","conexus-dotnet","ontogony-frontend","ontogony-ui") |
  ForEach-Object { Test-Path (Join-Path $root $_) }
```

## Results

| Check | Result |
| --- | --- |
| All 8 canonical doc paths exist | **8/8 True** |
| Six workspace repos present under `C:\dev` | **6/6 True** |
| Runtime / script changes | **None** (by design) |
| `run-full-sanity.ps1` | **Not run** (requires live stack; out of scope for SETUP-001) |

## Documentation coverage (spec § Must document)

| Requirement | Location |
| --- | --- |
| `C:\dev\...` layout including `ontogony-ui` | `01_WORKSPACE_LAYOUT.md` |
| Ports 5081 / 5082 / 5083 | `02_EXACT_SETTINGS.md`, `00_MANIFEST.json` |
| Fake/local provider first | `02_EXACT_SETTINGS.md`, `04_STARTUP_MODES.md` |
| `Allagma__Evaluation__ManualWriteEnabled=true` | `02_EXACT_SETTINGS.md` |
| No real external execution | `02_EXACT_SETTINGS.md`, `07_KNOWN_LIMITATIONS.md` |
| Not production readiness | All docs + manifest `boundary` |

## Safety confirmation

- No runtime source modified in `ontogony-platform`.
- No `scripts/env/*` implementation (deferred to ENV-SETUP-002 in `allagma-dotnet`).
- Provider secrets documented as backend-only, never in frontend or reports.

## Next step

**ENV-SETUP-002** — `allagma-dotnet` stack start/check/stop scripts and env template (`local-operator-sanity-package/04_PR_SPECS/ENV-SETUP-002.md`).
