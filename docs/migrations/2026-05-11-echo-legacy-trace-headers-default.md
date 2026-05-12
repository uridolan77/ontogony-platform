# Migration: `EchoLegacyHeaders` default is false

## Ontogony.Observability

- **`OntogonyObservabilityOptions.EchoLegacyHeaders`** default changed from **`true`** to **`false`**.
- **Responses** still always set the canonical trace header (`TraceHeaderName`, default `X-Ontogony-Trace-Id`).
- **Legacy response aliases** (`X-Athanor-Trace-Id`, `X-Agentor-Trace-Id`, `X-Conexus-Request-Id` mirroring the same value) are emitted only when **`EchoLegacyHeaders = true`** is set in `AddOntogonyObservability(...)`.
- **Inbound** correlation is unchanged: `OntogonyCorrelationContext.FromHeaders` still accepts legacy incoming headers when the canonical header is missing.

## Downstream repos

- **Clients, scripts, and UI** that read only a legacy response header must switch to **`X-Ontogony-Trace-Id`**, or read canonical first with a legacy fallback during migration.
- **Services** that must keep emitting legacy response headers for external callers set **`options.EchoLegacyHeaders = true`** explicitly until those callers migrate.
