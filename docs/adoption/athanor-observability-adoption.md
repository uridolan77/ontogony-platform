# Athanor Observability Adoption

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

- Keep X-Athanor-Trace-Id compatibility if existing clients depend on it.
- Ensure stable public error codes/messages for existing Athanor consumers.

## Verification Checklist

1. Trace id appears in response headers and error payloads.
2. Mapped Athanor exceptions return configured status and code.
3. Unmapped exceptions return safe generic 500 payload.

## Do Not Do This

- Do not place Athanor policy semantics in Ontogony.Platform.
- Do not add Athanor-specific exception types into shared package code.
