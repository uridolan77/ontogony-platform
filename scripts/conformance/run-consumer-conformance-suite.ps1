#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-MECH-011 — runs the platform mechanics consumer conformance suite.
.PARAMETER PlatformRoot
  ontogony-platform repository root.
.PARAMETER ConsumerRoot
  Optional product repo root for sibling checks.
.PARAMETER ConsumerName
  Consumer identifier: platform, conexus, kanon, allagma, metabole, aisthesis.
.PARAMETER OutputDirectory
  Output directory for summary.json.
.PARAMETER FixtureMode
  Run fixture/static checks only (no live services).
#>
param(
    [string]$PlatformRoot = '',
    [string]$ConsumerRoot = '',
    [string]$ConsumerName = 'platform',
    [string]$OutputDirectory = '',
    [switch]$FixtureMode
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot '_ConformanceCommon.ps1')

if ([string]::IsNullOrWhiteSpace($PlatformRoot)) {
    $platformScripts = Join-Path $PSScriptRoot '..'
    $PlatformRoot = (Resolve-Path (Join-Path $platformScripts '..')).Path
} else {
    $PlatformRoot = (Resolve-Path $PlatformRoot).Path
}

if ([string]::IsNullOrWhiteSpace($ConsumerName)) {
    throw 'ConsumerName is required.'
}

if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
    $stamp = Get-Date -Format 'yyyyMMddTHHmmssZ'
    $OutputDirectory = Join-Path $PlatformRoot "artifacts/platform-mechanics-conformance/$ConsumerName/$stamp"
}
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

$scriptMap = @{
    'header-propagation' = 'Test-HeaderPropagationConformance.ps1'
    'error-envelope' = 'Test-ErrorEnvelopeConformance.ps1'
    'idempotency' = 'Test-IdempotencyConformance.ps1'
    'outbox-artifact' = 'Test-OutboxArtifactConformance.ps1'
    'observability-meter-naming' = 'Test-ObservabilityMeterNaming.ps1'
    'no-product-semantics' = 'Test-NoProductSemantics.ps1'
    'schema-registry' = 'Test-MechanicalSchemaRegistry.ps1'
}

$checks = @()
foreach ($entry in $scriptMap.GetEnumerator()) {
    $scriptPath = Join-Path $PSScriptRoot $entry.Value
    if (-not (Test-Path -LiteralPath $scriptPath)) {
        $checks += [pscustomobject]@{
            name = $entry.Key
            status = 'NOT_RUN'
            details = @("Missing script: $scriptPath")
        }
        continue
    }
    $checkOutput = Join-Path $OutputDirectory $entry.Key
    New-Item -ItemType Directory -Force -Path $checkOutput | Out-Null
    if ($entry.Key -eq 'no-product-semantics' -and -not [string]::IsNullOrWhiteSpace($ConsumerRoot)) {
        & $scriptPath -RepoRoot $PlatformRoot -ConsumerName $ConsumerName -OutputDirectory $checkOutput -SiblingMode
    } else {
        & $scriptPath -RepoRoot $PlatformRoot -ConsumerName $ConsumerName -OutputDirectory $checkOutput
    }
    $exit = Get-LastExitCode
    if (-not $?) { $exit = 1 }
    $resultFile = Get-ChildItem -Path $checkOutput -Filter '*.json' | Select-Object -First 1
    $payload = if ($resultFile) {
        Get-Content -LiteralPath $resultFile.FullName -Raw | ConvertFrom-Json
    } else {
        [pscustomobject]@{ check = $entry.Key; status = 'NOT_RUN'; details = @('No result file emitted.') }
    }
    $checkStatus = switch ($exit) {
        0 { 'PASS' }
        1 { 'FAIL' }
        3 { 'NOT_RUN' }
        default { 'PARTIAL' }
    }
    $detailNotes = @()
    if ($payload.PSObject.Properties.Name -contains 'details' -and $null -ne $payload.details) {
        $detailNotes += @($payload.details)
    }
    if ($payload.PSObject.Properties.Name -contains 'note' -and -not [string]::IsNullOrWhiteSpace([string]$payload.note)) {
        $detailNotes += [string]$payload.note
    }
    $checks += [pscustomobject]@{
        name = $entry.Key
        status = $checkStatus
        details = $detailNotes
    }
}

$mode = if ($FixtureMode) { 'Fixture' } else { 'Static' }
$summaryPath = Join-Path $OutputDirectory 'summary.json'
& (Join-Path $PSScriptRoot 'New-ConsumerConformanceReport.ps1') `
    -ConsumerName $ConsumerName `
    -Checks $checks `
    -OutputPath $summaryPath `
    -ConsumerRoot $ConsumerRoot `
    -Mode $mode

$summary = Get-Content -LiteralPath $summaryPath -Raw | ConvertFrom-Json
Write-Host "Conformance summary: $summaryPath (overall=$($summary.overallStatus))"
switch ($summary.overallStatus) {
    'PASS' { exit 0 }
    'FAIL' { exit 1 }
    'NOT_RUN' { exit 3 }
    default { exit 2 }
}
