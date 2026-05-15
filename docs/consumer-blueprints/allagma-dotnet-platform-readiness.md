# Allagma.NET — platform readiness (Ontogony)

This blueprint defines the **minimal Ontogony package set** for a first **Allagma.NET** governed-execution service on .NET, and a **reference integration path** for outbound calls to Kanon and Conexus over HTTP. It is planning and inventory only: **no workflow engine, tool-intent model, human-gate policy, or semantic planning** belongs in Ontogony.Platform.

**Current platform package line:** `0.3.0-alpha.1` (see [`docs/FRAMEWORK_BASELINE.md`](../FRAMEWORK_BASELINE.md)). Pre-starter lock and recorded validation checklist: [`allagma-dotnet-starter-plan.md`](allagma-dotnet-starter-plan.md) (PLAT-ALLAGMA-001).

## Strategic rule

```text
Kanon owns meaning.
Allagma owns governed execution.
Conexus owns model access.
Ontogony.Platform owns mechanics.
```

Allagma is a **consumer** of Ontogony mechanics. The platform does not own Allagma runtime semantics, agent plans, or Kanon/Conexus product clients.

## Goals

- Use Ontogony for **mechanical** cross-cutting behavior: tracing, structured logging scopes, redaction-aware log fields, secret references, errors, HTTP resilience, security/actor context, idempotency, execution journal facts, artifact references, replay DTOs, and optional durable outbox when needed.
- Keep **all governed execution semantics** (plans, steps, tool intents, human gates, workflow policy) in the Allagma.NET repository.
- Call **Kanon** and **Conexus** only over HTTP (or future explicit integration contract packages) — never via product `ProjectReference` from platform or from this skeleton.

## Required packages (Allagma.NET v1)

These are the packages Allagma.NET should reference directly for the baseline execution host described below. Allowed `ProjectReference` edges between platform packages are defined in [`docs/architecture/package-levels.md`](../architecture/package-levels.md).

| Package | Role in Allagma |
| --- | --- |
| `Ontogony.Primitives` | Clock, identifiers, shared primitives (add explicit reference when publishing envelopes or stable IDs in your public surface) |
| `Ontogony.Hosting` | Service defaults, middleware ordering, health endpoints |
| `Ontogony.Observability` | Trace/correlation, request tracing, **integration metrics** (`IIntegrationOperationMeter`) |
| `Ontogony.Logging` | Stable log fields, correlation scopes, request logging-scope middleware |
| `Ontogony.Redaction` | Deterministic field-name redaction for logs, errors, and metadata maps |
| `Ontogony.Secrets` | Secret references, masking, fingerprints (no cloud vault SDK in platform) |
| `Ontogony.Errors` | Exception to HTTP problem mapping |
| `Ontogony.Http` | Resilient outbound `HttpClient` to Kanon, Conexus, and other integrations |
| `Ontogony.Security` | Service identity and current-actor context primitives |
| `Ontogony.Idempotency` | Idempotency keys and ledger for safe retries on outbound mutations |
| `Ontogony.Execution` | `IExecutionJournal` / records for run/step facts (**no workflow engine**) |
| `Ontogony.Artifacts` | `ArtifactRef` / `IArtifactStore` for large payloads by reference |
| `Ontogony.AI.Contracts` | Mechanical LLM telemetry DTOs when recording Conexus-assisted calls (no routing) |
| `Ontogony.Replay.Contracts` | Shared replay manifest DTOs (no replay engine in platform) |
| `Ontogony.Persistence` | Outbox / processed-message contracts (in-memory reference for dev/tests) |
| `Ontogony.Persistence.Postgres` | Durable transactional outbox when Allagma needs Postgres |
| `Ontogony.Testing` | Architecture-test helpers and conformance kits in Allagma.NET **test** projects |

**Transitive packages:** `Ontogony.Contracts`, `Ontogony.Hashing`, and `Ontogony.Configuration` are pulled by the graph where needed; add **explicit** references only when your repo publishes or fingerprints through those APIs directly.

## Optional later

Add when the execution host grows beyond “record facts + call Kanon/Conexus over HTTP”:

| Package | When |
| --- | --- |
| `Ontogony.Messaging` | In-process publish/dispatch of integration events |
| `Ontogony.ProtocolIngress` | Normalize external protocols into envelopes |
| `Ontogony.Quotas` | Mechanical quota windows (product owns plan tiers and enforcement policy) |

## Proposed integration flow (reference)

Mechanical ordering only; Allagma.NET owns handler names, DI, and policies.

```text
POST /v1/runs (Allagma product API — not in Ontogony)
  → request tracing (Ontogony.Observability)
  → structured logging scope (Ontogony.Logging + Ontogony.Redaction)
  → actor / service identity (Ontogony.Security)
  → idempotency / fingerprint on outbound mutations (Ontogony.Idempotency)
  → compile semantic plan (HTTP → Kanon — Allagma product)
  → evaluate action before consequential step (HTTP → Kanon — Allagma product)
  → optional model assist (HTTP → Conexus — Allagma product; record Ontogony.AI.Contracts telemetry)
  → append execution journal lines (Ontogony.Execution)
  → optional ArtifactRef for large payloads (Ontogony.Artifacts)
  → optional outbox event (Ontogony.Persistence / Postgres)
```

### Recommended ASP.NET middleware order (Ontogony)

```text
UseOntogonyRequestTracing()
UseOntogonyLoggingScope()
UseOntogonyExceptionHandling()
```

Register `AddOntogonyRedaction()` (or `AddOntogonySecrets()`) so `IRedactor` is available to the logging middleware.

## Platform boundaries (non-negotiable)

Ontogony.Platform **must not** include:

| Forbidden in platform | Belongs in |
| --- | --- |
| Microsoft Agent Framework | Allagma.NET (if adopted) |
| OpenAI / Anthropic / Azure.AI.OpenAI / Google AI SDKs | Conexus.NET |
| Kanon or Conexus product assemblies | respective product repos |
| Workflow execution semantics | Allagma.NET |
| Tool-intent / plan semantics | Allagma.NET |
| Human-gate policy rules | Kanon (meaning); Allagma orchestrates pauses only |
| Semantic planning / model routing | Kanon / Conexus |

## Related documentation

- [`docs/FRAMEWORK_BASELINE.md`](../FRAMEWORK_BASELINE.md) — SDK, TFM, and central package versions.
- [`docs/architecture/package-levels.md`](../architecture/package-levels.md) — allowed dependencies between packages.
- [`docs/adoption/architecture-tests-adoption.md`](../adoption/architecture-tests-adoption.md) — forbidden dependency scans with `Ontogony.Testing`.
- [`docs/adoption/integration-metrics-adoption.md`](../adoption/integration-metrics-adoption.md) — outbound integration metrics.
- [`ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md) — NuGet package-mode contract (when `allagma-dotnet` exists).
