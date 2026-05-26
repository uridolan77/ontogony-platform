# ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-006

Evidence Spine graph integration for decision-event reconstructability.

## Goal

When resolving an Allagma run in the Evidence Spine, show classified decision-event nodes with PASS/WARN/FAIL badges and open the `ReconstructabilityReportPanel` from each node.

## Loop

```text
resolveAllagmaRunEvidenceGraph
  → GET /allagma/v0/runs/{runId}/decision-events
  → POST /ontology/v0/reconstructability/classify-batch
  → append allagma.decisionEvent nodes to graph
  → Evidence Spine UI section + per-node report drawer
```

## Frontend deliverables

| Path | Purpose |
| --- | --- |
| `appendAllagmaDecisionEventReconstructabilityGraph.ts` | Graph merge |
| `loadAllagmaRunDecisionReconstruction.ts` | Live API load for spine |
| `EvidenceDecisionReconstructabilityPanel.tsx` | Node-level report panel |
| `EvidenceSpineWorkbench` | Dedicated section |
| `groupEvidenceGraphBySignal` | `decisionReconstructability` group |

## Acceptance

- [ ] Allagma run spine resolution appends `allagma.decisionEvent` nodes
- [ ] Node badge shows PASS/WARN/FAIL from Kanon classification
- [ ] Toggle opens `ReconstructabilityReportPanel` with full report
- [ ] Nodes link to run root via `derived_from`
- [ ] Unit test for graph append

## Out of scope

- Persisted report artifacts (DEC-RECON-007)
- Conexus decision-event export (CONEXUS-DECISION-EVENTS-001)
