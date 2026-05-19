# KANON-OP-001 — read guided/seed reports and fetch Allagma/Kanon topology decision evidence.
# Writes redacted JSON under docker/local-working-system/artifacts/.

param(
    [string]$GuidedReportPath = "",
    [string]$SeedReportPath = "",
    [string]$OutputPath = "",
    [string]$AllagmaBaseUrl = "http://localhost:5083",
    [string]$KanonBaseUrl = "http://localhost:5081",
    [string]$AllagmaServiceToken = "allagma-dev-service-token-change-in-production",
    [string]$KanonServiceToken = "kanon-dev-service-token-change-in-production",
    [int]$HttpTimeoutSeconds = 15
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
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

$secretPatterns = @(
    "allagma-dev-service-token-change-in-production",
    "kanon-dev-service-token-change-in-production",
    "cx-dev-key-change-me",
    "cx-conexus-admin-dev",
    "allagma_local_pw",
    "kanon_local_pw",
    "conexus_local_pw",
    "ontogony_admin_pw"
)

function Redact-String {
    param([string]$Value)
    if ([string]::IsNullOrWhiteSpace($Value)) { return $null }
    $redacted = $Value
    foreach ($pattern in $secretPatterns) {
        $redacted = $redacted.Replace($pattern, "***")
    }
    return $redacted
}

function Get-OptionalProperty {
    param(
        [object]$Object,
        [string]$Name
    )
    if ($null -eq $Object) { return $null }
    $prop = $Object.PSObject.Properties[$Name]
    if ($null -eq $prop) { return $null }
    return $prop.Value
}

function Redact-Object {
    param([object]$InputObject)
    if ($null -eq $InputObject) { return $null }
    $json = $InputObject | ConvertTo-Json -Depth 12 -Compress
    foreach ($pattern in $secretPatterns) {
        $json = $json.Replace($pattern, "***")
    }
    return ($json | ConvertFrom-Json)
}

function Read-ReportIds {
    param(
        [string]$GuidedPath,
        [string]$SeedPath
    )

    $ids = [ordered]@{
        source = $null
        baselineRunId = $null
        subjectRunId = $null
        subjectTopologyAuthorizationDecisionId = $null
        baselineTopologyAuthorizationDecisionId = $null
        baselineSelectedTopology = $null
        subjectSelectedTopology = $null
    }

    if (Test-Path -LiteralPath $GuidedPath) {
        $guided = Get-Content -Raw -LiteralPath $GuidedPath | ConvertFrom-Json
        $ids.source = "docker-guided-main-flow-report.json"
        $ids.baselineRunId = [string]$guided.baselineRunId
        $ids.subjectRunId = [string]$guided.subjectRunId
        $ids.subjectTopologyAuthorizationDecisionId = [string]$guided.subjectTopologyAuthorizationDecisionId
    }
    elseif (Test-Path -LiteralPath $SeedPath) {
        $seed = Get-Content -Raw -LiteralPath $SeedPath | ConvertFrom-Json
        $ids.source = "env-seed-001-report.json"
        $ids.baselineRunId = [string]$seed.runs.baselineRunId
        $ids.subjectRunId = [string]$seed.runs.subjectRunId
        $ids.subjectTopologyAuthorizationDecisionId = [string]$seed.topology.subjectTopologyAuthorizationDecisionId
        $ids.baselineTopologyAuthorizationDecisionId = $seed.topology.baselineTopologyAuthorizationDecisionId
        $ids.baselineSelectedTopology = [string]$seed.topology.baselineSelectedTopology
        $ids.subjectSelectedTopology = [string]$seed.topology.subjectSelectedTopology
    }
    else {
        throw "No guided or seed report found. Run seed-and-verify-local-working-system.ps1 or run-docker-guided-main-flow.ps1 first."
    }

    return [pscustomobject]$ids
}

function Invoke-AllagmaTopologySummary {
    param([string]$RunId)
    $headers = @{ Authorization = "Bearer $AllagmaServiceToken" }
    return Invoke-RestMethod `
        -Uri "$AllagmaBaseUrl/allagma/v0/runs/$RunId/events?includeTopologySummary=true" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds
}

function Invoke-AllagmaRunDetail {
    param([string]$RunId)
    $headers = @{ Authorization = "Bearer $AllagmaServiceToken" }
    return Invoke-RestMethod `
        -Uri "$AllagmaBaseUrl/allagma/v0/runs/$RunId" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds
}

function Invoke-KanonDecisionRecord {
    param([string]$DecisionId)
    $headers = @{
        Authorization = "Bearer $KanonServiceToken"
        "X-Ontogony-Actor-Id" = "kanon-op-001"
        "X-Ontogony-Actor-Type" = "service"
        "X-Ontogony-Roles" = "ProvenanceReader"
    }
    return Invoke-RestMethod `
        -Uri "$KanonBaseUrl/ontology/v0/decision-records/$DecisionId" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds
}

function Invoke-KanonProvenance {
    param([string]$DecisionId)
    $headers = @{
        Authorization = "Bearer $KanonServiceToken"
        "X-Ontogony-Actor-Id" = "kanon-op-001"
        "X-Ontogony-Actor-Type" = "service"
        "X-Ontogony-Roles" = "ProvenanceReader"
    }
    try {
        return Invoke-RestMethod `
            -Uri "$KanonBaseUrl/ontology/v0/decision-records/$DecisionId/provenance" `
            -Headers $headers `
            -TimeoutSec $HttpTimeoutSeconds
    }
    catch {
        return [ordered]@{ fetchError = (Redact-String $_.Exception.Message) }
    }
}

Write-Host ""
Write-Host "=== KANON-OP-001 inspect Kanon topology evidence ==="
Write-Host "Boundary: Docker-local operator visibility, not production readiness."
Write-Host ""

$reportIds = Read-ReportIds -GuidedPath $GuidedReportPath -SeedPath $SeedReportPath
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
        guidedReportPath = (Redact-String $GuidedReportPath)
        seedReportPath = (Redact-String $SeedReportPath)
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
        decisionRecord = (Redact-Object $kanonDecision)
        provenance = (Redact-Object $kanonProvenance)
        decisionType = $kanonDecision.decisionType
        outcomeStatus = $kanonDecision.outcome.status
    }
    allagma = [ordered]@{
        baselineRunStatus = $baselineRunDetail.status
        subjectRunStatus = $subjectRunDetail.status
        subjectContextKeys = $subjectContextKeys
    }
    services = [ordered]@{
        allagmaBaseUrl = $AllagmaBaseUrl
        kanonBaseUrl = $KanonBaseUrl
    }
    safety = [ordered]@{
        realProviderKeys = "no"
        realExternalExecution = "disabled"
        productionReadiness = "not_claimed"
        secretsRedacted = $true
    }
}

$json = $report | ConvertTo-Json -Depth 14
foreach ($pattern in $secretPatterns) {
    if ($json -match [regex]::Escape($pattern)) {
        throw "Report still contains raw secret pattern: $pattern"
    }
}

$json | Set-Content -LiteralPath $OutputPath -Encoding UTF8
Write-Host "Wrote Kanon topology evidence report: $OutputPath"
Write-Host "KANON-OP-001 inspect PASS."
