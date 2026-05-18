# ENV-COMPOSE-001 — Docker Compose orchestration evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** PASS (compose implementation complete; backend startup wait now passes with Conexus liveness probe)  
**Statement:** This package supports the first working local environment and first Dockerized local working system. It is not production readiness.

## Scope

`ontogony-platform` only:

- add working compose root artifacts (`docker-compose.yml`, `.env.example`)
- add compose operator scripts (`start`, `wait`, `reset`)
- update Docker local working-system docs and sequence status
- record targeted validation output for compose rendering + runtime startup behavior

No production hardening, no real provider keys, no real external execution enablement.

## Delivered

```text
docker/local-working-system/docker-compose.yml
docker/local-working-system/.env.example
docker/local-working-system/scripts/start-local-working-system.ps1
docker/local-working-system/scripts/wait-local-working-system.ps1
docker/local-working-system/scripts/reset-local-working-system.ps1
docker/local-working-system/README.md
docs/environments/docker-local-working-system/00_MANIFEST.json
docs/environments/docker-local-working-system/05_DOCKER_COMPOSE_PLAN.md
docs/environments/docker-local-working-system/06_MAIN_USE_FLOW.md
docs/environments/docker-local-working-system/09_KNOWN_LIMITATIONS.md
docs/environments/docker-local-working-system/README.md
docs/environments/README.md
docs/releases/FIRST_WORKING_ENVIRONMENT_NEXT_STEPS.md
docs/evidence/ENV_COMPOSE_001_DOCKER_COMPOSE_ORCHESTRATION_EVIDENCE.md
```

## Commands run

```powershell
cd C:\dev\ontogony-platform

# Compose schema/config rendering
docker compose --env-file .\docker\local-working-system\.env.example `
  -f .\docker\local-working-system\docker-compose.yml config

# PowerShell parser checks
$files = @(
  'docker/local-working-system/scripts/start-local-working-system.ps1',
  'docker/local-working-system/scripts/wait-local-working-system.ps1',
  'docker/local-working-system/scripts/reset-local-working-system.ps1'
)
foreach ($f in $files) {
  $tokens = $null; $errs = $null
  [System.Management.Automation.Language.Parser]::ParseFile((Resolve-Path $f), [ref]$tokens, [ref]$errs) | Out-Null
  if ($errs.Count -gt 0) { throw "Parse failed: $f" }
}

# Runtime startup validation on alternate host ports (to avoid local stack collisions)
$env:POSTGRES_HOST_PORT='55434'
$env:KANON_HOST_PORT='5181'
$env:CONEXUS_HOST_PORT='5182'
$env:ALLAGMA_HOST_PORT='5183'
$env:FRONTEND_HOST_PORT='5275'
docker compose --env-file .\docker\local-working-system\.env.example `
  -f .\docker\local-working-system\docker-compose.yml `
  up -d --build postgres kanon-api conexus-api allagma-api

powershell -NoProfile -ExecutionPolicy Bypass `
  -File .\docker\local-working-system\scripts\wait-local-working-system.ps1 `
  -SkipFrontend -TimeoutSeconds 60

# Probe Conexus liveness vs readiness before bootstrap
Invoke-WebRequest -UseBasicParsing http://localhost:5182/health/live
Invoke-WebRequest -UseBasicParsing http://localhost:5182/ready

# Cleanup
docker compose --env-file .\docker\local-working-system\.env.example `
  -f .\docker\local-working-system\docker-compose.yml `
  down -v --remove-orphans
```

## Results

| Check | Result |
| --- | --- |
| `docker compose config` renders full model | **PASS** |
| Build contexts resolved for all services (including `additional_contexts`) | **PASS** |
| `.env.example` placeholders expanded correctly | **PASS** |
| Script parser checks (`start`, `wait`, `reset`) | **PASS** |
| Runtime compose `up -d --build` for postgres+APIs | **PASS** (containers built and started) |
| `wait-local-working-system.ps1` postgres health gate | **PASS** |
| `wait-local-working-system.ps1` API health completion | **PASS** |
| Conexus liveness (`/health/live`) before bootstrap | **PASS** (`200`) |
| Conexus readiness (`/ready`) before bootstrap | **Expected non-green** (`503`, strict provider/readiness invariant) |
| Teardown cleanup (`down -v --remove-orphans`) | **PASS** |

## Safety

| Check | Status |
| --- | --- |
| Real provider keys committed | **no** |
| Dev placeholder credentials only | **yes** |
| Real external execution enabled | **no change** |
| Production readiness claim | **no** |

## Known limitations

- Frontend service runtime health was not gated in this evidence run (`-SkipFrontend`) to keep the check focused on backend orchestration.
- Conexus `/ready` is intentionally stricter than liveness and can remain non-green until provider/alias bootstrap has completed.
- ENV-SEED-001 remains host-local API evidence; restart-survival proof remains in ENV-DOCKER-RUN-001.

## Next step

**ENV-DOCKER-RUN-001** — Dockerized guided main flow and restart-survival persistence proof (after Kanon startup path is green on compose-backed Postgres).
