#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-MECH-003 — mechanics-only proposal gate and platform semantic boundary scan.
.PARAMETER RepoRoot
  Platform repository root.
.PARAMETER ProposalPath
  Optional proposal JSON to validate against platform-proposal-gate.schema.json.
.PARAMETER OutputDirectory
  Directory for machine-readable summary JSON.
#>
param(
    [string]$RepoRoot = '',
    [string]$ProposalPath = '',
    [string]$OutputDirectory = ''
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $governanceScripts = Join-Path $PSScriptRoot '..'
    $RepoRoot = (Resolve-Path (Join-Path $governanceScripts '..')).Path
} else {
    $RepoRoot = (Resolve-Path $RepoRoot).Path
}

if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
    $OutputDirectory = Join-Path $RepoRoot 'artifacts/platform-mechanics-conformance/governance'
}
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

$forbiddenProposalTerms = @(
    'canonical fact',
    'semantic authority',
    'ontology acceptance',
    'model routing',
    'provider fallback policy',
    'human gate policy',
    'workflow orchestration',
    'SLOD mapping algorithm',
    'reconstructability scoring',
    'memory graph semantics',
    'business approval rule',
    'provider ranking',
    'fallback strategy',
    'agent planning',
    'tool selection policy'
)

$proposalResult = $null
$overallStatus = 'PASS'
$exitCode = 0

if (-not [string]::IsNullOrWhiteSpace($ProposalPath)) {
    $proposalPathResolved = Resolve-Path $ProposalPath
    $proposal = Get-Content -LiteralPath $proposalPathResolved -Raw | ConvertFrom-Json

    $proposalText = @(
        [string]$proposal.title,
        [string]$proposal.mechanicalConcern,
        [string]$proposal.reuseJustification,
        [string]$proposal.forbiddenSemanticsAssessment
    ) -join ' '

    $hits = @()
    foreach ($term in $forbiddenProposalTerms) {
        if ($proposalText -match [regex]::Escape($term)) {
            $hits += $term
        }
    }

    $decision = [string]$proposal.ownerRoutingDecision
    if ($hits.Count -gt 0 -and $decision -eq 'accepted_platform_mechanics') {
        $proposalResult = [pscustomobject]@{
            status = 'FAIL'
            proposalPath = $proposalPathResolved.Path
            ownerRoutingDecision = $decision
            forbiddenHits = $hits
            note = 'Proposal claims platform acceptance but contains product semantics.'
        }
        $overallStatus = 'FAIL'
        $exitCode = 1
    } elseif ($decision -eq 'rejected_product_semantics') {
        $proposalResult = [pscustomobject]@{
            status = 'PASS'
            proposalPath = $proposalPathResolved.Path
            ownerRoutingDecision = $decision
            forbiddenHits = $hits
            note = 'Rejected proposal correctly routed away from Platform.'
        }
    } else {
        $proposalResult = [pscustomobject]@{
            status = if ($hits.Count -eq 0) { 'PASS' } else { 'PARTIAL' }
            proposalPath = $proposalPathResolved.Path
            ownerRoutingDecision = $decision
            forbiddenHits = $hits
            note = 'Proposal reviewed; ensure ownerRoutingDecision matches semantics.'
        }
        if ($hits.Count -gt 0 -and $overallStatus -eq 'PASS') {
            $overallStatus = 'PARTIAL'
            $exitCode = 2
        }
    }
}

Write-Host 'Running platform product-semantic boundary scan...'
& (Join-Path $RepoRoot 'scripts/check-no-product-semantics.ps1')
$boundaryExit = $LASTEXITCODE
$boundaryStatus = if ($boundaryExit -eq 0) { 'PASS' } else { 'FAIL' }

if ($boundaryStatus -eq 'FAIL') {
    $overallStatus = 'FAIL'
    $exitCode = 1
}

$summary = [pscustomobject]@{
    check = 'platform-mechanics-only'
    status = $overallStatus
    proposal = $proposalResult
    platformBoundaryScan = [pscustomobject]@{
        status = $boundaryStatus
        script = 'scripts/check-no-product-semantics.ps1'
    }
    reuseTest = 'Can this be reused by Conexus, Kanon, Allagma, Metabole, and Aisthesis without importing product meaning?'
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString('o')
}
$summary | ConvertTo-Json -Depth 10 | Set-Content (Join-Path $OutputDirectory 'mechanics-only-summary.json') -Encoding UTF8

if ($overallStatus -eq 'PASS') {
    Write-Host 'OK: Platform mechanics-only gate passed.'
} else {
    Write-Host "Platform mechanics-only gate: $overallStatus" -ForegroundColor $(if ($overallStatus -eq 'FAIL') { 'Red' } else { 'Yellow' })
}
exit $exitCode
