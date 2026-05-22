#!/usr/bin/env pwsh
<#
.SYNOPSIS
  PLAT-9-004 — validates public API baseline governance (alias for validate-public-api-governance.ps1).
#>
param(
    [switch] $Strict
)

$ErrorActionPreference = "Stop"
$governance = Join-Path $PSScriptRoot "validate-public-api-governance.ps1"
if (-not (Test-Path -LiteralPath $governance)) {
    throw "Missing $governance"
}

if ($Strict) {
    & $governance -Strict
} else {
    & $governance
}

exit $LASTEXITCODE
