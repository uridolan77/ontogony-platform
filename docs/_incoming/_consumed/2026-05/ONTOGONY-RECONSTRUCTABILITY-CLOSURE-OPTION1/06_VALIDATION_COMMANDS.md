# Validation commands

## Allagma focused

```powershell
cd C:\dev\allagma-dotnet

dotnet build Allagma.sln -c Release --no-incremental

dotnet test tests/Allagma.Tests/Allagma.Tests.csproj `
  --no-build `
  --filter "RunDecisionEvents" `
  -q

dotnet test tests/Allagma.Tests/Allagma.Tests.csproj `
  --no-build `
  --filter "AllagmaV0RouteInventoryTests|FeatureConnectionMatrixAuditTests" `
  -q
```

## Kanon focused

```powershell
cd C:\dev\kanon-dotnet

dotnet build Kanon.sln -c Release

dotnet test Kanon.sln -c Release `
  --filter "FullyQualifiedName~Reconstructability" `
  -q
```

## Conexus focused

```powershell
cd C:\dev\conexus-dotnet

dotnet build Conexus.sln -c Release -p:NoWarn=CS1591

dotnet test Conexus.sln -c Release `
  --filter "Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke&Category!=CapacityBaseline" `
  -p:NoWarn=CS1591 `
  -q
```

## Platform focused

```powershell
cd C:\dev\ontogony-platform

dotnet build Ontogony.Platform.sln -c Release

dotnet test Ontogony.Platform.sln -c Release -q
```

## Cross-service local fake-mode

After PR-004:

```powershell
cd C:\dev\allagma-dotnet

./scripts/system/run-reconstructability-golden-trace.ps1 -DevRoot C:\dev -UseFakeProvider

./scripts/system/validate-reconstructability-golden-trace.ps1 `
  -EvidencePath .\artifacts\reconstructability\golden-trace\latest\summary.json
```

## Required final report

Every PR must end with:

```text
Commands run
Pass/fail results
Known environment-gated failures
Real code failures
Evidence files written
Docs updated
```
