# Cross-service context propagation — v1

**Package:** `CROSS-REPO-IDENTITY-CORRELATION-001`  
**Owner:** `ontogony-platform` (`Ontogony.Http`, `Ontogony.Contracts`, `Ontogony.Security`)  
**Status:** draft (promote on slice 4 closeout)

## Purpose

Define which headers propagate across Ontogony HTTP calls so traces, correlation, actors, and idempotency survive Allagma → Kanon → Conexus → Aisthesis chains.

## Required propagation (mutating cross-service calls)

| Header | Constant source | Required |
| --- | --- | --- |
| `X-Ontogony-Trace-Id` | `OntogonyEventHeaders.TraceId` | Yes |
| `X-Ontogony-Correlation-Id` | `OntogonyEventHeaders.CorrelationId` | Yes when business correlation exists |
| `X-Ontogony-Idempotency-Key` | Idempotency middleware | Yes on POST/PUT/PATCH with side effects |
| `X-Ontogony-Actor-Id` | `OntogonyEventHeaders.ActorId` | When actor context present |

## Optional context headers

| Header | When |
| --- | --- |
| `X-Ontogony-Tenant-Id` | Multi-tenant alpha scenarios |
| `X-Ontogony-Project-Id` | Conexus project scope |
| `X-Ontogony-Workspace-Id` | Operator workspace |
| `X-Allagma-Run-Id` | Downstream attribution to governed run |

## Service identity (service-to-service)

For signed service calls, use `OntogonyServiceIdentityHeaders` (`X-Ontogony-Service-Id`, timestamp, nonce, signature). Production IAM deferred.

## Client obligations

| Client | Must forward |
| --- | --- |
| `Kanon.Client` | Trace + correlation + idempotency |
| `Conexus.Client` | Trace + correlation + project context |
| `Metabole.Client` | Trace + correlation |
| `Aisthesis.Client` | Trace + producer token (separate auth policy) |
| Allagma outbound | All of the above to peers |

## Verification

- `SYSTEM_TRACE_CONTEXT_MATRIX.md` matches constants
- Correlation chain scenario in `run-system-coh-001-acceptance.ps1 -IncludeCorrelationChain`
- Tests in `Ontogony.Http.Tests` for propagation helpers

## Non-goals

- Production OIDC/JWT federation (RC readiness)
- Cross-cloud W3C tracecontext-only mode (may add as optional profile later)
