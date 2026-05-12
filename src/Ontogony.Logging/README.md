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

Use this package with `Microsoft.Extensions.Logging` and your host/exporter of choice.
