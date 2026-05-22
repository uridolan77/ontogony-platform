#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Runs the six-repo compatibility gate and writes summary artifacts.
.PARAMETER DevRoot
  Folder containing all six sibling repos (default: parent of ontogony-platform).
.PARAMETER ArtifactDir
  Output directory (default: artifacts/six-repo-compat under repo root).
.PARAMETER ReleaseMode
  Treat Warn-status checks as failures. Use for release branches, tags, and promotion gates.
.PARAMETER Strict
  Alias for -ReleaseMode.
.PARAMETER Update
  Print the commands needed to regenerate the six-repo lock. Does not auto-update.
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [string] $ArtifactDir = "",
    [switch] $ReleaseMode,
    [switch] $Strict,
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

$isStrictMode = $ReleaseMode.IsPresent -or $Strict.IsPresent

if ($Update) {
    Write-Host ""
    Write-Host "To update the six-repo lock, regenerate contract hashes and commit the lock file:"
    Write-Host ""
    Write-Host "  1. Run the frontend UI consumer preflight:"
    Write-Host "       cd $DevRoot\ontogony-frontend && npm run check:ui-consumer-preflight"
    Write-Host ""
    Write-Host "  2. Sync the frontend release lock:"
    Write-Host "       cd $DevRoot\ontogony-frontend && node scripts/sync-frontend-release-lock.mjs"
    Write-Host ""
    Write-Host "  3. Compute OpenAPI snapshot hashes:"
    Write-Host "       Get-FileHash $DevRoot\ontogony-frontend\openapi\allagma.v0.json -Algorithm SHA256"
    Write-Host "       Get-FileHash $DevRoot\ontogony-frontend\openapi\conexus.v0.json -Algorithm SHA256"
    Write-Host "       Get-FileHash $DevRoot\ontogony-frontend\openapi\kanon.v0.json -Algorithm SHA256"
    Write-Host ""
    Write-Host "  4. Edit docs/system/ontogony-six-repo-lock.json in ontogony-platform with current commits and hashes."
    Write-Host "       docs/system/ontogony-six-repo-post-lock-deltas.json may also need updating."
    Write-Host ""
    Write-Host "  5. Re-run this gate to confirm all checks pass."
    Write-Host "       ./scripts/run-six-repo-compatibility-gate.ps1 -DevRoot $DevRoot"
    Write-Host ""
    exit 0
}

$env:ONTOGONY_DEV_ROOT = $DevRoot
$env:ONTOGONY_PLATFORM_ROOT = $RepoRoot
$env:ONTOGONY_SYSTEM_COMPAT_ARTIFACT_DIR = $ArtifactDir
$env:ONTOGONY_SIX_REPO_STRICT_MODE = if ($isStrictMode) { "1" } else { "0" }

$modeLabel = if ($isStrictMode) { "release/strict" } else { "development" }

Write-Host "Six-repo compatibility gate"
Write-Host "  Mode:        $modeLabel"
Write-Host "  DevRoot:     $DevRoot"
Write-Host "  Platform:    $RepoRoot"
Write-Host "  Artifacts:   $ArtifactDir"
Write-Host ""

if ($isStrictMode) {
    Write-Host "  *** STRICT MODE: Warn-status checks will be treated as failures ***"
    Write-Host ""
}

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

$summaryJson = Join-Path $ArtifactDir "six-repo-compatibility-summary.json"
if (Test-Path -LiteralPath $summaryJson) {
    Write-Host ""
    Write-Host "Summary artifact: $summaryJson"
}

Write-Host ""
Write-Host "PASS — six-repo compatibility gate complete ($modeLabel mode)."
Write-Host ""
Write-Host "To verify the full system compatibility gate (all checks), run:"
Write-Host "  ./scripts/run-system-compatibility-gate.ps1 -DevRoot $DevRoot"
