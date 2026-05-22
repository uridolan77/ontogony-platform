#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Validates PLAT-AGUI-000 agent interaction spine contract artifacts.
.DESCRIPTION
  Structural checks on contract, matrix, schemas, fixtures, and cross-links to
  Evidence Spine. JSON Schema validation of JSONL fixtures is enforced by
  AgentInteractionEventSchemaTests (JsonSchema.Net) — run via dotnet test when
  -RunSchemaTests is set or in CI.
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [switch] $SkipSiblingPaths,
    [switch] $RunSchemaTests
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = (Resolve-Path (Join-Path $RepoRoot "..")).Path
}

$contractPath = Join-Path $RepoRoot "docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md"
$matrixPath = Join-Path $RepoRoot "docs/system/agent-interaction-event.matrix.json"
$eventSchemaPath = Join-Path $RepoRoot "docs/schemas/ontogony-agent-interaction-event-v0.schema.json"
$sessionSchemaPath = Join-Path $RepoRoot "docs/schemas/ontogony-agent-interaction-session-v0.schema.json"
$evidenceSpineContractPath = Join-Path $RepoRoot "docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md"
$evidenceResolverContractPath = Join-Path $RepoRoot "docs/operators/AG_UI_EVIDENCE_RESOLVER_CONTRACT.md"
$evidenceGraphSchemaPath = Join-Path $RepoRoot "docs/schemas/ontogony-agent-interaction-evidence-graph-v0.schema.json"
$fixtureDir = Join-Path $RepoRoot "docs/schemas/fixtures/agent-interaction"

foreach ($p in @($contractPath, $matrixPath, $eventSchemaPath, $sessionSchemaPath, $evidenceSpineContractPath, $evidenceResolverContractPath, $evidenceGraphSchemaPath)) {
    if (-not (Test-Path -LiteralPath $p)) {
        throw "Missing required path: $p"
    }
}

if (-not (Test-Path -LiteralPath $fixtureDir)) {
    throw "Missing fixture directory: $fixtureDir"
}

$matrix = Get-Content -LiteralPath $matrixPath -Raw | ConvertFrom-Json

function Require-NonEmptyString([object]$value, [string]$name) {
    if ($null -eq $value -or ($value -isnot [string]) -or [string]::IsNullOrWhiteSpace($value)) {
        throw "Agent interaction spine validation failed: '$name' must be a non-empty string."
    }
}

Require-NonEmptyString $matrix.schema "schema"
if ([string]$matrix.schema -ne "ontogony-agent-interaction-event-matrix-v0") {
    throw "schema must be ontogony-agent-interaction-event-matrix-v0 (found '$($matrix.schema)')."
}

Require-NonEmptyString $matrix.baseline "baseline"
Require-NonEmptyString $matrix.contractDocument "contractDocument"

$requiredFamilies = @($matrix.requiredEventFamilies | ForEach-Object { [string]$_ })
$expectedFamilies = @("RUN", "STEP", "INTERRUPT", "DECISION", "MODEL_CALL", "EVIDENCE")
foreach ($family in $expectedFamilies) {
    if ($requiredFamilies -notcontains $family) {
        throw "requiredEventFamilies must include '$family'."
    }
}

$families = @($matrix.eventFamilies)
$familyNames = @($families | ForEach-Object { [string]$_.family })
foreach ($family in $requiredFamilies) {
    if ($familyNames -notcontains $family) {
        throw "eventFamilies must include required family '$family'."
    }
}

foreach ($entry in $families) {
    Require-NonEmptyString $entry.family "eventFamilies[].family"
    Require-NonEmptyString $entry.owner "eventFamilies[$($entry.family)].owner"
    $requiredEvents = @($entry.requiredEvents)
    if ($requiredEvents.Count -lt 1) {
        throw "eventFamilies[$($entry.family)] must list requiredEvents."
    }
    $sources = @($entry.existingSources)
    if ($sources.Count -lt 1) {
        throw "eventFamilies[$($entry.family)] must list existingSources."
    }
}

$nonGoals = @($matrix.nonGoals | ForEach-Object { [string]$_ })
foreach ($goal in @("hidden_chain_of_thought", "raw_prompt_completion_by_default", "replace_evidence_spine_graph_authority")) {
    if ($nonGoals -notcontains $goal) {
        throw "nonGoals must include '$goal'."
    }
}

