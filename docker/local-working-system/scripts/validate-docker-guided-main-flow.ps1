# ENV-DOCKER-RUN-001 — validate docker-guided-main-flow-report.json acceptance fields.

param(
    [string]$ReportPath = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($ReportPath)) {
    $ReportPath = Join-Path $composeRoot "artifacts\docker-guided-main-flow-report.json"
}

if (-not (Test-Path -LiteralPath $ReportPath)) {
    throw "Docker guided main flow report not found: $ReportPath"
}

$doc = Get-Content -Raw -LiteralPath $ReportPath | ConvertFrom-Json

function Assert-NonEmpty([string]$name, $value) {
    if ([string]::IsNullOrWhiteSpace([string]$value)) {
        throw "Docker guided flow: $name must be non-empty"
    }
}

if ($doc.schema -ne "docker-guided-main-flow-report-v1") {
    throw "Docker guided flow: schema must be docker-guided-main-flow-report-v1"
}
if ($doc.verdict -ne "PASS") {
    throw "Docker guided flow: verdict must be PASS"
}

Assert-NonEmpty "baselineRunId" $doc.baselineRunId
Assert-NonEmpty "subjectRunId" $doc.subjectRunId
Assert-NonEmpty "subjectTopologyAuthorizationDecisionId" $doc.subjectTopologyAuthorizationDecisionId
Assert-NonEmpty "baselineRouteDecisionId" $doc.baselineRouteDecisionId
Assert-NonEmpty "subjectRouteDecisionId" $doc.subjectRouteDecisionId
Assert-NonEmpty "baselineEvaluationRunId" $doc.baselineEvaluationRunId
Assert-NonEmpty "subjectEvaluationRunId" $doc.subjectEvaluationRunId
Assert-NonEmpty "baselineComparisonId" $doc.baselineComparisonId

if ($doc.restart.restartPersistencePassed -ne $true) {
    throw "Docker guided flow: restart.restartPersistencePassed must be true"
}
if ($doc.safety.realExternalExecution -ne "disabled") {
    throw "Docker guided flow: safety.realExternalExecution must be disabled"
}
if ($doc.safety.productionReadiness -ne "not_claimed") {
    throw "Docker guided flow: safety.productionReadiness must be not_claimed"
}

Write-Host "Docker guided main flow validation PASS: $ReportPath"
