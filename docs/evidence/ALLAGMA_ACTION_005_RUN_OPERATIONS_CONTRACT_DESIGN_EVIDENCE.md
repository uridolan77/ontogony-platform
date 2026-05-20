# ALLAGMA-ACTION-005 — Run operations contract design (platform index)

> **Historical design evidence.** Tables that mark retry/cancel/replay/operations routes as *absent* describe the pre–ACTION-006 audit. Implementation landed in **ACTION-006** per `allagma-dotnet/docs/architecture/RUN_OPERATIONS_CONTRACT_DESIGN.md` (status: Implemented).

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS** (design + frontend capability truth; no backend mutations at time of slice)  
**Slice:** ALLAGMA-ACTION-005 from Ontogony-Allagma-Actionability-Workbench-Package-v1

## Design artifact

[allagma-dotnet/docs/architecture/RUN_OPERATIONS_CONTRACT_DESIGN.md](../../../allagma-dotnet/docs/architecture/RUN_OPERATIONS_CONTRACT_DESIGN.md)

## Audit confirmation

| Route | Status (2026-05-20) |
| --- | --- |
| `POST /allagma/v0/runs/{runId}/retry` | Absent |
| `POST /allagma/v0/runs/{runId}/cancel` | Absent |
| `POST /allagma/v0/runs/{runId}/replay` | Absent |
| `GET /allagma/v0/runs/{runId}/operations` | Absent |
| Deny human gate on Allagma | Absent (Kanon + resume) |

Reference audit: [ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md](../reviews/ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md)

## Frontend capability display

- OpenAPI snapshot gating for retry/cancel/replay (no enabled buttons)
- `mapRunOperationsContract` + `useAllagmaRunOperations` prepared for ACTION-006
- Detail: `ontogony-frontend/src/allagma/lifecycle/` and `src/allagma/api/useAllagmaRunOperations.ts`

## Acceptance

- Retry/cancel/replay semantics defined in architecture doc
- UI does not pretend operations exist (unsupported list + limitation messages)
- ACTION-006 implementation checklist documented

## Next slice

**ALLAGMA-ACTION-006** — Implement accepted operation routes per design doc.
