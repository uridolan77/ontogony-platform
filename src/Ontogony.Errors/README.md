# Ontogony.Errors

ASP.NET Core **exception-to-HTTP** mapping and a stable **problem details** JSON shape (with trace correlation).

## What this is

- `AddOntogonyErrors`, `UseOntogonyExceptionHandling` — register and order exception middleware.
- `OntogonyErrorJsonPayloadBuilder` — builds the same JSON dictionary the middleware writes (shared with endpoint helpers).
- `OntogonyMappedJsonResults.ApiError` — minimal-API `Results.Json` helper that respects `OntogonyExceptionMappingOptions` JSON keys and merges `OntogonyCorrelationContext.TraceId` when missing.
- `ApiError` — stable endpoint error DTO (`code`, `message`, `traceId`, optional details/instance), with `ProblemDetails` mapping helpers.
- `CrossServiceErrorEnvelope` — neutral internal cross-service envelope for downstream client mapping (`code`, `message`, `system`, optional `stage`, `downstreamSystem`, `traceId`, `correlationId`, `retryable`, `detail`).
- `OperatorFailureTaxonomyAdapter` — maps `CrossServiceErrorEnvelope` to stable operator taxonomy kinds for consoles (SYS-TIGHT-006); does not alter public service error contracts.

## What this is not

- Not domain exception types from product repos or business validation rule engines.
- Not product-specific public HTTP contract design. Product repos own any external API contract variants.

## See also

- `docs/packages/Ontogony.Errors.md`.
