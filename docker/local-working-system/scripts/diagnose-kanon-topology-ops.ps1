# KANON-OP-002 — operational diagnostics for Docker-local Kanon topology authorization evidence.
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
    $OutputPath = Join-Path $composeRoot "artifacts\kanon-op-002-topology-diagnostics-report.json"
}

$outputDir = Split-Path -Parent $OutputPath
if (-not (Test-Path -LiteralPath $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

$findings = [System.Collections.Generic.List[object]]::new()
$failures = [System.Collections.Generic.List[string]]::new()

function Add-Finding {
    param(
        [string]$Id,
        [string]$Status,
        [string]$Diagnosis,
        [string]$Detail,
        [object]$Data = $null
    )
    $entry = [ordered]@{
        id = $Id
        status = $Status
        diagnosis = $Diagnosis
        detail = $Detail
    }
    if ($null -ne $Data) { $entry.data = $Data }
    $findings.Add([pscustomobject]$entry) | Out-Null
    if ($Status -eq "FAIL") { $failures.Add("$Diagnosis`: $Detail") | Out-Null }
}

function Get-HttpStatusCode {
    param([object]$ErrorRecord)
    if ($null -eq $ErrorRecord.Exception.Response) { return $null }
    return [int]$ErrorRecord.Exception.Response.StatusCode
}

function Test-ServiceHealth {
    param(
        [string]$Name,
        [string]$BaseUrl,
        [string[]]$Paths = @("/health", "/health/live")
    )
    foreach ($path in $Paths) {
        try {
            $response = Invoke-WebRequest -Uri "$BaseUrl$path" -UseBasicParsing -TimeoutSec $HttpTimeoutSeconds
            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 300) {
                return [pscustomobject]@{ ok = $true; path = $path; statusCode = $response.StatusCode }
            }
        }
        catch { }
    }
    return [pscustomobject]@{ ok = $false; path = $null; statusCode = $null }
}

function Invoke-AllagmaTopologySummary {
    param([string]$RunId)
    $headers = @{ Authorization = "Bearer $($config.AllagmaServiceToken)" }
    return Invoke-RestMethod `
        -Uri "$($config.AllagmaBaseUrl)/allagma/v0/runs/$RunId/events?includeTopologySummary=true" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds
}

function Invoke-KanonDecisionLookup {
    param([string]$DecisionId)
    $headers = @{
        Authorization = "Bearer $($config.KanonServiceToken)"
        "X-Ontogony-Actor-Id" = "kanon-op-002"
        "X-Ontogony-Actor-Type" = "service"
        "X-Ontogony-Roles" = "ProvenanceReader"
    }
    try {
        $record = Invoke-RestMethod `
            -Uri "$($config.KanonBaseUrl)/ontology/v0/decision-records/$DecisionId" `
            -Headers $headers `
            -TimeoutSec $HttpTimeoutSeconds
        return [pscustomobject]@{ ok = $true; httpStatus = 200; record = $record; error = $null }
    }
    catch {
        return [pscustomobject]@{
            ok = $false
            httpStatus = (Get-HttpStatusCode $_.Exception)
            record = $null
            error = (Redact-StringWithPatterns -Value $_.Exception.Message -SecretPatterns $secretPatterns)
        }
    }
}

Write-Host ""
Write-Host "=== KANON-OP-002 diagnose Kanon topology ops ==="
Write-Host "Boundary: Docker-local diagnostics, not production readiness."
Write-Host "Env file: $($config.EnvFilePath)"
Write-Host ""

# Artifact presence
$reportIds = Read-DockerLocalRunReportIds -GuidedPath $GuidedReportPath -SeedPath $SeedReportPath
if ([string]::IsNullOrWhiteSpace($reportIds.source)) {
    Add-Finding -Id "artifact.report" -Status "FAIL" -Diagnosis "ARTIFACT_MISSING" `
        -Detail "No docker-guided-main-flow-report.json or env-seed-001-report.json. Run seed or guided flow first."
}
else {
    Add-Finding -Id "artifact.report" -Status "PASS" -Diagnosis "ARTIFACT_PRESENT" `
        -Detail "Using $($reportIds.source)." -Data @{ source = $reportIds.source }
}

# Service health
$kanonHealth = Test-ServiceHealth -Name "kanon" -BaseUrl $config.KanonBaseUrl
if ($kanonHealth.ok) {
    Add-Finding -Id "kanon.health" -Status "PASS" -Diagnosis "KANON_AVAILABLE" `
        -Detail "Kanon healthy at $($config.KanonBaseUrl)$($kanonHealth.path) (HTTP $($kanonHealth.statusCode))."
}
else {
    Add-Finding -Id "kanon.health" -Status "FAIL" -Diagnosis "KANON_UNAVAILABLE" `
        -Detail "Kanon not reachable at $($config.KanonBaseUrl). Check kanon-api container and KANON_HOST_PORT."
}

