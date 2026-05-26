# Thin wrapper: DEC-RECON-004 API smoke against docker-local stack.
param(
    [string]$FrontendRoot = "",
    [string]$SeedReport = ""
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($FrontendRoot)) {
    $FrontendRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\ontogony-frontend")).Path
}

$defaultSeed = Join-Path $PSScriptRoot "..\docker\local-working-system\artifacts\env-seed-001-report.json"
if (-not [string]::IsNullOrWhiteSpace($SeedReport)) {
    $env:ENV_SEED_001_REPORT = $SeedReport
}
elseif (-not $env:ENV_SEED_001_REPORT) {
    $env:ENV_SEED_001_REPORT = $defaultSeed
}

Push-Location $FrontendRoot
try {
    node ./scripts/decision-reconstructability/run-dec-recon-004-api-smoke.mjs
    exit $LASTEXITCODE
}
finally {
    Pop-Location
}
