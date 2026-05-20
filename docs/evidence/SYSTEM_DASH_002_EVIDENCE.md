# SYSTEM-DASH-002 — Centralize operational dashboard ownership evidence

**Date:** 2026-05-20  
**Repo:** `ontogony-platform` (canonical entrypoint); assets remain in `allagma-dotnet`  
**Verdict:** PASS

## Acceptance criteria

1. `ontogony-platform/docs/operations/` links to the canonical dashboard/SLO pack.  
2. Dashboard provisioning path is documented from the platform local stack.  
3. Allagma-hosted observability assets are still referenced but not hidden.

## Deliverables

| Artifact | Path |
| --- | --- |
| Canonical index | `docs/operations/SYSTEM_DASHBOARD_SLO_INDEX.md` |
| Operations hub link | `docs/operations/index.md` (runtime dashboard section) |
| Platform local stack: start observability | `docker/local-working-system/scripts/start-observability-stack.ps1` |
| Platform local stack: verify provisioning | `docker/local-working-system/scripts/verify-observability-stack.ps1` |
| Compose README section | `docker/local-working-system/README.md` § Observability stack |
| Allagma back-links | `allagma-dotnet/docs/operations/SYSTEM_*` (canonical entry pointer) |

## Verification

### Static (no Docker)

```powershell
cd C:\dev\ontogony-platform
Test-Path .\docs\operations\SYSTEM_DASHBOARD_SLO_INDEX.md
Select-String -Path .\docs\operations\SYSTEM_DASHBOARD_SLO_INDEX.md -Pattern 'SYSTEM_SLO_STARTER_PACK|SYSTEM_DASH_PANEL_PACK|docker-compose.observability|verify-system-observability'
Select-String -Path .\docker\local-working-system\README.md -Pattern 'SYSTEM_DASHBOARD_SLO_INDEX|start-observability-stack'
```

### Local (operator)

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\start-observability-stack.ps1
.\docker\local-working-system\scripts\verify-observability-stack.ps1
# Grafana → Ontogony → Ontogony Alpha Runtime
```

## Notes

- Builds on **SYSTEM-DASH-001** (`allagma-dotnet`); does not move Grafana JSON or compose files.
- **SYSTEM-OBS-METERS-001** remains the follow-up for `/ready` scrape, Kanon plan-compile meters, and Conexus cost OTEL counters.
