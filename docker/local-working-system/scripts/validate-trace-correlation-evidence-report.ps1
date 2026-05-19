# TRACE-CONTRACT-001 — validate trace-contract-001-evidence-report.json acceptance fields.

param(
    [string]$ReportPath = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

$composeRoot = Get-DockerLocalComposeRoot
if ([string]::IsNullOrWhiteSpace($ReportPath)) {
    $ReportPath = Join-Path $composeRoot "artifacts\trace-contract-001-evidence-report.json"
}

$config = Get-DockerLocalComposeConfig
$secretPatterns = @(Get-DockerLocalSecretPatterns -ComposeConfig $config)

if (-not (Test-Path -LiteralPath $ReportPath)) {
    throw "Trace correlation evidence report not found: $ReportPath"
}

$raw = Get-Content -Raw -LiteralPath $ReportPath
Assert-ReportHasNoSecretPatterns -Json $raw -SecretPatterns $secretPatterns

$doc = $raw | ConvertFrom-Json

function Assert-NonEmpty([string]$name, $value) {
    if ([string]::IsNullOrWhiteSpace([string]$value)) {
        throw "Trace correlation evidence: $name must be non-empty"
    }
}

if ($doc.schema -ne "trace-contract-001-evidence-report-v1") {
    throw "Trace correlation evidence: schema must be trace-contract-001-evidence-report-v1"
}
if ($doc.verdict -ne "PASS") {
    throw "Trace correlation evidence: verdict must be PASS"
}

Assert-NonEmpty "probe.traceId" $doc.probe.traceId
Assert-NonEmpty "probe.correlationId" $doc.probe.correlationId
Assert-NonEmpty "probe.runId" $doc.probe.runId
Assert-NonEmpty "probe.planningDecisionId" $doc.probe.planningDecisionId
Assert-NonEmpty "probe.modelCallId" $doc.probe.modelCallId
Assert-NonEmpty "probe.responseTraceHeader" $doc.probe.responseTraceHeader

if ($doc.probe.traceId -eq $doc.probe.correlationId) {
    throw "Trace correlation evidence: probe traceId and correlationId must differ"
}
if ($doc.probe.responseTraceHeader -ne $doc.probe.traceId) {
    throw "Trace correlation evidence: responseTraceHeader must match probe traceId"
}
if ($doc.kanon.planningTraceId -and $doc.kanon.planningTraceId -ne $doc.probe.traceId) {
    throw "Trace correlation evidence: Kanon planning traceId must match probe traceId"
}
if ($doc.kanon.planningCorrelationId -ne $doc.probe.correlationId) {
    throw "Trace correlation evidence: Kanon planning correlationId must match probe correlationId"
}
if ($doc.kanon.planningCorrelationId -eq $doc.probe.traceId) {
    throw "Trace correlation evidence: Kanon correlationId must differ from traceId"
}
if ($doc.conexus.metadataAllagmaRunId -ne $doc.probe.runId) {
    throw "Trace correlation evidence: Conexus allagma_run_id must match probe runId"
}
if ($doc.conexus.metadataCorrelationId -ne $doc.probe.correlationId) {
    throw "Trace correlation evidence: Conexus correlation_id must match probe correlationId"
}
if ($doc.kanon.planningListedByTrace -ne $true) {
    throw "Trace correlation evidence: planning decision must be listed by trace"
}
if ($doc.safety.productionReadiness -ne "not_claimed") {
    throw "Trace correlation evidence: safety.productionReadiness must be not_claimed"
}

if ($doc.guidedReplay.attempted -eq $true -and $doc.guidedReplay.status -eq "FAIL") {
    throw "Trace correlation evidence: guidedReplay failed"
}

Write-Host "Trace correlation evidence validation PASS: $ReportPath"
