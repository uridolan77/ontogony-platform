# Cross-service error envelope — v1

**Package:** `SHARED-ERROR-CONTRACT-001`  
**Owner:** `ontogony-platform` (`Ontogony.Errors`)  
**Status:** promoted (2026-05-29)

## Purpose

Mechanical JSON shape for **middleware-mapped** HTTP errors across Ontogony backend services. Not a semantic error taxonomy — product meaning stays in each service's `code` namespace.

## Envelope (required fields)

| Field | Type | Rules |
| --- | --- | --- |
| `code` | string | Namespaced: `<system>.<domain>.<reason>` — must contain `.` |
| `message` | string | Safe human summary; no secrets |
| `system` | string | `platform` \| `conexus` \| `kanon` \| `allagma` \| `metabole` \| `aisthesis` |
| `traceId` | string? | From `X-Ontogony-Trace-Id` when available |
| `detail` | string? | Optional safe diagnostic; never raw prompts/responses |

Optional fields from `CrossServiceErrorEnvelope` (`stage`, `downstreamSystem`, `correlationId`, `retryable`) are allowed on the wire when a service needs them.

## Example

```json
{
  "code": "allagma.run.not_found",
  "message": "Run not found.",
  "system": "allagma",
  "traceId": "01HYZ...",
  "detail": "runId=abc123"
}
```

## Platform type

Use `CrossServiceErrorEnvelope` from `Ontogony.Errors` and `CrossServiceErrorEnvelopeConformanceAssertions` in tests.

## Middleware wire shapes (by service)

| Service | Middleware / endpoint errors | Notes |
| --- | --- | --- |
| Allagma | `CrossServiceErrorEnvelope` on `/allagma/v0` | Native |
| Metabole | `MetaboleErrorEnvelope` (compatible superset with `stage`, `details`) | Native |
| Aisthesis | `CrossServiceErrorEnvelope` on `/aisthesis/v0` | Native |
| Kanon | Ontogony `ApiError` (`code`, `message`, optional `traceId`, `detail`) | Consumers set `system=kanon` |
| Conexus | Ontogony `ApiError` on admin/middleware paths; OpenAI `error` on `/v1/chat/completions` | See Conexus boundaries |
| Platform | Ontogony `ApiError` from `OntogonyExceptionHandlingMiddleware` | Reference middleware |

## Intentional exceptions (document, do not migrate)

| Service | Endpoint family | Shape |
| --- | --- | --- |
| Kanon | `ValidateOntologyResponse` | Typed validation result DTO |
| Kanon | `CompileSemanticQueryPlanResponse` | Typed compile result DTO |
| Conexus | OpenAI-compatible errors | Separate mapping table in Conexus docs |

## Adoption checklist

- [x] Middleware maps unhandled exceptions to envelope (or documented `ApiError` / OpenAI variant)
- [x] 404/403 empty-body routes migrated (Kanon SYS-ERR-001)
- [x] Client parsers accept envelope
- [x] `SYSTEM_ERROR_COMPATIBILITY_MATRIX.md` row complete
- [x] System compat gate PASS

## Schema

- v1: [`docs/schemas/ontogony-cross-service-error-envelope-v1.schema.json`](../schemas/ontogony-cross-service-error-envelope-v1.schema.json)
- Conformance matrix: [`docs/system/schemas/cross-service-error-envelope-v0.schema.json`](../system/schemas/cross-service-error-envelope-v0.schema.json) (extended optional fields)

## Related

- [`CROSS_SERVICE_ERROR_ENVELOPE_GATE.md`](./CROSS_SERVICE_ERROR_ENVELOPE_GATE.md) — PLATFORM-9-002 conformance gate
- `allagma-dotnet/docs/system/SYSTEM_ERROR_COMPATIBILITY_MATRIX.md`
- `kanon-dotnet/docs/integrations/ERROR_CONTRACTS.md`
- `Ontogony.SystemCompatibility` error gate checks
