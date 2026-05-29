#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Builds a consumer conformance summary JSON document.
#>
param(
    [Parameter(Mandatory = $true)][string]$ConsumerName,
    [Parameter(Mandatory = $true)][array]$Checks,
    [Parameter(Mandatory = $true)][string]$OutputPath,
    [string]$ConsumerRoot = '',
    [string]$Mode = 'Static'
)

$ErrorActionPreference = 'Stop'

$overall = if ($Checks | Where-Object { $_.status -eq 'FAIL' }) {
    'FAIL'
} elseif ($Checks | Where-Object { $_.status -eq 'NOT_RUN' }) {
    'PARTIAL'
} elseif ($Checks | Where-Object { $_.status -eq 'PARTIAL' }) {
    'PARTIAL'
} else {
    'PASS'
}

[pscustomobject]@{
    schema = 'ontogony.consumer-conformance-report.v1'
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString('o')
    consumerName = $ConsumerName
    consumerRoot = if ([string]::IsNullOrWhiteSpace($ConsumerRoot)) { $null } else { $ConsumerRoot }
    mode = $Mode
    overallStatus = $overall
    checks = $Checks
    nonClaims = @(
        'No production readiness claim',
        'No product semantic approval'
    )
} | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $OutputPath -Encoding UTF8
