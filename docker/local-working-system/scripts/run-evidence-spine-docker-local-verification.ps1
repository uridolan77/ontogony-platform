# EVIDENCE-SPINE-008 — Docker-local manual verification for evidence spine workbench.
# Rebuilds frontend with fresh provenance, then prints operator checklist for paste-any-ID flows.

param(
    [switch]$Build,
    [switch]$Live,
    [switch]$Seed,
    [switch]$SkipProvenanceVerify,
    [string]$FrontendBaseUrl = "http://localhost:5175"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

$verifyScript = Join-Path $PSScriptRoot "verify-frontend-browser-provenance.ps1"
$composeRoot = Get-DockerLocalComposeRoot
$reportPath = Join-Path $composeRoot "artifacts\evidence-spine-008-docker-local-checklist.json"
$reportDir = Split-Path -Parent $reportPath
if (-not (Test-Path -LiteralPath $reportDir)) {
    New-Item -ItemType Directory -Path $reportDir -Force | Out-Null
}

Write-Host ""
Write-Host "=== EVIDENCE-SPINE-008 Docker-local verification ===" -ForegroundColor Cyan
Write-Host ""

if ($Live) {
    $liveScript = Join-Path $PSScriptRoot "run-evidence-spine-008a-docker-live-verification.ps1"
    & $liveScript -Build:$Build -Seed:$Seed -SkipProvenanceVerify:$SkipProvenanceVerify -FrontendBaseUrl $FrontendBaseUrl
    if ($LASTEXITCODE -ne 0) {
        throw "EVIDENCE-SPINE-008A live verification failed."
    }
    exit 0
}

if (-not $SkipProvenanceVerify) {
    if ($Build) {
        Write-Host "[1/4] Rebuild frontend image and verify browser provenance (DOCKER-LOCAL-VERIFY-001) ..."
        & $verifyScript -Build -FrontendBaseUrl $FrontendBaseUrl
    }
    else {
        Write-Host "[1/4] Verify browser provenance (no rebuild) ..."
        & $verifyScript -FrontendBaseUrl $FrontendBaseUrl
    }
    if ($LASTEXITCODE -ne 0) {
        throw "Frontend provenance verification failed. Re-run with -Build after frontend changes."
    }
}
else {
    Write-Host "[1/4] Skipped provenance verify (-SkipProvenanceVerify)."
}

Write-Host ""
Write-Host "[2/4] Operator checklist - capture IDs from guided flow or existing runs:" -ForegroundColor Yellow
Write-Host "  - Allagma run id       -> /allagma/runs/{runId}"
Write-Host "  - Evaluation run id    -> /allagma/evaluations/{evaluationRunId}"
Write-Host "  - Conexus model call   -> /conexus/observability (model call detail)"
Write-Host "  - Kanon decision id    -> /kanon/decisions"
Write-Host ""
Write-Host "[3/4] Open evidence spine workbench:" -ForegroundColor Yellow
Write-Host "  $FrontendBaseUrl/system/evidence-spine"
Write-Host "  Paste each ID; confirm graph nodes, source attempts, missing links (if partial), page links."
Write-Host "  Export bundle: Copy/Download JSON; confirm schema ontogony-cross-service-evidence-spine-bundle-v1."
Write-Host ""
Write-Host '[4/4] Confirm shell build label matches ontogony-frontend git HEAD (7-char prefix).'
Write-Host ""

$checklist = [ordered]@{
    recordedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    frontendBaseUrl = $FrontendBaseUrl
    buildRequested = [bool]$Build
    provenanceVerified = -not $SkipProvenanceVerify
    steps = @(
        "verify-frontend-browser-provenance.ps1",
        "capture run/eval/model-call/decision ids",
        "resolve each id at /system/evidence-spine",
        "export redacted bundle and validate schema id"
    )
    mockE2e = "cd C:\dev\ontogony-frontend; npm run test:e2e -- e2e/evidence-spine-workbench.spec.ts"
    live008a = ".\run-evidence-spine-008a-docker-live-verification.ps1 -Build [-Seed]"
}

$checklist | ConvertTo-Json -Depth 6 | Set-Content -Path $reportPath -Encoding utf8
Write-Host "Wrote checklist artifact: $reportPath" -ForegroundColor Green
Write-Host "EVIDENCE-SPINE-008 manual checklist complete." -ForegroundColor Green
