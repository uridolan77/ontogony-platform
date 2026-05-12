# PR36 — AI Runtime Contracts

## Package

`Ontogony.AI.Contracts`

## Goal

Provider-neutral LLM request/response/usage/cost/tool-call records.

## Forbidden

No model routing, fallback strategy, prompt policy, or provider ranking.

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
