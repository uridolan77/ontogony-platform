# ENV-DOCKER-RUN-001 — Dockerized guided main flow with Allagma restart durability proof.

param(
    [switch]$StartServices,
    [switch]$SkipFrontend,
    [string]$OutputPath = "",
    [string]$SeedReportPath = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $composeRoot "artifacts\docker-guided-main-flow-report.json"
}
if ([string]::IsNullOrWhiteSpace($SeedReportPath)) {
    $SeedReportPath = Join-Path $composeRoot "artifacts\env-seed-001-report.json"
}

$outputDir = Split-Path -Parent $OutputPath
if (-not (Test-Path -LiteralPath $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

$composeFile = Join-Path $composeRoot "docker-compose.yml"
$defaultEnvFile = Join-Path $composeRoot ".env"
$exampleEnvFile = Join-Path $composeRoot ".env.example"
$envFileToUse = if (Test-Path -LiteralPath $defaultEnvFile) { $defaultEnvFile } else { $exampleEnvFile }

function Get-DotEnvValue {
    param(
        [string]$Path,
        [string]$Key,
        [string]$DefaultValue
    )
    $processValue = [Environment]::GetEnvironmentVariable($Key)
    if (-not [string]::IsNullOrWhiteSpace($processValue)) { return $processValue }
    if (-not (Test-Path -LiteralPath $Path)) { return $DefaultValue }
    $line = Select-String -Path $Path -Pattern "^$Key=(.+)$" | Select-Object -First 1
    if ($null -eq $line) { return $DefaultValue }
    return $line.Matches[0].Groups[1].Value.Trim()
}

$kanonPort = Get-DotEnvValue -Path $envFileToUse -Key "KANON_HOST_PORT" -DefaultValue "5081"
$conexusPort = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_HOST_PORT" -DefaultValue "5082"
$allagmaPort = Get-DotEnvValue -Path $envFileToUse -Key "ALLAGMA_HOST_PORT" -DefaultValue "5083"

$allagmaBaseUrl = "http://localhost:$allagmaPort"
$allagmaServiceToken = "allagma-dev-service-token-change-in-production"
$allagmaHeaders = @{ Authorization = "Bearer $allagmaServiceToken" }

function Wait-HttpHealthy {
    param(
        [string]$Name,
        [string]$Url,
        [int]$TimeoutSeconds = 300
    )
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        try {
            $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 5
            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 300) {
                Write-Host "PASS $Name healthy after restart: $Url"
                return
            }
        }
        catch { }
        Start-Sleep -Seconds 2
    }
    throw "$Name did not become healthy within ${TimeoutSeconds}s at $Url."
}

function Assert-EvalListed {
    param(
        [string]$RunId,
        [string]$EvaluationRunId,
        [string]$Label
    )
    $list = Invoke-RestMethod -Uri "$allagmaBaseUrl/allagma/v0/runs/$RunId/evaluations" -Headers $allagmaHeaders
    if (-not ($list.items | Where-Object { $_.evaluationRunId -eq $EvaluationRunId })) {
        throw "$Label evaluation $EvaluationRunId not listed after Allagma restart."
    }
}

Write-Host ""
Write-Host "=== ENV-DOCKER-RUN-001 Docker guided main flow ==="
Write-Host "Boundary: first Dockerized local working system, not production readiness."
Write-Host ""

if ($StartServices) {
    $startArgs = @{ Build = $true }
    if ($SkipFrontend) { $startArgs["SkipFrontend"] = $true }
    & "$PSScriptRoot\start-local-working-system.ps1" @startArgs
    if ($LASTEXITCODE -ne 0) { throw "start-local-working-system.ps1 failed (exit $LASTEXITCODE)." }
}
else {
    $waitArgs = @{}
    if ($SkipFrontend) { $waitArgs["SkipFrontend"] = $true }
    & "$PSScriptRoot\wait-local-working-system.ps1" @waitArgs
    if ($LASTEXITCODE -ne 0) { throw "wait-local-working-system.ps1 failed (exit $LASTEXITCODE)." }
}

