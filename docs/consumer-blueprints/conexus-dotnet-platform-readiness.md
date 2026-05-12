# Conexus.NET — platform readiness (Ontogony)

This blueprint defines the **minimal Ontogony package set** for a first **Conexus.NET** service on .NET, and a **reference request path** for an OpenAI-style chat completions endpoint. It is planning and inventory only: **no routing, pricing, or provider policy** belongs in Ontogony.Platform.

## Goals

- Use Ontogony for **mechanical** cross-cutting behavior: tracing, errors, HTTP resilience, security context, idempotency, envelopes, LLM telemetry, large payloads by reference, and execution facts.
- Keep **all gateway semantics** (model routing, quotas, safety policy, catalog) in the Conexus.NET repository.

## Required packages (Conexus.NET v1)

These are the packages Conexus.NET should reference directly for the baseline gateway described below. They align with levels 1–3 in [`docs/architecture/package-levels.md`](../architecture/package-levels.md).

| Package | Role in Conexus |
| --- | --- |
| `Ontogony.Hosting` | Service defaults, middleware ordering, health endpoints |
| `Ontogony.Observability` | Trace/correlation, request tracing middleware |
| `Ontogony.Errors` | Exception to HTTP problem mapping |
| `Ontogony.Http` | Resilient outbound `HttpClient` to provider endpoints |
| `Ontogony.Security` | Service identity / actor context primitives (e.g. API key or signed service flows) |
| `Ontogony.Idempotency` | Idempotency keys and ledger for safe retries |
| `Ontogony.Hashing` | Canonical JSON / payload fingerprints for idempotency and envelopes |
| `Ontogony.Contracts` | `OntogonyEnvelope<T>`, headers, protocol-neutral event surface |
| `Ontogony.AI.Contracts` | `LlmRequestEnvelope`, `LlmResponseEnvelope`, `LlmProviderError`, usage/cost records |
| `Ontogony.Artifacts` | `ArtifactRef` / `IArtifactStore` for large raw payloads by reference |
| `Ontogony.Execution` | `IExecutionJournal` / records for internal run/step facts (no workflow engine) |

**Transitive packages:** `Ontogony.Primitives` (clock, IDs) and `Ontogony.Configuration` (validated options helpers) are pulled in by the graph where referenced; add an **explicit** `PackageReference` to `Ontogony.Configuration` only if Conexus uses `AddValidatedOptions` / `EnvironmentGuard` directly.

## Optional later

Add when the gateway grows beyond “call provider + record telemetry”:

| Package | When |
| --- | --- |
| `Ontogony.Messaging` | In-process publish/dispatch of integration events |
| `Ontogony.Persistence` | Outbox / processed-message contracts (without Postgres) |
| `Ontogony.Persistence.Postgres` | Durable transactional outbox in PostgreSQL |
| `Ontogony.ProtocolIngress` | Normalize external protocols (CloudEvents, MCP, etc.) into envelopes |
| `Ontogony.Testing` | Conformance kits and fakes in Conexus.NET tests |

## Proposed request flow (reference)

Mechanical ordering only; Conexus.NET owns handler names, DI, and policies.

```text
POST /v1/chat/completions
  → request tracing (Ontogony.Observability)
  → project API key auth (Conexus domain + Ontogony.Security primitives as needed)
  → idempotency / fingerprint (Ontogony.Idempotency + Ontogony.Hashing)
  → route resolution (Conexus domain only — not in Ontogony.Platform)
  → provider call via Microsoft.Extensions.AI / IChatClient (Conexus + BCL)
  → record LlmRequestEnvelope (Ontogony.AI.Contracts)
  → LlmResponseEnvelope or LlmProviderError
  → usage / cost record (Ontogony.AI.Contracts)
  → optional ArtifactRef for raw payload (Ontogony.Artifacts)
  → optional ExecutionRunRecord (or related journal lines) for internal trace (Ontogony.Execution)
```

## Boundaries

- **Ontogony.Platform** must not encode which model, which provider, or how to charge: only DTOs and ports for recording and infrastructure.
- Conexus.NET implements **routing policy**, **rate limits**, and **content policy** using its own modules; it may *populate* Ontogony DTO fields from those decisions.

## Related documentation

- [`docs/FRAMEWORK_BASELINE.md`](../FRAMEWORK_BASELINE.md) — SDK, TFM, and central package versions for the same baseline as this repo.
- [`docs/architecture/package-levels.md`](../architecture/package-levels.md) — allowed dependencies between packages.
- [`docs/adoption/conexus-platform-adoption.md`](../adoption/conexus-platform-adoption.md) — broader Conexus adoption notes (polyglot / correlation).
