# Platform reconstructability conformance evidence (PR-005)

## Scope

Platform deliverable: `Ontogony.Testing.Conformance` kits that product repos adopt to prove shared mechanics without duplicating assertion logic.

## Deliverables

| Artifact | Location |
| --- | --- |
| Conformance kits | `src/Ontogony.Testing/Conformance/` |
| Adoption guide | `docs/adoption/reconstructability-conformance-kits.md` |
| Platform self-tests | `tests/Ontogony.Infrastructure.Tests/ConformanceKitPr005Tests.cs` |

## Verification

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj -c Release --filter ConformanceKitPr005
```

Consumer repos add `*PlatformConformanceTests` classes (Allagma, Kanon, Conexus) referencing the same kits.

## Status

| Check | Result |
| --- | --- |
| Platform mechanics-only | Kits delegate to existing `Ontogony.Testing` / `Ontogony.Errors` types |
| Correlation + error baseline | `ConformanceKitPr005Tests` — **7/7 passed** (2026-05-26) |
| Idempotency harness | `InMemoryIdempotencyLedger` via `IdempotencyConformanceKit` |
| Export redaction + fragment refs | `ReconstructabilityExportConformanceAssertions` |
| Conexus consumer adoption | `ConexusPlatformConformanceTests` — **6/6 passed** (2026-05-26) |
| Allagma consumer adoption | `AllagmaPlatformConformanceTests` — run after stopping local stack (API locks `bin/`) |
| Kanon consumer adoption | `KanonPlatformConformanceTests` — run after stopping local stack (API locks `bin/`) |

```powershell
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj -c Release --filter ConformanceKitPr005
dotnet test tests/Conexus.Application.Tests/Conexus.Application.Tests.csproj -c Release --filter ConexusPlatformConformanceTests
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter AllagmaPlatformConformanceTests
dotnet test tests/Kanon.Tests/Kanon.Tests.csproj -c Release --filter KanonPlatformConformanceTests
```
