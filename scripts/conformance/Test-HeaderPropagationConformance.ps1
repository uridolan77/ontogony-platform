#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-MECH-007 — header propagation conformance harness.
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
$OutputDirectory = Resolve-ConformanceOutputDirectory -RepoRoot $RepoRoot -OutputDirectory $OutputDirectory -CheckName 'header-propagation'
$targetRoot = if ([string]::IsNullOrWhiteSpace($ConsumerName) -or $ConsumerName -eq 'platform') { $RepoRoot } else { $RepoRoot }

if (-not [string]::IsNullOrWhiteSpace($ConsumerName) -and $ConsumerName -ne 'platform') {
    $devRoot = (Resolve-Path (Join-Path $RepoRoot '..')).Path
    $consumerMap = @{
        conexus = 'conexus-dotnet'
        kanon = 'kanon-dotnet'
        allagma = 'allagma-dotnet'
        metabole = 'metabole-dotnet'
        aisthesis = 'aisthesis-dotnet'
    }
    if ($consumerMap.ContainsKey($ConsumerName)) {
        $candidate = Join-Path $devRoot $consumerMap[$ConsumerName]
        if (Test-Path -LiteralPath $candidate) {
            $targetRoot = $candidate
        }
    }
}

$status = 'PARTIAL'
$details = @()
$exitCode = 2

if ($targetRoot -eq $RepoRoot -or (Split-Path $targetRoot -Leaf) -eq 'ontogony-platform') {
    $skipSiblings = -not (Test-Path -LiteralPath (Join-Path (Split-Path $RepoRoot -Parent) 'allagma-dotnet'))
    $validateScript = Join-Path $RepoRoot 'scripts/validate-header-propagation-contract.ps1'
    if ($skipSiblings) {
        & $validateScript -RepoRoot $RepoRoot -SkipSiblingPaths
    } else {
        & $validateScript -RepoRoot $RepoRoot
    }
    if ($?) {
        $fixture = Join-Path $RepoRoot 'fixtures/mechanics/v1/valid/correlation-headers-minimal.json'
        $schemaOk = Test-JsonMatchesMechanicalSchema -RepoRoot $RepoRoot -SchemaFileName 'correlation-headers.schema.json' -FixturePath $fixture
        if ($schemaOk) {
            $status = 'PASS'
            $exitCode = 0
            $details += 'Platform header contract and correlation-headers fixture validated.'
        } else {
            $status = 'FAIL'
            $exitCode = 1
            $details += 'correlation-headers fixture failed schema validation.'
        }
    } else {
        $status = 'FAIL'
        $exitCode = 1
        $details += 'validate-header-propagation-contract.ps1 failed.'
    }
} else {
    $markers = @(
        'Ontogony.Http',
        'OntogonyIntegrationHeaders',
        'X-Ontogony-Trace-Id',
        'validate-header-propagation',
        'HeaderPropagation'
    )
    $found = @()
    foreach ($marker in $markers) {
        $hits = Get-ChildItem -Path $targetRoot -Recurse -File -Include *.cs,*.md,*.ps1 -ErrorAction SilentlyContinue |
            Where-Object { $_.FullName -notmatch '\\(bin|obj|artifacts|node_modules)\\' } |
            Select-String -Pattern [regex]::Escape($marker) -SimpleMatch -ErrorAction SilentlyContinue |
            Select-Object -First 1
        if ($hits) { $found += $marker }
    }
    if ($found.Count -ge 2) {
        $status = 'PASS'
        $exitCode = 0
        $details += "Static markers found: $($found -join ', ')."
    } else {
        $details += "Insufficient static header propagation markers in $targetRoot."
    }
}

$result = [pscustomobject]@{
    check = 'header-propagation'
    status = $status
    consumerName = $ConsumerName
    repoRoot = $targetRoot
    details = $details
}
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory 'header-propagation.json') -Encoding UTF8
exit $exitCode
