# PLAT-DEPTH-004 — Tier A public XML documentation

**Date:** 2026-05-26  
**Package:** ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001

## Policy

Staged enforcement per [`docs/quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md`](../quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md).

## Tier A status

Tier A packages (Conexus consumer baseline) build with **CS1591 enforced** — Release build of `Ontogony.Platform.sln` reports **0 warnings**.

## Tier B deferrals (unchanged)

`Ontogony.Configuration`, `Ontogony.Messaging`, `Ontogony.Persistence`, `Ontogony.Persistence.Postgres`, `Ontogony.Replay.Contracts`, `Ontogony.SystemCompatibility`, and other non-baseline packages remain on repo-wide `CS1591` suppression until promoted.

## Validation

```powershell
dotnet build Ontogony.Platform.sln -c Release
dotnet test tests/Ontogony.PublicApi.Tests/Ontogony.PublicApi.Tests.csproj -c Release
```
