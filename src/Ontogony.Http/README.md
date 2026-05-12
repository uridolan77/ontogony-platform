# Ontogony.Http

Resilient **outbound HTTP** client registration: retries, circuit breaker, correlation propagation.

## What this is

- `AddOntogonyIntegrationHttpClient` and related types wiring `Ontogony.Observability` correlation into `HttpClient` pipelines.
- `TransportResilienceOptions`, `ResilientIntegrationDelegatingHandler`, and classification hooks.

## What this is not

- Not inbound API design, authentication policy, or integration-specific DTO clients (those live in product code).

## See also

- `docs/packages/Ontogony.Http.md`.
