# ENV-SEED-001 — deterministic seed/bootstrap evidence

**Recorded at (UTC):** 2026-05-18T22:59:26Z  
**Verdict:** PASS  
**Statement:** This package supports the first working local environment and first Dockerized local working system. It is not production readiness.

## Scope

`ontogony-platform` only:

- deterministic seed/bootstrap runtime script in compose tree
- API-based verification for Conexus fake route, Kanon topology authorization behavior, and Allagma evaluation write/list/comparison flows
- docs/evidence updates for Docker local working system sequence

No compose orchestration, no raw schema SQL seed rows, and no production hardening.

## Delivered

```text
docker/local-working-system/scripts/seed-and-verify-local-working-system.ps1
docker/local-working-system/artifacts/env-seed-001-report.json
docker/local-working-system/README.md
docs/environments/docker-local-working-system/04_DATABASES_AND_SEEDS.md
docs/environments/docker-local-working-system/08_TROUBLESHOOTING.md
docs/environments/docker-local-working-system/09_KNOWN_LIMITATIONS.md
docs/environments/docker-local-working-system/README.md
docs/environments/README.md
docs/releases/FIRST_WORKING_ENVIRONMENT_NEXT_STEPS.md
docs/evidence/ENV_SEED_001_DETERMINISTIC_BOOTSTRAP_EVIDENCE.md
```

## Commands run

```powershell
cd C:\dev\ontogony-platform

# script syntax check
powershell -NoProfile -Command "`$tokens=`$null; `$errors=`$null; [void][System.Management.Automation.Language.Parser]::ParseFile('C:\dev\ontogony-platform\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1',[ref]`$tokens,[ref]`$errors); if(`$errors.Count -gt 0){ `$errors | ForEach-Object { Write-Host `$_.Message }; exit 1 }; Write-Host 'PS_PARSE_OK'"

# service health probe
powershell -NoProfile -Command "`$urls=@('http://localhost:5081/health','http://localhost:5082/health','http://localhost:5083/health'); foreach(`$u in `$urls){ try { `$r=Invoke-WebRequest -UseBasicParsing -TimeoutSec 3 -Uri `$u; Write-Host (`$u + ' => ' + `$r.StatusCode) } catch { Write-Host (`$u + ' => DOWN') } }"

# deterministic seed/bootstrap + verification
powershell -NoProfile -ExecutionPolicy Bypass -File "C:\dev\ontogony-platform\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1"
```

## Results

| Check | Result |
| --- | --- |
| PowerShell parser (`seed-and-verify-local-working-system.ps1`) | **PASS** (`PS_PARSE_OK`) |
| Health probe (`5081/5082/5083`) | **PASS** (`200` on all services) |
| Conexus dev bootstrap API | **PASS** (`projectId=dev-project`, alias `gpt-4o-mini`, provider `fake`) |
| Kanon topology behavior via Allagma runs | **PASS** (baseline `single_workflow` null auth decision id; subject `centralized_orchestrator` non-empty decision id) |
| Conexus route evidence | **PASS** (baseline + subject `routeDecisionId` non-empty and fetchable via admin route-decision API) |
| Allagma evaluation persistence APIs | **PASS** (write/list baseline+subject evals; baseline comparison create/fetch) |
| JSON report artifact | **PASS** (`docker/local-working-system/artifacts/env-seed-001-report.json`) |

## Key runtime IDs (from report)

- baseline run: `run_ae3661a64537436186b28b6951a50a6a`
- subject run: `run_b1e0011da8bf48d6bbbd39da5dcb4047`
- subject topology authorization decision: `decision_f8113a953e954c629d68a4eed34bbdf9`
- baseline route decision: `rd-0HNLKV6BPTQGQ-00000001`
- subject route decision: `rd-0HNLKV6BPTQGQ-00000002`
- baseline comparison: `cmp_86f24d4785cd4e1e993e8f718155e1e2`

## Safety

| Check | Status |
| --- | --- |
| Fake/local Conexus provider | **yes** (`providerKey=fake`) |
| Real external execution enabled | **no** (unchanged) |
| Real provider keys committed | **no** |
| Production readiness claim | **no** |

## Known limitations

- ENV-COMPOSE-001 remains next for `docker-compose.yml` + `.env.example`.
- Seed script verifies API-level persistence behavior; restart-survival proof remains in guided Docker run/closeout phases.

## Next step

**ENV-COMPOSE-001** — Docker Compose orchestration in `ontogony-platform/docker/local-working-system/`.
