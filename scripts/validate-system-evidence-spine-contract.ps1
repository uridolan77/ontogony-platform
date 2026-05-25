#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Validates SYS-TIGHT-002 system evidence spine contract artifacts.
.DESCRIPTION
  Checks system-evidence-spine-resolution.matrix.json structure, required identifier kinds,
  cross-links to taxonomy/contract docs, and (when siblings exist) Kanon handoff + Conexus
  evidence-flow supplements.
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

$matrixPath = Join-Path $RepoRoot "docs/system/system-evidence-spine-resolution.matrix.json"
$contractPath = Join-Path $RepoRoot "docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md"
$taxonomyPath = Join-Path $RepoRoot "docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md"
$graphTaxonomyPath = Join-Path $RepoRoot "docs/system/EVIDENCE_SPINE_GRAPH_TAXONOMY.md"
$schemaPath = Join-Path $RepoRoot "docs/schemas/ontogony-cross-service-evidence-spine-bundle-v1.schema.json"

foreach ($p in @($matrixPath, $contractPath, $taxonomyPath, $graphTaxonomyPath, $schemaPath)) {
    if (-not (Test-Path -LiteralPath $p)) {
        throw "Missing required path: $p"
    }
}

$matrix = Get-Content -LiteralPath $matrixPath -Raw | ConvertFrom-Json

function Require-NonEmptyString([object]$value, [string]$name) {
    if ($null -eq $value -or ($value -isnot [string]) -or [string]::IsNullOrWhiteSpace($value)) {
        throw "Evidence spine contract validation failed: '$name' must be a non-empty string."
    }
}

Require-NonEmptyString $matrix.schema "schema"
if ([string]$matrix.schema -ne "ontogony-system-evidence-spine-resolution-v1") {
    throw "schema must be ontogony-system-evidence-spine-resolution-v1 (found '$($matrix.schema)')."
}

Require-NonEmptyString $matrix.baseline "baseline"
Require-NonEmptyString $matrix.contractDocument "contractDocument"
Require-NonEmptyString $matrix.taxonomyDocument "taxonomyDocument"

if ([string]$matrix.unresolvedEdgePolicy -ne "non_fatal_explicit") {
    throw "unresolvedEdgePolicy must be 'non_fatal_explicit'."
}

$requiredKinds = @($matrix.requiredIdentifierKinds | ForEach-Object { [string]$_ })
$expectedRequired = @(
    "allagmaRunId",
    "kanonDecisionId",
    "conexusModelCallId",
    "conexusRouteDecisionId",
    "traceId",
    "correlationId",
    "humanGateId",
    "domainPackId"
)
foreach ($kind in $expectedRequired) {
    if ($requiredKinds -notcontains $kind) {
        throw "requiredIdentifierKinds must include '$kind'."
    }
}

$entries = @($matrix.identifiers)
$kindsInMatrix = @($entries | ForEach-Object { [string]$_.kind })
foreach ($kind in $requiredKinds) {
    if ($kindsInMatrix -notcontains $kind) {
        throw "identifiers[] must include required kind '$kind'."
    }
}

$supplementalKinds = @()
if ($null -ne $matrix.supplementalRequiredIdentifierKinds) {
    $supplementalKinds = @($matrix.supplementalRequiredIdentifierKinds | ForEach-Object { [string]$_ })
}
$expectedSupplemental = @("allagmaReplayId", "replayBundleId", "replayDeltaId")
foreach ($kind in $expectedSupplemental) {
    if ($supplementalKinds -notcontains $kind) {
        throw "supplementalRequiredIdentifierKinds must include '$kind'."
    }
    if ($kindsInMatrix -notcontains $kind) {
        throw "identifiers[] must include supplemental kind '$kind'."
    }
}

Require-NonEmptyString $matrix.graphTaxonomyDocument "graphTaxonomyDocument"
if ([string]$matrix.graphTaxonomyDocument -ne "docs/system/EVIDENCE_SPINE_GRAPH_TAXONOMY.md") {
    throw "graphTaxonomyDocument must reference docs/system/EVIDENCE_SPINE_GRAPH_TAXONOMY.md."
}

foreach ($entry in $entries) {
    Require-NonEmptyString $entry.kind "identifiers[].kind"
    Require-NonEmptyString $entry.owner "identifiers[$($entry.kind)].owner"
    if (-not $entry.resolverRoot) {
        throw "identifiers[$($entry.kind)] must set resolverRoot=true for v1."
    }
    $routes = @($entry.routes)
    if ($routes.Count -lt 1) {
        throw "identifiers[$($entry.kind)] must include at least one route."
    }
    $hasPrimary = $false
    foreach ($route in $routes) {
        Require-NonEmptyString $route.method "route.method"
        Require-NonEmptyString $route.path "route.path"
        Require-NonEmptyString $route.role "route.role"
        if ($route.role -eq "primary") { $hasPrimary = $true }
    }
    if (-not $hasPrimary -and $entry.kind -notin @("traceId", "correlationId")) {
        throw "identifiers[$($entry.kind)] should include a primary route (or discovery roles for trace/correlation)."
    }
}

$taxonomy = Get-Content -LiteralPath $taxonomyPath -Raw
if ($taxonomy -notmatch "SYSTEM_EVIDENCE_SPINE_CONTRACT") {
    throw "EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md must reference SYSTEM_EVIDENCE_SPINE_CONTRACT.md."
}

$contract = Get-Content -LiteralPath $contractPath -Raw
if ($contract -notmatch "system-evidence-spine-resolution\.matrix\.json") {
    throw "SYSTEM_EVIDENCE_SPINE_CONTRACT.md must reference system-evidence-spine-resolution.matrix.json."
}

if (-not $SkipSiblingPaths) {
    $kanonHandoffPath = Join-Path $DevRoot "kanon-dotnet/docs/generated/KANON_EVIDENCE_SPINE_ENTRYPOINTS.json"
    if (-not (Test-Path -LiteralPath $kanonHandoffPath)) {
        throw "Missing Kanon handoff: $kanonHandoffPath"
    }
    $kanonHandoff = Get-Content -LiteralPath $kanonHandoffPath -Raw | ConvertFrom-Json
    $kanonIds = @($kanonHandoff.entrypoints | ForEach-Object { [string]$_.id })
    foreach ($entry in $entries) {
        $handoffProp = $entry.PSObject.Properties["kanonHandoffId"]
        if ($null -ne $handoffProp -and -not [string]::IsNullOrWhiteSpace([string]$handoffProp.Value)) {
            $hid = [string]$handoffProp.Value
            if ($kanonIds -notcontains $hid) {
                throw "identifiers[$($entry.kind)].kanonHandoffId '$hid' not found in KANON_EVIDENCE_SPINE_ENTRYPOINTS.json."
            }
        }
    }

    $conexusFlowPath = Join-Path $DevRoot "conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md"
    if (-not (Test-Path -LiteralPath $conexusFlowPath)) {
        throw "Missing Conexus evidence flow: $conexusFlowPath"
    }

    $feResolver = Join-Path $DevRoot "ontogony-frontend/src/evidence-spine/resolveEvidenceSpine.ts"
    if (-not (Test-Path -LiteralPath $feResolver)) {
        throw "Missing frontend resolver: $feResolver"
    }
}

Write-Host "system-evidence-spine-contract OK: $matrixPath"
