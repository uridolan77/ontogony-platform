# EVIDENCE-SPINE-000 — current state audit

Goal:
Create a precise cross-service evidence baseline before implementing the unified resolver.

Repos:
- C:\dev\ontogony-frontend
- C:\dev\allagma-dotnet
- C:\dev\conexus-dotnet
- C:\dev\kanon-dotnet
- C:\dev\ontogony-platform

Tasks:

1. Frontend resolver audit
   Inspect:
   - src/system/correlation/resolveTraceCorrelation.ts
   - useTraceCorrelation
   - correlationTypes
   - correlationAdapters
   - enrichTraceCorrelationView
   - CrossServiceLinksCard
   - TraceCorrelationLookupBar
   - Allagma run/eval evidence journey builders
   - Conexus observability workbench
   - Kanon provenance evidence builders

2. Backend lookup route audit
   For each service, list current routes that can resolve:
   - run ID
   - eval ID
   - baseline comparison ID
   - model call/request ID
   - route decision ID
   - Kanon decision ID
   - trace ID
   - human gate ID

3. Gap matrix
   Create a matrix:
   - ID kind
   - owner service
   - direct lookup available?
   - indirect lookup available?
   - frontend wrapper exists?
   - UI link exists?
   - export/evidence available?
   - gap and owner

4. Test audit
   Identify existing unit/e2e coverage:
   - correlation resolver
   - trace URL sync
   - CrossServiceLinksCard
   - evidence journey links
   - diagnostics export
   - browser/e2e gaps

5. Deliver:
   ontogony-platform/docs/reviews/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT.md
   ontogony-platform/docs/evidence/EVIDENCE_SPINE_000_CURRENT_STATE_AUDIT_EVIDENCE.md

Validation:
- docs-only unless small correction is needed
- if code touched, run focused tests

Acceptance:
- current resolver behavior is documented
- ID taxonomy gaps are known
- first implementation slice is confirmed
