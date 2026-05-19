# EVIDENCE-SPINE-001 — resolver contract and ID taxonomy

Goal:
Define canonical ID kinds, graph data model, and resolver contract.

Repos:
- C:\dev\ontogony-frontend
- C:\dev\ontogony-platform
- C:\dev\ontogony-ui only if shared types belong there

Tasks:

1. Create frontend types:
   - EvidenceIdentifierKind
   - EvidenceLookupInput
   - EvidenceGraph
   - EvidenceNode
   - EvidenceEdge
   - EvidenceSourceAttempt
   - EvidenceCompleteness
   - EvidenceResolutionResult

2. Add parser:
   - parseEvidenceIdentifier(raw)
   - support manual kind override
   - ambiguous ID classification
   - no hard dependency on prefix only

3. Add graph model helpers:
   - addNode
   - addEdge
   - mergeIdentifiers
   - markUnresolvedEdge
   - buildPageLinks

4. Add tests:
   - parse known IDs
   - ambiguous ID behavior
   - graph merge behavior
   - unresolved edge behavior

5. Platform docs:
   - docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md
   - docs/evidence/EVIDENCE_SPINE_001_RESOLVER_CONTRACT_EVIDENCE.md

Acceptance:
- canonical ID taxonomy exists
- graph model exists
- no UI graph yet required
- tests cover parser and graph helper behavior
