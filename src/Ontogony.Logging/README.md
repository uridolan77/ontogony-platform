# Ontogony.Logging

## What this is

Structured logging mechanics for Ontogony services:

- stable field-name constants
- stable `EventId` constants
- correlation-aware logging scopes
- ASP.NET request logging-scope middleware
- service registration helpers

## What this is not

- not a log storage system
- not a log viewer
- not a Serilog/Seq/Datadog/Application Insights binding
- not a business audit ledger
- not a request/response body logger
- not a routing, pricing, or policy engine

`BeginOntogonyScope` does **not** redact `additionalFields` unless you pass a non-null `IRedactor` (register via `AddOntogonyRedaction()` or `AddOntogonySecrets()`). Correlation IDs are not treated as secrets by default.

## Middleware order with Hosting

`UseOntogonyServiceDefaults` does not register `UseOntogonyLoggingScope`. When you need request logging scopes, call `UseOntogonyLoggingScope()` **after** `UseOntogonyRequestTracing()` and **before** `UseOntogonyExceptionHandling()`. See the [Ontogony.Hosting README](../Ontogony.Hosting/README.md).

Use this package with `Microsoft.Extensions.Logging` and your host/exporter of choice.
