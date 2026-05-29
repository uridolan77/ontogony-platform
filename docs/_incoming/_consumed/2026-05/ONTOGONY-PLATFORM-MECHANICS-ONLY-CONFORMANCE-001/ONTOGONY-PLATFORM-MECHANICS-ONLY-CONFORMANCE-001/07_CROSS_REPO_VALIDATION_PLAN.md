# Cross-repo validation plan

## Local workspace assumption

```text
C:\dev\ontogony-platform
C:\dev\conexus-dotnet
C:\dev\kanon-dotnet
C:\dev\allagma-dotnet
C:\dev\metabole-dotnet
C:\dev\aisthesis-dotnet
```

## Platform-only validation

```powershell
cd C:\dev\ontogony-platform
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
.\scripts\validate-shipping-inventory.ps1
.\scripts\validate-package-levels.ps1
.\scripts\check-no-product-semantics.ps1
.\scripts\governance\check-platform-mechanics-only.ps1 -RepoRoot .
.\scripts\conformance\Test-MechanicalSchemaRegistry.ps1 -RepoRoot .
```

## Consumer fixture validation

```powershell
.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\conexus-dotnet `
  -ConsumerName conexus

.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\kanon-dotnet `
  -ConsumerName kanon

.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\allagma-dotnet `
  -ConsumerName allagma

.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\metabole-dotnet `
  -ConsumerName metabole

.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\aisthesis-dotnet `
  -ConsumerName aisthesis
```

## Output

Each run writes:

```text
artifacts/platform-mechanics-conformance/<consumer>/<timestamp>/summary.json
artifacts/platform-mechanics-conformance/<consumer>/<timestamp>/details/*.json
artifacts/platform-mechanics-conformance/<consumer>/<timestamp>/README.md
```

## Required closeout interpretation

- PASS means the consumer obeys mechanical contracts currently in scope.
- PARTIAL means missing evidence or intentionally deferred harnesses.
- FAIL means the consumer violates a mechanical contract.
- NOT_RUN means repo or configuration unavailable.
