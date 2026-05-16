# Observability Operations Pack

This guide makes Ontogony observability operational for local and pre-production environments.

It focuses on mechanics only: trace and metric transport, stable instrument names, and correlation behavior.

## 1. Wire OpenTelemetry exporters in a service

Use the Ontogony activity source and meter names as first-class telemetry sources.

```csharp
using Ontogony.Hosting;
using Ontogony.Observability;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOntogonyServiceDefaults(builder.Configuration, options =>
{
    options.ServiceName = "my-service";
});

// Or, without Hosting defaults:
// builder.Services.AddOntogonyObservability(options => options.ServiceName = "my-service");
// builder.Services.AddOntogonyOpenTelemetryExport("my-service");

var app = builder.Build();
app.UseOntogonyServiceDefaults();
```

`AddOntogonyServiceDefaults` calls `AddOntogonyOpenTelemetryExport`, which registers OTLP export only when `OTEL_EXPORTER_OTLP_ENDPOINT` is set.

Set OTLP endpoint via environment variable in development:

```powershell
$env:OTEL_EXPORTER_OTLP_ENDPOINT = "http://localhost:4317"
```

## 2. Run a local collector stack

Use the sample in `docs/observability/local-collector/`.

```powershell
docker compose -f docs/observability/local-collector/docker-compose.yml up -d
```

Local endpoints:

- Collector OTLP gRPC: `localhost:4317`
- Collector OTLP HTTP: `localhost:4318`
- Prometheus UI: `http://localhost:9090`
- Jaeger UI: `http://localhost:16686`

## 3. Dashboard queries

See `docs/observability/dashboard-queries.md` for PromQL examples.

## 4. Alert rules

See `docs/observability/alerts.prometheus.rules.yml` for baseline alert rules.

## 5. Log correlation pattern

Use a logging scope that includes Ontogony correlation IDs during request handling:

```csharp
using (_logger.BeginScope(new Dictionary<string, object?>
{
    ["traceId"] = OntogonyCorrelationContext.TraceId,
    ["operationId"] = OntogonyCorrelationContext.OperationId,
    ["tenantId"] = OntogonyCorrelationContext.TenantId,
    ["workspaceId"] = OntogonyCorrelationContext.WorkspaceId,
    ["projectId"] = OntogonyCorrelationContext.ProjectId,
    ["actorId"] = OntogonyCorrelationContext.ActorId,
    ["sessionId"] = OntogonyCorrelationContext.SessionId,
}))
{
    _logger.LogInformation("Handling integration callback");
}
```

Expected behavior:

- Request tracing middleware populates correlation context from incoming headers.
- The same IDs are available to logs, spans, and outbound HTTP propagation.

## 6. Trace header migration burn-in checks

See `docs/observability/trace-header-burn-in-checks.md`.

## 7. Catalogs

- `docs/observability/metrics-catalog.md`
- `docs/observability/trace-attributes.md`
