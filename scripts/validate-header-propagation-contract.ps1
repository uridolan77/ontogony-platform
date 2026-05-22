#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Validates PLATFORM-9-003 header propagation contract artifacts.
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

$matrixPath = Join-Path $RepoRoot "docs/system/propagation-header.matrix.json"
$contractPath = Join-Path $RepoRoot "docs/contracts/HEADER_PROPAGATION_CONTRACT.md"
$gatePath = Join-Path $RepoRoot "docs/contracts/SYSTEM_COMPATIBILITY_GATE.md"

foreach ($p in @($matrixPath, $contractPath, $gatePath)) {
    if (-not (Test-Path -LiteralPath $p)) {
        throw "Missing required path: $p"
    }
}

$matrix = Get-Content -LiteralPath $matrixPath -Raw | ConvertFrom-Json

if ([string]$matrix.schema -ne "ontogony-propagation-header-v1") {
    throw "propagation-header.matrix.json schema must be ontogony-propagation-header-v1."
}

$frozen = @($matrix.frozenRequiredHeaders | ForEach-Object { [string]$_ })
$required = @(
    "traceparent",
    "X-Correlation-ID",
    "X-Ontogony-Actor-Id",
    "X-Ontogony-Actor-Type",
    "X-Ontogony-Actor-Roles",
    "X-Ontogony-Idempotency-Key",
    "X-Allagma-Run-Id"
)

foreach ($header in $required) {
    if ($frozen -notcontains $header) {
        throw "frozenRequiredHeaders must include '$header'."
    }
}

$contractText = Get-Content -LiteralPath $contractPath -Raw
foreach ($header in $required) {
    if ($contractText -notlike "*$header*") {
        throw "HEADER_PROPAGATION_CONTRACT.md must mention '$header'."
    }
}

if (-not $SkipSiblingPaths) {
    foreach ($entry in $matrix.integrationContracts.PSObject.Properties) {
        $rel = [string]$entry.Value
        $full = Join-Path $DevRoot ($rel -replace '/', [IO.Path]::DirectorySeparatorChar)
        if (-not (Test-Path -LiteralPath $full)) {
            throw "Missing sibling integration doc: $full"
        }
    }
}

Write-Host "Header propagation contract validation passed."
