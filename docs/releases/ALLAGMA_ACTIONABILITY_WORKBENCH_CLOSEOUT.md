# Allagma actionability workbench — closeout

**Program:** Ontogony-Allagma-Actionability-Workbench-Package-v1 (ACTION-000 through ACTION-007)  
**Date:** 2026-05-20  
**Posture:** Docker-local operator workbench — **not production readiness**  
**Overall verdict:** **PARTIAL PASS**

## Summary

The actionability package wired existing Allagma v0 contracts into the operator console (start run, resume human gate, baseline compare, audit/evidence export), then added **run operations v2** (retry, cancel, replay manifest, per-run operations contract) per `allagma-dotnet/docs/architecture/RUN_OPERATIONS_CONTRACT_DESIGN.md`.

Implementation spans `ontogony-frontend`, `allagma-dotnet`, and platform evidence. **Browser end-to-end verification** and a **fresh Docker `allagma-api` image** remain operator follow-ups (see [007 evidence](../evidence/ALLAGMA_ACTION_007_CLOSEOUT_EVIDENCE.md)).

## Sequence outcomes

| ID | Outcome |
| --- | --- |
| 000 | Route inventory, capability matrix, unsupported-operation list |
| 001 | Start-run workbench + idempotency UX |
| 002 | Human-gate resume workbench (Kanon resolve + Allagma resume) |
| 003 | Baseline comparison create from operator UI |
| 004 | Run audit + eval evidence export actions on run/eval detail |
| 005 | Retry/cancel/replay contract design (accepted) |
| 006 | Backend routes + `AllagmaRunOperationsPanel` + capability merge |
| 007 | Closeout docs, API partial QA, browser checklist |

## Evidence index

- Sequence: [`docs/evidence/ALLAGMA_ACTION_SEQUENCE_STATUS.md`](../evidence/ALLAGMA_ACTION_SEQUENCE_STATUS.md)
- Closeout QA: [`docs/evidence/ALLAGMA_ACTION_007_CLOSEOUT_EVIDENCE.md`](../evidence/ALLAGMA_ACTION_007_CLOSEOUT_EVIDENCE.md)
- Design: `allagma-dotnet/docs/architecture/RUN_OPERATIONS_CONTRACT_DESIGN.md`

## Related programs (out of scope)

- **EVIDENCE-SPINE** — unified cross-service evidence resolver (separate package).
- **KANON-DEEPEN** — semantic authority surfaces (linked from Allagma run detail where DTOs allow).
- **UI-HARDEN** — shared `@ontogony/ui` primitives.

## Sign-off criteria

- [x] ACTION-000–006 have per-slice evidence
- [x] ACTION-007 closeout, scorecard, limitations, next-options
- [x] Unsupported operations documented and surfaced in UI
- [x] Docker `allagma-api` rebuilt with ACTION-006 routes (2026-05-20)
- [ ] Browser walkthrough executed and recorded
