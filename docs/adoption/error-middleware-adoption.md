# Error Middleware Adoption

This guide shows how Athanor and Agentor can replace local exception middleware with Ontogony.Errors while keeping service-specific exception types and mappings in each service repository.

## Goal

Adopt a shared mechanical error contract without moving product/domain behavior into Ontogony.Platform.

Shared mechanics:

- unified API error payload shape
- consistent status and error code mapping
- safe message redaction defaults
- trace ID propagation into error payloads
- optional ProblemDetails compatibility

Service-owned behavior:

- which exception types exist
- which exception maps to which status/code
- whether a mapping exposes exception message/details

## Package Surface

Use:

- AddOntogonyErrors(...)
- UseOntogonyExceptionHandling()
- Exception mapping options via options.Map<TException>(...)
- ApiError <-> ProblemDetails conversion helpers

Current shared contracts:

- ApiError: Code, Message, TraceId, Details, Instance
- ExceptionMapping:
  - StatusCode
  - ErrorCode
  - PublicMessage
  - IncludeExceptionMessage
  - LogAsWarning
  - IncludeDetails

## Minimal Program.cs Wiring

```csharp
using System.Net;
using Ontogony.Errors;
using Ontogony.Observability;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOntogonyObservability(options =>
{
    options.ServiceName = "Athanor.Api";
    options.ServiceVersion = "0.1.0";
});

builder.Services.AddOntogonyErrors(options =>
{
    // Keep service exception types in the service repo.
    options.Map<ValidationException>(
        HttpStatusCode.BadRequest,
        "ValidationFailed",
        publicMessage: "The request is invalid.");

    options.Map<UnauthorizedAccessException>(
        HttpStatusCode.Forbidden,
        "Forbidden",
        publicMessage: "The operation is not allowed.");
});

var app = builder.Build();

// Place tracing before exception handling so trace id is available on failures.
app.UseOntogonyRequestTracing();
app.UseOntogonyExceptionHandling();
```

## Safe Defaults and Redaction

Default behavior is intentionally conservative:

- unmapped exceptions return HTTP 500
- unmapped exceptions return generic message: An unexpected error occurred.
- exception stack traces are never sent in API payload
- mapped exception message is not exposed unless explicitly enabled

Recommended mapping pattern:

- set PublicMessage for expected client-facing failures
- avoid IncludeExceptionMessage unless the exception text is known-safe
- only set IncludeDetails for safe, structured payloads

## Mapping Strategy

Keep mappings in each service composition root. Do not register service exception types in Ontogony.Platform.

Suggested status/code discipline:

- 400: ValidationFailed
- 401: Unauthorized
- 403: Forbidden
- 404: NotFound
- 409: Conflict
- 422: SemanticValidationFailed (if needed)
- 500: UnhandledError (fallback)

Codes should be stable for API clients and telemetry dashboards.

## ProblemDetails Bridge

If an endpoint or external integration expects ProblemDetails, bridge without changing the canonical service payload type.

```csharp
using Microsoft.AspNetCore.Mvc;
using Ontogony.Errors;

ApiError apiError = new("ValidationFailed", "The request is invalid.", traceId: "trace-123");
ProblemDetails pd = apiError.ToProblemDetails(statusCode: 400);

ApiError roundTrip = pd.ToApiError();
```

## Migration Checklist

1. Remove local exception middleware registration in the service repo.
2. Add AddOntogonyErrors(...) with service-local Map<TException>(...) entries.
3. Ensure middleware order is tracing first, exception handling second.
4. Follow `docs/adoption/observability-error-ordering.md` for ordering rationale and verification checks.
5. Update tests to assert:
   - unmapped exception uses generic 500 message
   - mapped exceptions return configured status/code/message
   - trace ID appears in error response when tracing is active
   - details only appear for mappings that allow details
   - response-started scenarios do not write a second response
6. Keep backward-compatible error codes/messages for public APIs where required.

## Notes for Athanor and Agentor

- Athanor and Agentor should preserve their own exception taxonomies and policy semantics.
- Ontogony.Errors provides mechanics only: payload shape, mapping workflow, safe defaults, middleware behavior.
- If a service needs API compatibility with an existing error schema, perform adaptation in that service boundary.
