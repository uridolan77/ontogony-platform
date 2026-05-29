# Consumer conformance suite v1

## Purpose

Provide a uniform harness that product repos can run to prove they obey shared mechanical contracts.

## Conformance dimensions

1. Header propagation.
2. Error envelope.
3. Idempotency.
4. Outbox/artifact mechanics.
5. Observability meter naming.
6. No-product-semantics leakage.
7. Actor context propagation.
8. Mechanical schema compatibility.

## Modes

| Mode | Description |
|---|---|
| Static | Reads source/docs/config only |
| Fixture | Executes local test fixtures, no services |
| LocalService | Calls local service endpoints |
| LiveStack | Optional, only with explicit operator env |

## Required output

Every run writes `summary.json` matching `consumer-conformance-report.schema.json`.

## Status semantics

- `PASS`: all required checks passed.
- `PARTIAL`: some checks unavailable but no violation found.
- `FAIL`: at least one required check violated.
- `NOT_RUN`: repo/config unavailable.
