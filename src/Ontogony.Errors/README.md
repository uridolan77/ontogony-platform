# Ontogony.Errors

ASP.NET Core **exception-to-HTTP** mapping and a stable **problem details** JSON shape (with trace correlation).

## What this is

- `AddOntogonyErrors`, `UseOntogonyExceptionHandling` — register and order exception middleware.
- `OntogonyErrorJsonPayloadBuilder` — builds the same JSON dictionary the middleware writes (shared with endpoint helpers).
- `OntogonyMappedJsonResults.ApiError` — minimal-API `Results.Json` helper that respects `OntogonyExceptionMappingOptions` JSON keys and merges `OntogonyCorrelationContext.TraceId` when missing.
- `ApiError` — stable error DTO with code, message, trace id, and optional details; maps to ASP.NET Core `ProblemDetails` via extensions.
- `CrossServiceErrorEnvelope` — neutral cross-service JSON shape for downstream client mapping (`code`, `message`, `system`, optional `stage`, `downstreamSystem`, `traceId`, `retryable`, `detail`).

## What this is not

- Not domain exception types from product repos or business validation rule engines.

## See also

- `docs/packages/Ontogony.Errors.md`.
