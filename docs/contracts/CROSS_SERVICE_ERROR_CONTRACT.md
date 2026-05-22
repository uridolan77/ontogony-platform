# Cross-Service Error Contract

**Owner:** `Ontogony.Errors.CrossServiceErrorEnvelope`
**Schema:** `schemas/contracts/cross-service-error.schema.json`

---

## Envelope shape

```json
{
  "code": "allagma.run.not_found",
  "message": "Run abc123 was not found.",
  "system": "allagma",
  "stage": "run-lookup",
  "downstreamSystem": null,
  "traceId": "4bf92f3577b34da6a3ce929d0e0e4736",
  "correlationId": "a1b2c3d4-...",
  "retryable": false,
  "detail": null
}
```

## Fields

| Field | Required | Description |
| --- | --- | --- |
| `code` | Yes | Machine-readable error code.  Format: `{system}.{domain}.{kind}` |
| `message` | Yes | Human-readable summary safe for clients |
| `system` | Yes | Originating system (`conexus`, `kanon`, `allagma`) |
| `stage` | No | Processing stage label (e.g. `route-decision`, `run-dispatch`) |
| `downstreamSystem` | No | When this error wraps a downstream failure |
| `traceId` | No | Distributed trace identifier |
| `correlationId` | No | Correlation identifier |
| `retryable` | No | Whether the client may safely retry |
| `detail` | No | Structured detail payload (product-defined shape) |

---

## HTTP serialisation rules

- Returned on `4xx` and `5xx` responses as `application/problem+json` with property `ontogonyError`.
- `OntogonyExceptionHandlingMiddleware` maps internal exceptions to this envelope.
- `OperatorFailureView` provides a UI-safe flattened form for operator display.

---

## Error code namespacing

Codes are product-repo namespaced.  Platform owns only the envelope shape.

| Prefix | Owner repo |
| --- | --- |
| `conexus.*` | `conexus-dotnet` |
| `kanon.*` | `kanon-dotnet` |
| `allagma.*` | `allagma-dotnet` |
| `platform.*` | `ontogony-platform` |

---

## Compliance requirements

- Every 4xx/5xx response from a runtime service must include a `CrossServiceErrorEnvelope`-shaped body.
- `traceId` and `correlationId` must be populated when `OntogonyCorrelationContext` has a current state.
- Frontend must render the envelope's `code`, `message`, and `system` fields distinctly.
- UI must not render raw `detail` without redaction policy applied.
