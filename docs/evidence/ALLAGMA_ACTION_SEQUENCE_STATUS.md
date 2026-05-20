# Allagma actionability sequence status

**Package:** Ontogony-Allagma-Actionability-Workbench-Package-v1  
**Last updated:** 2026-05-20 (ACTION-007 closeout)

| ID | Theme | Status | Platform evidence |
| --- | --- | --- | --- |
| ACTION-000 | Current state audit | Done | [000](./ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT_EVIDENCE.md) |
| ACTION-001 | Start run workbench | Done | [001](./ALLAGMA_ACTION_001_START_RUN_WORKBENCH_EVIDENCE.md) |
| ACTION-001A | Start run cleanup | Done | [001A](./ALLAGMA_ACTION_001A_START_RUN_CLEANUP_EVIDENCE.md) |
| ACTION-002 | Human-gate resume | Done | [002](./ALLAGMA_ACTION_002_HUMAN_GATE_RESUME_WORKBENCH_EVIDENCE.md) |
| ACTION-003 | Baseline compare | Done | [003](./ALLAGMA_ACTION_003_BASELINE_COMPARE_ACTIONS_EVIDENCE.md) |
| ACTION-004 | Run audit + eval evidence | Done | [004](./ALLAGMA_ACTION_004_RUN_AUDIT_AND_EVIDENCE_ACTIONS_EVIDENCE.md) |
| ACTION-005 | Retry/cancel/replay design | Done | [005](./ALLAGMA_ACTION_005_RUN_OPERATIONS_CONTRACT_DESIGN_EVIDENCE.md) |
| ACTION-006 | Run operations v2 | Done (code) | [006](./ALLAGMA_ACTION_006_RUN_OPERATIONS_V2_EVIDENCE.md) |
| ACTION-007 | Closeout + manual QA | Done (docs) | [007](./ALLAGMA_ACTION_007_CLOSEOUT_EVIDENCE.md) |

## Verification matrix

| Item | Automated tests | Docker API | Browser |
| --- | --- | --- | --- |
| 001–004 | Frontend unit + mocked E2E (per-item evidence) | Start/audit/baseline verified 007 | Pending checklist |
| 005 | Design only | — | — |
| 006 | Backend unit + frontend unit + mocked E2E | **PASS** | — |
| 006A | Idempotency + result UX | **PASS** | — |
| 007 | Closeout docs | **PASS** | — |
| 007A | Browser @ :5175 | **PASS** | [browser evidence](../../../ontogony-frontend/docs/evidence/ALLAGMA_ACTION_007_BROWSER_MANUAL_QA_EVIDENCE.md) |
