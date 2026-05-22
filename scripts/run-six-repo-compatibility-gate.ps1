#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Runs the six-repo compatibility gate and writes summary artifacts.
.PARAMETER DevRoot
  Folder containing all six sibling repos (default: parent of ontogony-platform).
.PARAMETER ArtifactDir
  Output directory (default: artifacts/six-repo-compat under repo root).
.PARAMETER Update
  When set, prints the commands needed to regenerate the six-repo lock. Does not auto-update.
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [string] $ArtifactDir = "",
    [switch] $Update
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = (Resolve-Path (Join-Path $RepoRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($ArtifactDir)) {
    $ArtifactDir = Join-Path $RepoRoot "artifacts/six-repo-compat"
}

if ($Update) {
    Write-Host ""
    Write-Host "To update the six-repo lock, regenerate contract hashes and commit the lock file:"
    Write-Host ""
    Write-Host "  1. Run the frontend UI consumer preflight:"
    Write-Host "       cd $DevRoot\ontogony-frontend && npm run check:ui-consumer-preflight"
    Write-Host ""
    Write-Host "  2. Compute OpenAPI snapshot hashes:"
    Write-Host "       Get-FileHash $DevRoot\ontogony-frontend\openapi\allagma.v0.json -Algorithm SHA256"
    Write-Host "       Get-FileHash $DevRoot\ontogony-frontend\openapi\conexus.v0.json -Algorithm SHA256"
    Write-Host "       Get-FileHash $DevRoot\ontogony-frontend\openapi\kanon.v0.json -Algorithm SHA256"
    Write-Host ""
    Write-Host "  3. Edit docs/system/ontogony-six-repo-lock.json in ontogony-platform with current commits and hashes."
    Write-Host ""
    Write-Host "  4. Re-run this gate to confirm all checks pass."
    Write-Host ""
    exit 0
}

$env:ONTOGONY_DEV_ROOT = $DevRoot
$env:ONTOGONY_PLATFORM_ROOT = $RepoRoot
$env:ONTOGONY_SYSTEM_COMPAT_ARTIFACT_DIR = $ArtifactDir

Write-Host "Six-repo compatibility gate"
Write-Host "  DevRoot:     $DevRoot"
Write-Host "  Platform:    $RepoRoot"
Write-Host "  Artifacts:   $ArtifactDir"
Write-Host ""

$sixRepoLock = Join-Path $RepoRoot "docs/system/ontogony-six-repo-lock.json"
if (-not (Test-Path -LiteralPath $sixRepoLock)) {
    Write-Error "Missing six-repo lock: $sixRepoLock"
    exit 1
}

Write-Host "Lock file:   $sixRepoLock"
Write-Host ""

Push-Location $RepoRoot
try {
    dotnet test tests/Ontogony.SystemCompatibility.Tests/Ontogony.SystemCompatibility.Tests.csproj `
        -c Release `
        --filter "Category=SystemCompatGate&FullyQualifiedName~SixRepo"
    if ($LASTEXITCODE -ne 0) {
        throw "Six-repo compatibility gate failed (exit $LASTEXITCODE)."
    }
}
finally {
    Pop-Location
}

Write-Host ""
Write-Host "PASS — six-repo compatibility gate complete."
Write-Host ""
Write-Host "To verify the full system compatibility gate (all checks), run:"
Write-Host "  ./scripts/run-system-compatibility-gate.ps1 -DevRoot $DevRoot"