$contract = Get-Content -LiteralPath $contractPath -Raw
if ($contract -notmatch "SYSTEM_EVIDENCE_SPINE_CONTRACT") {
    throw "AGENT_INTERACTION_SPINE_CONTRACT.md must reference SYSTEM_EVIDENCE_SPINE_CONTRACT.md."
}
if ($contract -notmatch "agent-interaction-event\.matrix\.json") {
    throw "AGENT_INTERACTION_SPINE_CONTRACT.md must reference agent-interaction-event.matrix.json."
}
if ($contract -notmatch "hidden chain-of-thought|hidden reasoning|chain-of-thought") {
    throw "AGENT_INTERACTION_SPINE_CONTRACT.md must document hidden-reasoning non-goals."
}
if ($contract -notmatch "AG_UI_EVIDENCE_RESOLVER_CONTRACT") {
    throw "AGENT_INTERACTION_SPINE_CONTRACT.md must reference AG_UI_EVIDENCE_RESOLVER_CONTRACT.md."
}

$evidenceGraphSchema = Get-Content -LiteralPath $evidenceGraphSchemaPath -Raw | ConvertFrom-Json
if ([string]$evidenceGraphSchema.properties.schema.const -ne "ontogony-agent-interaction-evidence-graph-v0") {
    throw "Evidence graph schema const mismatch for schema property."
}

$evidenceContract = Get-Content -LiteralPath $evidenceSpineContractPath -Raw
if ($evidenceContract -notmatch "AGENT_INTERACTION_SPINE_CONTRACT") {
    throw "SYSTEM_EVIDENCE_SPINE_CONTRACT.md must reference AGENT_INTERACTION_SPINE_CONTRACT.md (bidirectional cross-link)."
}

$eventSchema = Get-Content -LiteralPath $eventSchemaPath -Raw | ConvertFrom-Json
if ([string]$eventSchema.properties.schema.const -ne "ontogony-agent-interaction-event-v0") {
    throw "Event schema const mismatch for schema property."
}

$fixtureFiles = Get-ChildItem -LiteralPath $fixtureDir -Filter "*.jsonl"
if ($fixtureFiles.Count -lt 1) {
    throw "At least one JSONL fixture required under $fixtureDir"
}

$allowedSources = @(
    "allagma", "kanon", "conexus", "ontogony-frontend", "ontogony-ui",
    "ontogony-platform", "adapter", "fixture"
)

foreach ($file in $fixtureFiles) {
    $lineNo = 0
    foreach ($line in [System.IO.File]::ReadLines($file.FullName)) {
        $lineNo++
        if ([string]::IsNullOrWhiteSpace($line)) { continue }
        $evt = $line | ConvertFrom-Json
        if ([string]$evt.schema -ne "ontogony-agent-interaction-event-v0") {
            throw "$($file.Name):$lineNo schema must be ontogony-agent-interaction-event-v0."
        }
        Require-NonEmptyString $evt.eventId "$($file.Name):$lineNo eventId"
        Require-NonEmptyString $evt.type "$($file.Name):$lineNo type"
        Require-NonEmptyString $evt.timestamp "$($file.Name):$lineNo timestamp"
        Require-NonEmptyString $evt.source "$($file.Name):$lineNo source"
        if ($allowedSources -notcontains [string]$evt.source) {
            throw "$($file.Name):$lineNo invalid source '$($evt.source)'."
        }
        if ($null -eq $evt.ids) {
            throw "$($file.Name):$lineNo ids object is required."
        }
        if ($null -eq $evt.PSObject.Properties["payload"]) {
            throw "$($file.Name):$lineNo payload is required."
        }
    }
}

if (-not $SkipSiblingPaths) {
    $allagmaMatrix = Join-Path $DevRoot "allagma-dotnet/docs/system/allagma-feature-connection.matrix.json"
    if (-not (Test-Path -LiteralPath $allagmaMatrix)) {
        throw "Missing Allagma feature matrix: $allagmaMatrix"
    }

    $kanonHandoff = Join-Path $DevRoot "kanon-dotnet/docs/operators/KANON_EVIDENCE_SPINE_HANDOFF.md"
    if (-not (Test-Path -LiteralPath $kanonHandoff)) {
        throw "Missing Kanon handoff: $kanonHandoff"
    }

    $conexusFlow = Join-Path $DevRoot "conexus-dotnet/docs/operators/MODEL_CALL_EVIDENCE_FLOW.md"
    if (-not (Test-Path -LiteralPath $conexusFlow)) {
        throw "Missing Conexus model-call evidence flow: $conexusFlow"
    }
}

if ($RunSchemaTests) {
    $testProj = Join-Path $RepoRoot "tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj"
    if (-not (Test-Path -LiteralPath $testProj)) {
        throw "Missing test project: $testProj"
    }
    dotnet test $testProj --filter "FullyQualifiedName~AgentInteraction" --no-restore:$false
    if ($LASTEXITCODE -ne 0) {
        throw "Agent interaction schema tests failed (exit $LASTEXITCODE)."
    }
}

Write-Host "agent-interaction-spine OK: $matrixPath ($($fixtureFiles.Count) JSONL fixture file(s))"
