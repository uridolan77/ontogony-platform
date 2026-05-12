# PR38 — Execution Journal Contracts

## Package

`Ontogony.Execution`

## Goal

Generic run/step/attempt/transition/checkpoint mechanics.

## Forbidden

No agent planning, tool choice, or review policy.

## Acceptance

- Package builds and packs.
- Public contracts serialize deterministically.
- No Agentor/Athanor/Conexus semantics are introduced.
- Tests cover boundary behavior and basic serialization.

## Commands

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
$env:PACKAGE_VERSION="0.3.0-local"; ./scripts/pack-all.ps1 -NoBuild
```
