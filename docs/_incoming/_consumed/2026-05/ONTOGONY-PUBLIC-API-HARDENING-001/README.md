# ONTOGONY-PUBLIC-API-HARDENING-001

**Status:** in_progress  
**Tracked in:** [`../MANIFEST.md`](../MANIFEST.md)

## Goal

PLAT-9-004 — expand Tier A XML documentation enforcement to all shipped consumer-surface packages; keep test/gate tooling on Tier C.

## Scope

```text
- XML docs for Tier B consumer packages (promoted to Tier A)
- Tier C policy for Ontogony.Testing and Ontogony.SystemCompatibility
- PublicApiDocumentationPolicyTests guard
- Public API snapshot refresh where API drifted on main
- Evidence: docs/evidence/PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md
```

## Acceptance

```powershell
dotnet build Ontogony.Platform.sln -c Release
dotnet test tests/Ontogony.PublicApi.Tests/Ontogony.PublicApi.Tests.csproj -c Release
dotnet test tests/Ontogony.Architecture.Tests/Ontogony.Architecture.Tests.csproj -c Release
./scripts/validate-public-api-baseline.ps1
```

## Archive

When green, move to `_consumed/2026-05/ONTOGONY-PUBLIC-API-HARDENING-001/`.
