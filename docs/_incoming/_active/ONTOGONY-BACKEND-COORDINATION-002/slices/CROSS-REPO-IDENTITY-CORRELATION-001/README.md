# Slice 4 — CROSS-REPO-IDENTITY-CORRELATION-001

**Owner:** `ontogony-platform` (contract); all HTTP clients/hosts (adoption)  
**Depends on:** Slice 3  
**Prompt:** [`../prompts/P04_CROSS_REPO_IDENTITY_CORRELATION_001.md`](../prompts/P04_CROSS_REPO_IDENTITY_CORRELATION_001.md)

## Goal

End-to-end propagation of trace, correlation, actor, and idempotency headers across the governed runtime and Aisthesis producers.

## Required headers (minimum)

| Header | Purpose |
| --- | --- |
| `X-Ontogony-Trace-Id` | Distributed trace |
| `X-Ontogony-Correlation-Id` | Business correlation |
| `X-Ontogony-Idempotency-Key` | Mutating request dedup |
| `X-Ontogony-Actor-Id` | Actor context (alpha/dev) |

See `Ontogony.Http` / `Ontogony.Contracts` for canonical constants.

## Deliverables

1. [`../../contracts/CROSS_SERVICE_CONTEXT_PROPAGATION_V1.md`](../../contracts/CROSS_SERVICE_CONTEXT_PROPAGATION_V1.md)
2. Allagma outbound clients attach propagation context
3. `SYSTEM_TRACE_CONTEXT_MATRIX.md` matches code
4. Correlation chain scenario PASS in system cohesion acceptance

## Evidence

`allagma-dotnet/docs/evidence/CROSS_REPO_IDENTITY_CORRELATION_001_CLOSEOUT.md`

## Non-claims

- Production OIDC/IAM (defer to RC readiness packages).
