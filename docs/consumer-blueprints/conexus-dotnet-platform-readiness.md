# Conexus.NET — platform readiness (Ontogony)

This blueprint defines the **minimal Ontogony package set** for a first **Conexus.NET** service on .NET, and a **reference request path** for an OpenAI-style chat completions endpoint. It is planning and inventory only: **no routing, pricing, or provider policy** belongs in Ontogony.Platform.

**Current platform package line:** `0.3.0-alpha.1` (see [`docs/FRAMEWORK_BASELINE.md`](../FRAMEWORK_BASELINE.md)). Pre-starter lock and recorded validation checklist: [`conexus-dotnet-starter-plan.md`](conexus-dotnet-starter-plan.md) (PR54).

## Goals

- Use Ontogony for **mechanical** cross-cutting behavior: tracing, structured logging scopes, redaction-aware log fields, secret references and fingerprints, mechanical quotas, errors, HTTP resilience, security context, idempotency, envelopes, LLM telemetry, large payloads by reference, and execution facts.
- Keep **all gateway semantics** (model routing, product quota policy, safety policy, catalog) in the Conexus.NET repository.

## Required packages (Conexus.NET v1)

These are the packages Conexus.NET should reference directly for the baseline gateway described below. They span **shared representation** (Contracts, Hashing), **service mechanics** (Hosting through Security, plus Logging / Redaction / Secrets), **mechanical quotas**, **idempotency**, and **AI runtime** tiers as described in [`docs/architecture/package-levels.md`](../architecture/package-levels.md); that document’s matrix is the authority on allowed `ProjectReference` edges.

| Package | Role in Conexus |
| --- | --- |
| `Ontogony.Hosting` | Service defaults, middleware ordering, health endpoints |
| `Ontogony.Observability` | Trace/correlation, request tracing middleware |
| `Ontogony.Logging` | Stable log fields, correlation scopes, request logging-scope middleware (pair with `IRedactor` from `Ontogony.Redaction` for sensitive `additionalFields`) |
| `Ontogony.Redaction` | Deterministic field-name redaction for logs, errors, and metadata maps |
| `Ontogony.Secrets` | Secret references, masking, fingerprints, dev-only protector (no cloud vault SDK) |
| `Ontogony.Quotas` | Mechanical quota windows/decisions and in-memory ledger for tests/single-process hosts (product owns plan tiers and durable enforcement) |
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

Add when the gateway grows beyond “call provider + record telemetry”, or when you implement deterministic replay/debug bundles:

| Package | When |
| --- | --- |
| `Ontogony.Replay.Contracts` | Shared DTOs for replay manifests and bundles (no replay engine in platform) |
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
  → structured logging scope (Ontogony.Logging; register Ontogony.Redaction so scope fields are redacted when matched)
  → project API key auth (Conexus domain + Ontogony.Security primitives as needed)
  → mechanical quota check (Ontogony.Quotas — policy and limits owned by Conexus)
  → idempotency / fingerprint (Ontogony.Idempotency + Ontogony.Hashing)
  → route resolution (Conexus domain only — not in Ontogony.Platform)
  → provider call via Microsoft.Extensions.AI / IChatClient (Conexus + BCL)
  → record LlmRequestEnvelope (Ontogony.AI.Contracts)
  → LlmResponseEnvelope or LlmProviderError
  → usage / cost record (Ontogony.AI.Contracts)
  → optional ArtifactRef for raw payload (Ontogony.Artifacts)
  → optional ExecutionRunRecord (or related journal lines) for internal trace (Ontogony.Execution)
```

### Recommended ASP.NET middleware order (Ontogony)

When composing middleware yourself (instead of only `UseOntogonyServiceDefaults`), use:

```text
UseOntogonyRequestTracing()
UseOntogonyLoggingScope()      // after tracing populates correlation context
UseOntogonyExceptionHandling()
```

`UseOntogonyServiceDefaults` today wires tracing then exception handling; **add** `UseOntogonyLoggingScope()` between those two when you want request scopes. Register `AddOntogonyRedaction()` (or `AddOntogonySecrets()`) so `IRedactor` is available to the logging middleware.

## Boundaries

- **Ontogony.Platform** must not encode which model, which provider, or how to charge: only DTOs and ports for recording and infrastructure.
- Conexus.NET implements **routing policy**, **product quota tiers**, and **content policy** using its own modules; it may *populate* Ontogony DTO fields from those decisions.

## Related documentation

- [`docs/FRAMEWORK_BASELINE.md`](../FRAMEWORK_BASELINE.md) — SDK, TFM, and central package versions for the same baseline as this repo.
- [`docs/architecture/package-levels.md`](../architecture/package-levels.md) — allowed dependencies between packages.
- [`docs/adoption/conexus-platform-adoption.md`](../adoption/conexus-platform-adoption.md) — broader Conexus adoption notes (polyglot / correlation).
