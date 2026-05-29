# Run the local system

> **Audience:** operator, developer  
> **Applies to:** `ontogony-platform` Docker local working system; optional script stack in product repos  
> **Source of truth:** [`../../docker/local-working-system/README.md`](../../docker/local-working-system/README.md), [`../../scripts/start-local-ontogony-system.ps1`](../../scripts/start-local-ontogony-system.ps1)  
> **Last verified:** 2026-05-25

## Recommended: Wave 7 one-command start

From **`ontogony-platform`** repo root:

```powershell
cd C:\dev\ontogony-platform
.\scripts\start-local-ontogony-system.ps1 -Build -OpenBrowser
.\scripts\validate-local-ontogony-system.ps1
```

This brings up Postgres, Kanon, Conexus, Allagma, Aisthesis, Metabole, seeds demo data, and serves the operator console. **Development credentials only** — not production.

Golden operator journey: [`../operators/OPERATOR_V1_DEMO_GUIDE.md`](../operators/OPERATOR_V1_DEMO_GUIDE.md)

## Compose tree (direct)

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
.\docker\local-working-system\scripts\wait-local-working-system.ps1
```

Copy env template if you need overrides:

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
Copy-Item .env.example .env
```

Full script catalog: [`../../docker/local-working-system/README.md`](../../docker/local-working-system/README.md)

## Backend-only script stack (no Docker)

When iterating on APIs without rebuilding the frontend image:

```powershell
# Terminal 1 — Kanon
cd C:\dev\kanon-dotnet
dotnet run --project src/Kanon.Api

# Terminal 2 — Conexus
cd C:\dev\conexus-dotnet
dotnet run --project src/Conexus.Api

# Terminal 3 — Allagma
cd C:\dev\allagma-dotnet
dotnet run --project src/Allagma.Api
```

Or use Allagma's helper:

```powershell
cd C:\dev\allagma-dotnet
.\scripts\run-local-stack.ps1
```

Default ports: Kanon **5081**, Conexus **5082**, Allagma **5083**. Health: `GET /health` on each base URL.

## Frontend after UI changes (Docker)

Code review does not prove the browser serves your latest commit:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\verify-frontend-browser-provenance.ps1 -Build
```

## Runtime config (operator URLs)

Generated operator config for Docker lives under `docker/local-working-system/generated/`. See consumed package [`docs/_incoming/_consumed/2026-05/RUNTIME-CONFIG-001/`](../_incoming/_consumed/2026-05/RUNTIME-CONFIG-001/) and frontend `scripts/runtime-config/`.

## Verify you are up

| Check | Command / URL |
| --- | --- |
| Kanon | `http://localhost:5081/health` |
| Conexus | `http://localhost:5082/health` |
| Allagma | `http://localhost:5083/health` |
| Aisthesis | `http://localhost:5084/health` |
| Metabole | `http://localhost:5085/health` |
| Console | `http://localhost:5175` (Docker) or `http://localhost:5173` (Vite) |

## Next

- End-to-end proof: [03_GOVERNED_FAKE_E2E.md](./03_GOVERNED_FAKE_E2E.md)
- Debugging: [14_DEBUGGING_PLAYBOOK.md](./14_DEBUGGING_PLAYBOOK.md)
