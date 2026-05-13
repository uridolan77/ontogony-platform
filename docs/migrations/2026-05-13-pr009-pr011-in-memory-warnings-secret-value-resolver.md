# PR-PLAT-009 / PR-PLAT-011 — in-memory startup warnings and secret-value resolver

## PR-PLAT-009

`AddOntogonyInMemoryArtifactStore`, `AddOntogonyInMemoryExecutionJournal`, `AddOntogonyInMemoryQuotaLedger`, and `AddOntogonyInMemoryOutboxStore` each register an `IHostedService` that logs a **single warning** on host start when `IHostEnvironment.IsDevelopment()` is **false**. **Staging** and **Production** are treated the same (anything outside Development).

Each warning includes the mechanism name and **explicit guidance** toward a durable replacement (`IArtifactStore`, `IExecutionJournal`, `IQuotaLedger`, Postgres outbox / durable outbox contracts).

No exception is thrown; services that need a hard fail should validate configuration separately.

## PR-PLAT-011

Mechanical secret **value** resolution (distinct from `SecretRef` display metadata):

- **`ISecretValueResolver`** — `TryResolveAsync(SecretValueReference, CancellationToken)`.
- **`EnvironmentVariableSecretValueResolver`** — handles scheme `env` (case-insensitive); locator is the environment variable name. Register via `AddOntogonyEnvironmentSecretValueResolver()` (optional; not part of `AddOntogonySecrets()` by default).
- **`CompositeSecretValueResolver`** — ordered chain until one resolver returns `IsResolved: true`.

**Consumers (e.g. Conexus):** compose additional `ISecretValueResolver` implementations for vault or provider-specific schemes; avoid logging resolved secret material.

`SecretValueResolveResult` is a **readonly struct** with a **safe `ToString()`** (resolved values are redacted). `CompositeSecretValueResolver` uses **constructor order**: first successful resolver wins; if none resolve, the composite returns `UnresolvedReason` **`unresolved`** (individual resolver reasons are not aggregated).

## Repos

- **Ontogony.Platform:** source of truth for types and defaults above.
- **Conexus.NET:** adopt `AddOntogonyEnvironmentSecretValueResolver()` and/or custom resolvers when wiring provider API keys from configuration.
