# 04 — Validation gates

## Baseline prep validation

Run per repo from clean `main`:

```powershell
# platform
cd C:\dev\ontogony-platform
git status --short

# frontend
cd C:\dev\ontogony-frontend
npm run check
npm run test:fe-high-value

# UI
cd C:\dev\ontogony-ui
npm run check

# Conexus
cd C:\dev\conexus-dotnet
dotnet test

# Kanon
cd C:\dev\kanon-dotnet
dotnet test

# Allagma
cd C:\devllagma-dotnet
dotnet test
```

## Docker-local baseline validation

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
```

Then run available validators:

```powershell
# frontend composition
cd C:\dev\ontogony-frontend
npm run docker:gate:composition
npm run test:e2e:docker-live:fe-live-smoke
npm run test:e2e:docker-live:evidence-spine
```

```powershell
# allagma/system
cd C:\devllagma-dotnet
.\scriptsun-system-cohesion-smoke.ps1 -UseExistingServices
.\scriptsun-restart-survival-smoke.ps1
```

## Sprint 4 service validation

```powershell
# Allagma evidence
cd C:\devllagma-dotnet
.\scriptsun-allagma-evidence-001-evidence-smoke.ps1 -WriteCiEvidenceSummary

# Allagma streaming, non-default local smoke
.\scriptsun-allagma-stream-001-evidence-smoke.ps1
```

```powershell
# Conexus retention
cd C:\dev\conexus-dotnet
dotnet test tests/Conexus.Api.Tests/Conexus.Api.Tests.csproj --filter "FullyQualifiedName~RetentionAdmin"
dotnet test tests/Conexus.Persistence.Tests/Conexus.Persistence.Tests.csproj --filter "FullyQualifiedName~EfRetentionCleanupService"
```

```powershell
# Kanon assistance/domain packs
cd C:\dev\kanon-dotnet
dotnet test tests/Kanon.Tests/Kanon.Tests.csproj --filter "FullyQualifiedName~ConexusAssistance"
dotnet test .\Kanon.sln -c Release --filter "FullyQualifiedName~DomainPackLifecycleGovernance|FullyQualifiedName~DomainPackGovernanceDocs"
```

## Lock cut rules

Do not update `ontogony-runtime.lock.json` until:

1. Required validation commands pass, or failures are explicitly quarantined with owner/date.
2. New evidence artifact paths exist.
3. Current commit SHAs are captured.
4. `SYSTEM_ALPHA_004_PREP_EVIDENCE.md` exists.
5. `SYSTEM_ALPHA_004_CLOSEOUT.md` is ready.
