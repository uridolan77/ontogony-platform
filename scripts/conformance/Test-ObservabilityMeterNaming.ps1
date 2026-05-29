#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-MECH-012 — observability meter naming conformance harness.
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
$OutputDirectory = Resolve-ConformanceOutputDirectory -RepoRoot $RepoRoot -OutputDirectory $OutputDirectory -CheckName 'observability-meter-naming'

$fixture = Join-Path $RepoRoot 'fixtures/mechanics/v1/valid/observability-meter-minimal.json'
$schemaOk = Test-JsonMatchesMechanicalSchema -RepoRoot $RepoRoot -SchemaFileName 'observability-meter.schema.json' -FixturePath $fixture
$status = 'FAIL'
$exitCode = 1
$details = @()

if ($schemaOk) {
    $details += 'observability-meter fixture passed schema validation.'
    $testProject = Join-Path $RepoRoot 'tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj'
    dotnet test $testProject -c Release --filter 'FullyQualifiedName~Platform_meter_uses_ontogony_prefix|FullyQualifiedName~Mechanical_meter_fixture_prefix_is_valid|FullyQualifiedName~Prometheus_export_name_follows_mechanical_contract' --nologo -v q
    if ((Get-LastExitCode) -eq 0) {
        $status = 'PASS'
        $exitCode = 0
        $details += 'MeterNamingConformanceTests passed.'
    } else {
        $details += 'MeterNamingConformanceTests failed.'
    }
} else {
    $details += 'observability-meter fixture failed schema validation.'
}

$expectedPrefix = if ([string]::IsNullOrWhiteSpace($ConsumerName) -or $ConsumerName -eq 'platform') {
    'ontogony.platform'
} else {
    "ontogony.$ConsumerName"
}

$result = [pscustomobject]@{
    check = 'observability-meter-naming'
    status = $status
    consumerName = $ConsumerName
    expectedPrefix = $expectedPrefix
    details = $details
}
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory 'observability-meter-naming.json') -Encoding UTF8
exit $exitCode
