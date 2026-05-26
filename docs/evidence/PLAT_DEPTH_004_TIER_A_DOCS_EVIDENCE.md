# PLAT-DEPTH-004 — Tier A public XML documentation

**Superseded for full shipping surface:** [`PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md`](./PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md) (PLAT-9-004, 2026-05-26).

**Date:** 2026-05-26  
**Package:** ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001

## Policy

Staged enforcement per [`docs/quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md`](../quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md).

## Tier A status (this slice)

Conexus consumer-baseline packages built with **CS1591 enforced** — Release build of `Ontogony.Platform.sln` reported **0 warnings** at PLAT-DEPTH-004 closeout.

## Follow-on (PLAT-9-004)

All consumer-facing shipped packages except Tier C are now Tier A. See [`PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md`](./PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md) for the promotion list and Tier C rationale.

PLAT-DEPTH-001 public API additions (`BackoffPolicy`, `RetryExceptionContext`, `IRetryClassifierV2`, `ICircuitBreakerRegistry`, `TransportResilienceBackoff`) are documented on Tier A `Ontogony.Http`.

## Validation

```powershell
dotnet build Ontogony.Platform.sln -c Release
dotnet test tests/Ontogony.PublicApi.Tests/Ontogony.PublicApi.Tests.csproj -c Release
```
