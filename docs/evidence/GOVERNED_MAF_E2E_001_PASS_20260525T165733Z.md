# GOVERNED-MAF-E2E-001 — governed MAF workflow smoke + Playwright (PASS)

**Status:** CLOSED (`MAF-RUNTIME-LOCK-PROMOTION-001`)  
**Recorded:** 2026-05-25T16:57:33Z  
**Packages:** ALLAGMA-MAF-INTEGRATION-DEPTH-001E, MAF-LIVE-GOVERNED-E2E-001, MAF-RUNTIME-LOCK-PROMOTION-001

---

## Environment

| Service | URL |
| --- | --- |
| Kanon | `http://localhost:5081` |
| Conexus | `http://localhost:5082` |
| Allagma | `http://localhost:5083` |
| Frontend (Docker) | `http://localhost:5175` |

Prerequisites: Docker local working system with `gaming-core@0.1.0`, Conexus fake route, rebuilt `ontogony-frontend` image (workflow workbench + Evidence Spine workflow append).

---

## Commands

```powershell
cd c:\dev\allagma-dotnet
.\scripts\smoke\run-governed-maf-e2e.ps1 -Strict -NoTimestampSubdirectory `
  -OutputDirectory "c:\dev\ontogony-platform\docs\evidence\artifacts\governed-maf-e2e\20260525T165733Z"

cd c:\dev\ontogony-frontend
npm run governed-maf-e2e:docker-live

cd c:\dev\allagma-dotnet
.\scripts\validate-runtime-lock.ps1 -RequireGovernedMafE2eEvidence
```

Platform wrapper (after path fix):

```powershell
powershell -NoProfile -File c:\dev\ontogony-platform\scripts\smoke\run-runtime-lock-governed-maf-e2e.ps1 -Strict -ValidateRuntimeLock
```

---

## Identifiers (baseline run)

| Field | Value |
| --- | --- |
| runId | `run_ed394f8cda854313b1fc7dc4cb3fdb2b` |
| replayId | `replay_ab508af36aac4918b185f0a260d850e1` |
| traceId | `cohesion-approve-0f27ef20093f4c94bab6c2e7a7d72a0b` |
| humanGateId | `gate_31381ee6ed51470dbfb55ced2ee46729` |
| planningDecisionId | `decision_1ea2fea5c3a744b399f43e2e95b9abb1` |
| modelCallId | `chatcmpl-0HNLQBVR0AC2N-00000002` |
| workflowVersion | `governed-spine-v1` |
| strictCheckpointRunId | `run_100e354ffd9c4e018dfea93505a5ed9e` |
| playwright | `PASS` (`governed-maf-e2e-docker-live`, 4 tests) |

---

## Artifacts

Committed baseline:

`ontogony-platform/docs/evidence/artifacts/governed-maf-e2e/20260525T165733Z/`

| File | Purpose |
| --- | --- |
| `governed-maf-e2e-summary.json` | PASS summary (`ontogony-governed-maf-e2e-summary-v1`, `playwright.status=PASS`) |
| `governed-maf-e2e-result.json` | Raw smoke result |
| `evidence-graph.json` / `evidence-spine-bundle.json` | Cross-service evidence graph with workflow nodes |
| `governed-maf-e2e-output.log` | Smoke transcript |

Runtime lock: `evidence.governedMafE2eSummary` in `allagma-dotnet/docs/system/ontogony-runtime.lock.json`.

Prior baseline (superseded): `20260525T132225Z/` (smoke-only, no Playwright).

---

## Assertions covered

- Backend: consequential probe + human gate resume, workflow projection, replay binding, strict checkpoint replay
- Playwright: Run Detail workbench, Evidence Spine workflow nodes (`allagma.workflow*`), Agent Interaction `governed_workflow` timeline
