# Observability PASS Contract

## Owner

`allagma-dotnet` + `ontogony-platform`

## Purpose

Define what a real observability PASS means for the backend alpha-RC.

## Required classification

Each check must be one of:

```text
PASS
DEGRADED
FAIL
NOT_RUN
```

A final `verdict: PASS` may not include `FAIL` or `NOT_RUN` for core checks.

## Core checks

```text
allagma_metrics
kanon_metrics
conexus_metrics
allagma_traces
kanon_traces
conexus_traces
cross_service_trace_chain
correlation_id_propagation
model_lifecycle_metrics
human_gate_metrics
gateway_metrics
prometheus_scrape
jaeger_query
grafana_dashboard
```

## Required output

```json
{
  "schema": "ontogony-observability-summary-v1",
  "verdict": "PASS",
  "generatedAt": "...",
  "checks": []
}
```

## Redaction rule

Observability labels/log fields must not include:

```text
raw prompts
raw completions
provider secrets
admin keys
service tokens
full PII payloads
```

## Release rule

No docs may claim observability PASS unless they link the current `observability-summary.json`.
