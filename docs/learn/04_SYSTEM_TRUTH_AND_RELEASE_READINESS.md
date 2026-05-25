# System truth and release readiness

> **Audience:** operator, frontend developer  
> **Applies to:** `ontogony-frontend`, `allagma-dotnet/docs/system/ontogony-runtime.lock.json`  
> **Source of truth:** `ontogony-frontend/e2e/system-truth.spec.ts`, `allagma-dotnet/docs/system/ONTOGONY_RUNTIME_LOCK.md`  
> **Last verified:** 2026-05-25

## What “system truth” means in the console

**System Truth** (Release Readiness and related pages) surfaces **live connectivity and contract posture** for the operator shell: which services respond, which generated catalogs are present, and whether local proofs (cohesion, governed fake, MAF) align with committed evidence pointers.

It is a **readiness dashboard**, not a deployment gate for production.

## Runtime lock (reproducibility spine)

Machine-readable quad-repo pin:

- File: `allagma-dotnet/docs/system/ontogony-runtime.lock.json`
- Human guide: `allagma-dotnet/docs/system/ONTOGONY_RUNTIME_LOCK.md`

Key fields:

| Field | Role |
| --- | --- |
| `lockedCommits` | Exact SHAs for sibling repos |
| `evidence.*` | Paths to PASS summaries (cohesion, governed fake, MAF, replay) |
| `packageVersions` | NuGet package-mode pins |

Validate locally:

```powershell
cd C:\dev\allagma-dotnet
.\scripts\validate-runtime-lock.ps1
.\scripts\validate-runtime-lock.ps1 -RequireGovernedFakeE2eEvidence
.\scripts\validate-runtime-lock.ps1 -RequireGovernedMafE2eEvidence
.\scripts\validate-runtime-lock.ps1 -RequireGovernedFakeReplayEvidence
```

## Frontend tests

Mocked (fast):

```powershell
cd C:\dev\ontogony-frontend
npx playwright test e2e/system-truth.spec.ts e2e/settings.spec.ts
```

Docker-live (requires compose frontend on **5175**):

```powershell
npm run docker:prepare:system-truth:frontend
npm run test:e2e:docker-live:system-truth
```

## Contract discipline gate

Before merging console or route catalog changes:

```powershell
cd C:\dev\ontogony-frontend
npm run contracts:discipline
npm run typecheck
```

See [08_CONTRACT_DISCIPLINE.md](./08_CONTRACT_DISCIPLINE.md).

## System cohesion smoke

Cross-scenario backend evidence (not the same as governed-fake, broader):

```powershell
cd C:\dev\allagma-dotnet
.\scripts\run-system-cohesion-smoke.ps1 -UseExistingServices
```

## Matrices (maintainer)

| Matrix | Path |
| --- | --- |
| System test matrix | `allagma-dotnet/docs/system/SYSTEM_TEST_MATRIX.md` |
| Feature connection | `allagma-dotnet/docs/system/ALLAGMA_FEATURE_CONNECTION_MATRIX.md` |
| Compatibility | `allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md` |

## Next

- Evidence Spine: [05_EVIDENCE_SPINE.md](./05_EVIDENCE_SPINE.md)
- Debugging: [14_DEBUGGING_PLAYBOOK.md](./14_DEBUGGING_PLAYBOOK.md)
