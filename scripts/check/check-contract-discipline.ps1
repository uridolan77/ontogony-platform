#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Cross-system contract discipline gate (CONTRACT-DISCIPLINE-001F).

.DESCRIPTION
  Orchestrates ontogony-frontend contract discipline checks with explicit stage policy:

    local-advisory     - skips allowed when sibling backend repos are missing (default)
    manual-ci          - sibling repos required; advisory skips fail the gate
    runtime-lock       - same as manual-ci; intended for pre-release / RC validation
    pr-gate            - reserved for future required PR enforcement

.PARAMETER Mode
  Gate mode. Maps to CONTRACT_DISCIPLINE_MODE for the frontend orchestrator.

.PARAMETER DevRoot
  Folder containing sibling repos (default: parent of ontogony-platform).

.PARAMETER FrontendRoot
  Path to ontogony-frontend (default: DevRoot/ontogony-frontend).

.PARAMETER ArtifactDir
  Output directory for summary artifacts (default: artifacts/contract-discipline under platform root).
#>
param(
    [ValidateSet('local-advisory', 'manual-ci', 'runtime-lock', 'pr-gate')]
    [string] $Mode = 'local-advisory',
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [string] $FrontendRoot = "",
    [string] $ArtifactDir = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
}

if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = (Resolve-Path (Join-Path $RepoRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($FrontendRoot)) {
    $FrontendRoot = Join-Path $DevRoot "ontogony-frontend"
}

if ([string]::IsNullOrWhiteSpace($ArtifactDir)) {
    $ArtifactDir = Join-Path $RepoRoot "artifacts/contract-discipline"
}

$frontendPackageJson = Join-Path $FrontendRoot "package.json"
if (-not (Test-Path -LiteralPath $frontendPackageJson)) {
    throw "Missing ontogony-frontend package.json: $frontendPackageJson"
}

New-Item -ItemType Directory -Force -Path $ArtifactDir | Out-Null

$env:ONTOGONY_DEV_ROOT = $DevRoot
$env:ONTOGONY_PLATFORM_ROOT = $RepoRoot
$env:CONTRACT_DISCIPLINE_MODE = $Mode

Write-Host "Contract discipline gate (CONTRACT-DISCIPLINE-001F)"
Write-Host "  Mode:        $Mode"
Write-Host "  DevRoot:     $DevRoot"
Write-Host "  Platform:    $RepoRoot"
Write-Host "  Frontend:    $FrontendRoot"
Write-Host "  Artifacts:   $ArtifactDir"
Write-Host ""

$requiredInventories = @(
    @{ Repo = "allagma-dotnet"; Inventory = "docs/generated/ALLAGMA_V0_ROUTE_INVENTORY.json" },
    @{ Repo = "conexus-dotnet"; Inventory = "docs/generated/CONEXUS_ROUTE_INVENTORY.json" },
    @{ Repo = "kanon-dotnet"; Inventory = "docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json" }
)

$strictModes = @('manual-ci', 'runtime-lock', 'pr-gate')
if ($strictModes -contains $Mode) {
    Write-Host "Preflight: verifying sibling backend inventories..."
    foreach ($entry in $requiredInventories) {
        $inventoryPath = Join-Path $DevRoot $entry.Repo $entry.Inventory
        if (-not (Test-Path -LiteralPath $inventoryPath)) {
            throw "Strict mode requires sibling inventory: $inventoryPath"
        }
    }
    Write-Host "  Preflight OK"
    Write-Host ""
}

Push-Location $FrontendRoot
try {
    npm run contracts:discipline -- --mode=$Mode
    if ($LASTEXITCODE -ne 0) {
        throw "contracts:discipline failed (exit $LASTEXITCODE)."
    }
}
finally {
    Pop-Location
}

$summarySource = Join-Path $FrontendRoot "docs/generated/contract-discipline.summary.json"
if (-not (Test-Path -LiteralPath $summarySource)) {
    throw "Expected summary artifact was not written: $summarySource"
}

$summaryDest = Join-Path $ArtifactDir "contract-discipline.summary.json"
Copy-Item -LiteralPath $summarySource -Destination $summaryDest -Force

Write-Host ""
Write-Host "PASS - contract discipline gate ($Mode)"
Write-Host "Summary: $summaryDest"
