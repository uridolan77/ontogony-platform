# EVIDENCE-SPINE-006 — graph workbench UI

Goal:
Build the operator workbench: paste any known ID and inspect the resolved graph.

Repos:
- C:\dev\ontogony-frontend
- C:\dev\ontogony-ui only if shared graph/card primitives are useful
- C:\dev\ontogony-platform for evidence

Tasks:

1. Add route:
   - `/evidence-spine` or `/system/evidence-spine`

2. Add nav entry:
   - System → Evidence spine
   or a top-level utility if appropriate.

3. UI components:
   - EvidenceSpineLookupBar
   - EvidenceGraphSummaryCard
   - EvidenceGraphNodeCard
   - EvidenceGraphEdgesPanel
   - EvidenceSourceAttemptsPanel
   - EvidenceMissingLinksPanel
   - EvidenceSpineExportPanel

4. Behavior:
   - paste raw ID
   - auto-detect kind
   - allow manual kind override
   - show loading
   - show graph
   - show partial results
   - source attempts visible
   - page links/actions visible

5. Integrate workbench link into:
   - Allagma run detail
   - Allagma eval detail
   - Conexus observability
   - Kanon decision/provenance pages
   - Command center

6. Tests:
   - lookup submits to resolver
   - graph renders nodes/edges
   - missing links render reasons
   - source attempts render
   - page links generated

Acceptance:
- operator can paste run/eval/model-call/decision ID and get a graph
- graph is useful even with partial data
- links open relevant product pages
