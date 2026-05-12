# Ontogony Platform

Shared infrastructure building blocks for the Ontogony service ecosystem: **Athanor**, **Agentor**, **Conexus**, protocol recorders, and future microservices.

This repository is intentionally **not** a domain framework. It contains reusable mechanics only:

- trace/correlation propagation
- typed event envelopes
- error contracts and exception middleware
- configuration/startup guards
- resilient integration HTTP clients
- hashing and canonical JSON fingerprints
- idempotency primitives
- event publishing abstractions
- security/current-actor context primitives
- test fixtures

It must not contain Athanor canonization logic, Agentor orchestration semantics, Conexus routing strategy, iGaming rules, or any product-specific workflow logic.

## Strategic rule

```text
Share mechanics. Do not share meaning.
```

Good shared code:

```text
How do we trace?
How do we propagate actor/tenant context?
How do we hash an event payload?
How do we publish an integration event?
How do we format errors?
How do we validate dangerous production config?
```

Bad shared code:

```text
What is canonical truth?
What counts as a valid decision?
What is the correct agent plan?
Which LLM provider should route a given request?
What is a valid business approval?
```

## What this repository extracts from the product repos

| Source repo | What we take | What we do not take |
| --- | --- | --- |
| Agentor | request tracing, correlation context, Activity/Meter pattern, fake/http/disabled integration modes, resilience, idempotency, outbox/queue discipline, auth-mode discipline | agent runtime semantics, policy business rules, plan execution domain |
| Athanor | evidence/provenance discipline, append-only invariants, trace ID discipline, startup guards, content hashing, DB-first caution, Postgres-canonical/graph-projection principle | canonization services, contradiction logic, snapshot semantics as shared domain |
| Conexus | gateway operational posture: request IDs, project API keys, usage/cost logging, readiness, encrypted config, production hardening | Python code, provider routing internals, BO-specific UI logic |
| KGB | historical lessons: chunk hashing, pipeline stages, human review, export formats | old infrastructure foundation |

## Repository layout

```text
.
├── src/
│   ├── Ontogony.Contracts/
│   ├── Ontogony.Observability/
│   ├── Ontogony.Configuration/
│   ├── Ontogony.Errors/
│   ├── Ontogony.Http/
│   ├── Ontogony.Hashing/
│   ├── Ontogony.Hosting/
│   ├── Ontogony.Idempotency/
│   ├── Ontogony.Messaging/
│   ├── Ontogony.Security/
│   ├── Ontogony.Persistence/
│   ├── Ontogony.Persistence.Postgres/
│   └── Ontogony.Testing/
├── tests/
├── docs/
├── schemas/
├── examples/
├── scripts/
└── .github/workflows/
```

## Examples

- `examples/MinimalApiWithOntogonyObservability/`: minimal API sample for `AddOntogonyObservability`, `UseOntogonyRequestTracing`, and outbound correlation propagation through `Ontogony.Http`.
- `examples/MinimalApiWithOntogonyHosting/`: minimal API sample for `AddOntogonyServiceDefaults`, `UseOntogonyServiceDefaults`, and `MapOntogonyHealthEndpoints`.

## Documentation map

- [`docs/00_START_HERE.md`](docs/00_START_HERE.md) — mental model, extraction targets, adoption guide index.
- [`docs/packages/`](docs/packages/) — per-package guarantees, non-goals, and adoption posture.
- [`CHANGELOG.md`](CHANGELOG.md) — PR history, migrations, and breaking-change notes.

## Recommended first adoption path

1. Read [`docs/00_START_HERE.md`](docs/00_START_HERE.md) and the adoption hub for your service (`docs/adoption/athanor-platform-adoption.md`, `agentor-platform-adoption.md`, or `conexus-platform-adoption.md`).
2. Build and test the packages locally (see **Build** below).
3. Publish packages to a private/internal NuGet feed, or reference by project path initially.
4. Adopt **low-risk mechanics first** (Primitives, Hashing, Idempotency, Configuration), then **controlled** API integration (Observability, Http, Errors) with compatibility tests.
5. Add Conexus event emission through any runtime client that conforms to [`schemas/ontogony-envelope.schema.json`](schemas/ontogony-envelope.schema.json).

## First packages to adopt

**Low risk (start in Athanor first):**

```text
Ontogony.Primitives
Ontogony.Configuration
Ontogony.Hashing
Ontogony.Idempotency
Ontogony.Contracts
```

**Controlled (Agentor / Athanor API surfaces — keep mappings and semantics local):**

```text
Ontogony.Observability
Ontogony.Http
Ontogony.Errors
```

**Dev, tests, and reference mechanics (not distributed production messaging or DB outbox):**

```text
Ontogony.Messaging
Ontogony.Persistence   // contracts + in-memory outbox reference; no Postgres implementation here
Ontogony.Persistence.Postgres // durable PostgreSQL outbox provider package
Ontogony.Security      // HMAC service identity + static shared-secret mode — requires correct wiring
Ontogony.Testing
```

## Build

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
```

If the solution file needs to be regenerated:

```powershell
pwsh ./scripts/bootstrap-solution.ps1
```

## Versioning

Use SemVer, but be conservative:

- `0.x`: still evolving, breaking changes allowed with migration notes.
- `1.0`: only after Agentor and Athanor both consume the core observability/contracts packages.

## Current status

**Shared infrastructure (0.x alpha)** — suitable for **selective, careful adoption** in Athanor, Agentor, and Conexus. This is no longer a throwaway starter; it ships contracts, reference implementations, semantic docs, and a broad automated test suite (see `CHANGELOG.md`).

**Ready for early production-style use (with integration tests):**

```text
Ontogony.Primitives
Ontogony.Configuration
Ontogony.Hashing
Ontogony.Idempotency
Ontogony.Contracts
Ontogony.Observability
Ontogony.Http
Ontogony.Errors
```

**Available but treat as infrastructure building blocks, not turnkey products:**

```text
Ontogony.Messaging        // in-process publisher, explicit publish/dispatch results, metrics; not Kafka/NATS/Event Hubs
Ontogony.Persistence      // SQL-agnostic outbox contracts + in-memory reference store + dead-letter hooks; no Postgres outbox here
Ontogony.Security         // HMAC service-identity verification + static shared-secret mode; requires IServiceSecretResolver, INonceReplayStore, clock skew policy
Ontogony.Persistence.Postgres // PostgreSQL durable outbox provider with claim-lease semantics
```

**Still evolving (check `docs/migrations/` before upgrading):**

```text
HTTP resilience          // linear backoff; Retry-After / jitter not implemented — see docs/packages/Ontogony.Http.md
HTTP resilience          // supports Retry-After and jitter; still evolving around richer policies, metrics, and retry budgets
Envelope rules           // mechanical validation + JSON schema; product ingest policies stay in product repos
Public XML (CS1591)      // suppressed at solution level until core types are fully documented
```

CI restores, builds, and tests on **.NET 9** (see `.github/workflows/`).
