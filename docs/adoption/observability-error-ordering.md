# Observability and Error Middleware Ordering

This guide defines the recommended middleware order when using Ontogony.Observability with Ontogony.Errors.

## Why Ordering Matters

Ontogony exception payloads can include a trace id from the current correlation context. That context is created by request tracing middleware.

If exception handling runs before tracing, exceptions thrown inside tracing can be handled without the expected trace context, producing less useful error telemetry.

## Recommended Order

Use this order in service apps:

1. UseOntogonyRequestTracing()
2. UseOntogonyExceptionHandling()
3. Authentication/Authorization
4. Endpoint mapping

Example:

```csharp
var app = builder.Build();

app.UseOntogonyRequestTracing();
app.UseOntogonyExceptionHandling();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
```

## Anti-Pattern

Avoid placing exception handling before tracing:

```csharp
app.UseOntogonyExceptionHandling();
app.UseOntogonyRequestTracing();
```

This can reduce trace continuity for thrown failures during early pipeline execution.

## Athanor and Agentor Pattern

Apply the same order in both services:

- Athanor.Api: tracing first, exception handling second
- Agentor.Api: tracing first, exception handling second

Service-specific behavior (exception mapping choices, error codes, public messages) remains in each service repository via AddOntogonyErrors mappings.

## Verification Checklist

1. Trigger an unmapped exception and confirm generic 500 response.
2. Confirm error payload includes trace id when tracing middleware is active.
3. Confirm mapped exceptions return configured status and stable error code.
4. Confirm response-started edge cases do not attempt a second write.
5. Confirm logs correlate with the same trace id used in payload/headers.
