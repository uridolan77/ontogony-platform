#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-MECH-008 — error envelope conformance harness.
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
$OutputDirectory = Resolve-ConformanceOutputDirectory -RepoRoot $RepoRoot -OutputDirectory $OutputDirectory -CheckName 'error-envelope'

$validFixture = Join-Path $RepoRoot 'fixtures/mechanics/v1/valid/error-envelope-minimal.json'
$invalidFixture = Join-Path $RepoRoot 'fixtures/mechanics/v1/invalid/error-envelope-missing-code.json'
$status = 'FAIL'
$details = @()
$exitCode = 1

$validOk = Test-JsonMatchesMechanicalSchema -RepoRoot $RepoRoot -SchemaFileName 'error-envelope.schema.json' -FixturePath $validFixture
if ($validOk) {
    $details += 'Valid error-envelope fixture passed.'
} else {
    $details += 'Valid error-envelope fixture failed.'
}

$env:ONTOGONY_MECH_EXPECT_INVALID = '1'
try {
    $invalidOk = Test-JsonMatchesMechanicalSchema -RepoRoot $RepoRoot -SchemaFileName 'error-envelope.schema.json' -FixturePath $invalidFixture
} finally {
    Remove-Item Env:ONTOGONY_MECH_EXPECT_INVALID -ErrorAction SilentlyContinue
}
if ($invalidOk) {
    $details += 'Invalid error-envelope fixture correctly rejected.'
} else {
    $details += 'Invalid error-envelope fixture was not rejected.'
}

if ($validOk -and $invalidOk) {
  if ([string]::IsNullOrWhiteSpace($ConsumerName) -or $ConsumerName -eq 'platform') {
    $testProject = Join-Path $RepoRoot 'tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj'
    dotnet test $testProject -c Release --filter 'FullyQualifiedName~ErrorEnvelopeKit' --nologo -v q
    if ((Get-LastExitCode) -eq 0) {
      $status = 'PASS'
      $exitCode = 0
      $details += 'ErrorEnvelopeConformanceKit baseline passed.'
    }
  } else {
    $status = 'PASS'
    $exitCode = 0
    $details += 'Schema fixtures passed for consumer static mode.'
  }
}

$result = [pscustomobject]@{
    check = 'error-envelope'
    status = $status
    consumerName = $ConsumerName
    required = @('code', 'message')
    details = $details
}
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory 'error-envelope.json') -Encoding UTF8
exit $exitCode
