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

## Status (PR-005 closeout gate)

Verified **2026-05-26** after stopping the local cohesion stack (release `dotnet test`, no API hosts on 5081–5083).

| Gate | Result |
| --- | --- |
| `ConformanceKitPr005Tests` (platform) | **PASS** — 7/7 |
| `ConexusPlatformConformanceTests` | **PASS** — 6/6 |
| `AllagmaPlatformConformanceTests` | **PASS** — 6/6 |
| `KanonPlatformConformanceTests` | **PASS** — 5/5 |
| Consumer test files on `main` | **Yes** — see paths below |

### Consumer test paths (remote `main`)

| Repo | Test class | Path on `main` |
| --- | --- | --- |
| Conexus | `ConexusPlatformConformanceTests` | `tests/Conexus.Application.Tests/ConexusPlatformConformanceTests.cs` |
| Allagma | `AllagmaPlatformConformanceTests` | `tests/Allagma.Tests/AllagmaPlatformConformanceTests.cs` |
| Kanon | `KanonPlatformConformanceTests` | `tests/Kanon.Tests/KanonPlatformConformanceTests.cs` |

Conexus tests landed in `75e3bac`; Allagma/Kanon tests are on `main` (bundled with reconstructability closeout commits, not a dedicated conformance-only commit message).

### Re-run commands

```powershell
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj -c Release --filter ConformanceKitPr005
dotnet test tests/Conexus.Application.Tests/Conexus.Application.Tests.csproj -c Release --filter ConexusPlatformConformanceTests
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter AllagmaPlatformConformanceTests
dotnet test tests/Kanon.Tests/Kanon.Tests.csproj -c Release --filter KanonPlatformConformanceTests
```

Stop local API hosts first if `dotnet test` fails with MSB3021 / CS2012 file-lock errors.
