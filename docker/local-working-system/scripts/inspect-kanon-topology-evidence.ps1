# KANON-OP-001 — read guided/seed reports and fetch Allagma/Kanon topology decision evidence.
# Writes redacted JSON under docker/local-working-system/artifacts/.

param(
    [string]$GuidedReportPath = "",
    [string]$SeedReportPath = "",
    [string]$OutputPath = "",
    [string]$AllagmaBaseUrl = "",
    [string]$KanonBaseUrl = "",
    [string]$AllagmaServiceToken = "",
    [string]$KanonServiceToken = "",
    [int]$HttpTimeoutSeconds = 15
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

$composeRoot = Get-DockerLocalComposeRoot
$config = Get-DockerLocalComposeConfig `
    -AllagmaBaseUrl $AllagmaBaseUrl `
    -KanonBaseUrl $KanonBaseUrl `
    -AllagmaServiceToken $AllagmaServiceToken `
    -KanonServiceToken $KanonServiceToken
$secretPatterns = @(Get-DockerLocalSecretPatterns -ComposeConfig $config)

if ([string]::IsNullOrWhiteSpace($GuidedReportPath)) {
    $GuidedReportPath = Join-Path $composeRoot "artifacts\docker-guided-main-flow-report.json"
}
if ([string]::IsNullOrWhiteSpace($SeedReportPath)) {
    $SeedReportPath = Join-Path $composeRoot "artifacts\env-seed-001-report.json"
}
if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $composeRoot "artifacts\kanon-op-001-topology-evidence-report.json"
}

