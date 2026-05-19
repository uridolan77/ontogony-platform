# CONEXUS-PERSIST-003 — Conexus restart/durability regression for Docker-local persistence.
# Restarts conexus-api only and proves durable bootstrap/routing state survives.

param(
    [string]$OutputPath = "",
    [string]$ValidateBeforeReportPath = "",
    [string]$ValidateAfterReportPath = "",
    [switch]$SkipFrontend,
    [switch]$InvokeBootstrap,
    [int]$HealthTimeoutSeconds = 300
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
$composeFile = Join-Path $composeRoot "docker-compose.yml"
$defaultEnvFile = Join-Path $composeRoot ".env"
$exampleEnvFile = Join-Path $composeRoot ".env.example"
$envFileToUse = if (Test-Path -LiteralPath $defaultEnvFile) { $defaultEnvFile } else { $exampleEnvFile }

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $composeRoot "artifacts\conexus-persist-003-durability-report.json"
}
if ([string]::IsNullOrWhiteSpace($ValidateBeforeReportPath)) {
    $ValidateBeforeReportPath = Join-Path $composeRoot "artifacts\conexus-persist-002-before-restart-report.json"
}
if ([string]::IsNullOrWhiteSpace($ValidateAfterReportPath)) {
    $ValidateAfterReportPath = Join-Path $composeRoot "artifacts\conexus-persist-002-after-restart-report.json"
}

$outputDir = Split-Path -Parent $OutputPath
if (-not (Test-Path -LiteralPath $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

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

function Invoke-HttpStatus {
    param(
        [string]$Url,
        [int]$TimeoutSeconds = 10
    )
    try {
        $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec $TimeoutSeconds
        return [ordered]@{ statusCode = [int]$response.StatusCode; error = $null }
    }
    catch {
        if ($_.Exception.Response -and $_.Exception.Response.StatusCode) {
            return [ordered]@{ statusCode = [int]$_.Exception.Response.StatusCode; error = $null }
        }
        return [ordered]@{ statusCode = $null; error = $_.Exception.Message }
    }
}

function Wait-HttpHealthy {
    param(
        [string]$Name,
        [string]$Url,
        [int]$TimeoutSeconds
    )
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        try {
            $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 5
            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 300) {
                Write-Host "PASS $Name healthy: $Url"
                return
            }
        }
        catch { }
        Start-Sleep -Seconds 2
    }
    throw "$Name did not become healthy within ${TimeoutSeconds}s at $Url."
}

function Get-RouteEvidenceFromArtifacts {
    param([string]$ComposeRootPath)
    $guidedReportPath = Join-Path $ComposeRootPath "artifacts\docker-guided-main-flow-report.json"
    $seedReportPath = Join-Path $ComposeRootPath "artifacts\env-seed-001-report.json"
    $result = [ordered]@{
        source = $null
        baselineRouteDecisionId = $null
        subjectRouteDecisionId = $null
    }

    if (Test-Path -LiteralPath $guidedReportPath) {
        $guided = Get-Content -Raw -LiteralPath $guidedReportPath | ConvertFrom-Json
        $result.source = "docker-guided-main-flow-report.json"
        $result.baselineRouteDecisionId = $guided.baselineRouteDecisionId
        $result.subjectRouteDecisionId = $guided.subjectRouteDecisionId
        return [pscustomobject]$result
    }

    if (Test-Path -LiteralPath $seedReportPath) {
        $seed = Get-Content -Raw -LiteralPath $seedReportPath | ConvertFrom-Json
        $result.source = "env-seed-001-report.json"
        if ($seed.PSObject.Properties.Name -contains "routeEvidence") {
            $result.baselineRouteDecisionId = $seed.routeEvidence.baselineRouteDecisionId
            $result.subjectRouteDecisionId = $seed.routeEvidence.subjectRouteDecisionId
        }
        elseif ($seed.PSObject.Properties.Name -contains "runs") {
            if ($seed.runs.PSObject.Properties.Name -contains "baseline") {
                $result.baselineRouteDecisionId = $seed.runs.baseline.routeDecisionId
            }
            if ($seed.runs.PSObject.Properties.Name -contains "subject") {
                $result.subjectRouteDecisionId = $seed.runs.subject.routeDecisionId
            }
        }
        return [pscustomobject]$result
    }

    return [pscustomobject]$result
}

function Test-RoutingAdminState {
    param(
        [string]$BaseUrl,
        [hashtable]$AdminHeaders
    )
    $providers = @(Invoke-RestMethod -Uri "$BaseUrl/admin/v0/providers" -Headers $AdminHeaders -TimeoutSec 10)
    $aliases = @(Invoke-RestMethod -Uri "$BaseUrl/admin/v0/aliases" -Headers $AdminHeaders -TimeoutSec 10)
    $fakeProvider = $providers | Where-Object { $_.providerKey -eq "fake" -and $_.enabled -eq $true }
    $alias = $aliases | Where-Object { $_.alias -eq "gpt-4o-mini" -and $_.enabled -eq $true }
    return [ordered]@{
        fakeProviderEnabled = [bool]$fakeProvider
        modelAliasEnabled = [bool]$alias
    }
}

