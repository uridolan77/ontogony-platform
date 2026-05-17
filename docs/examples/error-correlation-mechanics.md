# Error and correlation mechanics (no product semantics)

Minimal patterns for **Ontogony.Errors** and **Ontogony.Observability** in any ASP.NET service. Replace placeholder route and exception names with your product’s types in the **consumer repo** — do not add domain meaning here.

## Correlation: trace id vs correlation id

| Concept | Header / field | Notes |
| --- | --- | --- |
| **Trace id** | `X-Ontogony-Trace-Id` | Primary distributed trace; echoed on responses when middleware is enabled |
| **Correlation id** | `X-Ontogony-Correlation-Id` | Optional operation-level id; may differ from trace id |
| **Legacy inbound** | `X-Correlation-ID`, `X-Agentor-Trace-Id`, … | Accepted for interop; prefer canonical headers in new code |

See [`docs/contracts/header-compatibility-matrix.md`](../contracts/header-compatibility-matrix.md).

### Startup (mechanical)

```csharp
builder.Services.Configure<OntogonyObservabilityOptions>(o =>
{
    o.ServiceName = "my-service"; // consumer chooses name
    o.TraceHeaderName = "X-Ontogony-Trace-Id";
    // EchoLegacyHeaders = false by default; set true only during legacy rollout
});

builder.Services.AddOntogonyObservability();
```

### Middleware order

```csharp
app.UseOntogonyRequestTracing();   // early: parse/propagate trace + correlation
app.UseOntogonyExceptionHandling(); // early: map exceptions to JSON + traceId
```

## API errors: middleware-mapped exceptions

Map **your** exception types to HTTP status and stable `code` strings in the consumer repo:

```csharp
builder.Services.AddOntogonyErrors(opts =>
{
    opts.Map<ArgumentException>(StatusCodes.Status400BadRequest, "invalid_argument");
    opts.Map<KeyNotFoundException>(StatusCodes.Status404NotFound, "not_found");
});
```

Unhandled mapped exceptions produce JSON shaped like:

```json
{
  "code": "not_found",
  "message": "Resource was not found.",
  "traceId": "01JABC..."
}
```

`traceId` is taken from `OntogonyCorrelationContext` when the handler did not set one explicitly.

## Minimal API: same payload shape

```csharp
app.MapGet("/items/{id}", (string id) =>
{
    if (id.Length == 0)
        return OntogonyMappedJsonResults.ApiError(
            StatusCodes.Status400BadRequest,
            code: "invalid_argument",
            message: "id is required");
    return Results.Ok();
});
```

Uses the same JSON builder as exception middleware (`OntogonyErrorJsonPayloadBuilder`).

## Outbound calls: propagate trace to downstream

```csharp
builder.Services.AddOntogonyIntegrationHttpClient(
    "downstream-api",
    _ => new HttpIntegrationOptions
    {
        BaseUrl = builder.Configuration["Downstream:BaseUrl"]!,
        TimeoutSeconds = 30
    });
```

Ontogony.Http copies trace/correlation (and optional actor/tenant headers per configuration) onto outbound requests. Downstream services should run `UseOntogonyRequestTracing()` to continue the chain.

## Cross-service error envelope (internal)

For **service-to-service** responses, `CrossServiceErrorEnvelope` in `Ontogony.Errors` is a mechanical DTO — not the public HTTP contract of Kanon, Conexus, or Allagma. Each product repo owns its route-level error DTOs; they may align field names (`code`, `message`, `traceId`, `detail`) without importing each other’s types.

## What not to put in Platform examples

- Canonization, ontology validation, or policy denial messages (Kanon)
- Provider routing, model aliases, or quota denial codes (Conexus)
- Run/step/tool/human-gate outcomes (Allagma)

## Related

- [`docs/packages/Ontogony.Errors.md`](../packages/Ontogony.Errors.md)
- [`docs/03_TRACE_CORRELATION_STANDARD.md`](../03_TRACE_CORRELATION_STANDARD.md)
- [`docs/examples/index.md`](./index.md) — broader integration examples
