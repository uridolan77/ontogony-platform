# SYSTEM-9B-005 Evidence — Live-artifact evidence journey (platform slice)

**Date:** 2026-05-22  
**Maps to:** PHASE_TIGHT `SYSTEM-9B-005`

## Summary

Platform compatibility gate extended with frontend live-artifact evidence journey script/catalog presence. Full Playwright proof lives in `ontogony-frontend` (`npm run test:e2e:docker-live:evidence-journey`).

## Implementation

| Artifact | Change |
| --- | --- |
| `LiveArtifactEvidenceJourneyConformance.cs` | `live-artifact-evidence-journey-gate` check |
| `SystemCompatibilityGate.cs` | Registers check after route-client drift |
| `run-system-9b005-live-artifact-evidence-journey-docker-live-verification.ps1` | ENV-SEED-001 + API preflight + Playwright orchestration |

## Frontend gate (authoritative)

See [`ontogony-frontend/docs/evidence/SYSTEM_9B_005_LIVE_ARTIFACT_EVIDENCE_JOURNEY_EVIDENCE.md`](../../ontogony-frontend/docs/evidence/SYSTEM_9B_005_LIVE_ARTIFACT_EVIDENCE_JOURNEY_EVIDENCE.md).

```powershell
cd C:\dev\ontogony-frontend
npm run live-artifact-evidence-journey:check
```

## Docker-live orchestration

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system\scripts
.\run-system-9b005-live-artifact-evidence-journey-docker-live-verification.ps1 -Build -Seed
```

## Acceptance

- [x] Frontend exposes static gate in `npm run check`
- [x] Docker-live spec uses ENV-SEED-001 ids (no API mocks)
- [x] Run / eval / replay evidence journey navigation against live artifacts
- [x] Protocol registry documents validator (see `frontend-live-artifact-evidence-journey`)
