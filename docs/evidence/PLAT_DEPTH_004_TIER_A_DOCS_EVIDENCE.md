# PLAT-DEPTH-004 — Tier A public XML documentation

**Superseded for Tier B backlog:** [`PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md`](./PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md) (PLAT-9-004, 2026-05-26).

**Date:** 2026-05-26  
**Package:** ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001

## Policy

Staged enforcement per [`docs/quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md`](../quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md).

## Tier A status

Tier A packages (Conexus consumer baseline) build with **CS1591 enforced** — Release build of `Ontogony.Platform.sln` reports **0 warnings**.

## Tier B deferrals (explicit backlog)

| Package | Notes |
| --- | --- |
| Ontogony.Configuration | Mechanical configuration helpers |
| Ontogony.Messaging | Messaging abstractions |
| Ontogony.Persistence | Persistence ports |
| Ontogony.Persistence.Postgres | Postgres adapters |
| Ontogony.ProtocolIngress | Ingress contracts |
| Ontogony.Replay.Contracts | Replay-oriented contracts only |
| Ontogony.Testing | Shipped test support; docs relaxed |
| Ontogony.SystemCompatibility | Gate tooling |
| Ontogony.Evaluation.Contracts | Cross-cutting contracts |
| Ontogony.Topology.Contracts | Cross-cutting contracts |
| Ontogony.Secrets.AzureKeyVault | Optional adapter |

PLAT-DEPTH-001 public API additions (`BackoffPolicy`, `RetryExceptionContext`, `IRetryClassifierV2`, `ICircuitBreakerRegistry`, `TransportResilienceBackoff`) are documented on Tier A `Ontogony.Http` with **0 CS1591** on Release build.

## Validation

```powershell
dotnet build Ontogony.Platform.sln -c Release
dotnet test tests/Ontogony.PublicApi.Tests/Ontogony.PublicApi.Tests.csproj -c Release
```
