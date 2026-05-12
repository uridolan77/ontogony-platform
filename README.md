# Ontogony Platform

**Ontogony.Platform** is the mechanical infrastructure base for new Ontogony services. It is safe to break before v1 because no external consumers exist. The first target consumer is **Conexus.NET**.

This repository is intentionally **not** a domain framework. It contains reusable mechanics only:

- trace/correlation propagation
- typed event envelopes and protocol-neutral DTOs
- error contracts and exception middleware
- configuration/startup guards
- resilient outbound HTTP clients (correlation, backoff, **Retry-After**, **jitter**, circuit breaking — richer policies still evolve; see [`docs/packages/Ontogony.Http.md`](docs/packages/Ontogony.Http.md))
- hashing and canonical JSON fingerprints
- idempotency primitives
- event publishing abstractions
- persistence/outbox/processed-message contracts and PostgreSQL providers
- security/current-actor context primitives
- LLM telemetry DTOs, artifact references, execution journal facts
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

## Do not add product semantics

Keep this repository free of product meaning: no canonization, no agent plans, no provider routing policy, no business approval rules, no RAG/graph extraction logic, no UI. If a change requires understanding those domains, it belongs in a product repo. See [`AGENTS.md`](AGENTS.md).

## Conexus.NET starter target

The platform is shaped so **Conexus.NET** can adopt it as the default substrate: observability, errors, HTTP, security, idempotency, contracts, AI telemetry, artifacts, and execution journaling — without importing Ontogony-specific business rules. Optional packages (messaging, persistence, Postgres outbox, protocol ingress, testing) layer on when a service needs them. See [`docs/consumer-blueprints/conexus-dotnet-platform-readiness.md`](docs/consumer-blueprints/conexus-dotnet-platform-readiness.md) for the minimal package list and reference request flow.

## Current finalized package layers

Documentation levels (including **0.5 — shared representation**) describe how packages group; **allowed `ProjectReference` edges** are defined and checked in [`docs/architecture/package-levels.md`](docs/architecture/package-levels.md) and [`scripts/validate-package-levels.ps1`](scripts/validate-package-levels.ps1). Levels are a mental model, not a strict topological sort — the matrix in that doc is authoritative.

```text
Level 0 — Pure foundation
  Ontogony.Primitives, Ontogony.Configuration

Level 0.5 — Shared representation
  Ontogony.Contracts, Ontogony.Hashing

Level 1 — Service mechanics
  Ontogony.Hosting, Ontogony.Observability, Ontogony.Errors, Ontogony.Http, Ontogony.Security

Level 2 — Event, consistency, persistence mechanics
  Ontogony.Messaging, Ontogony.Idempotency, Ontogony.Persistence,
  Ontogony.Persistence.Postgres, Ontogony.ProtocolIngress

Level 3 — AI runtime mechanics
  Ontogony.AI.Contracts, Ontogony.Artifacts, Ontogony.Execution
```

**Aggregate (not a runtime tier):** `Ontogony.Testing` (references many packages for fixtures).

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
├── src/                          # 18 shipping library packages (see tree below)
├── tests/
├── docs/
│   └── architecture/             # package levels and dependency rules
├── schemas/
├── examples/
├── scripts/
└── .github/workflows/
```

### `src/` packages (18)

```text
src/
├── Ontogony.Primitives/
├── Ontogony.Configuration/
├── Ontogony.Hashing/
├── Ontogony.Contracts/
├── Ontogony.Observability/
├── Ontogony.Errors/
├── Ontogony.Http/
├── Ontogony.Hosting/
├── Ontogony.Security/
├── Ontogony.Idempotency/
├── Ontogony.Messaging/
├── Ontogony.Persistence/
├── Ontogony.Persistence.Postgres/
├── Ontogony.ProtocolIngress/
├── Ontogony.AI.Contracts/
├── Ontogony.Artifacts/
├── Ontogony.Execution/
└── Ontogony.Testing/
```

## Examples

- `examples/MinimalApiWithOntogonyObservability/`: minimal API sample for `AddOntogonyObservability`, `UseOntogonyRequestTracing`, and outbound correlation propagation through `Ontogony.Http`.
- `examples/MinimalApiWithOntogonyHosting/`: minimal API sample for `AddOntogonyServiceDefaults`, `UseOntogonyServiceDefaults`, and `MapOntogonyHealthEndpoints`.
- `examples/ConexusDotNetSkeleton/`: compile-only smoke with **direct** project references to every required Conexus v1 package (see [readiness blueprint](docs/consumer-blueprints/conexus-dotnet-platform-readiness.md)); not a product.

## Documentation map

- [`docs/00_START_HERE.md`](docs/00_START_HERE.md) — mental model and documentation index.
- [`docs/architecture/package-levels.md`](docs/architecture/package-levels.md) — package layers, dependency matrix, forbidden edges.
- [`docs/FRAMEWORK_BASELINE.md`](docs/FRAMEWORK_BASELINE.md) — SDK, target framework, central package versions, upgrade procedure.
- [`docs/consumer-blueprints/conexus-dotnet-platform-readiness.md`](docs/consumer-blueprints/conexus-dotnet-platform-readiness.md) — Conexus.NET minimal package set and reference request flow.
- [`docs/packages/`](docs/packages/) — per-package guarantees and non-goals.
- [`CHANGELOG.md`](CHANGELOG.md) — PR history, migrations, and breaking-change notes.

## Using this repository (starter substrate)

1. Read [`docs/00_START_HERE.md`](docs/00_START_HERE.md) and [`docs/architecture/package-levels.md`](docs/architecture/package-levels.md).
2. Add package references or project references for the mechanics your service needs (see layer map above).
3. Build and test locally (see **Build** below).
4. Publish to a private feed when you cut releases, or reference projects directly while iterating.

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

**Current shipping line:** `0.3.0-alpha.1` (set in [`Directory.Build.props`](Directory.Build.props) as `<Version>`; CI uses the same value for `PACKAGE_VERSION` when packing).

**Before 1.0** there are no stability guarantees for external consumers that do not exist yet:

- Breaking API and contract changes are allowed without pretending full SemVer stability.
- Migration notes in `docs/migrations/` are required when the change matters for **adopted** mechanics; unused internal experiments do not need a migration file.
- What matters pre-1.0 is a coherent **package shape** and honest docs.

**After Conexus.NET starts consuming this stack** (informal roadmap):

- `0.4.x` — Conexus.NET incubation baseline while the gateway is built against the platform.
- `1.0.0` — reserved for when a shipped Conexus.NET (or successor) uses the core packages end-to-end in production and smoke-validates telemetry, artifacts, and execution recording.

Use SemVer syntax for NuGet; treat **0.x** and **pre-release** tags as “evolving substrate,” not locked LTS.

## Current status

**Shared infrastructure (0.3.0-alpha.1)** — contracts, reference implementations, docs, and automated tests (see `CHANGELOG.md`). CI restores, builds, tests, validates docs, **shipping inventory**, **AI runtime docs**, **package dependency levels**, and packs **18** libraries on **.NET 9** (see `.github/workflows/`). A non-shipping [`examples/ConexusDotNetSkeleton/`](examples/ConexusDotNetSkeleton/) project compiles against the Conexus v1 package slice.

**Still evolving (check `docs/migrations/` before upgrading):**

```text
HTTP resilience          // Retry-After, jitter, backoff, and circuit breaking exist; richer policies and budgets still evolve — see docs/packages/Ontogony.Http.md
Envelope rules           // mechanical validation + JSON schema; product ingest policies stay in product repos
Public XML (CS1591)      // suppressed at solution level until core types are fully documented
```