function Test-RouteDecisionFetch {
    param(
        [string]$BaseUrl,
        [hashtable]$AdminHeaders,
        [string]$RouteDecisionId
    )
    if ([string]::IsNullOrWhiteSpace($RouteDecisionId)) { return $false }
    try {
        $route = Invoke-RestMethod -Uri "$BaseUrl/admin/v0/route-decisions/$RouteDecisionId" -Headers $AdminHeaders -TimeoutSec 10
        return -not [string]::IsNullOrWhiteSpace([string]$route.routeDecisionId)
    }
    catch {
        return $false
    }
}

Write-Host ""
Write-Host "=== CONEXUS-PERSIST-003 Conexus durability regression ==="
Write-Host "Boundary: post-Docker-local hardening; not production readiness."
Write-Host "Output: $OutputPath"
Write-Host ""

$conexusPort = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_HOST_PORT" -DefaultValue "5082"
$conexusBaseUrl = "http://localhost:$conexusPort"
$conexusAdminKey = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_ADMIN_API_KEY" -DefaultValue "cx-conexus-admin-dev"
$conexusDevProjectKey = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_DEV_PROJECT_API_KEY" -DefaultValue "cx-dev-key-change-me"
$conexusAllagmaKey = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_PROJECT_API_KEY_FOR_ALLAGMA" -DefaultValue "cx-dev-key-change-me"
$keysAligned = ($conexusDevProjectKey -eq $conexusAllagmaKey)
$adminHeaders = @{ "X-Conexus-Admin-Key" = $conexusAdminKey }

$waitArgs = @{}
if ($SkipFrontend) { $waitArgs["SkipFrontend"] = $true }
& "$PSScriptRoot\wait-local-working-system.ps1" @waitArgs

