# PLAT-9-004 — Public API hardening (XML docs + Tier A expansion)

**Date:** 2026-05-26  
**Package:** ONTOGONY-PUBLIC-API-HARDENING-001

## Policy

Staged enforcement per [`docs/quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md`](../quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md).

| Tier | Count | Meaning |
| --- | ---: | --- |
| **A** | 25 | Shipped consumer-surface packages; `CS1591` enforced via `src/Directory.Build.targets` |
| **C** | 2 | `Ontogony.Testing`, `Ontogony.SystemCompatibility` — intentional doc relaxation |
| **B** | 0 | — |

Guard: `tests/Ontogony.Architecture.Tests/PublicApiDocumentationPolicyTests.cs`

## Tier A promotion (this slice)

Packages promoted from Tier B / unstaged backlog:

```text
Ontogony.Configuration
Ontogony.Evaluation.Contracts
Ontogony.Messaging
Ontogony.Persistence
Ontogony.Persistence.Postgres
Ontogony.ProtocolIngress
Ontogony.Replay.Contracts
Ontogony.Secrets.AzureKeyVault
Ontogony.Topology.Contracts
```

## Validation

```powershell
dotnet build Ontogony.Platform.sln -c Release
dotnet test tests/Ontogony.PublicApi.Tests/Ontogony.PublicApi.Tests.csproj -c Release
dotnet test tests/Ontogony.Architecture.Tests/Ontogony.Architecture.Tests.csproj -c Release
./scripts/validate-public-api-baseline.ps1
./scripts/validate-package-levels.ps1
./scripts/validate-shipping-inventory.ps1
```

**Result:** Release solution build **0 warnings** with expanded Tier A; Public API snapshots refreshed for `Ontogony.Idempotency`, `Ontogony.Observability`, `Ontogony.Testing` (API drift on `main`, not XML-only).

## Tier C rationale

| Package | Rationale |
| --- | --- |
| `Ontogony.Testing` | Test/architecture helpers; consumers use for CI guards, not runtime contracts |
| `Ontogony.SystemCompatibility` | Six-repo lock gate executable; not referenced as a product integration API |
