# SYSTEM-9B-005 — Docker-local live-artifact evidence journey verification (live APIs + Playwright).

param(
    [switch]$Build,
    [switch]$Seed,
    [switch]$SkipProvenanceVerify,
    [string]$FrontendBaseUrl = "http://localhost:5175"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

$composeRoot = Get-DockerLocalComposeRoot
$frontendRoot = Get-FrontendRepoRoot
$seedPath = Join-Path $composeRoot "artifacts\env-seed-001-report.json"
$reportPath = Join-Path $composeRoot "artifacts\system-9b005-live-artifact-evidence-journey-report.json"
$apiReportPath = Join-Path $composeRoot "artifacts\system-9b005-live-api-preflight.json"
$config = Get-DockerLocalComposeConfig -FrontendBaseUrl $FrontendBaseUrl

Write-Host ""
Write-Host "=== SYSTEM-9B-005 live-artifact evidence journey verification ===" -ForegroundColor Cyan
Write-Host ""

if (-not $SkipProvenanceVerify) {
    $verifyScript = Join-Path $PSScriptRoot "verify-frontend-browser-provenance.ps1"
    if ($Build) {
        Write-Host "[1/5] Rebuild frontend + provenance verify ..."
        & $verifyScript -Build -FrontendBaseUrl $FrontendBaseUrl
    }
    else {
        Write-Host "[1/5] Provenance verify (no rebuild) ..."
        & $verifyScript -FrontendBaseUrl $FrontendBaseUrl
    }
    if ($LASTEXITCODE -ne 0) {
        throw "Frontend provenance verification failed."
    }
}
else {
    Write-Host "[1/5] Skipped provenance verify."
}

if ($Seed -or -not (Test-Path -LiteralPath $seedPath)) {
    Write-Host "[2/5] ENV-SEED-001 seed + verify ..."
    & "$PSScriptRoot\seed-and-verify-local-working-system.ps1" -OutputPath $seedPath
    if ($LASTEXITCODE -ne 0) {
        throw "ENV-SEED-001 failed."
    }
}
else {
    Write-Host "[2/5] Using existing seed report: $seedPath"
}

Write-Host "[3/5] Live API preflight for evidence journey backing routes ..."
$seedReport = Get-Content -Raw -LiteralPath $seedPath | ConvertFrom-Json
$allagmaHeaders = @{ Authorization = "Bearer $($config.AllagmaServiceToken)" }
$kanonHeaders = @{ Authorization = "Bearer $($config.KanonServiceToken)" }
$conexusAdminHeaders = @{ "X-Conexus-Admin-Key" = $config.ConexusAdminApiKey }

function Test-ApiRow {
    param(
        [string]$Kind,
        [string]$Id,
        [string]$Url,
        [hashtable]$Headers,
        [switch]$Optional
    )
    $row = [ordered]@{
        kind = $Kind
        id = $Id
        url = $Url
        status = $null
        ok = $false
        optional = [bool]$Optional
    }
    if ([string]::IsNullOrWhiteSpace($Id)) {
        $row.status = "skipped"
        $row.ok = [bool]$Optional
        return [pscustomobject]$row
    }
    try {
        $response = Invoke-WebRequest -Uri $Url -Headers $Headers -Method Get -UseBasicParsing
        $row.status = [int]$response.StatusCode
        $row.ok = $response.StatusCode -ge 200 -and $response.StatusCode -lt 300
    }
    catch {
        if ($_.Exception.Response) {
            $row.status = [int]$_.Exception.Response.StatusCode.value__
        }
        else {
            $row.status = "error"
        }
        $row.ok = $false
    }
    return [pscustomobject]$row
}

$apiRows = @(
    (Test-ApiRow -Kind "allagmaRun" -Id $seedReport.runs.subjectRunId -Url "$($config.AllagmaBaseUrl)/allagma/v0/runs/$($seedReport.runs.subjectRunId)" -Headers $allagmaHeaders)
    (Test-ApiRow -Kind "allagmaRunAudit" -Id $seedReport.runs.subjectRunId -Url "$($config.AllagmaBaseUrl)/allagma/v0/runs/$($seedReport.runs.subjectRunId)/audit" -Headers $allagmaHeaders)
    (Test-ApiRow -Kind "allagmaEvaluation" -Id $seedReport.evaluations.subjectEvaluationRunId -Url "$($config.AllagmaBaseUrl)/allagma/v0/evaluations/$($seedReport.evaluations.subjectEvaluationRunId)" -Headers $allagmaHeaders)
    (Test-ApiRow -Kind "kanonDecision" -Id $seedReport.topology.subjectTopologyAuthorizationDecisionId -Url "$($config.KanonBaseUrl)/ontology/v0/decision-records/$($seedReport.topology.subjectTopologyAuthorizationDecisionId)" -Headers $kanonHeaders -Optional)
    (Test-ApiRow -Kind "conexusModelCall" -Id $seedReport.runs.subjectModelCallId -Url "$($config.ConexusBaseUrl)/admin/v0/model-calls/$([uri]::EscapeDataString($seedReport.runs.subjectModelCallId))" -Headers $conexusAdminHeaders -Optional)
)

$failedRequiredRows = @($apiRows | Where-Object { $_.ok -ne $true -and -not $_.optional })
$apiPreflightVerdict = if ($failedRequiredRows.Count -eq 0) { "PASS" } else { "FAIL" }
if ($apiPreflightVerdict -eq "FAIL") {
    throw "API preflight failed for required evidence journey routes."
}

$apiReport = [ordered]@{
    schema = "system-9b005-live-api-preflight-v1"
    recordedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    seedReportPath = $seedPath
    rows = $apiRows
    verdict = $apiPreflightVerdict
}
$apiReport | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $apiReportPath -Encoding utf8
Write-Host "Wrote API preflight: $apiReportPath"

try {
    $frontendProbe = Invoke-WebRequest -Uri "$FrontendBaseUrl/allagma/replay" -UseBasicParsing -TimeoutSec 30
    if ($frontendProbe.StatusCode -ne 200) {
        throw "Frontend replay route returned HTTP $($frontendProbe.StatusCode)."
    }
}
catch {
    throw "Frontend not reachable at $FrontendBaseUrl. Start docker compose frontend service first."
}

Write-Host "[4/5] Playwright live evidence journey verification ..."
Push-Location $frontendRoot
$env:E2E_BASE_URL = $FrontendBaseUrl
$env:ENV_SEED_001_REPORT = $seedPath
$env:SYSTEM_9B005_REPORT_PATH = $reportPath
$env:ALLAGMA_BASE_URL = $config.AllagmaBaseUrl
$env:KANON_BASE_URL = $config.KanonBaseUrl
$env:CONEXUS_BASE_URL = $config.ConexusBaseUrl
$env:ALLAGMA_SERVICE_TOKEN = $config.AllagmaServiceToken
$env:KANON_SERVICE_TOKEN = $config.KanonServiceToken
$env:CONEXUS_ADMIN_API_KEY = $config.ConexusAdminApiKey

npm run test:e2e:docker-live:evidence-journey
$playwrightExit = $LASTEXITCODE
Pop-Location

if ($playwrightExit -ne 0) {
    throw "Playwright evidence-journey Docker-live tests failed (exit $playwrightExit)."
}

Write-Host "[5/5] Report artifacts" -ForegroundColor Green
if (-not (Test-Path -LiteralPath $reportPath)) {
    throw "Expected Playwright report at $reportPath"
}

$summary = [ordered]@{
    schema = "system-9b005-live-artifact-evidence-journey-verification-v1"
    recordedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    frontendBaseUrl = $FrontendBaseUrl
    seedReportPath = $seedPath
    apiPreflightPath = $apiReportPath
    playwrightReportPath = $reportPath
    buildRequested = [bool]$Build
    seedRequested = [bool]$Seed
    verdict = "PASS"
}
$summary | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $composeRoot "artifacts\system-9b005-live-artifact-evidence-journey-verification.json") -Encoding utf8

Write-Host "Wrote $reportPath" -ForegroundColor Green
Write-Host "SYSTEM-9B-005 live-artifact evidence journey verification PASS." -ForegroundColor Green
