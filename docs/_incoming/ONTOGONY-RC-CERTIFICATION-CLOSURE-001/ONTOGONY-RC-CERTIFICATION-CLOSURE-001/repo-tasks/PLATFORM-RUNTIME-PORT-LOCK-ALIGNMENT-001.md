# PLATFORM-RUNTIME-PORT-LOCK-ALIGNMENT-001

## Owner repos

- `ontogony-platform`
- `allagma-dotnet`
- `metabole-dotnet`
- `aisthesis-dotnet`
- docker/local-working-system source

## Problem

The live certification path is ambiguous about Metabole/Aisthesis ports. The canonical runtime lock must be the single source of truth:

```text
Kanon     5081
Conexus   5082
Allagma   5083
Metabole  5084
Aisthesis 5085
```

If docker compose binds Aisthesis to 5084 and Metabole to 5085, Aisthesis live-cert triggers may call the wrong service and fail with 502/500/no trace.

## Required implementation

1. Create or update a machine-readable runtime-port contract.
2. Ensure `allagma-dotnet/docs/system/ontogony-runtime.lock.json` matches the same map.
3. Ensure all docker compose files bind the same ports.
4. Ensure Aisthesis producer BaseUrl defaults are generated or validated from the lock.
5. Add a preflight script: `scripts/system/verify-ontogony-runtime-service-identity.ps1`.

## Acceptance

```powershell
pwsh .\scripts\systemerify-ontogony-runtime-service-identity.ps1 -RequireAll -WriteEvidence
```

Evidence: `artifacts/runtime-port-lock/<timestamp>/service-identity-summary.json`.
