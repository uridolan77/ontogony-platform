# PR40 — Knowledge Contracts

## Package

`Ontogony.Knowledge.Contracts`

## Goal

Mechanical source/document/version/chunk/embedding/corpus contracts.

## Forbidden

No canonization, contradiction, relevance, or epistemic status.

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
