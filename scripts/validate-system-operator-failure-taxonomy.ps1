#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Validates SYS-TIGHT-006 / 006A operator failure taxonomy contract artifacts.
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [switch] $SkipSiblingPaths,
    [switch] $SkipSchemaValidation
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
$registryPath = Join-Path $RepoRoot "docs/system/system-protocol-registry.json"

foreach ($p in @($matrixPath, $contractPath, $schemaPath, $adapterPath, $registryPath)) {
    if (-not (Test-Path -LiteralPath $p)) {
        throw "Missing required path: $p"
    }
}

$matrix = Get-Content -LiteralPath $matrixPath -Raw | ConvertFrom-Json

if ([string]$matrix.schema -ne "ontogony-operator-failure-taxonomy-v1") {
    throw "schema must be ontogony-operator-failure-taxonomy-v1."
}

if ([string]::IsNullOrWhiteSpace($matrix.baseline)) {
    throw "baseline is required."
}

if ([string]::IsNullOrWhiteSpace($matrix.contractDocument)) {
    throw "contractDocument is required."
}

$impl = $matrix.implementation
if ($null -eq $impl -or [string]::IsNullOrWhiteSpace($impl.platformAdapter)) {
    throw "implementation.platformAdapter is required."
}

if (-not (Test-Path -LiteralPath (Join-Path $RepoRoot $impl.platformAdapter))) {
    throw "implementation.platformAdapter path missing: $($impl.platformAdapter)"
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

$producedKinds = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$seenIds = @{}
foreach ($row in $mappings) {
    $id = [string]$row.id
    if ([string]::IsNullOrWhiteSpace($id)) {
        throw "representativeMappings row missing id."
    }
    if ($seenIds.ContainsKey($id)) {
        throw "Duplicate mapping id '$id'."
    }
    $seenIds[$id] = $true

    foreach ($field in @("sourceSystem", "wireShape", "envelope", "expectedTaxonomy")) {
        if ($null -eq $row.$field -or [string]::IsNullOrWhiteSpace([string]$row.$field)) {
            throw "Mapping '$id' missing required field '$field'."
        }
    }

    $envelope = $row.envelope
    foreach ($field in @("code", "message", "system")) {
        if ($null -eq $envelope.$field -or [string]::IsNullOrWhiteSpace([string]$envelope.$field)) {
            throw "Mapping '$id' envelope missing '$field'."
        }
    }

    $taxonomy = [string]$row.expectedTaxonomy
    if ($required -notcontains $taxonomy) {
        throw "Mapping '$id' expectedTaxonomy '$taxonomy' is not listed in requiredTaxonomyKinds."
    }

    [void]$producedKinds.Add($taxonomy)
}

foreach ($kind in $required) {
    if (-not $producedKinds.Contains($kind)) {
        throw "requiredTaxonomyKind '$kind' has no representative mapping (requiredTaxonomyKinds ⊆ produced taxonomy kinds)."
    }
}

$contract = Get-Content -LiteralPath $contractPath -Raw
if ($contract -notmatch "operator-failure-taxonomy\.matrix\.json") {
    throw "SYSTEM_OPERATOR_FAILURE_TAXONOMY_CONTRACT.md must reference operator-failure-taxonomy.matrix.json."
}

$registry = Get-Content -LiteralPath $registryPath -Raw | ConvertFrom-Json
$taxonomyProtocol = @($registry.protocols | Where-Object { $_.id -eq "system-operator-failure-taxonomy" })
if ($taxonomyProtocol.Count -ne 1) {
    throw "system-protocol-registry.json must include exactly one system-operator-failure-taxonomy protocol."
}

if (-not $SkipSchemaValidation) {
    $schema = Get-Content -LiteralPath $schemaPath -Raw | ConvertFrom-Json
    if ([string]$schema.'$schema' -notmatch "json-schema") {
        throw "operator-failure-taxonomy.matrix.schema.json must declare a JSON Schema meta-schema."
    }
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

Write-Host "system-operator-failure-taxonomy OK: $matrixPath ($($mappings.Count) mappings, $($producedKinds.Count) taxonomy kinds covered)"
