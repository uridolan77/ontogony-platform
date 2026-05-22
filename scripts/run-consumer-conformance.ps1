#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-9-003 — runs consumer conformance suite and writes summary artifacts.
.PARAMETER DevRoot
  Folder containing sibling repos (default: parent of ontogony-platform).
.PARAMETER ArtifactDir
  Output directory (default: artifacts/consumer-conformance/<timestamp>).
.PARAMETER ReleaseMode
  Treat Warn-status consumer proofs as failures.
.PARAMETER SkipPackageSmoke
  Skip Conexus/Allagma package-mode build smoke (faster local runs).
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [string] $ArtifactDir = "",
    [switch] $ReleaseMode,
    [switch] $Strict,
    [switch] $SkipPackageSmoke
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = (Resolve-Path (Join-Path $RepoRoot "..")).Path
}

$isStrictMode = $ReleaseMode.IsPresent -or $Strict.IsPresent
$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"

if ([string]::IsNullOrWhiteSpace($ArtifactDir)) {
    $ArtifactDir = Join-Path $RepoRoot "artifacts/consumer-conformance/$timestamp"
}

$env:ONTOGONY_DEV_ROOT = $DevRoot
$env:ONTOGONY_PLATFORM_ROOT = $RepoRoot
if ($isStrictMode) {
    $env:ONTOGONY_CONSUMER_CONFORMANCE_STRICT = "1"
} else {
    $env:ONTOGONY_CONSUMER_CONFORMANCE_STRICT = "0"
}

$hasSiblingWorkspace = Test-Path -LiteralPath (Join-Path $DevRoot "allagma-dotnet\Allagma.sln")
if ($hasSiblingWorkspace) {
    $env:ONTOGONY_CONSUMER_CONFORMANCE_ARTIFACT_DIR = $ArtifactDir
} else {
    Remove-Item Env:ONTOGONY_CONSUMER_CONFORMANCE_ARTIFACT_DIR -ErrorAction SilentlyContinue
    Write-Host "  Sibling workspace not found - running fixture conformance tests only."
}

Write-Host "Consumer conformance suite (PLAT-9-003)"
$modeLabel = if ($isStrictMode) { "release/strict" } else { "development" }
Write-Host "  Mode:        $modeLabel"
Write-Host "  DevRoot:     $DevRoot"
Write-Host "  Platform:    $RepoRoot"
Write-Host "  Artifacts:   $ArtifactDir"
Write-Host ""

Push-Location $RepoRoot
try {
    & (Join-Path $PSScriptRoot "validate-public-api-baseline.ps1")
    if ($LASTEXITCODE -ne 0) { throw "validate-public-api-baseline failed." }

    & (Join-Path $PSScriptRoot "validate-package-levels.ps1")
    if ($LASTEXITCODE -ne 0) { throw "validate-package-levels failed." }

    & (Join-Path $PSScriptRoot "validate-shipping-inventory.ps1")
    if ($LASTEXITCODE -ne 0) { throw "validate-shipping-inventory failed." }

    if (-not $SkipPackageSmoke) {
        $packageVersion = "0.3.0-alpha.1"
        if (Test-Path -LiteralPath (Join-Path $RepoRoot "artifacts/package-manifest.json")) {
            $manifest = Get-Content -LiteralPath (Join-Path $RepoRoot "artifacts/package-manifest.json") -Raw | ConvertFrom-Json
            if ($manifest.packageVersion) { $packageVersion = $manifest.packageVersion }
        }

        Write-Host "Package-mode smoke: ConexusDotNetPackageSmoke"
        dotnet build (Join-Path $RepoRoot "examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj") `
            -c Release -p:OntogonyPackageVersion=$packageVersion --no-restore 2>$null
        if ($LASTEXITCODE -ne 0) {
            dotnet restore (Join-Path $RepoRoot "examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj") -p:OntogonyPackageVersion=$packageVersion
            dotnet build (Join-Path $RepoRoot "examples/ConexusDotNetPackageSmoke/ConexusDotNetPackageSmoke.csproj") `
                -c Release -p:OntogonyPackageVersion=$packageVersion --no-restore
        }
        if ($LASTEXITCODE -ne 0) { throw "Conexus package-mode smoke failed." }

        Write-Host "Package-mode smoke: AllagmaDotNetSkeleton"
        dotnet build (Join-Path $RepoRoot "examples/AllagmaDotNetSkeleton/AllagmaDotNetSkeleton.csproj") -c Release
        if ($LASTEXITCODE -ne 0) { throw "Allagma package-mode smoke failed." }
    }

    dotnet test tests/Ontogony.ConsumerConformance.Tests/Ontogony.ConsumerConformance.Tests.csproj `
        -c Release `
        --filter "Category=ConsumerConformance"
    if ($LASTEXITCODE -ne 0) {
        throw "Consumer conformance tests failed (exit $LASTEXITCODE)."
    }
}
finally {
    Pop-Location
}

$summaryJson = Join-Path $ArtifactDir "summary.json"
if ($hasSiblingWorkspace) {
    if (-not (Test-Path -LiteralPath $summaryJson)) {
        throw "Expected summary artifact was not written: $summaryJson"
    }
} else {
    New-Item -ItemType Directory -Force -Path $ArtifactDir | Out-Null
    @{
        schema = "ontogony-consumer-conformance-v1"
        mode = "fixture-only"
        devRoot = $DevRoot
        generatedAt = (Get-Date).ToUniversalTime().ToString("o")
        note = "Sibling repos not present; integration proofs skipped."
    } | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $summaryJson -Encoding utf8
}

Write-Host ""
Write-Host "PASS - consumer conformance summary: $summaryJson"
