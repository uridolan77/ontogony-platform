# Mechanical schema registry v1

## Scope

This registry contains neutral cross-service schema contracts only.

## Schemas

| Schema | Purpose | Product-neutral? |
|---|---|---:|
| error-envelope | Problem-details/error envelope shape | Yes |
| correlation-headers | Trace/correlation/header context | Yes |
| evidence-reference | Reference to evidence without interpreting it | Yes |
| idempotency-state | Ledger state and replay-safe response mechanics | Yes |
| replay-contract | Replay request/manifest mechanics, not replay engine | Yes |
| actor-context | Actor identity propagation | Yes |
| observability-meter | Meter naming descriptor | Yes |
| consumer-conformance-report | Harness output | Yes |

## Versioning

- Schemas live under `schemas/mechanics/v1/`.
- Breaking changes require `v2`.
- Additive optional fields may remain in v1.
- Required-field changes are breaking.
- Product-specific enums are forbidden.
