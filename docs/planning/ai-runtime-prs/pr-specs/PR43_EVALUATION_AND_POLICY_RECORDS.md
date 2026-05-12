# PR43 — Evaluation and Policy Records

## Package

`Ontogony.Evaluation.Contracts + Ontogony.Policy.Contracts`

## Goal

Record evaluation outcomes and policy decisions as facts.

## Forbidden

No evaluator, judge policy, or policy engine.

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
