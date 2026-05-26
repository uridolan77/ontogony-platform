# Standard span names and activity sources

## Platform activity source

| Field | Value |
| --- | --- |
| Logical name | `Ontogony.Platform` |
| Type | `OntogonyDiagnostics.ActivitySource` |
| Used by | `RequestTracingMiddleware`, `IntegrationOperationMeter` |

Product services should register **their own** `ActivitySource` with a service-specific name (`Allagma.Runtime`, `Conexus.Gateway`, etc.). Do not reuse `Ontogony.Platform` for product spans.

## Recommended span naming pattern

```text
<service>.<area>.<operation>
```

Examples (product-owned):

```text
allagma.run.execute
conexus.gateway.route
kanon.ontology.query_compile
```

Platform HTTP ingress (middleware):

```text
HTTP <method> <path>   — created by RequestTracingMiddleware (implementation detail)
```

## Required correlation tags

Apply via `SystemCorrelationConventions.ApplyToActivity` or equivalent:

| Span attribute | Source |
| --- | --- |
| `ontogony.trace_id` | `X-Ontogony-Trace-Id` / `CorrelationState.TraceId` |
| `ontogony.operation_id` | `X-Ontogony-Correlation-Id` / `CorrelationState.OperationId` |
| `ontogony.actor_id` | `X-Ontogony-Actor-Id` when present |
| `ontogony.tenant_id` | `X-Ontogony-Tenant-Id` when present |
| `ontogony.project_id` | `X-Ontogony-Project-Id` when present |

Full list: [trace-attributes.md](./trace-attributes.md).

## W3C traceparent

`traceparent` / `tracestate` are orthogonal to `X-Ontogony-Trace-Id`. Services may participate in OTLP/Jaeger export while still echoing Ontogony headers for operator tooling. See [`TRACE_CORRELATION_CONTRACT.md`](../operators/TRACE_CORRELATION_CONTRACT.md).
