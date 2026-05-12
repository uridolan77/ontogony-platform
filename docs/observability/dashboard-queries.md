# Dashboard Queries

These are PromQL examples for Ontogony metrics exported through the OpenTelemetry Collector to Prometheus.

Adjust `service` label values to your deployment.

## Request volume and errors

```promql
sum by (service, method) (
  rate(ontogony_http_server_request_count_total[5m])
)
```

```promql
sum by (service, method) (
  rate(ontogony_http_server_error_count_total[5m])
)
```

```promql
sum(rate(ontogony_http_server_error_count_total[5m]))
/
sum(rate(ontogony_http_server_request_count_total[5m]))
```

## Request latency

```promql
histogram_quantile(
  0.95,
  sum by (le, service, method) (
    rate(ontogony_http_server_duration_ms_bucket[5m])
  )
)
```

## Integration call failures

```promql
sum by (integration, operation) (
  rate(ontogony_integration_error_count_total[5m])
)
```

## Event dispatch reliability

```promql
sum by (event_type, protocol, operation_mode) (
  rate(ontogony_event_dispatch_count_total[5m])
)
```

```promql
sum by (event_type, protocol, operation_mode) (
  rate(ontogony_event_dispatch_failure_count_total[5m])
)
```

## Event handler latency

```promql
histogram_quantile(
  0.95,
  sum by (le, event_type, handler_description) (
    rate(ontogony_event_handler_duration_ms_bucket[5m])
  )
)
```
