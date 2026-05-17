# Ontogony Platform

**Ontogony.Platform** is the mechanical infrastructure base for new Ontogony services.

Ontogony.Platform is pre-1.0 and still evolving, with **Conexus.NET** and **Allagma.NET** as active alpha consumers. Breaking changes are allowed only with public API snapshot review, `CHANGELOG` notes, migration guidance when applicable, and consumer compatibility validation.

This repository is intentionally **not** a domain framework. It contains reusable mechanics only:

- trace/correlation propagation
- typed event envelopes and protocol-neutral DTOs
- error contracts and exception middleware
- configuration/startup guards
- resilient outbound HTTP clients (correlation, backoff, **Retry-After**, **jitter**, circuit breaking — richer policies still evolve; see [`docs/packages/Ontogony.Http.md`](docs/packages/Ontogony.Http.md))
- hashing and canonical JSON fingerprints
- idempotency primitives
- structured logging fields, redaction, secret-reference mechanics, mechanical quotas, replay contracts (no replay engine)
- event publishing abstractions
- persistence/outbox/processed-message contracts and PostgreSQL providers
- security/current-actor context primitives
- LLM telemetry DTOs, artifact references, execution journal facts
- test fixtures

It must not contain Kanon semantic authority logic, Allagma orchestration semantics, Conexus routing strategy, iGaming rules, or any product-specific workflow logic.

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

## Active consumer (Conexus.NET, alpha)

**Conexus.NET** consumes this stack today in **sibling-source** and **NuGet package** modes as the default substrate: observability, errors, HTTP, security, idempotency, contracts, AI telemetry, artifacts, and execution journaling — without importing Ontogony-specific business rules. Optional packages (messaging, persistence, Postgres outbox, protocol ingress, testing) layer on when a service needs them.

## Active consumer (Allagma.NET, alpha)

**Allagma.NET** consumes this stack today for governed execution: observability, errors, HTTP, security, idempotency, execution journaling, and related mechanics — without importing Ontogony-specific business rules. The first cross-repo system path (Allagma → Kanon → Conexus fake-provider) is documented in the Allagma repo; package-mode and readiness blueprints below remain the upgrade/breaking-change contract.

## Consumer blueprint (Allagma.NET)

Historical **blueprint-only** wording referred to pre–first-system integration. Active runtime consumption is described above; see [`docs/consumer-blueprints/README.md`](docs/consumer-blueprints/README.md) and compile-only [`examples/AllagmaDotNetSkeleton/`](examples/AllagmaDotNetSkeleton/) smoke for the mechanical package slice.

Consumer alignment and governance (read before upgrading or breaking public surface area):

- [`docs/governance/README.md`](docs/governance/README.md) — Phase 1 release governance index (PLATFORM-GOV-001).
- [`docs/governance/PHASE1_CONSUMER_COMPATIBILITY.md`](docs/governance/PHASE1_CONSUMER_COMPATIBILITY.md) — Allagma, Kanon, Conexus compatibility on `0.3.0-alpha.1`.
- [`docs/governance/PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md`](docs/governance/PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md) — pre-tag / pre-merge checklist.
- [`docs/governance/NUGET_SOURCE_MAPPING.md`](docs/governance/NUGET_SOURCE_MAPPING.md) — repo `nuget.config` and consumer feed mapping.
- [`docs/consumer-blueprints/conexus-dotnet-platform-readiness.md`](docs/consumer-blueprints/conexus-dotnet-platform-readiness.md) — minimal package set and reference request flow.
- [`docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md) — package-mode contract and validation expectations.
- [`docs/consumer-blueprints/allagma-dotnet-platform-readiness.md`](docs/consumer-blueprints/allagma-dotnet-platform-readiness.md) — Allagma.NET minimal package set.
- [`docs/consumer-blueprints/ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](docs/consumer-blueprints/ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md) — Allagma package-mode contract.
- [`docs/VERSION_COMPATIBILITY_MATRIX.md`](docs/VERSION_COMPATIBILITY_MATRIX.md) — version compatibility expectations across packages.
- [`docs/public-api-review.md`](docs/public-api-review.md) — public API snapshot and review process.
- [`docs/planning/next-phase/architecture/PACKAGE_RELEASE_EVIDENCE.md`](docs/planning/next-phase/architecture/PACKAGE_RELEASE_EVIDENCE.md) — release evidence model for shipped packages.
- [`docs/security/PLAT-NP-003-supply-chain-first-run-evidence.md`](docs/security/PLAT-NP-003-supply-chain-first-run-evidence.md) — security and supply-chain workflow proof (CodeQL, dependency review, SBOM).

`PLAT-NP-008` remains an **intentionally open** maintenance guard for in-memory registration warning coverage when new public DI surfaces land; see [`docs/planning/next-phase/pr-specs/PR-PLAT-NP-008-in-memory-warning-coverage-expansion.md`](docs/planning/next-phase/pr-specs/PR-PLAT-NP-008-in-memory-warning-coverage-expansion.md).

