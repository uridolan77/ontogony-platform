# GOVERNED-MAF-E2E-001 — governed MAF workflow smoke (PASS)

**Status:** CLOSED  
**Recorded:** 2026-05-25T13:22:25Z  
**Package:** ALLAGMA-MAF-INTEGRATION-DEPTH-001E / MAF-PROOF-LOCK-001

---

## Environment

| Service | URL |
| --- | --- |
| Kanon | `http://localhost:5081` |
| Conexus | `http://localhost:5082` |
| Allagma | `http://localhost:5083` |

Prerequisites: `ENV-SEED-001` (or equivalent) so `gaming-core@0.1.0` topology and Conexus fake route exist. Docker compose should set `Conexus__AdminApiKey` on `allagma-api` for cross-service replay dry-runs.

---

## Commands

```powershell
cd c:\dev\allagma-dotnet
.\scripts\smoke\run-governed-maf-e2e.ps1 -Strict
.\scripts\check-governed-maf-e2e-summary.ps1

powershell -NoProfile -File c:\dev\ontogony-platform\scripts\smoke\run-runtime-lock-governed-maf-e2e.ps1 -Strict -ValidateRuntimeLock
```

---

## Identifiers (baseline run)

| Field | Value |
| --- | --- |
| runId | `run_ad05288767d34b5f802b3078304a538a` |
| replayId | `replay_8c5bca09121b44a090455f3822efd8aa` |
| traceId | `cohesion-approve-6205a83f50604a21b27fc17d9d5ff527` |
| humanGateId | `gate_2a25f309340b4b8c8baf1186d560c586` |
| planningDecisionId | `decision_15023fe641c040daa54187e265231137` |
| modelCallId | `chatcmpl-0HNLQBVR0A9I7-00000001` |
| workflowVersion | `governed-spine-v1` |
| strictCheckpointRunId | `run_c67d9ccd94164c4fa0aff7c4a4907853` |

---

## Artifacts

Committed baseline (platform evidence spine):

`ontogony-platform/docs/evidence/artifacts/governed-maf-e2e/20260525T132225Z/`

| File | Purpose |
| --- | --- |
| `governed-maf-e2e-summary.json` | PASS summary (`ontogony-governed-maf-e2e-summary-v1`) |
| `governed-maf-e2e-result.json` | Raw smoke result |
| `evidence-graph.json` / `evidence-spine-bundle.json` | Cross-service evidence graph with workflow nodes |
| `governed-maf-e2e-output.log` | Transcript |

Runtime lock key: `evidence.governedMafE2eSummary` in `allagma-dotnet/docs/system/ontogony-runtime.lock.json`.

---

## Assertions covered

- Consequential probe + human gate resume → `Completed`
- `GET /allagma/v0/runs/{runId}/workflow` — governed-spine projection (10 nodes, 3+ checkpoints)
- Tool intent + Kanon evaluation before Conexus model
- Evidence graph PASS with `allagma.workflow`, `allagma.workflow_step`, `allagma.workflow_checkpoint`
- Cross-service replay binding (`evidence_only`) with Allagma + Kanon + Conexus attempts
- Strict: unregistered tool probe + `POST …/workflow/resume` `replay_from_checkpoint` with idempotency key
