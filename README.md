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

## What this starter integrates from the current repos

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
│   ├── Ontogony.Idempotency/
│   ├── Ontogony.Messaging/
│   ├── Ontogony.Security/
│   ├── Ontogony.Persistence/
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

## Recommended first adoption path

1. Add this repo as a separate GitHub repository: `uridolan77/ontogony-platform`.
2. Build and test the packages locally.
3. Publish packages to a private/internal NuGet feed, or reference by project path initially.
4. Replace duplicated trace/error code in Agentor and Athanor first.
5. Add Conexus event emission later through a thin Python client that emits the same envelope schema.

## First packages to adopt

Start with these only:

```text
Ontogony.Contracts
Ontogony.Observability
Ontogony.Errors
Ontogony.Configuration
Ontogony.Http
Ontogony.Hashing
```

Then add:

```text
Ontogony.Idempotency
Ontogony.Messaging
Ontogony.Security
Ontogony.Persistence
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

Starter package. The code is intentionally compact and extraction-ready. Before production adoption, run the build/test suite in a machine with .NET 9 SDK installed and wire CI.
