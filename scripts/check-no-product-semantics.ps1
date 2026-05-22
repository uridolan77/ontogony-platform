#!/usr/bin/env pwsh
# PLAT-9-006 — scans platform source for product-meaning phrases.
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$policyPath = Join-Path $PSScriptRoot 'product-semantic-boundary-policy.json'
$testProject = Join-Path $repoRoot 'tests/Ontogony.Architecture.Tests/Ontogony.Architecture.Tests.csproj'

if (-not (Test-Path -LiteralPath $policyPath)) {
    throw "Missing product semantic boundary policy: $policyPath"
}

if (-not (Test-Path -LiteralPath $testProject)) {
    throw "Missing architecture tests project: $testProject"
}

dotnet test $testProject -c Release --filter "FullyQualifiedName~ProductSemanticLeakageTests.Platform_source_has_no_product_semantic_leakage" --nologo

if ($LASTEXITCODE -ne 0) {
    Write-Host 'Platform product-semantic boundary validation failed.' -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host 'OK: Platform product-semantic boundary scan passed.'
exit 0