## Current finalized package layers

Documentation levels (including **0.5 — shared representation**) describe how packages group; **allowed `ProjectReference` edges** are defined and checked in [`docs/architecture/package-levels.md`](docs/architecture/package-levels.md) and [`scripts/validate-package-levels.ps1`](scripts/validate-package-levels.ps1). Levels are a mental model, not a strict topological sort — the matrix in that doc is authoritative.

```text
Level 0 — Pure foundation
  Ontogony.Primitives, Ontogony.Configuration

Level 0.5 — Shared representation
  Ontogony.Contracts, Ontogony.Hashing

Level 1 — Service mechanics
  Ontogony.Hosting, Ontogony.Observability, Ontogony.Errors, Ontogony.Http, Ontogony.Security,
  Ontogony.Logging, Ontogony.Redaction, Ontogony.Secrets

Level 2 — Event, consistency, persistence mechanics
  Ontogony.Messaging, Ontogony.Idempotency, Ontogony.Persistence,
  Ontogony.Persistence.Postgres, Ontogony.ProtocolIngress, Ontogony.Quotas

Level 3 — AI runtime mechanics
  Ontogony.AI.Contracts, Ontogony.Artifacts, Ontogony.Execution, Ontogony.Replay.Contracts
```

**Aggregate (not a runtime tier):** `Ontogony.Testing` (references many packages for fixtures).

## What this repository extracts from the product repos

| Source repo | What we take | What we do not take |
| --- | --- | --- |
| Agentor (historical donor) | request tracing, correlation context, Activity/Meter pattern, fake/http/disabled integration modes, resilience, idempotency, outbox/queue discipline, auth-mode discipline | agent runtime semantics, policy business rules, plan execution domain |
| Athanor | evidence/provenance discipline, append-only invariants, trace ID discipline, startup guards, content hashing, DB-first caution, Postgres-canonical/graph-projection principle | canonization services, contradiction logic, snapshot semantics as shared domain |
| Conexus | gateway operational posture: request IDs, project API keys, usage/cost logging, readiness, encrypted config, production hardening | Python code, provider routing internals, BO-specific UI logic |
| KGB | historical lessons: chunk hashing, pipeline stages, human review, export formats | old infrastructure foundation |

## Repository layout

```text
.
├── src/                          # 23 shipping library packages (see tree below)
├── tests/
├── docs/
│   └── architecture/             # package levels and dependency rules
├── schemas/
├── examples/
├── scripts/
└── .github/workflows/
```

### `src/` packages (23)

```text
src/
├── Ontogony.Primitives/
├── Ontogony.Configuration/
├── Ontogony.Hashing/
├── Ontogony.Contracts/
├── Ontogony.Observability/
├── Ontogony.Logging/
├── Ontogony.Errors/
├── Ontogony.Http/
├── Ontogony.Hosting/
├── Ontogony.Security/
├── Ontogony.Redaction/
├── Ontogony.Secrets/
├── Ontogony.Idempotency/
├── Ontogony.Messaging/
├── Ontogony.Persistence/
├── Ontogony.Persistence.Postgres/
├── Ontogony.ProtocolIngress/
├── Ontogony.Quotas/
├── Ontogony.AI.Contracts/
├── Ontogony.Artifacts/
├── Ontogony.Execution/
├── Ontogony.Replay.Contracts/
└── Ontogony.Testing/
```

## Examples

- `examples/MinimalApiWithOntogonyObservability/`: minimal API sample for `AddOntogonyObservability`, `UseOntogonyRequestTracing`, and outbound correlation propagation through `Ontogony.Http`.
- `examples/MinimalApiWithOntogonyHosting/`: minimal API sample for `AddOntogonyServiceDefaults`, `UseOntogonyServiceDefaults`, and `MapOntogonyHealthEndpoints`.
- `examples/ConexusDotNetSkeleton/`: compile-only smoke with **direct** project references to the Conexus v1 required set including Logging, Redaction, Secrets, and Quotas (see [readiness blueprint](docs/consumer-blueprints/conexus-dotnet-platform-readiness.md)); wires tracing → logging scope → exception handling; not a product.
- `examples/AllagmaDotNetSkeleton/`: compile-only smoke with **direct** project references to the Allagma v1 required set (see [Allagma readiness blueprint](docs/consumer-blueprints/allagma-dotnet-platform-readiness.md)); integration metrics, actor context, idempotency, execution journal; not a product.

## Documentation map

