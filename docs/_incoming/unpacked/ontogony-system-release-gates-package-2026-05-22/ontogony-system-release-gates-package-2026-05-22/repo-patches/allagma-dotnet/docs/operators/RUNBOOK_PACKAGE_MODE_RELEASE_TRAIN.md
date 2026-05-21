# Runbook — package-mode release train

## Purpose

Prove Allagma can consume Platform/Kanon/Conexus as packages, not sibling source.

## Local command

```powershell
cd C:\dev\allagma-dotnet
./scripts/release/run-package-mode-release-train.ps1 `
  -WorkspaceRoot C:\dev `
  -FeedDirectory C:\dev\ontogony-local-feed `
  -RuntimeLockPath .\docs\system\ontogony-runtime.lock.json
```

## Required success criteria

- Local feed contains required packages.
- Package versions match runtime lock.
- Allagma restore/build/test pass with:
  - `UseOntogonyPackages=true`
  - `UseKanonPackages=true`
  - `UseConexusPackages=true`
- No sibling project references are used.

## Common failures

| Failure | Fix |
|---|---|
| Missing package | Ensure upstream pack script includes package |
| Version mismatch | Update runtime lock or package props together |
| Restore from sibling source | Check MSBuild conditions and `Directory.Packages.props` |
| API contract break | Update typed clients or hold platform/package release |
