# DOCKER-LOCAL-VERIFY-001 — rebuild (optional), browser provenance probe, and report validation.

param(
    [switch]$Build,
    [switch]$SkipStart,
    [switch]$NoWait,
    [switch]$DisableAutoCaInjection,
    [string]$FrontendBaseUrl = "",
    [string]$ReportPath = "",
    [string]$ExpectedGitSha = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$inspectScript = Join-Path $PSScriptRoot "inspect-frontend-browser-provenance.ps1"
$validateScript = Join-Path $PSScriptRoot "validate-frontend-browser-provenance-report.ps1"
$startScript = Join-Path $PSScriptRoot "start-local-working-system.ps1"

if ($Build) {
    Write-Host "DOCKER-LOCAL-VERIFY-001: rebuilding Docker local working system (CA-aware) ..."
    $startArgs = @("-Build")
    if ($NoWait) { $startArgs += "-NoWait" }
    if ($DisableAutoCaInjection) { $startArgs += "-DisableAutoCaInjection" }
    & $startScript @startArgs
    if ($LASTEXITCODE -ne 0) {
        throw "start-local-working-system.ps1 -Build failed (exit $LASTEXITCODE)."
    }
}
elseif (-not $SkipStart) {
    Write-Host "DOCKER-LOCAL-VERIFY-001: ensuring stack is up (no rebuild) ..."
    & $startScript
    if ($LASTEXITCODE -ne 0) {
        throw "start-local-working-system.ps1 failed (exit $LASTEXITCODE)."
    }
}

$inspectArgs = @{}
if (-not [string]::IsNullOrWhiteSpace($FrontendBaseUrl)) {
    $inspectArgs.FrontendBaseUrl = $FrontendBaseUrl
}
if (-not [string]::IsNullOrWhiteSpace($ReportPath)) {
    $inspectArgs.OutputPath = $ReportPath
}
if (-not [string]::IsNullOrWhiteSpace($ExpectedGitSha)) {
    $inspectArgs.ExpectedGitSha = $ExpectedGitSha
}

& $inspectScript @inspectArgs
if ($LASTEXITCODE -ne 0) {
    throw "inspect-frontend-browser-provenance.ps1 failed (exit $LASTEXITCODE)."
}

$validateArgs = @{}
if (-not [string]::IsNullOrWhiteSpace($ReportPath)) {
    $validateArgs.ReportPath = $ReportPath
}

& $validateScript @validateArgs
if ($LASTEXITCODE -ne 0) {
    throw "validate-frontend-browser-provenance-report.ps1 failed (exit $LASTEXITCODE)."
}

Write-Host "DOCKER-LOCAL-VERIFY-001 verify PASS (rebuild=$Build)."
