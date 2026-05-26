# Expected validation commands

Run from `allagma-dotnet` unless noted.

## Static validation

```powershell
./scripts/validate-runtime-lock.ps1 -ReleaseMode
./scripts/validate-system-coh-001.ps1 -ReleaseMode
```

## Allagma tests

```powershell
dotnet test Allagma.sln -c Release
dotnet test tests/Allagma.ArchitectureConformance.Tests -c Release
```

## Cross-repo conformance

```powershell
./scripts/architecture-conformance/run-cross-repo-conformance.ps1
```

## Live acceptance

```powershell
./scripts/run-system-coh-001-acceptance.ps1 `
  -DevRoot C:\dev `
  -IncludeKanonAssistance `
  -IncludeConexusFallback `
  -IncludeCorrelationChain `
  -IncludeEvidenceSpineExport `
  -RequireEvidence `
  -ReleaseMode
```

## Package mode

```powershell
./scripts/run-package-mode-build.ps1
```

## Frontend

Run from `ontogony-frontend`:

```powershell
npm test -- src/evidence-spine
npm run contracts:discipline
```

## Platform schemas

Run whatever existing schema validation command the platform uses, then add the new schema fixtures to it.
