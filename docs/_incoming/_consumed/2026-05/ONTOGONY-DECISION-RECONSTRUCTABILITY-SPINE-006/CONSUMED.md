# CONSUMED — ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-006

**Archived:** 2026-05-26  
**Reason:** DEC-RECON-006 Evidence Spine graph integration delivered in `ontogony-frontend` (classify-batch during Allagma run resolution, decision-event nodes, report drawer).

## Canonical references

| Artifact | Location |
| --- | --- |
| Protocol index | [`docs/contracts/DECISION_RECONSTRUCTABILITY_PROTOCOL_V0.md`](../../../contracts/DECISION_RECONSTRUCTABILITY_PROTOCOL_V0.md) |
| Closure closeout (PR-006 UI) | [`docs/evidence/ONTOGONY_RECONSTRUCTABILITY_CLOSURE_OPTION1_CLOSEOUT.md`](../../../evidence/ONTOGONY_RECONSTRUCTABILITY_CLOSURE_OPTION1_CLOSEOUT.md) |
| Frontend implementation | `ontogony-frontend` — `appendAllagmaDecisionEventReconstructabilityGraph.ts`, Evidence Spine workbench |

## Acceptance (from intake)

- Allagma run spine resolution appends `allagma.decisionEvent` nodes with PASS/WARN/FAIL badges
- Per-node toggle opens reconstructability report panel
- Unit test for graph append
