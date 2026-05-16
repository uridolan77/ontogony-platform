# Ontogony.Http

Resilient **outbound HTTP** client registration: retries, circuit breaker, and service-to-service integration conventions.

## What this is

- `OntogonyIntegrationHeaders` — canonical cross-service header names.
- `AddOntogonyIntegrationHttpClient` / typed `AddOntogonyIntegrationHttpClient<TClient,TImplementation>` — named clients with correlation, actor, and idempotency propagation.
- `IntegrationHeadersDelegatingHandler`, `TransportResilienceOptions`, `ResilientIntegrationDelegatingHandler`, and `IntegrationHttpError`.

## What this is not

- Not inbound API design, authentication policy, or product clients (`KanonClient`, `AllagmaClient`, `ConexusClient`).

## See also

- `docs/packages/Ontogony.Http.md`
- `docs/adoption/service-to-service-integration.md`