$routeEvidence = Get-RouteEvidenceFromArtifacts -ComposeRootPath $composeRoot
if ([string]::IsNullOrWhiteSpace([string]$routeEvidence.baselineRouteDecisionId) `
    -or [string]::IsNullOrWhiteSpace([string]$routeEvidence.subjectRouteDecisionId)) {
    throw "Route evidence required before regression. Run seed-and-verify-local-working-system.ps1 or run-docker-guided-main-flow.ps1 first."
}
Write-Host "PASS route evidence artifacts: $($routeEvidence.source)"

$validateArgs = @{
    OutputPath = $ValidateBeforeReportPath
    RequireRouteEvidence = $true
    UseExistingReports = $true
}
if ($InvokeBootstrap) { $validateArgs["InvokeBootstrap"] = $true }
& "$PSScriptRoot\validate-conexus-persistence-bootstrap.ps1" @validateArgs

$beforeValidate = Get-Content -Raw -LiteralPath $ValidateBeforeReportPath | ConvertFrom-Json
if ($beforeValidate.verdict -ne "PASS") {
    throw "CONEXUS-PERSIST-002 validator must PASS before restart (verdict=$($beforeValidate.verdict))."
}
Write-Host "PASS CONEXUS-PERSIST-002 validator before restart."

$liveBefore = Invoke-HttpStatus -Url "$conexusBaseUrl/health/live"
$readyBefore = Invoke-HttpStatus -Url "$conexusBaseUrl/ready"
$routingBefore = Test-RoutingAdminState -BaseUrl $conexusBaseUrl -AdminHeaders $adminHeaders
$baselineRouteBefore = Test-RouteDecisionFetch -BaseUrl $conexusBaseUrl -AdminHeaders $adminHeaders -RouteDecisionId $routeEvidence.baselineRouteDecisionId
$subjectRouteBefore = Test-RouteDecisionFetch -BaseUrl $conexusBaseUrl -AdminHeaders $adminHeaders -RouteDecisionId $routeEvidence.subjectRouteDecisionId

if (-not $routingBefore.fakeProviderEnabled -or -not $routingBefore.modelAliasEnabled) {
    throw "Bootstrap routing state missing before restart (fake provider or gpt-4o-mini alias)."
}
if (-not $keysAligned) {
    throw "CONEXUS_DEV_PROJECT_API_KEY and CONEXUS_PROJECT_API_KEY_FOR_ALLAGMA must align before regression."
}
if (-not $baselineRouteBefore -or -not $subjectRouteBefore) {
    throw "Route decision records not fetchable before restart."
}
Write-Host "PASS bootstrap and route evidence before restart."

Write-Host ""
Write-Host "Restarting conexus-api only ..."
docker compose --env-file $envFileToUse -f $composeFile restart conexus-api
if ($LASTEXITCODE -ne 0) { throw "docker compose restart conexus-api failed (exit $LASTEXITCODE)." }

Wait-HttpHealthy -Name "conexus-api" -Url "$conexusBaseUrl/health/live" -TimeoutSeconds $HealthTimeoutSeconds

$liveAfter = Invoke-HttpStatus -Url "$conexusBaseUrl/health/live"
if ($liveAfter.statusCode -lt 200 -or $liveAfter.statusCode -ge 300) {
    throw "GET /health/live after restart returned $($liveAfter.statusCode); expected 2xx."
}
Write-Host "PASS /health/live after restart."

$readyAfter = Invoke-HttpStatus -Url "$conexusBaseUrl/ready"
Write-Host "Captured /ready after restart: HTTP $($readyAfter.statusCode) (informational)."

$routingAfter = Test-RoutingAdminState -BaseUrl $conexusBaseUrl -AdminHeaders $adminHeaders
if (-not $routingAfter.fakeProviderEnabled -or -not $routingAfter.modelAliasEnabled) {
    throw "Fake provider or gpt-4o-mini alias missing after restart."
}
Write-Host "PASS fake provider and alias after restart."

$baselineRouteAfter = Test-RouteDecisionFetch -BaseUrl $conexusBaseUrl -AdminHeaders $adminHeaders -RouteDecisionId $routeEvidence.baselineRouteDecisionId
$subjectRouteAfter = Test-RouteDecisionFetch -BaseUrl $conexusBaseUrl -AdminHeaders $adminHeaders -RouteDecisionId $routeEvidence.subjectRouteDecisionId
if (-not $baselineRouteAfter -or -not $subjectRouteAfter) {
    throw "Route decision records not fetchable after restart."
}
Write-Host "PASS route decision records after restart."

if (-not $keysAligned) {
    throw "Project API key alignment invalid after restart."
}
Write-Host "PASS project API key alignment unchanged."

$validateAfterArgs = @{
    OutputPath = $ValidateAfterReportPath
    RequireRouteEvidence = $true
    UseExistingReports = $true
}
if ($InvokeBootstrap) { $validateAfterArgs["InvokeBootstrap"] = $true }
& "$PSScriptRoot\validate-conexus-persistence-bootstrap.ps1" @validateAfterArgs

$afterValidate = Get-Content -Raw -LiteralPath $ValidateAfterReportPath | ConvertFrom-Json
if ($afterValidate.verdict -ne "PASS") {
    throw "CONEXUS-PERSIST-002 validator must PASS after restart (verdict=$($afterValidate.verdict))."
}
Write-Host "PASS CONEXUS-PERSIST-002 validator after restart."

$report = [ordered]@{
    schema = "conexus-persist-003-durability-report-v1"
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    verdict = "PASS"
    boundary = "post-Docker-local hardening; not production readiness"
    services = [ordered]@{
        conexusBaseUrl = $conexusBaseUrl
        envFile = $envFileToUse
    }
    configuration = [ordered]@{
        conexusDevProjectApiKeyConfigured = -not [string]::IsNullOrWhiteSpace($conexusDevProjectKey)
        conexusProjectApiKeyForAllagmaConfigured = -not [string]::IsNullOrWhiteSpace($conexusAllagmaKey)
        keysAligned = $keysAligned
    }
    routeEvidence = [ordered]@{
        source = $routeEvidence.source
        baselineRouteDecisionId = $routeEvidence.baselineRouteDecisionId
        subjectRouteDecisionId = $routeEvidence.subjectRouteDecisionId
        adminFetchBeforeRestart = @{
            baseline = $baselineRouteBefore
            subject = $subjectRouteBefore
        }
        adminFetchAfterRestart = @{
            baseline = $baselineRouteAfter
            subject = $subjectRouteAfter
        }
    }
    readiness = [ordered]@{
        beforeRestart = [ordered]@{
            healthLive = $liveBefore
            ready = $readyBefore
        }
        afterRestart = [ordered]@{
            healthLive = $liveAfter
            ready = $readyAfter
        }
    }
    routing = [ordered]@{
        beforeRestart = $routingBefore
        afterRestart = $routingAfter
    }
    validation002 = [ordered]@{
        beforeReportPath = $ValidateBeforeReportPath
        afterReportPath = $ValidateAfterReportPath
        beforeVerdict = $beforeValidate.verdict
        afterVerdict = $afterValidate.verdict
    }
    restart = [ordered]@{
        service = "conexus-api"
        restarted = $true
        restartPersistencePassed = $true
    }
    safety = [ordered]@{
        realProviderKeys = "no"
        realExternalExecution = "disabled"
        productionReadiness = "not_claimed"
    }
}

$report | ConvertTo-Json -Depth 12 | Set-Content -LiteralPath $OutputPath -Encoding UTF8
Write-Host ""
Write-Host "Wrote durability report: $OutputPath"

& "$PSScriptRoot\validate-conexus-persistence-durability-report.ps1" -ReportPath $OutputPath

Write-Host ""
Write-Host "CONEXUS-PERSIST-003 durability regression PASS."