$allagmaHealth = Test-ServiceHealth -Name "allagma" -BaseUrl $config.AllagmaBaseUrl
if ($allagmaHealth.ok) {
    Add-Finding -Id "allagma.health" -Status "PASS" -Diagnosis "ALLAGMA_AVAILABLE" `
        -Detail "Allagma healthy at $($config.AllagmaBaseUrl)$($allagmaHealth.path) (HTTP $($allagmaHealth.statusCode))."
}
else {
    Add-Finding -Id "allagma.health" -Status "FAIL" -Diagnosis "ALLAGMA_UNAVAILABLE" `
        -Detail "Allagma not reachable at $($config.AllagmaBaseUrl). Check allagma-api container and ALLAGMA_HOST_PORT."
}

if ([string]::IsNullOrWhiteSpace($reportIds.baselineRunId) -or [string]::IsNullOrWhiteSpace($reportIds.subjectRunId)) {
    Add-Finding -Id "artifact.runIds" -Status "FAIL" -Diagnosis "ARTIFACT_INCOMPLETE" `
        -Detail "Report missing baselineRunId or subjectRunId."
}
elseif (-not $kanonHealth.ok -or -not $allagmaHealth.ok) {
    Add-Finding -Id "topology.live" -Status "WARN" -Diagnosis "LIVE_CHECKS_SKIPPED" `
        -Detail "Skipping live topology checks because Kanon or Allagma health failed."
}
else {
    try {
        $baselineTopology = Invoke-AllagmaTopologySummary -RunId $reportIds.baselineRunId
        $subjectTopology = Invoke-AllagmaTopologySummary -RunId $reportIds.subjectRunId

        $baselineAuthId = $baselineTopology.topology.topologyAuthorizationDecisionId
        $subjectAuthId = $subjectTopology.topology.topologyAuthorizationDecisionId
        $baselineRequiresAuth = [bool]$baselineTopology.topology.topologySelection.value.requiresKanonAuthorization
        $subjectRequiresAuth = [bool]$subjectTopology.topology.topologySelection.value.requiresKanonAuthorization
        $baselineSelected = [string]$baselineTopology.topology.topologySelection.value.selectedTopology
        $subjectSelected = [string]$subjectTopology.topology.topologySelection.value.selectedTopology

        if ($baselineRequiresAuth) {
            Add-Finding -Id "baseline.topology" -Status "WARN" -Diagnosis "BASELINE_UNEXPECTED_AUTH_REQUIRED" `
                -Detail "Baseline requiresKanonAuthorization=true (expected false for single_workflow seed path)."
        }
        elseif ($null -eq $baselineAuthId) {
            Add-Finding -Id "baseline.topology" -Status "PASS" -Diagnosis "BASELINE_NULL_BY_DESIGN" `
                -Detail "Baseline topologyAuthorizationDecisionId is null; Allagma did not call Kanon topology evaluate (requiresKanonAuthorization=false)." `
                -Data @{ selectedTopology = $baselineSelected; requiresKanonAuthorization = $false }
        }
        else {
            Add-Finding -Id "baseline.topology" -Status "WARN" -Diagnosis "BASELINE_UNEXPECTED_AUTH_ID" `
                -Detail "Baseline has topologyAuthorizationDecisionId='$baselineAuthId' but requiresKanonAuthorization=false."
        }

        if (-not $subjectRequiresAuth) {
            Add-Finding -Id "subject.topology" -Status "FAIL" -Diagnosis "ALLAGMA_NO_KANON_CALL" `
                -Detail "Subject requiresKanonAuthorization=false. For centralized_orchestrator override, Allagma should require Kanon authorization." `
                -Data @{ selectedTopology = $subjectSelected }
        }
        elseif ([string]::IsNullOrWhiteSpace([string]$subjectAuthId)) {
            Add-Finding -Id "subject.topology" -Status "FAIL" -Diagnosis "SUBJECT_MISSING_AUTH_ID" `
                -Detail "Subject requires Kanon authorization but topologyAuthorizationDecisionId is missing. Check Allagma topology events and Kanon connectivity." `
                -Data @{ selectedTopology = $subjectSelected }
        }
        else {
            Add-Finding -Id "subject.topology" -Status "PASS" -Diagnosis "SUBJECT_AUTH_REQUIRED" `
                -Detail "Subject has topologyAuthorizationDecisionId and requiresKanonAuthorization=true." `
                -Data @{
                    selectedTopology = $subjectSelected
                    topologyAuthorizationDecisionId = [string]$subjectAuthId
                }

            if (-not [string]::IsNullOrWhiteSpace($reportIds.subjectTopologyAuthorizationDecisionId) `
                -and [string]$reportIds.subjectTopologyAuthorizationDecisionId -ne [string]$subjectAuthId) {
                Add-Finding -Id "artifact.stale" -Status "WARN" -Diagnosis "REPORT_STALE" `
                    -Detail "Report subjectTopologyAuthorizationDecisionId differs from live Allagma topology summary. Re-run guided flow." `
                    -Data @{
                        reportDecisionId = $reportIds.subjectTopologyAuthorizationDecisionId
                        liveDecisionId = [string]$subjectAuthId
                    }
            }

            $lookup = Invoke-KanonDecisionLookup -DecisionId ([string]$subjectAuthId)
            if ($lookup.ok) {
                $outcome = [string]$lookup.record.outcome.status
                $decisionType = [string]$lookup.record.decisionType
                if ($decisionType -ne "topology_policy_evaluation") {
                    Add-Finding -Id "subject.kanon.record" -Status "WARN" -Diagnosis "KANON_UNEXPECTED_DECISION_TYPE" `
                        -Detail "Decision type is '$decisionType' (expected topology_policy_evaluation)."
                }
                switch ($outcome.ToLowerInvariant()) {
                    "allow" {
                        Add-Finding -Id "subject.kanon.outcome" -Status "PASS" -Diagnosis "KANON_ALLOW" `
                            -Detail "Kanon topology policy outcome is allow (Docker-local happy path)." `
                            -Data @{ decisionId = [string]$subjectAuthId; outcomeStatus = $outcome }
                    }
                    "deny" {
                        Add-Finding -Id "subject.kanon.outcome" -Status "FAIL" -Diagnosis "KANON_DENY" `
                            -Detail "Kanon denied the requested topology. Inspect decision record rules and override context." `
                            -Data @{ decisionId = [string]$subjectAuthId; outcomeStatus = $outcome }
                    }
                    "human_gate" {
                        Add-Finding -Id "subject.kanon.outcome" -Status "WARN" -Diagnosis "KANON_HUMAN_GATE" `
                            -Detail "Kanon returned human_gate; run may be paused waiting for approval." `
                            -Data @{ decisionId = [string]$subjectAuthId; outcomeStatus = $outcome }
                    }
                    default {
                        Add-Finding -Id "subject.kanon.outcome" -Status "WARN" -Diagnosis "KANON_UNKNOWN_OUTCOME" `
                            -Detail "Unexpected outcome status '$outcome'." `
                            -Data @{ decisionId = [string]$subjectAuthId; outcomeStatus = $outcome }
                    }
                }
            }
            elseif ($lookup.httpStatus -eq 401 -or $lookup.httpStatus -eq 403) {
                Add-Finding -Id "subject.kanon.lookup" -Status "FAIL" -Diagnosis "KANON_AUTH_FAILURE" `
                    -Detail "Decision-record lookup returned HTTP $($lookup.httpStatus). Check KANON_SERVICE_TOKEN and ProvenanceReader role headers." `
                    -Data @{ decisionId = [string]$subjectAuthId; httpStatus = $lookup.httpStatus }
            }
            elseif ($lookup.httpStatus -eq 404) {
                Add-Finding -Id "subject.kanon.lookup" -Status "FAIL" -Diagnosis "KANON_DECISION_NOT_FOUND" `
                    -Detail "Decision record not found in Kanon (HTTP 404). Check Kanon Postgres persistence and run/decision linkage." `
                    -Data @{ decisionId = [string]$subjectAuthId; httpStatus = 404 }
            }
            else {
                Add-Finding -Id "subject.kanon.lookup" -Status "FAIL" -Diagnosis "KANON_LOOKUP_FAILED" `
                    -Detail "Decision-record lookup failed: $($lookup.error)" `
                    -Data @{ decisionId = [string]$subjectAuthId; httpStatus = $lookup.httpStatus }
            }
        }
    }
    catch {
        $msg = Redact-StringWithPatterns -Value $_.Exception.Message -SecretPatterns $secretPatterns
        Add-Finding -Id "topology.live" -Status "FAIL" -Diagnosis "ALLAGMA_TOPOLOGY_FETCH_FAILED" `
            -Detail "Failed to fetch Allagma topology summary: $msg"
    }
}

$verdict = if ($failures.Count -eq 0) { "PASS" } else { "FAIL" }

$report = [ordered]@{
    schema = "kanon-op-002-topology-diagnostics-report-v1"
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    verdict = $verdict
    boundary = "first Dockerized local working system, not production readiness"
    services = [ordered]@{
        allagmaBaseUrl = $config.AllagmaBaseUrl
        kanonBaseUrl = $config.KanonBaseUrl
        envFilePath = (Redact-StringWithPatterns -Value $config.EnvFilePath -SecretPatterns $secretPatterns)
    }
    inputReport = [ordered]@{
        source = $reportIds.source
        baselineRunId = $reportIds.baselineRunId
        subjectRunId = $reportIds.subjectRunId
        subjectTopologyAuthorizationDecisionId = $reportIds.subjectTopologyAuthorizationDecisionId
    }
    findings = @($findings)
    failureCount = $failures.Count
    safety = [ordered]@{
        realProviderKeys = "no"
        realExternalExecution = "disabled"
        productionReadiness = "not_claimed"
        secretsRedacted = $true
        tokensLoadedFromEnvFile = $true
    }
}

$json = $report | ConvertTo-Json -Depth 12
Assert-ReportHasNoSecretPatterns -Json $json -SecretPatterns $secretPatterns

$json | Set-Content -LiteralPath $OutputPath -Encoding UTF8
Write-Host "Wrote Kanon topology diagnostics report: $OutputPath"
Write-Host "KANON-OP-002 diagnose verdict: $verdict"

if ($verdict -ne "PASS") {
    foreach ($f in $failures) { Write-Host "FAIL $f" }
    exit 1
}

exit 0
