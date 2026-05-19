# CONEXUS-PERSIST-002 — Validation automation evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS**  
**Statement:** Post-Docker-local hardening validation automation; **not production readiness**.

## Scope

Add `validate-conexus-persistence-bootstrap.ps1` and supporting docs for repeatable Conexus Docker-local persistence/bootstrap checks. No runtime source changes in service repos; no workflows or secrets committed.

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

## Report redaction

The JSON report under `docker/local-working-system/artifacts/conexus-persist-002-report.json` is a local operator artifact. It must **not** contain raw API keys or connection-string passwords.

The script records:

- configured booleans for keys and connection strings
- `keysAligned` boolean
- `conexusPostgresConnectionStringRedacted` with `Password=***`
- bootstrap response metadata only (`apiKeyIssued` boolean, no raw `apiKey`)

Postgres admin credentials are read from `POSTGRES_USER` / `POSTGRES_PASSWORD` / `POSTGRES_DB` in `.env` / `.env.example` (not hardcoded in the script).

## Prerequisites

- Docker local working system running (`docker compose ps` shows `conexus-api`, `postgres` healthy).
- Prior guided flow or seed artifacts optional; required when using `-RequireRouteEvidence`.

## Commands run

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\validate-conexus-persistence-bootstrap.ps1
.\docker\local-working-system\scripts\validate-conexus-persistence-bootstrap.ps1 -RequireRouteEvidence

# Confirm report contains no raw secrets
Select-String -Path .\docker\local-working-system\artifacts\conexus-persist-002-report.json `
  -Pattern 'cx-dev-key-change-me|cx-conexus-admin-dev|conexus_local_pw|ontogony_admin_pw'
# Expected: no matches
```

## Results

| Check | Result |
| --- | --- |
| Script exit code (default) | **0** |
| Script exit code (`-RequireRouteEvidence`) | **0** |
| Report verdict | **PASS** |
| Raw keys/passwords in report | **none** (grep confirmed) |
| `stack.reachable` | PASS |
| `conexus.health.live` | PASS (HTTP 200) |
| `conexus.ready.captured` | INFO (HTTP 503 — expected strict readiness) |
| `postgres.conexus_local` | PASS |
| `postgres.migrations` | PASS |
| `compose.render` | PASS |
| `keys.aligned` | PASS |
| `bootstrap.state` | PASS |
| `route.evidence` | PASS (from `docker-guided-main-flow-report.json`) |
| Production readiness claimed | **no** |

Sample redacted configuration from report:

```json
"configuration": {
  "conexusPostgresConnectionStringConfigured": true,
  "conexusPostgresConnectionStringRedacted": "Host=postgres;Port=5432;Database=conexus_local;Username=conexus_local;Password=***",
  "conexusDevProjectApiKeyConfigured": true,
  "conexusProjectApiKeyForAllagmaConfigured": true,
  "keysAligned": true
}
```

## Seed report route-evidence fallback

The validator reads route IDs from:

1. `docker-guided-main-flow-report.json` (`baselineRouteDecisionId` / `subjectRouteDecisionId`)
2. `env-seed-001-report.json` — `routeEvidence.*` (current seed schema) or legacy `runs.baseline` / `runs.subject`

## Validation checks (repo diff)

| Check | Result |
| --- | --- |
| No service `src/` changes | **yes** |
| No workflow changes | **yes** |
| No secrets committed | **yes** |
| JSON report local only (not committed) | **yes** |

## Safety

| Check | Status |
| --- | --- |
| Fake/local provider only | **yes** |
| Real external execution | **disabled** |
| Report redacts secrets | **yes** |

## Follow-up

`CONEXUS-PERSIST-003` — Conexus restart/durability regression checks.
