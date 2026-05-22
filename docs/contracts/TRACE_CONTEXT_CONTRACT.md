# Trace Context Contract

**Owner:** `Ontogony.Contracts.Events.OntogonyEventHeaders`
**Schema:** `schemas/contracts/trace-context.schema.json`

---

## Headers

| Header | Canonical? | Direction | Definition |
| --- | --- | --- | --- |
| `traceparent` | Yes | Inbound + outbound | W3C Trace Context Level 2; always extracted and propagated |
| `tracestate` | Yes | Inbound + outbound | W3C Trace Context; propagated when present |
| `X-Ontogony-Trace-Id` | Yes | Inbound + outbound | Ontogony trace identifier (hex 32-char or GUID) |
| `X-Ontogony-Correlation-Id` | Yes | Inbound + outbound | Per-request operation correlation id |
| `X-Correlation-ID` | Legacy alias | Inbound only | Accepted for interop; prefer `X-Ontogony-Correlation-Id` on outbound |

### Legacy inbound aliases (accepted, not emitted)

| Header | Replaced by |
| --- | --- |
| `X-Athanor-Trace-Id` | `X-Ontogony-Trace-Id` |
| `X-Agentor-Trace-Id` | `X-Ontogony-Trace-Id` |
| `X-Conexus-Request-Id` | `X-Ontogony-Trace-Id` |

---

## Span attributes

| Attribute | Value |
| --- | --- |
| `ontogony.trace_id` | Resolved trace id |
| `ontogony.operation_id` | Per-request operation id |
| `ontogony.actor_id` | Actor id when present |
| `ontogony.tenant_id` | Tenant id when present |
| `ontogony.workspace_id` | Workspace id when present |

---

## Resolution order

`OntogonyCorrelationContext.FromHeaders` resolves a trace id in this order:

1. `X-Ontogony-Trace-Id`
2. Legacy aliases in `OntogonyObservabilityOptions.AcceptedIncomingTraceHeaders`
3. `traceparent` (W3C parse)

If none yield a trace id, the request is treated as a new trace boundary.

---

## Compliance rule

Every runtime service must:

1. Extract incoming trace context via `RequestTracingMiddleware` or `OntogonyCorrelationContext.FromHeaders`.
2. Propagate `traceparent`, `X-Ontogony-Trace-Id`, and `X-Ontogony-Correlation-Id` on all outbound
   integration calls via `IntegrationHeadersDelegatingHandler`.
3. Attach `ontogony.trace_id` and `ontogony.operation_id` to every outbound span.

Frontend must:

- Display trace/correlation ids on all detail surfaces.
- Provide a copy affordance.
- Deep-link to the evidence spine using the same ids.
