# KANON-PACKAGE-MODE-REPLAYTARGET-FIX-001

## Owner repo

- `kanon-dotnet`

## Problem

Allagma package-mode build is blocked by a sibling `Kanon.Contracts` pack failure involving missing `ReplayTarget` types.

## Required implementation

1. Locate `ReplayTarget` and related replay DTOs.
2. Decide whether they belong in `Kanon.Contracts` or a shared Ontogony replay contract package.
3. Ensure the package includes public DTOs required by consumers.
4. Ensure no Application/Infrastructure dependencies leak into Contracts.
5. Add package-mode test coverage.

## Acceptance

```powershell
cd C:\dev\kanon-dotnet
dotnet restore Kanon.sln
dotnet build Kanon.sln -c Release
dotnet pack src\Kanon.Contracts\Kanon.Contracts.csproj -c Release -o artifacts\packages

cd C:\devllagma-dotnet
pwsh .\scriptsun-package-mode-build.ps1
```
