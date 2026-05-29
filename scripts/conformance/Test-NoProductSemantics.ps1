#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-MECH-013 — no-product-semantics conformance harness.
#>
param(
    [string]$RepoRoot = '',
    [string]$ConsumerName = '',
    [string]$OutputDirectory = '',
    [switch]$SiblingMode
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. (Join-Path $PSScriptRoot '_ConformanceCommon.ps1')

$RepoRoot = Resolve-ConformanceRepoRoot -RepoRoot $RepoRoot
$OutputDirectory = Resolve-ConformanceOutputDirectory -RepoRoot $RepoRoot -OutputDirectory $OutputDirectory -CheckName 'no-product-semantics'

$scanRoot = $RepoRoot
if ($SiblingMode -and -not [string]::IsNullOrWhiteSpace($ConsumerName) -and $ConsumerName -ne 'platform') {
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
            $scanRoot = $candidate
        }
    }
}

$status = 'PARTIAL'
$exitCode = 2
$details = @()

if ($scanRoot -eq $RepoRoot -or (Split-Path $scanRoot -Leaf) -eq 'ontogony-platform') {
    & (Join-Path $RepoRoot 'scripts/check-no-product-semantics.ps1')
    if ((Get-LastExitCode) -eq 0) {
        $status = 'PASS'
        $exitCode = 0
        $details += 'Platform product-semantic boundary scan passed.'
    } else {
        $status = 'FAIL'
        $exitCode = 1
        $details += 'Platform product-semantic boundary scan failed.'
    }
} else {
    $policyPath = Join-Path $RepoRoot 'scripts/product-semantic-boundary-policy.json'
    if (-not (Test-Path -LiteralPath $policyPath)) {
        $status = 'NOT_RUN'
        $exitCode = 3
        $details += 'Platform policy file unavailable for sibling scan.'
    } else {
        $policy = Get-Content -LiteralPath $policyPath -Raw | ConvertFrom-Json
        $hits = @()
        $files = Get-ChildItem -Path (Join-Path $scanRoot 'src') -Recurse -Filter *.cs -ErrorAction SilentlyContinue
        foreach ($file in $files) {
            $text = Get-Content $file.FullName -Raw
            foreach ($phrase in $policy.forbiddenPhrases) {
                if ($text -match [regex]::Escape($phrase)) {
                    $hits += [pscustomobject]@{
                        file = $file.FullName
                        phrase = $phrase
                        suggestedOwner = $ConsumerName
                    }
                }
            }
        }
        if ($hits.Count -eq 0) {
            $status = 'PASS'
            $exitCode = 0
            $details += "No forbidden platform semantics detected in $scanRoot/src."
        } else {
            $status = 'PARTIAL'
            $exitCode = 2
            $details += "Found $($hits.Count) lexical hits requiring product-owner review."
        }
    }
}

$result = [pscustomobject]@{
    check = 'no-product-semantics'
    status = $status
    consumerName = $ConsumerName
    scanRoot = $scanRoot
    siblingMode = $SiblingMode.IsPresent
    details = $details
}
$result | ConvertTo-Json -Depth 6 | Set-Content (Join-Path $OutputDirectory 'no-product-semantics.json') -Encoding UTF8
exit $exitCode
