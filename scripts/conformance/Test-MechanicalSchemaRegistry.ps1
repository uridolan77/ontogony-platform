#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-MECH-006 — validates mechanical schema registry and fixture coverage.
#>
param(
    [string]$RepoRoot = '',
    [string]$ConsumerName = '',
    [string]$OutputDirectory = ''
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot '_ConformanceCommon.ps1')

$RepoRoot = Resolve-ConformanceRepoRoot -RepoRoot $RepoRoot
$OutputDirectory = Resolve-ConformanceOutputDirectory -RepoRoot $RepoRoot -OutputDirectory $OutputDirectory -CheckName 'schema-registry'

$requiredSchemas = @(
    'error-envelope.schema.json',
    'correlation-headers.schema.json',
    'evidence-reference.schema.json',
    'idempotency-state.schema.json',
    'replay-contract.schema.json',
    'actor-context.schema.json',
    'observability-meter.schema.json',
    'consumer-conformance-report.schema.json',
    'platform-proposal-gate.schema.json'
)

$schemaDir = Join-Path $RepoRoot 'schemas/mechanics/v1'
$missing = @()
foreach ($schema in $requiredSchemas) {
    if (-not (Test-Path -LiteralPath (Join-Path $schemaDir $schema))) {
        $missing += $schema
    }
}

if ($missing.Count -gt 0) {
    Write-ConformanceResult -OutputDirectory $OutputDirectory -FileName 'schema-registry.json' -Result ([pscustomobject]@{
        check = 'mechanical-schema-registry'
        status = 'FAIL'
        missingSchemas = $missing
        path = $schemaDir
    }) -Status 'FAIL'
}

Write-Host 'Running MechanicalSchemaRegistryTests...'
$testProject = Join-Path $RepoRoot 'tests/Ontogony.SystemCompatibility.Tests/Ontogony.SystemCompatibility.Tests.csproj'
dotnet test $testProject -c Release --filter 'FullyQualifiedName~MechanicalSchemaRegistryTests' --nologo
$testPassed = (Get-LastExitCode) -eq 0

$status = if ($testPassed) { 'PASS' } else { 'FAIL' }
Write-ConformanceResult -OutputDirectory $OutputDirectory -FileName 'schema-registry.json' -Result ([pscustomobject]@{
    check = 'mechanical-schema-registry'
    status = $status
    schemaCount = $requiredSchemas.Count
    path = $schemaDir
    consumerName = $ConsumerName
}) -Status $status
