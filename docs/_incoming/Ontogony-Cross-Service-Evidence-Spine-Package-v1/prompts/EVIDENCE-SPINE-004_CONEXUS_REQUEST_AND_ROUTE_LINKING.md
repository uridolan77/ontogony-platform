# EVIDENCE-SPINE-004 — Conexus request and route linking

Goal:
Normalize Conexus model-call/request/route-decision evidence and link it into the evidence graph.

Repos:
- C:\dev\ontogony-frontend
- C:\dev\conexus-dotnet only if lookup/list contract gaps are found
- C:\dev\ontogony-platform for evidence

Tasks:

1. Audit current Conexus clients:
   - execution run by request/model-call ID
   - route decision by routeDecisionId
   - diagnostics/usage
   - provider inventory
   - recent request list if available

2. Normalize nodes:
   - conexus.modelCall
   - conexus.executionRun
   - conexus.routeDecision
   - conexus.providerAttempt if available
   - conexus.usageWindow if relevant

3. Extract IDs:
   - traceId
   - modelCallId/requestId
   - routeDecisionId
   - providerKey
   - providerModel
   - modelAlias
   - token counts
   - error code

4. Add edges:
   - run used model call
   - model call used route decision
   - route decision selected provider
   - provider attempt succeeded/failed
   - model call has token usage

5. If current Conexus API lacks recent/detail endpoints needed by the graph:
   - document exact gap
   - defer to CONEXUS-DEEPEN-001 or add small route only if clearly required

6. Tests:
   - modelCallId root graph
   - routeDecisionId root graph
   - Conexus failure classification
   - no prompt/raw completion leakage in graph preview

Acceptance:
- Conexus observability can use EvidenceGraph rather than only local lookup panels
- model-call roots resolve into graph where data exists
