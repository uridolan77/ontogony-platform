# ENV-MIGRATION-AUTO-001 — Allagma fresh-volume migration automation evidence

- Date: `2026-05-19`
- Scope: Docker-local operator reliability only
- Boundary: not production readiness, no product semantics changes

## Problem

Fresh Docker-local volume resets left `allagma_local` without EF schema. `seed-and-verify-local-working-system.ps1` failed on first `POST /allagma/v0/runs` with `relation "allagma_runs" does not exist` until operators manually ran `allagma-dotnet/scripts/apply-allagma-postgres-migration.ps1`.

Kanon and Conexus already apply schema on Development startup; Allagma did not.

## Fix

- Added `Allagma.Api/PersistenceHostExtensions.ApplyAllagmaPersistenceMigrationsIfConfiguredAsync`.
- `Allagma.Api` calls it when `ASPNETCORE_ENVIRONMENT=Development` and persistence mode is `Postgres`.
- Added optional `Allagma:Persistence:Postgres:ApplyMigrationsOnStartup` for non-Development explicit opt-in.
- Documented in `allagma-dotnet/docs/migrations/ALLAGMA_POSTGRES.md` and `docker/local-working-system/README.md`.

## Verification

Fresh one-command path (no manual `apply-allagma-postgres-migration.ps1`):

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\reset-local-working-system.ps1 -Force
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
.\docker\local-working-system\scripts\wait-local-working-system.ps1
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
```

## Result

**PASS** — `2026-05-19T18:08:07Z` verification on Windows Docker-local host.

| Step | Verdict |
| --- | --- |
| `reset-local-working-system.ps1 -Force` | PASS |
| `start-local-working-system.ps1 -Build` | PASS |
| `wait-local-working-system.ps1` | PASS |
| `seed-and-verify-local-working-system.ps1` (no manual migration script) | PASS on first attempt |

Allagma container logs confirmed EF migrations applied at startup:

```text
Applying migration '20260515213403_InitialAllagmaPersistence'
...
Applying migration '20260518164637_EvalDur001EvaluationPersistence'
```

Seed then completed baseline/subject runs, route evidence, evaluation APIs, and baseline comparison without `relation "allagma_runs" does not exist` errors.

## Acceptance

- Fresh volume no longer requires manual `apply-allagma-postgres-migration.ps1` before seed: **PASS**
- Production default remains non-auto-migrate (Development-only unless explicit opt-in flag): **PASS**
- Boundary preserved: Docker-local operator reliability only: **PASS**
