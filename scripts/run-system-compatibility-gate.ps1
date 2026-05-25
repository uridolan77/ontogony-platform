#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Runs PLATFORM-9-001 system compatibility gate and writes summary artifacts.
.PARAMETER DevRoot
  Folder containing sibling repos (default: parent of ontogony-platform).
.PARAMETER ArtifactDir
  Output directory (default: artifacts/system-compat under repo root).
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [string] $ArtifactDir = ""
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
    $ArtifactDir = Join-Path $RepoRoot "artifacts/system-compat"
}

$env:ONTOGONY_DEV_ROOT = $DevRoot
$env:ONTOGONY_PLATFORM_ROOT = $RepoRoot
$env:ONTOGONY_SYSTEM_COMPAT_ARTIFACT_DIR = $ArtifactDir

Write-Host "System compatibility gate"
Write-Host "  DevRoot:     $DevRoot"
Write-Host "  Platform:    $RepoRoot"
Write-Host "  Artifacts:   $ArtifactDir"

Push-Location $RepoRoot
try {
    dotnet test tests/Ontogony.SystemCompatibility.Tests/Ontogony.SystemCompatibility.Tests.csproj `
        -c Release `
        --filter "Category=SystemCompatGate"
    if ($LASTEXITCODE -ne 0) {
        throw "System compatibility gate failed (exit $LASTEXITCODE)."
    }
}
finally {
    Pop-Location
}

$summaryJson = Join-Path $ArtifactDir "system-compatibility-summary.json"
if (-not (Test-Path -LiteralPath $summaryJson)) {
    throw "Expected summary artifact was not written: $summaryJson"
}

Write-Host "PASS - summary written to $ArtifactDir"
