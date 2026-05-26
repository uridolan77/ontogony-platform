# Migration — PLAT-DEPTH HTTP resilience completion (2026-05-26)

## Summary

Completes HTTP resilience v1 mechanics: exponential backoff option, `RetryBypassingBudget` wiring, `IRetryClassifierV2` + `RetryExceptionContext`, `ICircuitBreakerRegistry`, and shared `TransportResilienceBackoff`.

## Additive API

| Type | Notes |
| --- | --- |
| `BackoffPolicy` | `Linear` (default) or `Exponential` on `TransportResilienceOptions` |
| `TransportResilienceBackoff` | Public delay calculator for harnesses |
| `RetryExceptionContext` | Timeout/attempt metadata for classifiers |
| `IRetryClassifierV2` | Context-aware classifier; `DefaultRetryClassifier` implements it |
| `ICircuitBreakerRegistry` | Implemented by `TransportResilienceRegistry` |

## Behavior fixes

- `RetryBypassingBudget` now retries without consuming `RetryBudgetPerMinute`.
- `DefaultRetryClassifier` does not retry on caller cancellation or total-timeout exhaustion when using V2 context.

## Consumer action

- Custom `IRetryClassifier` implementations are unchanged.
- Optional: implement `IRetryClassifierV2` for attempt/total timeout-aware retry rules.
- Optional: set `BackoffPolicy = Exponential` where longer tails are desired (default remains linear).

## Repos

Conexus, Kanon, and Allagma consumers of `Ontogony.Http` — no lockstep required; changes are backward compatible.
