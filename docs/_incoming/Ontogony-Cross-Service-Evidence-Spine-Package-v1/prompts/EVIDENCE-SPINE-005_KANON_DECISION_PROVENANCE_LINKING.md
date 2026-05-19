# EVIDENCE-SPINE-005 — Kanon decision provenance linking

Goal:
Normalize Kanon planning/provenance decisions and link them to Allagma and Conexus evidence.

Repos:
- C:\dev\ontogony-frontend
- C:\dev\kanon-dotnet only if lookup gaps are found
- C:\dev\ontogony-platform for evidence

Tasks:

1. Audit Kanon client wrappers:
   - get decision by ID
   - get decision by trace
   - provenance/evidence routes
   - domain pack/ontology version context if relevant

2. Normalize nodes:
   - kanon.decision
   - kanon.provenance
   - kanon.domainPack
   - kanon.ontologyVersion
   - kanon.humanGate or policy/gate decision if exposed

3. Extract IDs:
   - decisionId
   - traceId
   - correlationId
   - allagmaRunId
   - ontologyVersionId
   - domainPackId
   - actorId
   - decision kind/status

4. Add edges:
   - run used Kanon decision
   - decision used ontology version
   - decision used domain pack
   - decision produced human gate
   - decision linked to trace/correlation

5. Tests:
   - decision ID root graph
   - trace ID resolves decision
   - missing role/403 classified as authorization warning, not service down
   - decision links render

Acceptance:
- Kanon decisions/provenance become first-class graph nodes
- evidence spine makes semantic authority visible
