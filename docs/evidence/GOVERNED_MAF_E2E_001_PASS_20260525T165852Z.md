# GOVERNED-MAF-E2E-001 — governed MAF workflow smoke + Playwright (PASS)

**Status:** CLOSED (`MAF-RUNTIME-LOCK-PROMOTION-001`)  
**Recorded:** 2026-05-25T16:58:52Z  
**Packages:** ALLAGMA-MAF-INTEGRATION-DEPTH-001E, MAF-LIVE-GOVERNED-E2E-001, MAF-RUNTIME-LOCK-PROMOTION-001

---

## Environment

| Service | URL |
| --- | --- |
| Kanon | `http://localhost:5081` |
| Conexus | `http://localhost:5082` |
| Allagma | `http://localhost:5083` |
| Frontend (Docker) | `http://localhost:5175` |

---

## Command (canonical)

```powershell
powershell -NoProfile -File c:\dev\ontogony-platform\scripts\smoke\run-runtime-lock-governed-maf-e2e.ps1 -Strict -ValidateRuntimeLock
```

---

## Identifiers (baseline run)

| Field | Value |
| --- | --- |
| runId | `run_a75e4ab576df45e68341f8a51f20c9af` |
| replayId | `replay_ffa44cfe6c99477fb307d20decf7cd04` |
| traceId | `cohesion-approve-be753b6fda4a4e5ebdbd339f37e5b586` |
| humanGateId | `gate_6ed08ee173ca4f2c80595eac9e5b756e` |
| planningDecisionId | `decision_24f933f9233e4c919854c6ee492b1fe2` |
| modelCallId | `chatcmpl-0HNLQBVR0AC2N-00000004` |
| workflowVersion | `governed-spine-v1` |
| strictCheckpointRunId | `run_464a6d2cee7d4ff0be35465d73bffc91` |
| playwright | `PASS` (4 tests, `governed-maf-e2e-docker-live`) |

---

## Artifacts

`ontogony-platform/docs/evidence/artifacts/governed-maf-e2e/20260525T165852Z/`

Runtime lock: `evidence.governedMafE2eSummary` → `governed-maf-e2e-summary.json` in that folder.

Prior baselines: `20260525T132225Z/` (smoke-only), `20260525T165733Z/` (intermediate local run).
