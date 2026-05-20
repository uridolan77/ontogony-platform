# SYSTEM-DEMO-FLOWS-001 — seed stack and emit operator demo id map (requires APIs on localhost:5081–5083).
param(
    [switch]$SkipSeed
)

$ErrorActionPreference = "Stop"
$scripts = $PSScriptRoot

if (-not $SkipSeed) {
    Write-Host "=== ENV-SEED-001 (prerequisite) ==="
    & (Join-Path $scripts "seed-and-verify-local-working-system.ps1")
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}

Write-Host "=== Operator V1 demo id map ==="
& (Join-Path $scripts "write-operator-v1-demo-ids.ps1")
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

Write-Host "SYSTEM-DEMO-FLOWS-001 demo prep PASS."
Write-Host "Guide: ontogony-platform/docs/operators/OPERATOR_V1_DEMO_GUIDE.md"
Write-Host "Playwright (mocked): cd ontogony-frontend && npm run test:e2e:demo-flows"
