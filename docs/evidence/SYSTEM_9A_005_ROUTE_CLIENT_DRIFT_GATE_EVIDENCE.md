# SYSTEM-9A-005 Evidence — Route/client drift gate (platform slice)

**Date:** 2026-05-22  
**Maps to:** PHASE_TIGHT `SYSTEM-9A-005`

## Summary

Platform compatibility gate extended with Allagma OpenAPI cross-repo parity and frontend `route-client-drift:check` script presence. Full Kanon/Conexus/catalog/workflow enforcement lives in `ontogony-frontend` (`npm run route-client-drift:check`).

## Implementation

| Artifact | Change |
| --- | --- |
| `RouteClientDriftConformance.cs` | `route-client-drift-script`, `route-client-drift-allagma-openapi` checks |
| `SystemCompatibilityGate.cs` | Registers new checks after `frontend-matrix` |
| `system-protocol-registry.json` | `frontend-routes-openapi` validators include `route-client-drift:check` |

## Frontend gate (authoritative)

See [`ontogony-frontend/docs/evidence/SYSTEM_9A_005_ROUTE_CLIENT_DRIFT_GATE_EVIDENCE.md`](../../ontogony-frontend/docs/evidence/SYSTEM_9A_005_ROUTE_CLIENT_DRIFT_GATE_EVIDENCE.md).

```powershell
cd C:\dev\ontogony-frontend
npm run route-client-drift:check
```

## Acceptance

- [x] Allagma backend OpenAPI snapshot matches frontend `openapi/allagma.v0.json` signatures when siblings present
- [x] Frontend exposes unified drift gate in `npm run check`
- [x] Protocol registry documents validator
- [x] Kanon parity remains enforced (subsection of unified gate)