& "$PSScriptRoot\seed-and-verify-local-working-system.ps1" -OutputPath $SeedReportPath
if ($LASTEXITCODE -ne 0) { throw "seed-and-verify-local-working-system.ps1 failed (exit $LASTEXITCODE)." }

$seed = Get-Content -Raw -LiteralPath $SeedReportPath | ConvertFrom-Json
if ($seed.verdict -ne "PASS") { throw "Seed report verdict must be PASS." }

$baselineRunId = [string]$seed.runs.baselineRunId
$subjectRunId = [string]$seed.runs.subjectRunId
$subjectTopologyAuthorizationDecisionId = [string]$seed.topology.subjectTopologyAuthorizationDecisionId
$baselineRouteDecisionId = [string]$seed.routeEvidence.baselineRouteDecisionId
$subjectRouteDecisionId = [string]$seed.routeEvidence.subjectRouteDecisionId
$baselineEvaluationRunId = [string]$seed.evaluations.baselineEvaluationRunId
$subjectEvaluationRunId = [string]$seed.evaluations.subjectEvaluationRunId
$baselineComparisonId = [string]$seed.evaluations.baselineComparisonId

Write-Host ""
Write-Host "Restarting allagma-api to prove persistence after container restart ..."
docker compose --env-file $envFileToUse -f $composeFile restart allagma-api
if ($LASTEXITCODE -ne 0) { throw "docker compose restart allagma-api failed (exit $LASTEXITCODE)." }

Wait-HttpHealthy -Name "allagma-api" -Url "$allagmaBaseUrl/health" -TimeoutSeconds 300

Assert-EvalListed -RunId $baselineRunId -EvaluationRunId $baselineEvaluationRunId -Label "baseline"
Assert-EvalListed -RunId $subjectRunId -EvaluationRunId $subjectEvaluationRunId -Label "subject"

$comparisonAfterRestart = Invoke-RestMethod `
    -Uri "$allagmaBaseUrl/allagma/v0/evaluations/baseline-comparisons/$baselineComparisonId" `
    -Headers $allagmaHeaders
if ([string]::IsNullOrWhiteSpace($comparisonAfterRestart.comparisonId)) {
    throw "Baseline comparison not fetchable after Allagma restart."
}
Write-Host "PASS restart durability: evaluations and comparison survived Allagma restart."

$report = [ordered]@{
    schema = "docker-guided-main-flow-report-v1"
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    verdict = "PASS"
    boundary = "first Dockerized local working system, not production readiness"
    verificationMode = "docker-compose-host-local-api"
    services = [ordered]@{
        kanonBaseUrl = "http://localhost:$kanonPort"
        conexusBaseUrl = "http://localhost:$conexusPort"
        allagmaBaseUrl = $allagmaBaseUrl
    }
    baselineRunId = $baselineRunId
    subjectRunId = $subjectRunId
    subjectTopologyAuthorizationDecisionId = $subjectTopologyAuthorizationDecisionId
    baselineRouteDecisionId = $baselineRouteDecisionId
    subjectRouteDecisionId = $subjectRouteDecisionId
    baselineEvaluationRunId = $baselineEvaluationRunId
    subjectEvaluationRunId = $subjectEvaluationRunId
    baselineComparisonId = $baselineComparisonId
    restart = [ordered]@{
        service = "allagma-api"
        restarted = $true
        restartPersistencePassed = $true
    }
    seedReportPath = $SeedReportPath
    safety = [ordered]@{
        realProviderKeys = "no"
        realExternalExecution = "disabled"
        productionReadiness = "not_claimed"
    }
}

$report | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $OutputPath -Encoding UTF8
Write-Host "Wrote Docker guided main flow report: $OutputPath"

& "$PSScriptRoot\validate-docker-guided-main-flow.ps1" -ReportPath $OutputPath
if ($LASTEXITCODE -ne 0) { throw "validate-docker-guided-main-flow.ps1 failed (exit $LASTEXITCODE)." }

Write-Host ""
Write-Host "ENV-DOCKER-RUN-001 Docker guided main flow PASS."
