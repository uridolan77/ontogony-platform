#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Validates PLATFORM-9-002 cross-service error envelope conformance artifacts.
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [switch] $SkipSiblingPaths
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = (Resolve-Path (Join-Path $RepoRoot "..")).Path
}

$matrixPath = Join-Path $RepoRoot "docs/system/cross-service-error-envelope.matrix.json"
$schemaPath = Join-Path $RepoRoot "docs/system/schemas/cross-service-error-envelope-v0.schema.json"
$schemaV1Path = Join-Path $RepoRoot "docs/schemas/ontogony-cross-service-error-envelope-v1.schema.json"
$contractPath = Join-Path $RepoRoot "docs/contracts/CROSS_SERVICE_ERROR_ENVELOPE_V1.md"
$gatePath = Join-Path $RepoRoot "docs/contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md"
$taxonomyPath = Join-Path $RepoRoot "docs/system/operator-failure-taxonomy.matrix.json"

foreach ($p in @($matrixPath, $schemaPath, $schemaV1Path, $contractPath, $gatePath, $taxonomyPath)) {
    if (-not (Test-Path -LiteralPath $p)) {
        throw "Missing required path: $p"
    }
}

$matrix = Get-Content -LiteralPath $matrixPath -Raw | ConvertFrom-Json

if ([string]$matrix.schema -ne "ontogony-cross-service-error-envelope-v1") {
    throw "cross-service-error-envelope.matrix.json schema must be ontogony-cross-service-error-envelope-v1."
}

$requiredFields = @($matrix.requiredEnvelopeFields | ForEach-Object { [string]$_ })
foreach ($field in @("code", "message", "system")) {
    if ($requiredFields -notcontains $field) {
        throw "requiredEnvelopeFields must include '$field'."
    }
}

foreach ($sample in @($matrix.platformSamples)) {
    $rel = [string]$sample.path
    $samplePath = Join-Path $RepoRoot ($rel -replace '/', [IO.Path]::DirectorySeparatorChar)
    if (-not (Test-Path -LiteralPath $samplePath)) {
        throw "Missing platform sample: $samplePath"
    }
}

$contract = Get-Content -LiteralPath $contractPath -Raw
if ($contract -notmatch "CrossServiceErrorEnvelope") {
    throw "CROSS_SERVICE_ERROR_ENVELOPE_V1.md must reference CrossServiceErrorEnvelope."
}

$gate = Get-Content -LiteralPath $gatePath -Raw
if ($gate -notmatch "cross-service-error-envelope\.matrix\.json") {
    throw "CROSS_SERVICE_ERROR_ENVELOPE_GATE.md must reference cross-service-error-envelope.matrix.json."
}

if (-not $SkipSiblingPaths) {
    foreach ($entry in $matrix.integrationContracts.PSObject.Properties) {
        $rel = [string]$entry.Value
        $full = Join-Path $DevRoot ($rel -replace '/', [IO.Path]::DirectorySeparatorChar)
        if (-not (Test-Path -LiteralPath $full)) {
            throw "Missing integration contract for $($entry.Name): $full"
        }
    }

    $feAdapter = Join-Path $DevRoot "ontogony-frontend/src/system/errors/operatorFailureTaxonomy.ts"
    if (-not (Test-Path -LiteralPath $feAdapter)) {
        throw "Missing frontend taxonomy module: $feAdapter"
    }
}

Write-Host "cross-service-error-envelope OK: $matrixPath ($(@($matrix.platformSamples).Count) platform samples)"
