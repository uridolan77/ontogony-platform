# KANON-OP-002 — validate kanon-op-002-topology-diagnostics-report.json acceptance fields.

param(
    [string]$ReportPath = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

$composeRoot = Get-DockerLocalComposeRoot
if ([string]::IsNullOrWhiteSpace($ReportPath)) {
    $ReportPath = Join-Path $composeRoot "artifacts\kanon-op-002-topology-diagnostics-report.json"
}

$config = Get-DockerLocalComposeConfig
$secretPatterns = @(Get-DockerLocalSecretPatterns -ComposeConfig $config)

if (-not (Test-Path -LiteralPath $ReportPath)) {
    throw "Kanon topology diagnostics report not found: $ReportPath"
}

$raw = Get-Content -Raw -LiteralPath $ReportPath
Assert-ReportHasNoSecretPatterns -Json $raw -SecretPatterns $secretPatterns

$doc = $raw | ConvertFrom-Json

if ($doc.schema -ne "kanon-op-002-topology-diagnostics-report-v1") {
    throw "Kanon topology diagnostics: schema must be kanon-op-002-topology-diagnostics-report-v1"
}
if ($doc.verdict -ne "PASS") {
    throw "Kanon topology diagnostics: verdict must be PASS (was '$($doc.verdict)')"
}
if ($doc.safety.productionReadiness -ne "not_claimed") {
    throw "Kanon topology diagnostics: safety.productionReadiness must be not_claimed"
}
if ($doc.safety.tokensLoadedFromEnvFile -ne $true) {
    throw "Kanon topology diagnostics: safety.tokensLoadedFromEnvFile must be true"
}

$requiredDiagnoses = @(
    "ARTIFACT_PRESENT",
    "KANON_AVAILABLE",
    "ALLAGMA_AVAILABLE",
    "BASELINE_NULL_BY_DESIGN",
    "SUBJECT_AUTH_REQUIRED",
    "KANON_ALLOW"
)

$found = @{}
foreach ($f in $doc.findings) {
    $found[$f.diagnosis] = $true
}

foreach ($code in $requiredDiagnoses) {
    if (-not $found.ContainsKey($code)) {
        throw "Kanon topology diagnostics: expected finding diagnosis '$code' missing"
    }
}

Write-Host "Kanon topology diagnostics validation PASS: $ReportPath"
