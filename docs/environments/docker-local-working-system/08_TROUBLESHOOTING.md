# Troubleshooting — Docker local working system

## API cannot connect to Postgres

Inside containers use the compose service name:

```text
Host=postgres;Port=5432
```

**Not** `localhost` — that refers to the container itself, not the postgres service.

## Allagma cannot call Kanon or Conexus

Inside the Allagma container:

```text
http://kanon-api:8080
http://conexus-api:8080
```

From the host browser or frontend on the host:

```text
http://localhost:5081
http://localhost:5082
http://localhost:5083
```

Mixing these causes connection refused or DNS failures.

## Manual eval write fails

Confirm on `allagma-api`:

```text
ASPNETCORE_ENVIRONMENT=Development
Allagma__Evaluation__ManualWriteEnabled=true
Allagma__Persistence__Mode=Postgres
```

## Route evidence missing

- Confirm Conexus has `ConnectionStrings__ConexusPostgres` set (persistence enabled).
- Confirm fake/local provider bootstrap ran (ENV-SEED-001): `docker/local-working-system/scripts/seed-and-verify-local-working-system.ps1`.
- Check Conexus logs for migration or project-key errors.

## Conexus health vs readiness confusion

- `wait-local-working-system.ps1` uses **Conexus liveness** (`/health/live`) for startup gating.
- Conexus `/ready` is intentionally stricter and includes provider-credential consistency checks.
- Before dev bootstrap (`POST /admin/v0/dev/bootstrap`), `/ready` can remain non-green even when liveness is healthy.
- Do not add real provider keys to fix local startup probes; use fake/local bootstrap flow.

## Kanon migrations fail

- Verify `Kanon__Persistence__Postgres__ConnectionString` (not `ConnectionStrings__Kanon`).
- Verify `kanon_local` database and user exist (ENV-DB-001 init SQL).

## Port already allocated

| Symptom | Mitigation |
| --- | --- |
| Host `5432` in use | Map `55433:5432` for postgres in compose |
| Host `5081`–`5083` in use | Stop script-based stack (`allagma-dotnet/scripts/env/stop-local-operator-sanity.ps1`) before `docker compose up` |

## Frontend cannot reach APIs

`VITE_*_BASE_URL` must use **host** ports (`localhost:5081`–`5083`), not container DNS names — the browser runs outside Docker.

## Build context not found

Compose build contexts assume sibling repos:

```text
C:\dev\kanon-dotnet
C:\dev\conexus-dotnet
C:\dev\allagma-dotnet
C:\dev\ontogony-frontend
```

Run compose from `ontogony-platform/docker/local-working-system/` with repos checked out at `C:\dev\`.

## Related

- Script-based troubleshooting: `../local-operator-sanity/06_TROUBLESHOOTING.md`
- Settings reference: `03_EXACT_SETTINGS.md`
