#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-9-005 — runs observability mechanics conformance and writes summary artifacts.
.PARAMETER DevRoot
  Folder containing sibling repos (default: parent of ontogony-platform).
.PARAMETER ArtifactDir
  Output directory (default: artifacts/observability-mechanics/<timestamp>).
.PARAMETER SkipConsumerTests
  Skip optional Allagma/Kanon/Conexus PlatformConformanceTests when siblings exist.
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [string] $ArtifactDir = "",
    [switch] $SkipConsumerTests
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = (Resolve-Path (Join-Path $RepoRoot "..")).Path
}

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
if ([string]::IsNullOrWhiteSpace($ArtifactDir)) {
    $ArtifactDir = Join-Path $RepoRoot "artifacts/observability-mechanics/$timestamp"
}

New-Item -ItemType Directory -Force -Path $ArtifactDir | Out-Null

Write-Host "Observability mechanics conformance (PLAT-9-005)"
Write-Host "  DevRoot:   $DevRoot"
Write-Host "  Platform:  $RepoRoot"
Write-Host "  Artifacts: $ArtifactDir"
Write-Host ""

$results = [ordered]@{
    schema      = "ontogony-observability-mechanics-v1"
    generatedAt = (Get-Date).ToUniversalTime().ToString("o")
    devRoot     = $DevRoot
    platform    = @{ status = "pending"; tests = @() }
    artifacts   = @{ status = "pending" }
    consumers   = @()
}

function Add-TestResult {
    param(
        [string]$Scope,
        [string]$Name,
        [string]$Status,
        [string]$Detail = ""
    )
    if ($Scope -eq "platform") {
        $results.platform.tests += @{
            name   = $Name
            status = $Status
            detail = $Detail
        }
    }
}

Push-Location $RepoRoot
try {
    Write-Host "Validating pack artifacts..."
    & (Join-Path $PSScriptRoot "validate-observability-pack-artifacts.ps1") -RepoRoot $RepoRoot
    if (-not $?) { throw "validate-observability-pack-artifacts failed." }
    $results.artifacts = @{
        status    = "PASS"
        dashboard = "docs/observability/dashboards/grafana-dashboard-starter.json"
        alerts    = "docs/observability/alerts/alerts.prometheus.rules.yml"
    }

    Write-Host "Platform observability tests..."
    $platformFilter = "FullyQualifiedName~DiagnosticsContractSmokeTests|FullyQualifiedName~SystemCorrelationConventionsTests|FullyQualifiedName~ObservabilityPackArtifactTests"
    dotnet test tests/Ontogony.Observability.Tests/Ontogony.Observability.Tests.csproj -c Release --filter $platformFilter
    if (-not $?) {
        Add-TestResult -Scope platform -Name "Ontogony.Observability.Tests" -Status "FAIL" -Detail "exit $LASTEXITCODE"
        throw "Platform observability tests failed."
    }
    Add-TestResult -Scope platform -Name "Ontogony.Observability.Tests" -Status "PASS"
    $results.platform.status = "PASS"
}
finally {
    Pop-Location
}

$consumerMatrix = @(
    @{ repo = "allagma-dotnet"; testProject = "tests/Allagma.Tests/Allagma.Tests.csproj"; filter = "AllagmaPlatformConformanceTests" }
    @{ repo = "conexus-dotnet"; testProject = "tests/Conexus.Application.Tests/Conexus.Application.Tests.csproj"; filter = "ConexusPlatformConformanceTests" }
    @{ repo = "kanon-dotnet"; testProject = "tests/Kanon.Tests/Kanon.Tests.csproj"; filter = "KanonPlatformConformanceTests" }
)

if (-not $SkipConsumerTests) {
    foreach ($entry in $consumerMatrix) {
        $repoPath = Join-Path $DevRoot $entry.repo
        $projPath = Join-Path $repoPath $entry.testProject
        $row = [ordered]@{
            repo   = $entry.repo
            filter = $entry.filter
            status = "SKIP"
            detail = ""
        }

        if (-not (Test-Path -LiteralPath $projPath)) {
            $row.detail = "test project not found"
            $results.consumers += $row
            Write-Host "  SKIP $($entry.repo) - project missing"
            continue
        }

        Write-Host "Consumer: $($entry.repo) ($($entry.filter))..."
        Push-Location $repoPath
        try {
            dotnet test $projPath -c Release --filter $entry.filter
            if (-not $?) {
                $row.status = "FAIL"
                $row.detail = "dotnet test failed"
                $results.consumers += $row
                throw "$($entry.repo) consumer conformance failed."
            }
            $row.status = "PASS"
            $results.consumers += $row
        }
        finally {
            Pop-Location
        }
    }
} else {
    Write-Host "Skipping consumer PlatformConformanceTests (-SkipConsumerTests)."
}

$summaryPath = Join-Path $ArtifactDir "summary.json"
$results | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $summaryPath -Encoding utf8

Write-Host ""
Write-Host "PASS - observability mechanics summary: $summaryPath"
