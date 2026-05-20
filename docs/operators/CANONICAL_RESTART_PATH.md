# Canonical restart path (Docker-local)

**Issue:** SYS-Q006-004-RESTART-PATH-DECISION  
**Baseline:** SYSTEM-ALPHA-006  
**Non-claims:** Not production readiness.

## Canonical path (use this)

For **Docker-local** governed runtime (`docker/local-working-system`), restart survival is proven by **restarting the Allagma API container** while Kanon and Conexus keep running:

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
docker compose restart allagma-api
.\scripts\wait-local-working-system.ps1
```

Evidence template: `allagma-dotnet/artifacts/restart-e2e/<timestamp>/summary.json` with `restartMode: docker-compose-restart-allagma-api`.

Related regression scripts:

- `scripts/run-conexus-persistence-durability-regression.ps1` — Conexus container restart (CONEXUS-PERSIST-003)
- System cohesion with restart proof skipped on CI runners; use local `-ReleaseMode` when restart artifacts exist

## Legacy path (process stack — not canonical with compose up)

`allagma-dotnet/scripts/restart-e2e-first-real-system.ps1` starts **host processes** on **5081 / 5082 / 5083**. It **conflicts** with Docker compose or local `dotnet run` on the same ports.

| Situation | Action |
| --- | --- |
| Compose stack running on 5081–5083 | Use **canonical Docker restart** above; do not run the process script |
| Need process-stack restart E2E | Stop compose and host APIs on 5081–5083, then run with `-ForceProcessStack` |

## Operator rule (port hygiene)

Before health checks or E2E, ensure host ports are not double-bound:

```powershell
netstat -ano | findstr ":5081 :5082 :5083"
```

Stop stray `Kanon.Api` / `Conexus.Api` / `Allagma.Api` processes or override `*_HOST_PORT` in `docker/local-working-system/.env`.

See also: [docker/local-working-system/README.md](../../docker/local-working-system/README.md) (operator rule § port hygiene).
