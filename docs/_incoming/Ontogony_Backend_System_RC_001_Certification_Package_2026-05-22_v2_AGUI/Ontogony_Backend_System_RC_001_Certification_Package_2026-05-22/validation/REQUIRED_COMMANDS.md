# Required Commands

## Allagma release-candidate gates

```powershell
cd C:\dev\allagma-dotnet

pwsh ./scripts/validate-runtime-lock.ps1
pwsh ./scripts/validate-runtime-lock.ps1 -RequireEvidence
pwsh ./scripts/validate-runtime-lock.ps1 -ReleaseMode

pwsh ./scripts/run-system-cohesion-smoke.ps1 -UseExistingServices `
  -IncludeKanonAssistance -IncludeConexusFallback -IncludeStreamingEvidence

pwsh ./scripts/run-package-mode-build.ps1 -WriteCiEvidenceSummary

dotnet test tests/Allagma.ArchitectureConformance.Tests/Allagma.ArchitectureConformance.Tests.csproj `
  -c Release --filter "FullyQualifiedName~KanonCompatibilityManifestConformanceTests"

pwsh ./scripts/validate-feature-connection-matrix.ps1 -DevRoot C:\dev

dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release `
  --filter "FullyQualifiedName~ConexusStreamingPackageContractTests"

pwsh ./scripts/run-persistence-smoke.ps1 -WriteCiEvidenceSummary

pwsh ./scripts/verify-system-observability.ps1 -UseExistingServices

pwsh ./scripts/validate-release-lock-crossref.ps1
pwsh ./scripts/validate-runtime-correctness-matrix.ps1

pwsh ./scripts/validate-system-tight-rc-prep.ps1
pwsh ./scripts/validate-system-tight-rc-readiness.ps1
pwsh ./scripts/validate-system-tight-rc-evidence.ps1
```

## Conexus focused certification

```powershell
cd C:\dev\conexus-dotnet

dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~ModelCallEvidenceBundle"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~UsageCostDrilldown"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~RouteDecision"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~Streaming"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~Quota"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~Idempotency"
dotnet test Conexus.sln -c Release --filter "FullyQualifiedName~Fallback"
```

## Kanon focused certification

```powershell
cd C:\dev\kanon-dotnet

dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~KanonCompatibilityManifestTests"
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~OntologyV0RouteInventoryTests"
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~OpenApiBaselineTests"
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~KanonV1GraduationGuardTests"
dotnet test Kanon.sln -c Release --filter "FullyQualifiedName~PostgresSemanticSmokeTests"
```

## Platform focused certification

```powershell
cd C:\dev\ontogony-platform

dotnet test Ontogony.Platform.sln -c Release
pwsh ./scripts/validate-system-protocol-registry.ps1
pwsh ./scripts/validate-system-evidence-spine-contract.ps1
pwsh ./scripts/validate-system-operator-failure-taxonomy.ps1
pwsh ./scripts/validate-package-levels.ps1
```

## Frontend/operator optional but recommended

```powershell
cd C:\dev\ontogony-frontend

npm run kanon:route-parity:check
npm run test:e2e:docker-live:fe-live-smoke
npm run test:e2e:docker-live:evidence-spine
npm run test:e2e:docker-live:operator-audit-journey
```
