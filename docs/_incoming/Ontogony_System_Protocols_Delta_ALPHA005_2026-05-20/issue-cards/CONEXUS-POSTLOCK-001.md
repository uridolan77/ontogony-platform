# CONEXUS-POSTLOCK-001 — Classify post-Alpha-005 Conexus route preview, quota status, streaming contract, and evidence-flow additions

**Priority:** P1  
**Repo:** conexus-dotnet  
**Theme:** Post-lock API deltas

## Problem

Conexus main has meaningful additions after the Alpha-005 lock: route preview endpoints, quota status contracts, streaming contract docs/tests, model-call evidence operator flow, and store-registration cleanup.

## Scope

Create a short post-lock classification doc: additive safe API, test-only, operator-doc-only, runtime behavior change, lock-impacting package contract, or follow-up needed.

## Acceptance criteria

Each changed file group is classified; lock-impacting changes are either validated into Alpha-006 or left as moving-main deltas. Frontend/API clients know which new endpoints are safe to consume.

## Source anchors

- `compare Conexus Alpha-005 lock SHA 9847f9... to main`
- `docs/contracts/STREAMING_CHAT_COMPLETION_CONTRACT.md`
- `docs/operators/MODEL_CALL_EVIDENCE_FLOW.md`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
