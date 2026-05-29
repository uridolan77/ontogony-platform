# Cross-service error envelope — v1

**Package:** `SHARED-ERROR-CONTRACT-001`  
**Owner:** `ontogony-platform` (`Ontogony.Errors`)  
**Status:** draft (promote on slice 3 closeout)

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

## Intentional exceptions (document, do not migrate)

| Service | Endpoint family | Shape |
| --- | --- | --- |
| Kanon | `ValidateOntologyResponse` | Typed validation result DTO |
| Kanon | `CompileSemanticQueryPlanResponse` | Typed compile result DTO |
| Conexus | OpenAI-compatible errors | Separate mapping table in Conexus docs |

## Adoption checklist

- [ ] Middleware maps unhandled exceptions to envelope
- [ ] 404/403 empty-body routes migrated
- [ ] Client parsers accept envelope
- [ ] `SYSTEM_ERROR_COMPATIBILITY_MATRIX.md` row complete
- [ ] System compat gate PASS

## Schema

See `docs/schemas/ontogony-cross-service-error-envelope-v1.schema.json` (create in slice 3).

## Related

- `allagma-dotnet/docs/system/SYSTEM_ERROR_COMPATIBILITY_MATRIX.md`
- `kanon-dotnet/docs/integrations/ERROR_CONTRACTS.md`
- `Ontogony.SystemCompatibility` error gate checks
