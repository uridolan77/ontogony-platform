# DOCKER-LOCAL-VERIFY-001 — validate docker-local-verify-001-report.json acceptance fields.

param(
    [string]$ReportPath = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

$composeRoot = Get-DockerLocalComposeRoot
if ([string]::IsNullOrWhiteSpace($ReportPath)) {
    $ReportPath = Join-Path $composeRoot "artifacts\docker-local-verify-001-report.json"
}

if (-not (Test-Path -LiteralPath $ReportPath)) {
    throw "Frontend browser provenance report not found: $ReportPath"
}

$doc = Get-Content -Raw -LiteralPath $ReportPath | ConvertFrom-Json

function Assert-NonEmpty([string]$name, $value) {
    if ([string]::IsNullOrWhiteSpace([string]$value)) {
        throw "Frontend browser provenance: $name must be non-empty"
    }
}

if ($doc.schema -ne "docker-local-verify-001-report-v1") {
    throw "Frontend browser provenance: schema must be docker-local-verify-001-report-v1"
}
if ($doc.verdict -ne "PASS") {
    throw "Frontend browser provenance: verdict must be PASS (see issues in report)"
}

Assert-NonEmpty "expected.gitSha" $doc.expected.gitSha
Assert-NonEmpty "served.provenanceGitSha" $doc.served.provenanceGitSha
Assert-NonEmpty "browser.baseUrl" $doc.browser.baseUrl

if ($doc.browser.provenanceFetchStatus -ne "ok") {
    throw "Frontend browser provenance: browser.provenanceFetchStatus must be ok"
}
if ($doc.browser.indexFetchStatus -ne "ok") {
    throw "Frontend browser provenance: browser.indexFetchStatus must be ok"
}
if ($doc.checks.matchesExpectedRepo -ne $true) {
    throw "Frontend browser provenance: checks.matchesExpectedRepo must be true"
}
if ($doc.checks.provenanceMatchesExpected -ne $true) {
    throw "Frontend browser provenance: checks.provenanceMatchesExpected must be true"
}
if ($doc.served.provenanceGitSha -ne $doc.expected.gitSha) {
    throw "Frontend browser provenance: served provenance gitSha must equal expected gitSha"
}
if (-not [string]::IsNullOrWhiteSpace([string]$doc.served.indexMetaGitSha) -and $doc.served.indexMetaGitSha -ne $doc.expected.gitSha) {
    throw "Frontend browser provenance: index meta gitSha must equal expected gitSha when present"
}
if ($doc.checks.bundleContainsGitSha -ne $true -and $doc.expected.gitSha -ne "local") {
    throw "Frontend browser provenance: main bundle must contain served git SHA"
}

if ($doc.docker.imageLabelGitSha -and $doc.docker.imageMatchesExpected -ne $true) {
    throw "Frontend browser provenance: docker image label must match expected gitSha when present"
}

Write-Host "DOCKER-LOCAL-VERIFY-001 validate PASS ($ReportPath)."
