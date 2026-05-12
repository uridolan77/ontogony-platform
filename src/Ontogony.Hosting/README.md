# Ontogony.Hosting

Shared ASP.NET service-default wiring for Ontogony platform services.

## What this is

- Compose observability + errors defaults.
- Apply stable middleware ordering.
- Optionally include service-identity body-hash preload middleware.
- Map health/readiness endpoints with configurable paths.

## Ontogony.Logging middleware order

`UseOntogonyServiceDefaults` wires request tracing then exception handling. If you use `Ontogony.Logging` request scopes, insert **`UseOntogonyLoggingScope()`** from `Ontogony.Logging` **after** `UseOntogonyRequestTracing()` and **before** `UseOntogonyExceptionHandling()` so correlation context exists and exceptions still flow through the error middleware. Register `IRedactor` (for example `services.AddOntogonyRedaction()` or `AddOntogonySecrets()`) if you want the logging middleware to redact sensitive field names in the small default scope payload.

## What this is not

- No Athanor/Agentor/Conexus domain behavior.
- No product endpoint registration.
- No auth policy decisions.
