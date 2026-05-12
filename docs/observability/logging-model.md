# Structured logging model

`Ontogony.Logging` defines a provider-neutral structured logging model over `Microsoft.Extensions.Logging`.

## Principles

- Logs are operational evidence, not product truth.
- Every service log should be correlateable by `trace_id` and `operation_id`.
- Raw prompts, responses, API keys, headers, and credentials must be redacted before logging.
- The platform defines fields and event IDs; product services define their operation names and policy meaning.

## Required baseline fields

- `trace_id`
- `operation_id`
- `service_name`
- `service_version`
- `environment`
- `operation`
- `component`
- `outcome`
- `error_code`
- `duration_ms`

## AI/gateway fields

- `request_id`
- `provider`
- `model`
- `model_alias`
- `route_id`
- `attempt`
- `fallback_used`
- `input_tokens`
- `output_tokens`
- `total_tokens`
- `cost_amount`
- `cost_currency`
- `artifact_id`
- `idempotency_key`
