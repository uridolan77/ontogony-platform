#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Wave 7 - start the canonical local Ontogony operator system.

.DESCRIPTION
  One command from ontogony-platform to bring up Postgres, Kanon, Conexus, Allagma,
  and ontogony-frontend, seed governed demo data, and emit operator demo IDs.

  Boundary: Docker-local development credentials only - not production readiness.

.EXAMPLE
  ./scripts/start-local-ontogony-system.ps1 -Build -OpenBrowser

.EXAMPLE
  ./scripts/start-local-ontogony-system.ps1 -Quick
#>
param(
    [string]$DevRoot = "",
    [switch]$Build,
    [switch]$SkipFrontend,
    [switch]$NoWait,
    [switch]$Quick,
    [switch]$OpenBrowser,
    [switch]$SkipWorkspaceCheck
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. (Join-Path $PSScriptRoot "lib/local-ontogony-system.ps1")

$paths = Get-LocalOntogonyPaths -DevRoot $DevRoot
$DevRoot = $paths.DevRoot

Write-Host ""
Write-Host "=== Wave 7 - start local Ontogony system ===" -ForegroundColor Cyan
Write-Host "Boundary: Docker-local operator system; not production readiness."
Write-Host "  DevRoot:  $DevRoot"
Write-Host "  Platform: $($paths.RepoRoot)"
Write-Host ""

if (-not $SkipWorkspaceCheck) {
    Write-Host "Checking six-repo workspace layout ..."
    Test-SixRepoWorkspaceLayout -DevRoot $DevRoot
}

$startArgs = @{}
if ($Build) { $startArgs["Build"] = $true }
if ($SkipFrontend) { $startArgs["SkipFrontend"] = $true }
if ($NoWait) { $startArgs["NoWait"] = $true }

Write-Host ""
Write-Host "Starting Docker local working system ..."
Invoke-LocalOntogonyScript `
    -ScriptPath (Join-Path $paths.DockerScripts "start-local-working-system.ps1") `
    -Arguments $startArgs

if ($Quick) {
    Write-Host ""
    Write-Host "Quick start complete (stack only; no seed/guided flow)."
    Write-Host "Next: ./scripts/validate-local-ontogony-system.ps1"
    Write-Host "Guide: docs/operators/OPERATOR_V1_DEMO_GUIDE.md"
    exit 0
}

Write-Host ""
Write-Host "Running governed demo seed + durability proof (ENV-DOCKER-RUN-001) ..."
$guidedArgs = @{}
if ($SkipFrontend) { $guidedArgs["SkipFrontend"] = $true }
Invoke-LocalOntogonyScript `
    -ScriptPath (Join-Path $paths.DockerScripts "run-docker-guided-main-flow.ps1") `
    -Arguments $guidedArgs

Write-Host ""
Write-Host "Writing operator demo ID map ..."
Invoke-LocalOntogonyScript `
    -ScriptPath (Join-Path $paths.DockerScripts "run-operator-v1-demo-prep.ps1") `
    -Arguments @{ SkipSeed = $true }

$frontendBaseUrl = Get-LocalOntogonyFrontendBaseUrl -Paths $paths
$demo = Get-Content -Raw -LiteralPath $paths.DemoIdsPath | ConvertFrom-Json
$subjectRunId = [string]$demo.flows.'simple-governed-run'.runId

Write-Host ""
Write-Host "Local Ontogony system is ready." -ForegroundColor Green
Write-Host ""
Write-Host "Golden journey entry points:"
Write-Host "  Frontend:       $frontendBaseUrl"
Write-Host "  System posture: $frontendBaseUrl/system"
Write-Host "  Settings:       $frontendBaseUrl/settings"
Write-Host "  Governed run:   $frontendBaseUrl/allagma/runs/$subjectRunId"
Write-Host "  Evidence spine: $frontendBaseUrl/system/evidence-spine"
Write-Host ""
Write-Host "Validate: ./scripts/validate-local-ontogony-system.ps1"
Write-Host "Guide:    docs/operators/OPERATOR_V1_DEMO_GUIDE.md"

if ($OpenBrowser -and -not $SkipFrontend) {
    $openScript = Join-Path $paths.FrontendRoot "scripts\docker\open-docker-local-operator-pages.ps1"
    if (Test-Path -LiteralPath $openScript) {
        Write-Host ""
        Write-Host "Opening operator pages in the default browser ..."
        & $openScript -FrontendBaseUrl $frontendBaseUrl -ReportPath $paths.GuidedReportPath
    }
    else {
        Write-Warning "Browser opener not found: $openScript"
    }
}
