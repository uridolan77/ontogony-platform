# ALLAGMA-ACTION-003 — Baseline compare actions (platform index)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (consumer implementation in ontogony-frontend)  
**Slice:** ALLAGMA-ACTION-003 from Ontogony-Allagma-Actionability-Workbench-Package-v1

## Scope

Platform repo carries the cross-service evidence index only. Implementation lives in **ontogony-frontend**; backend contract unchanged (`POST /allagma/v0/evaluations/baseline-comparisons` already shipped).

## Consumer evidence

See [ontogony-frontend/docs/evidence/ALLAGMA_ACTION_003_BASELINE_COMPARE_ACTIONS_EVIDENCE.md](../../../ontogony-frontend/docs/evidence/ALLAGMA_ACTION_003_BASELINE_COMPARE_ACTIONS_EVIDENCE.md).

## Backend routes (unchanged)

- `POST /allagma/v0/evaluations/baseline-comparisons`
- `GET /allagma/v0/evaluations/baseline-comparisons`
- `GET /allagma/v0/evaluations/baseline-comparisons/{comparisonId}`

## Acceptance

- Operator workbench can create comparisons (not read-only)
- Detail navigation and list refresh verified in frontend evidence
