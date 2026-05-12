# Metrics Catalog

All metrics are emitted from meter name `Ontogony.Platform`.

## ontogony.http.server.request.count

- Name: `ontogony.http.server.request.count`
- Type: Counter<long>
- Dimensions: `service`, `method`, `status_code`
- Emitted when: request pipeline finishes and final status code is known
- Example query:

```promql
sum by (service, method, status_code) (
  rate(ontogony_http_server_request_count_total[5m])
)
```

- Alert idea: trigger when request volume drops to zero unexpectedly for a critical service

## ontogony.http.server.error.count

- Name: `ontogony.http.server.error.count`
- Type: Counter<long>
- Dimensions: `service`, `method`, `status_code`
- Emitted when: request ends with unhandled exception or status code >= 500
- Example query:

```promql
sum by (service, method) (
  rate(ontogony_http_server_error_count_total[5m])
)
```

- Alert idea: sustained error ratio above threshold for 10 minutes

## ontogony.http.server.duration.ms

- Name: `ontogony.http.server.duration.ms`
- Type: Histogram<double> (unit: ms)
- Dimensions: `service`, `method`
- Emitted when: request pipeline finishes and elapsed milliseconds are known
- Example query:

```promql
histogram_quantile(
  0.95,
  sum by (le, service, method) (
    rate(ontogony_http_server_duration_ms_bucket[5m])
  )
)
```

- Alert idea: P95 latency above objective for sustained interval

## ontogony.integration.call.count

- Name: `ontogony.integration.call.count`
- Type: Counter<long>
- Dimensions: `integration`, `operation`, `status_code`
- Emitted when: integration call completion is recorded through `OntogonyMetrics.RecordIntegrationCall`
- Example query:

```promql
sum by (integration, operation, status_code) (
  rate(ontogony_integration_call_count_total[5m])
)
```

- Alert idea: sudden drop in expected call volume for required integrations

## ontogony.integration.error.count

- Name: `ontogony.integration.error.count`
- Type: Counter<long>
- Dimensions: `integration`, `operation`, `status_code`
- Emitted when: integration failure is recorded through `OntogonyMetrics.RecordIntegrationError`
- Example query:

```promql
sum by (integration, operation) (
  rate(ontogony_integration_error_count_total[5m])
)
```

- Alert idea: non-zero error rate for a critical integration over a sustained window

## ontogony.integration.duration.ms

- Name: `ontogony.integration.duration.ms`
- Type: Histogram<double> (unit: ms)
- Dimensions: `integration`, `operation`
- Emitted when: integration call duration is recorded through `OntogonyMetrics.RecordIntegrationDuration`
- Example query:

```promql
histogram_quantile(
  0.95,
  sum by (le, integration, operation) (
    rate(ontogony_integration_duration_ms_bucket[5m])
  )
)
```

- Alert idea: P95 integration latency above baseline for sustained period

## ontogony.event.publish.count

- Name: `ontogony.event.publish.count`
- Type: Counter<long>
- Dimensions: `event_type`, `protocol`, `operation_mode`
- Emitted when: publish attempt is accepted into publisher pipeline
- Example query:

```promql
sum by (event_type, protocol, operation_mode) (
  rate(ontogony_event_publish_count_total[5m])
)
```

- Alert idea: publish throughput drops sharply during expected traffic windows

## ontogony.event.dispatch.count

- Name: `ontogony.event.dispatch.count`
- Type: Counter<long>
- Dimensions: `event_type`, `protocol`, `operation_mode`
- Emitted when: in-process handler dispatch completes successfully
- Example query:

```promql
sum by (event_type, protocol, operation_mode) (
  rate(ontogony_event_dispatch_count_total[5m])
)
```

- Alert idea: dispatch count diverges significantly from publish count

## ontogony.event.dispatch.failure.count

- Name: `ontogony.event.dispatch.failure.count`
- Type: Counter<long>
- Dimensions: `event_type`, `protocol`, `operation_mode`
- Emitted when: a handler dispatch throws
- Example query:

```promql
sum by (event_type, protocol, operation_mode) (
  rate(ontogony_event_dispatch_failure_count_total[5m])
)
```

- Alert idea: any non-zero failure rate for critical event types

## ontogony.event.handler.duration.ms

- Name: `ontogony.event.handler.duration.ms`
- Type: Histogram<double> (unit: ms)
- Dimensions: `event_type`, `protocol`, `operation_mode`, `handler_description`
- Emitted when: each event handler execution duration is recorded
- Example query:

```promql
histogram_quantile(
  0.95,
  sum by (le, event_type, handler_description) (
    rate(ontogony_event_handler_duration_ms_bucket[5m])
  )
)
```

- Alert idea: handler latency regression for specific handler or event type
