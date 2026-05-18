# ENV-DOCKER-001 — Docker local working system plan

**Recorded at (UTC):** 2026-05-19  
**Verdict:** PASS  
**Statement:** This package supports the first working local environment and first Dockerized local working system. It is not production readiness.

## Scope

Documentation-only PR in `ontogony-platform`: canonical Docker local working system plan under `docs/environments/docker-local-working-system/`. No Dockerfiles, compose files, runtime source, workflows, or secrets.

## Delivered

```text
docs/environments/docker-local-working-system/
  00_MANIFEST.json
  README.md
  01_WORKSPACE_LAYOUT.md
  02_TARGET_ARCHITECTURE.md
  03_EXACT_SETTINGS.md
  04_DATABASES_AND_SEEDS.md
  05_DOCKER_COMPOSE_PLAN.md
  06_MAIN_USE_FLOW.md
  07_ACCEPTANCE_CHECKLIST.md
  08_TROUBLESHOOTING.md
  09_KNOWN_LIMITATIONS.md
docs/environments/README.md (updated)
docs/evidence/ENV_DOCKER_001_PLAN_EVIDENCE.md
```

## Commands run

```powershell
cd C:\dev\ontogony-platform
$base = "docs\environments\docker-local-working-system"
@(
  "00_MANIFEST.json", "README.md",
  "01_WORKSPACE_LAYOUT.md", "02_TARGET_ARCHITECTURE.md", "03_EXACT_SETTINGS.md",
  "04_DATABASES_AND_SEEDS.md", "05_DOCKER_COMPOSE_PLAN.md", "06_MAIN_USE_FLOW.md",
  "07_ACCEPTANCE_CHECKLIST.md", "08_TROUBLESHOOTING.md", "09_KNOWN_LIMITATIONS.md"
) | ForEach-Object { [PSCustomObject]@{ File = $_; Exists = (Test-Path (Join-Path $base $_)) } }

cd C:\dev\allagma-dotnet
.\scripts\env\check-local-operator-sanity.ps1 -DevRoot C:\dev -SkipHealthCheck
```

## Results

| Check | Result |
| --- | --- |
| All 11 plan files present | **PASS** |
| Six-repo workspace layout | **PASS** |
| Config key verification (Kanon/Conexus/Allagma) | **PASS** — documented in `03_EXACT_SETTINGS.md` with repo-accurate keys |
| Compose/docker implementation | **Not in scope** (ENV-DOCKER-002 … ENV-COMPOSE-001) |

## Config notes captured in plan

| Service | Verified setting |
| --- | --- |
| Allagma | `Allagma__Persistence__Mode=Postgres`, `ConnectionStrings__Allagma` |
| Kanon | `Kanon__Persistence__Mode=Postgres`, `Kanon__Persistence__Postgres__ConnectionString` |
| Conexus | `ConnectionStrings__ConexusPostgres` (no `Persistence__Mode` toggle) |

## Safety

| Check | Status |
| --- | --- |
| No secrets committed | **yes** (placeholder dev passwords only in docs) |
| Real external execution | **disabled** (documented) |
| Production readiness claimed | **no** |

## Known limitations

Plan only — no `docker compose up` until ENV-COMPOSE-001. See `09_KNOWN_LIMITATIONS.md`.

## Next step

**ENV-DOCKER-002** — Dockerfiles and `.dockerignore` for allagma-dotnet, kanon-dotnet, conexus-dotnet, ontogony-frontend (`allagma-dotnet/docs/environments/working-local-environment-complete-package/04_PR_SPECS/ENV-DOCKER-002.md`).
