# EVIDENCE-SPINE-003 — Allagma evidence normalization

Goal:
Make Allagma run/eval/baseline/audit/evidence data first-class graph nodes.

Repos:
- C:\dev\ontogony-frontend
- C:\dev\allagma-dotnet only if a lookup gap is found
- C:\dev\ontogony-platform for evidence

Tasks:

1. Normalize Allagma nodes:
   - allagma.run
   - allagma.runEvent
   - allagma.evaluation
   - allagma.baselineComparison
   - allagma.auditBundle
   - allagma.evidenceExport

2. Extract IDs:
   - traceId
   - correlationId
   - planningDecisionId
   - modelCallId
   - humanGateId
   - evaluationRunId
   - baselineComparisonId
   - datasetId
   - scenarioId

3. Add source attempts for:
   - get run
   - list run events
   - list run evaluations
   - get evaluation
   - get evidence bundle
   - get baseline comparison
   - get run audit

4. Add graph edges:
   - run has events
   - run produced evaluation
   - evaluation has evidence export
   - evaluation has baseline comparison
   - run used model call
   - run used Kanon decision
   - run has audit bundle

5. Tests:
   - run detail DTO to graph
   - eval DTO to graph
   - baseline comparison to graph
   - missing modelCallId creates unresolved edge only when expected

Acceptance:
- Allagma pages can embed graph panels using normalized Allagma nodes
- run/eval/baseline evidence no longer uses bespoke one-off shapes
