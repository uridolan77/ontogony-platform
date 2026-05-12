# Ontogony.Errors

ASP.NET Core **exception-to-HTTP** mapping and a stable **problem details** JSON shape (with trace correlation).

## What this is

- `AddOntogonyErrors`, `UseOntogonyExceptionHandling` — register and order exception middleware.
- `ApiError` — stable error DTO with code, message, trace id, and optional details; maps to ASP.NET Core `ProblemDetails` via extensions.

## What this is not

- Not domain exception types from product repos or business validation rule engines.

## See also

- `docs/packages/Ontogony.Errors.md`.
