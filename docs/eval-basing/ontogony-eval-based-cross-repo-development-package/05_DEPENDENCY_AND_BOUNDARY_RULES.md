# 05 — Dependency and Boundary Rules

## Ontogony.Platform

Allowed:

- neutral contracts,
- mechanics,
- serialization helpers,
- test fixtures,
- generic journal/evidence DTOs.

Forbidden:

- Allagma orchestration semantics,
- Kanon policy semantics,
- Conexus provider routing policy,
- eval scoring rubrics with product meaning,
- agent planner logic.

## Kanon.NET

Allowed:

- topology policy evaluation,
- ontology-level topology constraints,
- human gate requirements,
- decision records,
- provenance links.

Forbidden:

- model provider routing,
- cost optimization routing,
- MAF/runtime execution,
- running tools,
- autonomous workflow orchestration.

## Conexus.NET

Allowed:

- model alias/provider routing,
- fallback policy,
- capability profiles,
- cost/usage telemetry,
- route decision evidence,
- model quality feedback aggregation.

Forbidden:

- semantic truth,
- ontology policy,
- business approval,
- human gate authority,
- Allagma workflow control.

## Allagma.NET

Allowed:

- task classification,
- topology selection proposal,
- governed execution,
- baseline execution,
- eval harness,
- audit bundle,
- tool-intent lifecycle,
- Kanon and Conexus client orchestration.

Forbidden:

- owning ontology truth,
- bypassing Kanon policy,
- bypassing Conexus model access,
- real external tool execution before dedicated enablement gates.

## Cross-repo dependency shape

```text
allagma-dotnet -> kanon-dotnet client/contracts
allagma-dotnet -> conexus-dotnet client/contracts
allagma-dotnet -> ontogony-platform packages

kanon-dotnet -> ontogony-platform packages
conexus-dotnet -> ontogony-platform packages

ontogony-platform -> no product repos
kanon-dotnet -> no Allagma implementation
conexus-dotnet -> no Kanon implementation
```
