# CONEXUS-PERSIST-001 — Operator docs evidence (ontogony-platform)

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS** (docs-only slice)  
**Statement:** Hardening only — first Dockerized local working system documentation; **not production readiness**.

## Scope

Document Conexus Docker-local persistence, bootstrap, health/readiness, and operator verification in `ontogony-platform` only for this slice. No runtime source, workflows, migrations, or secrets.

## Delivered

```text
docker/local-working-system/README.md
docs/environments/compose-to-docker-closeout-package-v2/post-closeout-hardening/CONEXUS-PERSIST-001.md
docs/releases/FIRST_DOCKER_LOCAL_WORKING_SYSTEM_NEXT_STEPS.md
docs/evidence/CONEXUS_PERSIST_001_OPERATOR_DOCS_EVIDENCE.md
```

## Acceptance criteria mapped

| # | Criterion | Documented in |
| --- | --- | --- |
| 1 | Postgres via `ConnectionStrings__ConexusPostgres` | `docker/local-working-system/README.md` |
| 2 | Migrations on startup (Development) | README + link to `conexus-dotnet` runbook |
| 3 | Fresh-volume requires bootstrap | README (fresh vs existing table) |
| 4 | Re-seed on existing volume | README + known limitations cross-ref |
| 5 | `POST /admin/v0/dev/bootstrap` | README (bootstrap section) |
| 6 | `CONEXUS_DEV_PROJECT_API_KEY` ↔ Allagma `Conexus__ProjectApiKey` | README (settings table) |
| 7 | `/health/live` liveness | README (health table) |
| 8 | `/ready` strict readiness | README (health table) |
| 9 | `/ready` 503 before bootstrap expected | README + troubleshooting |
| 10 | Route/model evidence after bootstrap | README (verification commands) |

## Validation

```powershell
cd C:\dev\ontogony-platform
git diff --name-only main...HEAD
# Expect only docs paths under docker/ and docs/
```

| Check | Result |
| --- | --- |
| Docs-only diff | **yes** (no `src/`, no `.github/workflows/`) |
| Secrets committed | **no** |
| Real provider keys documented | **no** (fake/local only) |
| Production readiness claimed | **no** |

## Operator commands (reference)

Documented in README; not re-run for this evidence record:

```powershell
curl -s http://localhost:5082/health/live
curl -s -w "\nHTTP %{http_code}\n" http://localhost:5082/ready
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1
Get-Content .\docker\local-working-system\artifacts\docker-guided-main-flow-report.json |
  ConvertFrom-Json | Select-Object baselineRouteDecisionId, subjectRouteDecisionId
```

## Cross-repo

Paired evidence: `conexus-dotnet/docs/evidence/CONEXUS_PERSIST_001_OPERATOR_DOCS_EVIDENCE.md`

## Safety

| Check | Status |
| --- | --- |
| Fake/local provider only | **yes** |
| No real external execution | **yes** |
| Dev key path marked dev-only | **yes** |
