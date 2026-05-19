# KANON-OP-001 — validate kanon-op-001-topology-evidence-report.json acceptance fields.

param(
    [string]$ReportPath = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

$composeRoot = Get-DockerLocalComposeRoot
if ([string]::IsNullOrWhiteSpace($ReportPath)) {
    $ReportPath = Join-Path $composeRoot "artifacts\kanon-op-001-topology-evidence-report.json"
}

$config = Get-DockerLocalComposeConfig
$secretPatterns = @(Get-DockerLocalSecretPatterns -ComposeConfig $config)

if (-not (Test-Path -LiteralPath $ReportPath)) {
    throw "Kanon topology evidence report not found: $ReportPath"
}

$raw = Get-Content -Raw -LiteralPath $ReportPath
Assert-ReportHasNoSecretPatterns -Json $raw -SecretPatterns $secretPatterns

$doc = $raw | ConvertFrom-Json

function Assert-NonEmpty([string]$name, $value) {
    if ([string]::IsNullOrWhiteSpace([string]$value)) {
        throw "Kanon topology evidence: $name must be non-empty"
    }
}

if ($doc.schema -ne "kanon-op-001-topology-evidence-report-v1") {
    throw "Kanon topology evidence: schema must be kanon-op-001-topology-evidence-report-v1"
}
if ($doc.verdict -ne "PASS") {
    throw "Kanon topology evidence: verdict must be PASS"
}

Assert-NonEmpty "linkage.baselineRunId" $doc.linkage.baselineRunId
Assert-NonEmpty "linkage.subjectRunId" $doc.linkage.subjectRunId
Assert-NonEmpty "linkage.subjectTopologyAuthorizationDecisionId" $doc.linkage.subjectTopologyAuthorizationDecisionId
Assert-NonEmpty "linkage.subjectPlanningDecisionId" $doc.linkage.subjectPlanningDecisionId

if ($null -ne $doc.linkage.baselineTopologyAuthorizationDecisionId) {
    throw "Kanon topology evidence: baselineTopologyAuthorizationDecisionId must be null"
}
if ($doc.topologySummary.baseline.topologyAuthorizationDecisionId -ne $null) {
    throw "Kanon topology evidence: baseline topologyAuthorizationDecisionId must be null"
}
if ($doc.topologySummary.baseline.requiresKanonAuthorization -ne $false) {
    throw "Kanon topology evidence: baseline requiresKanonAuthorization must be false"
}
if ($doc.topologySummary.subject.requiresKanonAuthorization -ne $true) {
    throw "Kanon topology evidence: subject requiresKanonAuthorization must be true"
}
if ($doc.topologySummary.subject.selectedTopology -ne "centralized_orchestrator") {
    throw "Kanon topology evidence: subject selectedTopology must be centralized_orchestrator"
}
if ($doc.safety.productionReadiness -ne "not_claimed") {
    throw "Kanon topology evidence: safety.productionReadiness must be not_claimed"
}

Write-Host "Kanon topology evidence validation PASS: $ReportPath"
