#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-MECH-010 — outbox/artifact reference conformance harness.
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
$OutputDirectory = Resolve-ConformanceOutputDirectory -RepoRoot $RepoRoot -OutputDirectory $OutputDirectory -CheckName 'outbox-artifact'

$fixture = Join-Path $RepoRoot 'fixtures/mechanics/v1/valid/evidence-reference-minimal.json'
$schemaOk = Test-JsonMatchesMechanicalSchema -RepoRoot $RepoRoot -SchemaFileName 'evidence-reference.schema.json' -FixturePath $fixture
$artifactAssembly = Join-Path $RepoRoot 'src/Ontogony.Artifacts/Ontogony.Artifacts.csproj'
$persistenceAssembly = Join-Path $RepoRoot 'src/Ontogony.Persistence/Ontogony.Persistence.csproj'

$status = 'FAIL'
$exitCode = 1
$details = @()

if ($schemaOk) {
    $details += 'evidence-reference fixture passed schema validation.'
} else {
    $details += 'evidence-reference fixture failed schema validation.'
}

if ((Test-Path -LiteralPath $artifactAssembly) -and (Test-Path -LiteralPath $persistenceAssembly)) {
    $details += 'Ontogony.Artifacts and Ontogony.Persistence packages present.'
    if ($schemaOk) {
        $status = 'PASS'
        $exitCode = 0
    }
} else {
    $details += 'Missing Ontogony.Artifacts or Ontogony.Persistence assembly.'
}

$result = [pscustomobject]@{
    check = 'outbox-artifact'
    status = $status
    consumerName = $ConsumerName
    details = $details
    note = 'Validates neutral evidence reference mechanics, not product payload semantics.'
}
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory 'outbox-artifact.json') -Encoding UTF8
exit $exitCode
