#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-MECH-009 — idempotency conformance harness.
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
$OutputDirectory = Resolve-ConformanceOutputDirectory -RepoRoot $RepoRoot -OutputDirectory $OutputDirectory -CheckName 'idempotency'

$fixture = Join-Path $RepoRoot 'fixtures/mechanics/v1/valid/idempotency-state-reserved.json'
$schemaOk = Test-JsonMatchesMechanicalSchema -RepoRoot $RepoRoot -SchemaFileName 'idempotency-state.schema.json' -FixturePath $fixture
$status = 'FAIL'
$exitCode = 1
$details = @()

if ($schemaOk) {
    $details += 'idempotency-state fixture passed schema validation.'
    if ([string]::IsNullOrWhiteSpace($ConsumerName) -or $ConsumerName -eq 'platform') {
        $testProject = Join-Path $RepoRoot 'tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj'
        dotnet test $testProject -c Release --filter 'FullyQualifiedName~IdempotencyKit' --nologo -v q
        if ((Get-LastExitCode) -eq 0) {
            $status = 'PASS'
            $exitCode = 0
            $details += 'IdempotencyConformanceKit ledger checks passed.'
        }
    } else {
        $status = 'PASS'
        $exitCode = 0
        $details += 'Static schema validation passed for consumer mode.'
    }
} else {
    $details += 'idempotency-state fixture failed schema validation.'
}

$result = [pscustomobject]@{
    check = 'idempotency'
    status = $status
    consumerName = $ConsumerName
    states = @('reserved', 'running', 'completed', 'failed', 'expired', 'conflict')
    details = $details
}
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory 'idempotency.json') -Encoding UTF8
exit $exitCode
