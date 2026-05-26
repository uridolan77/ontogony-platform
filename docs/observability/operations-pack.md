# Observability operations pack

Local development and burn-in guidance for OTLP traces and Prometheus metrics across Ontogony services.

## Prerequisites

- Docker-local stack healthy (Kanon **5081**, Conexus **5082**, Allagma **5083**).
- Platform trace contract scripts: [`docs/operators/TRACE_CORRELATION_CONTRACT.md`](../operators/TRACE_CORRELATION_CONTRACT.md).

## Verify correlation end-to-end

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\wait-local-working-system.ps1 -SkipFrontend
.\docker\local-working-system\scripts\inspect-trace-correlation-evidence.ps1
.\docker\local-working-system\scripts\validate-trace-correlation-evidence-report.ps1
```

Artifact: `docker/local-working-system/artifacts/trace-contract-001-evidence-report.json`

## OTLP export (services)

Configure ASP.NET services with `AddOntogonyObservability()` and standard OTLP exporter environment variables:

```text
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4318
OTEL_SERVICE_NAME=<service-name>
```

Use the Allagma observability compose stack when running the full local mesh (see `allagma-dotnet/docker-compose.observability.yml`).

## Metrics scrape

Prometheus should scrape each service’s `/metrics` (or OTLP collector forwarding). Platform instruments use the `ontogony.*` prefix — see [metrics-catalog.md](./metrics-catalog.md).

## Burn-in checklist

Before disabling legacy trace header aliases:

1. Confirm `X-Ontogony-Trace-Id` echoed on Allagma → Kanon → Conexus chain.
2. Confirm span tags `ontogony.trace_id` present in Jaeger for the same request.
3. Confirm structured logs include `traceId` matching the header.
4. Run reconstructability golden trace if validating decision-event correlation: `allagma-dotnet/scripts/system/run-reconstructability-golden-trace.ps1`.

## Dashboards and alerts (phase 2)

Starter Grafana queries and Prometheus alert rules are tracked under PLAT-9-005 phase 2. Until checked in, use Jaeger service map + `ontogony.http.server.error.count` spikes as the primary local signals.
