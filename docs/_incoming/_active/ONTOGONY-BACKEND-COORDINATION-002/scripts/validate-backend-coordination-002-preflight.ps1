#!/usr/bin/env pwsh
# Preflight checks for ONTOGONY-BACKEND-COORDINATION-002 slices.
param(
    [string]$DevRoot = "C:\dev",
    [Parameter(Mandatory = $true)]
    [ValidateSet(
        'BACKEND-REPO-DOCS-ORDER-002',
        'SYSTEM-COMPATIBILITY-MATRIX-001',
        'SHARED-ERROR-CONTRACT-001',
        'CROSS-REPO-IDENTITY-CORRELATION-001',
        'ALLAGMA-CONEXUS-MODEL-ALIAS-001',
        'BACKEND-SYSTEM-E2E-001',
        'AISTHESIS-RECONSTRUCTABILITY-SPINE-001',
        'METABOLE-DATA-SPINE-HARDENING-001'
    )]
    [string]$Slice
)

$ErrorActionPreference = 'Stop'
$repos = @(
    'ontogony-platform',
    'conexus-dotnet',
    'kanon-dotnet',
    'allagma-dotnet',
    'metabole-dotnet',
    'aisthesis-dotnet'
)

Write-Host "ONTOGONY-BACKEND-COORDINATION-002 preflight — slice: $Slice" -ForegroundColor Cyan
Write-Host "DevRoot: $DevRoot"

$missing = @()
foreach ($r in $repos) {
    $path = Join-Path $DevRoot $r
    if (-not (Test-Path -LiteralPath $path)) {
        $missing += $r
    }
}

if ($missing.Count -gt 0) {
    Write-Warning "Missing sibling repos: $($missing -join ', ')"
}

$pkgRoot = Join-Path $DevRoot 'ontogony-platform\docs\_incoming\_active\ONTOGONY-BACKEND-COORDINATION-002'
if (-not (Test-Path -LiteralPath $pkgRoot)) {
    throw "Package not found: $pkgRoot"
}

$sliceReadme = Join-Path $pkgRoot "slices\$Slice\README.md"
if (-not (Test-Path -LiteralPath $sliceReadme)) {
    throw "Slice README not found: $sliceReadme"
}

Write-Host "OK: package and slice folder present" -ForegroundColor Green
Write-Host "Next: read $sliceReadme"
Write-Host "Validation plan: $pkgRoot\07_CROSS_REPO_VALIDATION_PLAN.md"

exit 0
