# Standard metric tags (dimensions)

## Platform HTTP server (`ontogony.http.server.*`)

| Tag | Description |
| --- | --- |
| `service` | Logical service name from options |
| `method` | HTTP method |
| `status_code` | Response status |

## Platform integration (`ontogony.integration.*`)

| Tag | Description |
| --- | --- |
| `source_service` | Caller service |
| `target_service` | Callee service |
| `operation` | Operation name |
| `status` | `success` / `failure` |
| `error_code` | Stable error code when failed |
| `http_status` | HTTP status when applicable |

Defined on `IntegrationMetricDimensions`.

## Platform events (`ontogony.event.*`)

| Tag | Description |
| --- | --- |
| `event_type` | Envelope event type |
| `protocol` | Protocol discriminator |
| `operation_mode` | Publish/dispatch mode |
| `handler_description` | Handler label (duration histogram) |

## Product metrics

Product repos add domain tags **only** on their own meters (for example `provider_id`, `model_alias`). Keep cardinality bounded — no unbounded user ids or raw model names in labels unless hashed.

## Conformance

Consumer tests should freeze at least one counter’s logical name and Prometheus export name per service. See `ObservabilityNamingConformanceAssertions` in `Ontogony.Testing.Conformance`.
