# Ontogony.Observability

Async-local correlation context, trace header propagation, and diagnostic event sources for Ontogony services.

## Overview

`Ontogony.Observability` provides:

- **`OntogonyCorrelationContext`** — Async-local storage for trace IDs, actor context, and tenant scope
- **`RequestTracingMiddleware`** — ASP.NET Core middleware that extracts/generates trace IDs and echoes them in responses
- **Trace header propagation** — Automatic injection of trace ID and other context into outbound HTTP requests and event envelopes
- **Diagnostic activity source** — Built-in `OntogonyDiagnostics.ActivitySource` for OpenTelemetry integration

## Usage

```csharp
// In Startup
services.Configure<OntogonyObservabilityOptions>(opts => 
{
    opts.ServiceName = "my-service";
    opts.TraceHeaderName = "X-Ontogony-Trace-Id";  // Canonical name
});

app.UseRequestTracingMiddleware();  // Populates OntogonyCorrelationContext

// In request handlers
var traceId = OntogonyCorrelationContext.TraceId;
var tenant = OntogonyCorrelationContext.Current?.TenantId;
```

## Key Types

- **`OntogonyCorrelationContext`** — Static async-local accessor for `CorrelationState`
- **`CorrelationState`** — Record holding trace ID, operation ID, tenant, workspace, actor, session IDs
- **`RequestTracingMiddleware`** — Extracts trace headers, creates correlation scope, echoes trace ID in response
- **`OntogonyDiagnostics`** — Static diagnostic activity source and meter for instrumentation

## Design

- **Async-safe.** Uses `AsyncLocal<T>` so each request/background task has isolated context.
- **Header-aware.** Accepts legacy trace header aliases for backward compatibility during migrations.
- **Non-invasive.** Works with any downstream handler; context is set up before your code runs.

## License

MIT — see LICENSE in the repository root.