- [`docs/00_START_HERE.md`](docs/00_START_HERE.md) — mental model and documentation index.
- [`docs/architecture/package-levels.md`](docs/architecture/package-levels.md) — package layers, dependency matrix, forbidden edges.
- [`docs/FRAMEWORK_BASELINE.md`](docs/FRAMEWORK_BASELINE.md) — SDK, target framework, central package versions, upgrade procedure.
- [`docs/consumer-blueprints/conexus-dotnet-platform-readiness.md`](docs/consumer-blueprints/conexus-dotnet-platform-readiness.md) — Conexus.NET minimal package set and reference request flow.
- [`docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md) — Conexus package-mode contract and proof expectations.
- [`docs/VERSION_COMPATIBILITY_MATRIX.md`](docs/VERSION_COMPATIBILITY_MATRIX.md) — cross-package compatibility expectations.
- [`docs/public-api-review.md`](docs/public-api-review.md) — public API governance and snapshot review.
- [`docs/consumer-blueprints/conexus-dotnet-starter-plan.md`](docs/consumer-blueprints/conexus-dotnet-starter-plan.md) — v0 substrate freeze and validation checkpoint before the external starter (PR54).
- [`docs/consumer-blueprints/README.md`](docs/consumer-blueprints/README.md) — consumer blueprint index (Conexus and Allagma active alpha consumers).
- [`docs/consumer-blueprints/allagma-dotnet-starter-plan.md`](docs/consumer-blueprints/allagma-dotnet-starter-plan.md) — Allagma v0 substrate checkpoint (PLAT-ALLAGMA-001).
- [`docs/packages/`](docs/packages/) — per-package guarantees and non-goals.
- [`CHANGELOG.md`](CHANGELOG.md) — PR history, migrations, and breaking-change notes.
- [`docs/backlog/PLATFORM_CLEANUP_TIGHTENING_STATUS.md`](docs/backlog/PLATFORM_CLEANUP_TIGHTENING_STATUS.md) — periodic substrate audit status, build proof, and recommended next PRs.
- [`docs/backlog/PLATFORM_EXTRACTION_CANDIDATES.md`](docs/backlog/PLATFORM_EXTRACTION_CANDIDATES.md) — mechanical extraction candidates under review (no product semantics).

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

**Before 1.0** the line is still **alpha**: breaking API and contract changes remain possible, but they must not be silent for **Conexus.NET** (see [ADR-PLAT-013](docs/adr/ADR-PLAT-013-active-consumer-versioning.md) and the links under **Active consumer** above).

- Breaking changes need public API snapshot review where applicable, `CHANGELOG.md` entries, and migration notes in `docs/migrations/` when consumer-visible behavior shifts.
- Conexus package-mode and baseline scripts should stay green across upgrades (see [`docs/planning/next-phase/pr-specs/PR-PLAT-NP-002-real-conexus-package-mode-compatibility.md`](docs/planning/next-phase/pr-specs/PR-PLAT-NP-002-real-conexus-package-mode-compatibility.md) and [`scripts/validate-conexus-consumer-baseline-alignment.ps1`](scripts/validate-conexus-consumer-baseline-alignment.ps1)).
- Unused internal experiments do not need a migration file; what matters pre-1.0 is a coherent **package shape** and honest docs.

**Informal roadmap** (subject to consumer validation):

- `0.4.x` — Conexus.NET incubation baseline while the gateway hardens against the platform.
- `1.0.0` — reserved for when a shipped Conexus.NET (or successor) uses the core packages end-to-end in production and smoke-validates telemetry, artifacts, and execution recording.

Use SemVer syntax for NuGet; treat **0.x** and **pre-release** tags as “evolving substrate,” not locked LTS.

## Current status

**Shared infrastructure (0.3.0-alpha.1)** — contracts, reference implementations, docs, and automated tests (see `CHANGELOG.md`). CI restores, builds, tests, uploads Cobertura/TRX coverage artifacts and a **ReportGenerator HTML** bundle (`coverage-report-html`; see [`docs/quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md`](docs/quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md)), validates docs, **shipping inventory**, **AI runtime docs**, **package dependency levels**, and packs **23** libraries on **.NET 9** (see `.github/workflows/`). Numeric coverage **thresholds** remain advisory until baselines stabilize (same doc). A non-shipping [`examples/ConexusDotNetSkeleton/`](examples/ConexusDotNetSkeleton/) project compiles against the Conexus v1 package slice as a compile-only smoke; **Conexus.NET** in `conexus-dotnet` is the live alpha consumer exercising the same stack in product code.

**Still evolving (check `docs/migrations/` before upgrading):**

```text
HTTP resilience          // Retry-After, jitter, backoff, and circuit breaking exist; richer policies and budgets still evolve — see docs/packages/Ontogony.Http.md
Envelope rules           // mechanical validation + JSON schema; product ingest policies stay in product repos
Public XML (CS1591)      // Tier A = Conexus baseline (enforced); Tier B = other shipped libs still suppressed — see docs/quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md
```
