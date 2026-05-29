# Slice 3 — SHARED-ERROR-CONTRACT-001

**Status:** PASS (2026-05-29)  
**Owner:** `ontogony-platform` (contract); all APIs (adoption)  
**Depends on:** Slice 2  
**Prompt:** [`../prompts/P03_SHARED_ERROR_CONTRACT_001.md`](../prompts/P03_SHARED_ERROR_CONTRACT_001.md)

## Goal

Uniform **middleware-mapped** error JSON across all six backend HTTP hosts using `CrossServiceErrorEnvelope`.

## Contract shape (v1)

```json
{
  "code": "service.domain.reason",
  "message": "Human-readable summary",
  "system": "kanon|conexus|allagma|metabole|aisthesis|platform",
  "traceId": "optional-correlation-id",
  "detail": "optional-safe-detail"
}
```

## Intentional exceptions

- Kanon `ValidateOntologyResponse` / `CompileSemanticQueryPlanResponse` — typed success/failure DTOs (document in `ERROR_CONTRACTS.md`).
- Conexus OpenAI-compatible error bodies where spec requires (map taxonomy separately).

## Deliverables

1. [`../../contracts/CROSS_SERVICE_ERROR_ENVELOPE_V1.md`](../../contracts/CROSS_SERVICE_ERROR_ENVELOPE_V1.md)
2. JSON schema under `ontogony-platform/docs/schemas/`
3. `SYSTEM_ERROR_COMPATIBILITY_MATRIX.md` complete
4. Per-service tests for representative status codes
5. `validate-cross-service-error-envelope.ps1` PASS

## Evidence

Per-repo `docs/evidence/SHARED_ERROR_CONTRACT_001_CLOSEOUT.md`
