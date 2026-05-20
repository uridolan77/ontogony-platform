# ALLAGMA-ACTION-004 — Run audit and evidence actions (platform index)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (consumer implementation in ontogony-frontend)  
**Slice:** ALLAGMA-ACTION-004 from Ontogony-Allagma-Actionability-Workbench-Package-v1

## Consumer evidence

See [ontogony-frontend/docs/evidence/ALLAGMA_ACTION_004_RUN_AUDIT_AND_EVIDENCE_ACTIONS_EVIDENCE.md](../../../ontogony-frontend/docs/evidence/ALLAGMA_ACTION_004_RUN_AUDIT_AND_EVIDENCE_ACTIONS_EVIDENCE.md).

## Backend routes (unchanged)

- `GET /allagma/v0/runs/{runId}/audit`
- `GET /allagma/v0/evaluations/{evaluationRunId}/evidence`
- `GET /allagma/v0/runs/{runId}/evaluations`

## Acceptance

- Run audit and eval evidence export are first-class operator actions in the frontend workbench
