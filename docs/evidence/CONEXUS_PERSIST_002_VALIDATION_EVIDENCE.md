# CONEXUS-PERSIST-002 — Validation automation evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS**  
**Statement:** Post-Docker-local hardening validation automation; **not production readiness**.

## Scope

Add `validate-conexus-persistence-bootstrap.ps1` and supporting docs for repeatable Conexus Docker-local persistence/bootstrap checks. No runtime source changes in service repos; no workflows or secrets.

## Delivered

```text
docker/local-working-system/scripts/validate-conexus-persistence-bootstrap.ps1
docker/local-working-system/README.md
docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/CONEXUS-PERSIST-002.md
docs/environments/compose-to-docker-closeout-package-v2/01_PR_SEQUENCE.md
docs/environments/compose-to-docker-closeout-package-v2/04_STATUS_BOARD.md
docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md
docs/evidence/CONEXUS_PERSIST_002_VALIDATION_EVIDENCE.md
```

## Prerequisites

- Docker local working system running (`docker compose ps` shows `conexus-api`, `postgres` healthy).
- Prior guided flow artifacts optional but present on validation host (`docker-guided-main-flow-report.json`).

## Commands run

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\validate-conexus-persistence-bootstrap.ps1
Get-Content .\docker\local-working-system\artifacts\conexus-persist-002-report.json |
  ConvertFrom-Json | Select-Object schema, verdict, @{n='checks';e={$_.checks.Count}}
```

## Results

| Check | Result |
| --- | --- |
| Script exit code | **0** |
| Report verdict | **PASS** |
| `stack.reachable` | PASS |
| `conexus.health.live` | PASS (HTTP 200) |
| `conexus.ready.captured` | INFO (HTTP 503 — expected; strict readiness) |
| `postgres.conexus_local` | PASS |
| `postgres.migrations` | PASS (11 migration rows; `conexus_model_alias` present) |
| `compose.render` | PASS |
| `keys.aligned` | PASS (`cx-dev-key-change-me` both sides) |
| `bootstrap.state` | PASS (fake provider + `gpt-4o-mini` alias detected; bootstrap not invoked) |
| `route.evidence` | PASS (from `docker-guided-main-flow-report.json`) |
| Production readiness claimed | **no** |

Sample route evidence IDs from report:

```text
baselineRouteDecisionId: rd-0HNLL7I92B7KF-00000001
subjectRouteDecisionId: rd-0HNLL7I92B7KF-00000002
```

## Validation checks (repo diff)

| Check | Result |
| --- | --- |
| No `src/` changes in service repos | **yes** |
| No workflow changes | **yes** |
| No secrets committed | **yes** |
| JSON report under `artifacts/` (local, not committed) | **yes** |

## Safety

| Check | Status |
| --- | --- |
| Fake/local provider only | **yes** |
| Real external execution | **disabled** |
| Dev bootstrap auto-invoke optional | **yes** (`-SkipBootstrap` / `-InvokeBootstrap`) |

## Follow-up

`CONEXUS-PERSIST-003` — Conexus restart/durability regression checks.
