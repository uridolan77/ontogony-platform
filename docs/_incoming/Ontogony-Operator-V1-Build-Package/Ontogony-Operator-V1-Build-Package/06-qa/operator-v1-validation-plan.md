# Operator V1 Validation Plan

## Repo checks

```powershell
cd C:\dev\ontogony-platform
dotnet test Ontogony.Platform.sln -c Release
.\scriptsalidate-system-protocol-registry.ps1
.\scriptsalidate-stale-incoming-package.ps1
.\scriptsalidate-real-tools-block.ps1

cd C:\devllagma-dotnet
dotnet test Allagma.sln -c Release
.\scriptsalidate-real-tools-block.ps1
.\scriptsalidate-feature-connection-matrix.ps1

cd C:\dev\kanon-dotnet
dotnet test -c Release

cd C:\dev\conexus-dotnet
dotnet test -c Release

cd C:\dev\ontogony-frontend
npm run openapi:gen
npm run check

cd C:\dev\ontogony-ui
npm run check
```

## Docker/browser gates

- system cohesion;
- restart survival;
- observability;
- FE live smoke;
- Evidence Spine Docker-live;
- operator demo flows.
