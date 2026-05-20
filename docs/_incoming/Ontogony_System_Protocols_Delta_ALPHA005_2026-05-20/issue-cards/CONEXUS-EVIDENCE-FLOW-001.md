# CONEXUS-EVIDENCE-FLOW-001 — Integrate Conexus model-call evidence operator flow into system evidence spine and frontend route catalog

**Priority:** P1  
**Repo:** conexus-dotnet and ontogony-frontend  
**Theme:** Operator evidence

## Problem

Conexus now documents a deterministic operator flow from chat completion to route decision, model-call detail, evidence links, bundle, usage/cost, and trace slots.

## Scope

Ensure frontend observability pages and Evidence Spine use the documented Conexus flow. Verify modelCallId, routeDecisionId, traceId, correlationId, and evidence bundle links align in live Docker-local runs.

## Acceptance criteria

Given a modelCallId from Allagma or Kanon assistance, operator UI can resolve route evidence, admin detail, evidence links, evidence bundle, route decision detail, and usage/cost without raw prompt/completion leakage.

## Source anchors

- `conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md`
- `conexus-dotnet/tests/Conexus.Api.Tests/ModelCallEvidenceOperatorFlowTests.cs`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