$outputDir = Split-Path -Parent $OutputPath
if (-not (Test-Path -LiteralPath $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

function Invoke-AllagmaTopologySummary {
    param([string]$RunId)
    $headers = @{ Authorization = "Bearer $($config.AllagmaServiceToken)" }
    return Invoke-RestMethod `
        -Uri "$($config.AllagmaBaseUrl)/allagma/v0/runs/$RunId/events?includeTopologySummary=true" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds
}

function Invoke-AllagmaRunDetail {
    param([string]$RunId)
    $headers = @{ Authorization = "Bearer $($config.AllagmaServiceToken)" }
    return Invoke-RestMethod `
        -Uri "$($config.AllagmaBaseUrl)/allagma/v0/runs/$RunId" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds
}

function Invoke-KanonDecisionRecord {
    param([string]$DecisionId)
    $headers = @{
        Authorization = "Bearer $($config.KanonServiceToken)"
        "X-Ontogony-Actor-Id" = "kanon-op-001"
        "X-Ontogony-Actor-Type" = "service"
        "X-Ontogony-Roles" = "ProvenanceReader"
    }
    return Invoke-RestMethod `
        -Uri "$($config.KanonBaseUrl)/ontology/v0/decision-records/$DecisionId" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds
}

function Invoke-KanonProvenance {
    param([string]$DecisionId)
    $headers = @{
        Authorization = "Bearer $($config.KanonServiceToken)"
        "X-Ontogony-Actor-Id" = "kanon-op-001"
        "X-Ontogony-Actor-Type" = "service"
        "X-Ontogony-Roles" = "ProvenanceReader"
    }
    try {
        return Invoke-RestMethod `
            -Uri "$($config.KanonBaseUrl)/ontology/v0/decision-records/$DecisionId/provenance" `
            -Headers $headers `
            -TimeoutSec $HttpTimeoutSeconds
    }
    catch {
        return [ordered]@{ fetchError = (Redact-StringWithPatterns -Value $_.Exception.Message -SecretPatterns $secretPatterns) }
    }
}

Write-Host ""
Write-Host "=== KANON-OP-001 inspect Kanon topology evidence ==="
Write-Host "Boundary: Docker-local operator visibility, not production readiness."
Write-Host "Env file: $($config.EnvFilePath)"
Write-Host ""

$reportIds = Read-DockerLocalRunReportIds -GuidedPath $GuidedReportPath -SeedPath $SeedReportPath
if ([string]::IsNullOrWhiteSpace($reportIds.source)) {
    throw "No guided or seed report found. Run seed-and-verify-local-working-system.ps1 or run-docker-guided-main-flow.ps1 first."
}
if ([string]::IsNullOrWhiteSpace($reportIds.baselineRunId) -or [string]::IsNullOrWhiteSpace($reportIds.subjectRunId)) {
    throw "baselineRunId and subjectRunId must be present in input report."
}

$baselineTopology = Invoke-AllagmaTopologySummary -RunId $reportIds.baselineRunId
$subjectTopology = Invoke-AllagmaTopologySummary -RunId $reportIds.subjectRunId
$subjectRunDetail = Invoke-AllagmaRunDetail -RunId $reportIds.subjectRunId
$baselineRunDetail = Invoke-AllagmaRunDetail -RunId $reportIds.baselineRunId

$baselineAuthId = $baselineTopology.topology.topologyAuthorizationDecisionId
$subjectAuthId = $subjectTopology.topology.topologyAuthorizationDecisionId
if ([string]::IsNullOrWhiteSpace([string]$reportIds.subjectTopologyAuthorizationDecisionId)) {
    $reportIds.subjectTopologyAuthorizationDecisionId = [string]$subjectAuthId
}

if ($null -ne $baselineAuthId) {
    throw "baseline topologyAuthorizationDecisionId must be null by design (was '$baselineAuthId')."
}
if ([string]::IsNullOrWhiteSpace([string]$subjectAuthId)) {
    throw "subject topologyAuthorizationDecisionId must be present for centralized_orchestrator override path."
}

$kanonDecision = Invoke-KanonDecisionRecord -DecisionId $reportIds.subjectTopologyAuthorizationDecisionId
$kanonProvenance = Invoke-KanonProvenance -DecisionId $reportIds.subjectTopologyAuthorizationDecisionId

$subjectContext = Get-OptionalProperty -Object $subjectRunDetail -Name "context"
$subjectContextKeys = if ($null -ne $subjectContext -and $subjectContext -is [System.Management.Automation.PSCustomObject]) {
    @($subjectContext.PSObject.Properties.Name)
}
else {
    @()
}

$baselineSelected = $baselineTopology.topology.topologySelection.value.selectedTopology
$subjectSelected = $subjectTopology.topology.topologySelection.value.selectedTopology
$baselineRequiresAuth = [bool]$baselineTopology.topology.topologySelection.value.requiresKanonAuthorization
$subjectRequiresAuth = [bool]$subjectTopology.topology.topologySelection.value.requiresKanonAuthorization

$report = [ordered]@{
    schema = "kanon-op-001-topology-evidence-report-v1"
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    verdict = "PASS"
    boundary = "first Dockerized local working system, not production readiness"
    inputReport = [ordered]@{
        source = $reportIds.source
        guidedReportPath = (Redact-StringWithPatterns -Value $GuidedReportPath -SecretPatterns $secretPatterns)
        seedReportPath = (Redact-StringWithPatterns -Value $SeedReportPath -SecretPatterns $secretPatterns)
    }
    linkage = [ordered]@{
        baselineRunId = $reportIds.baselineRunId
        subjectRunId = $reportIds.subjectRunId
        baselinePlanningDecisionId = $baselineRunDetail.planningDecisionId
        subjectPlanningDecisionId = $subjectRunDetail.planningDecisionId
        subjectTopologyAuthorizationDecisionId = $reportIds.subjectTopologyAuthorizationDecisionId
        baselineTopologyAuthorizationDecisionId = $null
    }
    topologySummary = [ordered]@{
        baseline = [ordered]@{
            selectedTopology = $baselineSelected
            requiresKanonAuthorization = $baselineRequiresAuth
            topologyAuthorizationDecisionId = $null
        }
        subject = [ordered]@{
            selectedTopology = $subjectSelected
            requiresKanonAuthorization = $subjectRequiresAuth
            topologyAuthorizationDecisionId = $subjectAuthId
        }
    }
    kanon = [ordered]@{
        decisionRecord = (Redact-ObjectWithPatterns -InputObject $kanonDecision -SecretPatterns $secretPatterns)
        provenance = (Redact-ObjectWithPatterns -InputObject $kanonProvenance -SecretPatterns $secretPatterns)
        decisionType = $kanonDecision.decisionType
        outcomeStatus = $kanonDecision.outcome.status
    }
    allagma = [ordered]@{
        baselineRunStatus = $baselineRunDetail.status
        subjectRunStatus = $subjectRunDetail.status
        subjectContextKeys = $subjectContextKeys
    }
    services = [ordered]@{
        allagmaBaseUrl = $config.AllagmaBaseUrl
        kanonBaseUrl = $config.KanonBaseUrl
        envFilePath = (Redact-StringWithPatterns -Value $config.EnvFilePath -SecretPatterns $secretPatterns)
    }
    safety = [ordered]@{
        realProviderKeys = "no"
        realExternalExecution = "disabled"
        productionReadiness = "not_claimed"
        secretsRedacted = $true
        tokensLoadedFromEnvFile = $true
    }
}

$json = $report | ConvertTo-Json -Depth 14
Assert-ReportHasNoSecretPatterns -Json $json -SecretPatterns $secretPatterns

$json | Set-Content -LiteralPath $OutputPath -Encoding UTF8
Write-Host "Wrote Kanon topology evidence report: $OutputPath"
Write-Host "KANON-OP-001 inspect PASS."
