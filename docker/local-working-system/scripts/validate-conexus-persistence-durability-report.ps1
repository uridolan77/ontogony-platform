# CONEXUS-PERSIST-003 — validate conexus-persist-003-durability-report.json acceptance fields.

param(
    [string]$ReportPath = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($ReportPath)) {
    $ReportPath = Join-Path $composeRoot "artifacts\conexus-persist-003-durability-report.json"
}

if (-not (Test-Path -LiteralPath $ReportPath)) {
    throw "Conexus durability report not found: $ReportPath"
}

$doc = Get-Content -Raw -LiteralPath $ReportPath | ConvertFrom-Json

function Assert-NonEmpty([string]$name, $value) {
    if ([string]::IsNullOrWhiteSpace([string]$value)) {
        throw "Conexus durability report: $name must be non-empty"
    }
}

function Assert-True([string]$name, $value) {
    if ($value -ne $true) {
        throw "Conexus durability report: $name must be true"
    }
}

if ($doc.schema -ne "conexus-persist-003-durability-report-v1") {
    throw "Conexus durability report: schema must be conexus-persist-003-durability-report-v1"
}
if ($doc.verdict -ne "PASS") {
    throw "Conexus durability report: verdict must be PASS"
}

Assert-NonEmpty "routeEvidence.baselineRouteDecisionId" $doc.routeEvidence.baselineRouteDecisionId
Assert-NonEmpty "routeEvidence.subjectRouteDecisionId" $doc.routeEvidence.subjectRouteDecisionId
Assert-True "routeEvidence.adminFetchBeforeRestart.baseline" $doc.routeEvidence.adminFetchBeforeRestart.baseline
Assert-True "routeEvidence.adminFetchBeforeRestart.subject" $doc.routeEvidence.adminFetchBeforeRestart.subject
Assert-True "routeEvidence.adminFetchAfterRestart.baseline" $doc.routeEvidence.adminFetchAfterRestart.baseline
Assert-True "routeEvidence.adminFetchAfterRestart.subject" $doc.routeEvidence.adminFetchAfterRestart.subject

Assert-True "routing.beforeRestart.fakeProviderEnabled" $doc.routing.beforeRestart.fakeProviderEnabled
Assert-True "routing.beforeRestart.modelAliasEnabled" $doc.routing.beforeRestart.modelAliasEnabled
Assert-True "routing.afterRestart.fakeProviderEnabled" $doc.routing.afterRestart.fakeProviderEnabled
Assert-True "routing.afterRestart.modelAliasEnabled" $doc.routing.afterRestart.modelAliasEnabled

if ($doc.readiness.afterRestart.healthLive.statusCode -lt 200 -or $doc.readiness.afterRestart.healthLive.statusCode -ge 300) {
    throw "Conexus durability report: readiness.afterRestart.healthLive must be 2xx"
}

Assert-True "configuration.keysAligned" $doc.configuration.keysAligned
if ($doc.validation002.beforeVerdict -ne "PASS") {
    throw "Conexus durability report: validation002.beforeVerdict must be PASS"
}
if ($doc.validation002.afterVerdict -ne "PASS") {
    throw "Conexus durability report: validation002.afterVerdict must be PASS"
}

if ($doc.restart.service -ne "conexus-api") {
    throw "Conexus durability report: restart.service must be conexus-api"
}
Assert-True "restart.restarted" $doc.restart.restarted
Assert-True "restart.restartPersistencePassed" $doc.restart.restartPersistencePassed

if ($doc.safety.realExternalExecution -ne "disabled") {
    throw "Conexus durability report: safety.realExternalExecution must be disabled"
}
if ($doc.safety.productionReadiness -ne "not_claimed") {
    throw "Conexus durability report: safety.productionReadiness must be not_claimed"
}

$rawReport = Get-Content -Raw -LiteralPath $ReportPath
$secretPatterns = @(
    "cx-dev-key-change-me",
    "cx-conexus-admin-dev",
    "conexus_local_pw",
    "ontogony_admin_pw"
)
foreach ($pattern in $secretPatterns) {
    if ($rawReport -match [regex]::Escape($pattern)) {
        throw "Conexus durability report must not contain raw secret pattern: $pattern"
    }
}

Write-Host "Conexus durability report validation PASS: $ReportPath"
