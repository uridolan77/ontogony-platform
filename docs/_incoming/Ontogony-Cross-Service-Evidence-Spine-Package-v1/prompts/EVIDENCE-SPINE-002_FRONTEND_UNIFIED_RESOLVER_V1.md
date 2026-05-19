# EVIDENCE-SPINE-002 — frontend unified resolver v1

Goal:
Implement a frontend resolver that can accept any known ID and return a normalized evidence graph.

Repos:
- C:\dev\ontogony-frontend
- C:\dev\ontogony-platform for evidence

Tasks:

1. Add resolver:
   src/evidence-spine/resolveEvidenceSpine.ts

2. Support direct roots:
   - Allagma run ID
   - Allagma eval ID
   - baseline comparison ID
   - Conexus model call/request ID
   - Conexus route decision ID if client route exists
   - Kanon decision ID
   - trace ID
   - correlation ID if supported
   - human gate ID best-effort from events

3. Use existing service clients where possible.
4. Reuse existing resolveTraceCorrelation where helpful, but do not force all evidence into trace-only model.
5. Record every source attempt:
   - service
   - endpoint/client method
   - input ID
   - status: success/not_found/error/skipped
   - reason

6. Build normalized graph:
   - nodes
   - edges
   - links
   - missing edge warnings

7. Tests:
   - run ID resolves Allagma run node
   - eval ID resolves eval + subject run
   - modelCallId resolves Conexus node and trace if available
   - decisionId resolves Kanon node
   - partial missing links still produce graph
   - service error records source failure

Evidence:
- ontogony-frontend/docs/evidence/EVIDENCE_SPINE_002_FRONTEND_UNIFIED_RESOLVER_EVIDENCE.md
- ontogony-platform/docs/evidence/EVIDENCE_SPINE_002_FRONTEND_UNIFIED_RESOLVER_EVIDENCE.md

Acceptance:
- unified resolver returns graph for at least run/eval/model-call/decision roots
- no new backend route required unless audit proves one is necessary
