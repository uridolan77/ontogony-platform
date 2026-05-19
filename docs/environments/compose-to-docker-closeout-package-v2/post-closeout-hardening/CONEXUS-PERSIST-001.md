# CONEXUS-PERSIST-001 — Conexus Docker-local persistence/bootstrap operator docs

## Purpose

Harden Conexus persistence visibility and migration/operator docs after `ENV-DOCKER-CLOSEOUT-001`.

Turn Docker-closeout lessons (provider readiness, dev key alignment, bootstrap, `/ready` vs liveness, route evidence) into durable operator knowledge before further backend changes.

## Timing

Post-`ENV-DOCKER-CLOSEOUT-001`. Does not block the closed Docker-local milestone.

## Boundary

- Hardening only — **not production readiness**.
- Fake/local Conexus provider only in Docker-local defaults.
- No real provider keys, no real external execution.
- `CONEXUS_DEV_PROJECT_API_KEY` / `preferredRawKey` bootstrap path is **dev-only**; not a public production key issuance path.

## Repos

| Repo | Deliverables |
| --- | --- |
| `ontogony-platform` | `docker/local-working-system/README.md`, this spec, next-steps link, evidence |
| `conexus-dotnet` | `docs/development/DOCKER_LOCAL.md`, `docs/deployment/STARTUP_AND_READINESS.md`, evidence |

## Slice 1 (this PR) — operator docs only

No runtime code, workflows, migrations, seed logic, or provider behavior changes.

## Acceptance criteria

An operator can answer:

| # | Topic | Expected understanding |
| --- | --- | --- |
| 1 | Docker-local Conexus persistence | Postgres via `ConnectionStrings__ConexusPostgres` → `conexus_local`; no separate `Conexus__Persistence__Mode` flag in compose |
| 2 | Migration behavior | Development: EF migrations auto-apply on startup when Postgres is configured |
| 3 | Fresh-volume behavior | Empty Postgres volume → schema from migrations; **API bootstrap required** (`POST /admin/v0/dev/bootstrap`) before route/model evidence |
| 4 | Re-seed on existing volume | Durable rows (projects, keys, providers, aliases, telemetry) may already exist; re-run seed may fail key checks — reset volumes for clean re-seed |
| 5 | Dev bootstrap | `POST /admin/v0/dev/bootstrap` creates/verifies fake provider, alias, catalog, dev project; fake provider only |
| 6 | Dev key alignment | `CONEXUS_DEV_PROJECT_API_KEY` must match `CONEXUS_PROJECT_API_KEY_FOR_ALLAGMA` / Allagma `Conexus__ProjectApiKey` (`cx-dev-key-change-me` default) |
| 7 | Liveness | `/health`, `/health/live`, `/live` — compose startup/wait probes; process up, no strict provider checks |
| 8 | Readiness | `/ready` — strict provider credentials, Postgres, durable stores, alias/runtime client invariants |
| 9 | Pre-bootstrap `/ready` | **503 expected** before bootstrap — not a compose startup failure |
| 10 | Post-bootstrap proof | Route/model evidence (`routeDecisionId` on model-calls, admin route-decision fetch) stronger than liveness alone |

## Operator verification (reference)

```powershell
# Liveness (expect 200 once container is up)
curl -s -o NUL -w "%{http_code}" http://localhost:5082/health/live

# Readiness (503 before bootstrap is OK; 200 after seed/bootstrap is OK)
curl -s -o NUL -w "%{http_code}" http://localhost:5082/ready

cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
.\docker\local-working-system\scripts\run-docker-guided-main-flow.ps1
.\docker\local-working-system\scripts\validate-docker-guided-main-flow.ps1

# Inspect route/model evidence IDs
Get-Content .\docker\local-working-system\artifacts\docker-guided-main-flow-report.json | ConvertFrom-Json |
  Select-Object baselineRouteDecisionId, subjectRouteDecisionId, baselineRunId, subjectRunId
```

## Evidence

| Repo | Path |
| --- | --- |
| `ontogony-platform` | `docs/evidence/CONEXUS_PERSIST_001_OPERATOR_DOCS_EVIDENCE.md` |
| `conexus-dotnet` | `docs/evidence/CONEXUS_PERSIST_001_OPERATOR_DOCS_EVIDENCE.md` |

## Follow-up (out of slice 1)

| PR | Focus |
| --- | --- |
| `CONEXUS-PERSIST-002` | Migration/bootstrap validation automation |
| `CONEXUS-PERSIST-003` | Restart/durability regression checks |
