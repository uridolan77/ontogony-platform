# Athanor Observability Adoption

Phased package order and when to adopt hashing/contracts first: [`athanor-platform-adoption.md`](./athanor-platform-adoption.md).

**Host wiring status:** Athanor.Api is expected to run `AddOntogonyObservability`, `AddOntogonyErrors`, `UseOntogonyRequestTracing`, and `UseOntogonyExceptionHandling` in production code paths (see Athanor `docs/engineering/PR20-athanor-ontogony-errors-tracing.md`). Remaining PR24-class work is **ops validation** (dashboards, alerts, burn-in), not duplicate middleware implementation in this repository.

This guide describes adopting Ontogony.Observability and Ontogony.Errors in Athanor while preserving Athanor semantics.

## Scope

Adopt shared mechanics:

- request tracing middleware
- correlation context and headers
- shared error contract middleware

Keep Athanor-owned behavior:

- decision/canonization semantics
- Athanor exception taxonomy and policy decisions

## Wiring Pattern

```csharp
builder.Services.AddOntogonyObservability(options =>
{
    options.ServiceName = "Athanor.Api";
    options.ServiceVersion = "0.1.0";
});

builder.Services.AddOntogonyErrors(options =>
{
    options.Map<ValidationException>(HttpStatusCode.BadRequest, "ValidationFailed", "The request is invalid.");
});

var app = builder.Build();
app.UseOntogonyRequestTracing();
app.UseOntogonyExceptionHandling();
```

## Compatibility Notes

- `OntogonyCorrelationContext.FromHeaders` still accepts `X-Athanor-Trace-Id` (`OntogonyEventHeaders.LegacyAthanorTraceId`) when `X-Ontogony-Trace-Id` is absent.
- Prefer emitting `X-Ontogony-Trace-Id` (and W3C `traceparent` / `tracestate` when applicable) for new callers. Legacy trace aliases on **responses** are off by default (`EchoLegacyHeaders = false`); set `EchoLegacyHeaders = true` only while external clients still require `X-Athanor-Trace-Id` on responses.
- Ensure stable public error codes/messages for existing Athanor consumers.

## Verification Checklist

1. Trace id appears in response headers and error payloads.
2. Mapped Athanor exceptions return configured status and code.
3. Unmapped exceptions return safe generic 500 payload.

## Do Not Do This

- Do not place Athanor policy semantics in Ontogony.Platform.
- Do not add Athanor-specific exception types into shared package code.
