# Governed fake E2E

> **Audience:** operator, backend developer  
> **Applies to:** `allagma-dotnet`, `ontogony-platform` smoke wrappers  
> **Source of truth:** `allagma-dotnet/docs/evidence/GOVERNED_FAKE_E2E_001_PASS_*.md`, `allagma-dotnet/docs/TESTING.md`  
> **Last verified:** 2026-05-25

## What this proves

**GOVERNED-FAKE-E2E-001** exercises a full governed path: Kanon planning decision → Conexus fake provider model call → Allagma run completion → cross-service identifier linkage suitable for Evidence Spine and runtime-lock evidence.

This is a **local development proof**, not production readiness.

## Prerequisites

- Kanon, Conexus, Allagma healthy on **5081–5083** (script stack) or Docker compose (see [02_RUN_LOCAL_SYSTEM.md](./02_RUN_LOCAL_SYSTEM.md)).
- Conexus dev bootstrap (fake provider + alias) — smoke scripts call this automatically.

## Run (Allagma repo)

```powershell
cd C:\dev\allagma-dotnet
.\scripts\smoke\run-governed-fake-e2e.ps1
```

Artifacts: `artifacts/governed-fake-e2e/<timestamp>/` including `governed-fake-e2e-summary.json` (`ontogony-governed-fake-e2e-summary-v1`).

Checker:

```powershell
.\scripts\check-governed-fake-e2e-summary.ps1
```

## Cross-service replay proof (optional)

```powershell
.\scripts\smoke\run-governed-fake-replay-e2e.ps1
.\scripts\check-governed-fake-replay-summary.ps1
```

Requires a completed governed-fake source run. Summary includes `proof.replayRunEventIdempotencyVerified` (REPLAY-RUNTIME-PROOF-LOCK-001). See `allagma-dotnet/docs/evidence/AGM_REPLAY_RUNTIME_PROOF_LOCK_001.md`.

## Platform wrapper (runtime lock)

```powershell
cd C:\dev\ontogony-platform
.\scripts\smoke\run-runtime-lock-governed-fake-e2e.ps1 -IncludeReplay
```

Replay-only:

```powershell
.\scripts\smoke\run-runtime-lock-governed-fake-replay-e2e.ps1
```

Validate lock pointer (from `allagma-dotnet`):

```powershell
cd C:\dev\allagma-dotnet
.\scripts\validate-runtime-lock.ps1 -RequireGovernedFakeE2eEvidence
.\scripts\validate-runtime-lock.ps1 -RequireGovernedFakeReplayEvidence
```

## ToolIntent lifecycle (first-system smoke)

```powershell
cd C:\dev\allagma-dotnet
.\scripts\smoke-first-system.ps1
.\scripts\smoke-first-system-tool-allowed.ps1
```

Traffic smoke accepts **Allowed or Blocked** after Kanon evaluation; strict allow smoke requires `policy-record-plan-snapshot` in `gaming-core@0.1.0`. See `allagma-dotnet/docs/evidence/AGM_TOOL_INTENT_LIFECYCLE_STRICT_001.md`.

## Governed MAF workflow smoke (related)

```powershell
cd C:\dev\allagma-dotnet
.\scripts\smoke\run-governed-maf-e2e.ps1
.\scripts\smoke\run-governed-maf-e2e.ps1 -Strict
```

Evidence: `allagma-dotnet/docs/evidence/AGM_MAF_PROOF_LOCK_001.md`

## What to inspect after PASS

| Artifact | Meaning |
| --- | --- |
| `runId` | Allagma governed run |
| `traceId` / `correlationId` | Cross-service correlation |
| `planningDecisionId` | Kanon decision |
| `modelCallId` | Conexus call id |
| Summary `verdict` | Must be `PASS` for lock promotion |

Open console: `/allagma/runs/{runId}` or Evidence Spine with pasted `runId`.

## Playwright (frontend, Docker)

From `ontogony-frontend` (stack on **5175**):

```powershell
npm run test:e2e:docker-live:system-truth
```

See `ontogony-frontend/docs/ux/CONSOLE_PAGE_MIGRATION_PLAN.md` for docker-live prep scripts.

## Next

- Runtime lock semantics: [04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md](./04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md)
- Evidence Spine UI: [05_EVIDENCE_SPINE.md](./05_EVIDENCE_SPINE.md)
