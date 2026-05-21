#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Validates SYS-TIGHT-006 operator failure taxonomy contract artifacts.
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

$matrixPath = Join-Path $RepoRoot "docs/system/operator-failure-taxonomy.matrix.json"
$contractPath = Join-Path $RepoRoot "docs/operators/SYSTEM_OPERATOR_FAILURE_TAXONOMY_CONTRACT.md"
$schemaPath = Join-Path $RepoRoot "docs/system/schemas/operator-failure-taxonomy.matrix.schema.json"
$adapterPath = Join-Path $RepoRoot "src/Ontogony.Errors/OperatorFailureTaxonomyAdapter.cs"

foreach ($p in @($matrixPath, $contractPath, $schemaPath, $adapterPath)) {
    if (-not (Test-Path -LiteralPath $p)) {
        throw "Missing required path: $p"
    }
}

$matrix = Get-Content -LiteralPath $matrixPath -Raw | ConvertFrom-Json

if ([string]$matrix.schema -ne "ontogony-operator-failure-taxonomy-v1") {
    throw "schema must be ontogony-operator-failure-taxonomy-v1."
}

$expectedKinds = @(
    "auth_failed",
    "forbidden",
    "validation_failed",
    "not_found",
    "conflict",
    "idempotency_conflict",
    "downstream_unavailable",
    "provider_failed_retryable",
    "provider_failed_terminal",
    "quota_exceeded",
    "timeout",
    "unknown"
)

$required = @($matrix.requiredTaxonomyKinds | ForEach-Object { [string]$_ })
foreach ($kind in $expectedKinds) {
    if ($required -notcontains $kind) {
        throw "requiredTaxonomyKinds must include '$kind'."
    }
}

$mappings = @($matrix.representativeMappings)
if ($mappings.Count -lt 10) {
    throw "representativeMappings must include at least 10 rows."
}

$seenIds = @{}
foreach ($row in $mappings) {
    $id = [string]$row.id
    if ($seenIds.ContainsKey($id)) {
        throw "Duplicate mapping id '$id'."
    }
    $seenIds[$id] = $true

    $taxonomy = [string]$row.expectedTaxonomy
    if ($required -notcontains $taxonomy) {
        throw "Mapping '$id' expectedTaxonomy '$taxonomy' is not listed in requiredTaxonomyKinds."
    }
}

$contract = Get-Content -LiteralPath $contractPath -Raw
if ($contract -notmatch "operator-failure-taxonomy\.matrix\.json") {
    throw "SYSTEM_OPERATOR_FAILURE_TAXONOMY_CONTRACT.md must reference operator-failure-taxonomy.matrix.json."
}

if (-not $SkipSiblingPaths) {
    $feAdapter = Join-Path $DevRoot "ontogony-frontend/src/system/errors/operatorFailureTaxonomy.ts"
    $feBanner = Join-Path $DevRoot "ontogony-frontend/src/system/errors/OperatorFailureBanner.tsx"
    foreach ($p in @($feAdapter, $feBanner)) {
        if (-not (Test-Path -LiteralPath $p)) {
            throw "Missing frontend artifact: $p"
        }
    }

    $allagmaContract = Join-Path $DevRoot "allagma-dotnet/docs/integrations/CROSS_SERVICE_ERROR_CONTRACT.md"
    if (-not (Test-Path -LiteralPath $allagmaContract)) {
        throw "Missing Allagma cross-service contract: $allagmaContract"
    }
}

Write-Host "system-operator-failure-taxonomy OK: $matrixPath"
