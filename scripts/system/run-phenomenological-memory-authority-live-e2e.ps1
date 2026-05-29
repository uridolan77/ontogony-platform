param(
    [Parameter(Mandatory = $true)][string]$KanonBaseUrl,
    [Parameter(Mandatory = $true)][string]$AisthesisBaseUrl,
    [Parameter(Mandatory = $true)][string]$AllagmaBaseUrl,
    [string]$OutDir = "artifacts/phenomenological-memory-authority/live",
    [string]$AllagmaServiceToken = $env:ALLAGMA_SERVICE_TOKEN,
    [string]$KanonServiceToken = $env:KANON_SERVICE_TOKEN,
    [int]$PollTimeoutSeconds = 120,
    [int]$PollIntervalSeconds = 2
)

$ErrorActionPreference = "Stop"
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

$terminalProjectionStatuses = @(
    "projected",
    "skipped",
    "skipped_below_grade",
    "failed",
    "pending_authority",
    "not_requested"
)
$terminalAuthorityStatuses = @("applied", "rejected", "pending_review", "pending_validation")

function Get-AllagmaHeaders {
    param([hashtable]$Extra = @{})
    $headers = @{ "Content-Type" = "application/json" }
    if (-not [string]::IsNullOrWhiteSpace($AllagmaServiceToken)) {
        $headers["Authorization"] = "Bearer $AllagmaServiceToken"
    }
    foreach ($key in $Extra.Keys) { $headers[$key] = $Extra[$key] }
    return $headers
}

function Get-KanonHeaders {
  param([hashtable]$Extra = @{})
  $headers = @{
    "Content-Type"              = "application/json"
    "X-Ontogony-Actor-Id"       = "phenom-auth-live-probe"
    "X-Ontogony-Actor-Type"     = "service"
    "X-Ontogony-Roles"          = "Admin,ProvenanceReader"
  }
  if (-not [string]::IsNullOrWhiteSpace($KanonServiceToken)) {
    $headers["Authorization"] = "Bearer $KanonServiceToken"
  }
  foreach ($key in $Extra.Keys) { $headers[$key] = $Extra[$key] }
  return $headers
}

function Write-LiveSummary {
    param(
        [string]$Status,
        [array]$Checks,
        [array]$Diagnostics,
        [hashtable]$Evidence = @{}
    )
    $summary = [ordered]@{
        schemaVersion = "phenomenological-authority-certification-v2"
        packageId     = "PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001"
        mode          = "Live"
        status        = $Status
        checks        = $Checks
        diagnostics   = $Diagnostics
        evidence      = $Evidence
        generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    }
    $summaryPath = Join-Path $OutDir "summary.json"
    $summary | ConvertTo-Json -Depth 20 | Set-Content -Encoding UTF8 $summaryPath
    Write-Host "Summary: $summaryPath"
    return $summaryPath
}

function Assert-DecisionRecordUrl {
    param([string]$Url)
    if ([string]::IsNullOrWhiteSpace($Url)) {
        throw "decisionRecordUrl is missing."
    }
    if ($Url -match "/decisions/") {
        throw "decisionRecordUrl must not use /decisions/ — got '$Url'."
    }
    if ($Url -notmatch "/ontology/v0/decision-records/") {
        throw "decisionRecordUrl must contain /ontology/v0/decision-records/ — got '$Url'."
    }
}

function Wait-Until {
    param(
        [string]$Label,
        [scriptblock]$Predicate,
        [int]$TimeoutSeconds = $PollTimeoutSeconds,
        [int]$IntervalSeconds = $PollIntervalSeconds
    )
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    $lastError = $null
    while ((Get-Date) -lt $deadline) {
        try {
            $result = & $Predicate
            if ($result) { return $result }
        }
        catch {
            $lastError = $_.Exception.Message
        }
        Start-Sleep -Seconds $IntervalSeconds
    }
    $suffix = if ($lastError) { " Last error: $lastError" } else { "" }
    throw "Timed out waiting for $Label after ${TimeoutSeconds}s.$suffix"
}

function Invoke-HealthChecks {
    foreach ($pair in @(
            @{ Name = "Kanon"; Url = "$KanonBaseUrl/health" }
            @{ Name = "Aisthesis"; Url = "$AisthesisBaseUrl/health" }
            @{ Name = "Allagma"; Url = "$AllagmaBaseUrl/health" }
        )) {
        try {
            Invoke-RestMethod -Method Get -Uri $pair.Url -TimeoutSec 15 | Out-Null
            Write-Host "Health OK: $($pair.Name)"
        }
        catch {
            throw "Health check failed for $($pair.Name) at $($pair.Url): $($_.Exception.Message)"
        }
    }
}

function Invoke-ExternalLiveTrigger {
    param([string]$TriggerUrl)
    Write-Host "Triggering live probe via ONTOGONY_PHENOM_AUTH_LIVE_TRIGGER_URL"
    $response = Invoke-RestMethod -Method Post -Uri $TriggerUrl -Headers (Get-AllagmaHeaders) -Body "{}" -ContentType "application/json"
    if (-not $response.runId) { throw "Live trigger response missing runId." }
    if (-not $response.mutationId) { throw "Live trigger response missing mutationId." }
    return @{
        runId                   = [string]$response.runId
        mutationId              = [string]$response.mutationId
        traceId                 = if ($response.traceId) { [string]$response.traceId } else { $null }
        expectedAuthorityStatus = if ($response.expectedAuthorityStatus) { [string]$response.expectedAuthorityStatus } else { "pending_review" }
        expectedProjectionStatus = if ($response.expectedProjectionStatus) { [string]$response.expectedProjectionStatus } else { $null }
    }
}

function Invoke-BuiltInLiveProbe {
    $suffix = (Get-Date).ToUniversalTime().ToString("yyyyMMddHHmmss")
    $episodeId = "ep_phenom_auth_live_$suffix"
    $proposalId = "prop_phenom_auth_live_$suffix"
    $mutationId = "mut_phenom_auth_live_$suffix"
    $traceId = "trace_phenom_auth_live_$suffix"
    $idempotencyKey = "live:phenom-auth:$suffix"
    $domainId = if ($env:ONTOGONY_PHENOM_AUTH_DOMAIN_ID) { $env:ONTOGONY_PHENOM_AUTH_DOMAIN_ID } else { "samaraya" }

    Write-Host "Seeding Aisthesis episode $episodeId"
    $episodeBody = @{
        episodeId = $episodeId
        domainId  = $domainId
        profileId = "profile"
        title     = "Phenomenological authority live probe"
        summary   = "Deterministic graduation probe episode (metadata only)."
        startedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
        intention = @{ primaryIntention = "certification_probe" }
        actorRefs = @(@{ actorId = "phenom-auth-live-probe"; actorKind = "service" })
        sourceEventIds = @()
    } | ConvertTo-Json -Depth 10
    Invoke-RestMethod -Method Post -Uri "$AisthesisBaseUrl/aisthesis/v1/experience/episodes" `
        -ContentType "application/json" -Body $episodeBody | Out-Null

    Write-Host "Proposing graph mutation $mutationId"
    $proposeBody = @{
        proposalId              = $proposalId
        domainId                = $domainId
        mutationType            = "WeakenEdge"
        sourceEpisodeIds        = @($episodeId)
        sourceFeedbackAttributionIds = @()
        rationale               = "Live phenomenological authority certification probe."
        confidence              = 0.82
        requiresKanonValidation = $true
        metadata                = @{ probe = "PHENOMENOLOGICAL-AUTHORITY-CERTIFICATION-GRADUATION-001" }
    } | ConvertTo-Json -Depth 10
    Invoke-RestMethod -Method Post -Uri "$AisthesisBaseUrl/aisthesis/v1/memory/mutations/propose" `
        -ContentType "application/json" -Body $proposeBody | Out-Null

    Write-Host "Applying mutation proposal $proposalId"
    $applyBody = @{ proposalId = $proposalId; appliedBy = "phenom-auth-live-probe" } | ConvertTo-Json
    $mutation = Invoke-RestMethod -Method Post -Uri "$AisthesisBaseUrl/aisthesis/v1/memory/mutations/apply" `
        -ContentType "application/json" -Body $applyBody
    $mutationId = [string]$mutation.mutationId

    $callbackUrl = "$AisthesisBaseUrl/aisthesis/v1/memory/mutations/$mutationId/validation-outcome/kanon-callback"
    Write-Host "Requesting Kanon authority evaluation for $mutationId"
    $evaluateBody = @{
        ontologyVersionId = "gaming-core@0.1.0"
        actor = @{
            actorId   = "aisthesis:memory-graph"
            actorType = "system"
            roles     = @()
        }
        actionName   = "Aisthesis.MemoryGraphMutation"
        toolIntentId = $mutationId
        context      = @{
            mutationId                 = $mutationId
            proposalId                 = $proposalId
            domainId                   = $domainId
            mutationType               = "WeakenEdge"
            episodeIds                 = @($episodeId)
            relationCategoriesAffected = @("normative")
            semanticImpact             = "high"
            callbackUrl                = $callbackUrl
            callbackIdempotencyKey     = $idempotencyKey
            rationale                  = "Live phenomenological authority certification probe."
        }
    } | ConvertTo-Json -Depth 10

    $evaluate = Invoke-RestMethod -Method Post -Uri "$KanonBaseUrl/ontology/v0/actions/evaluate" `
        -Headers (Get-KanonHeaders @{ "X-Ontogony-Trace-Id" = $traceId }) -Body $evaluateBody
  if (-not $evaluate.decisionRecord.decisionId) {
        throw "Kanon evaluate did not return decisionRecord.decisionId."
    }

    Write-Host "Starting Allagma run for phenomenological projection"
    $runBody = @{
        ontologyVersionId = "gaming-core@0.1.0"
        actorId           = "phenom-auth-live-probe"
        actorType         = "service"
        actorRoles        = @("AgentExecutor")
        objective         = "PHENOM-AUTH-LIVE-001: phenomenological authority certification probe."
        context           = @{ playerId = "phenom-auth-live"; mutationId = $mutationId; episodeId = $episodeId }
        modelPurpose      = "summarize-player-risk"
    } | ConvertTo-Json -Depth 10

    $run = Invoke-RestMethod -Method Post -Uri "$AllagmaBaseUrl/allagma/v0/runs" `
        -Headers (Get-AllagmaHeaders @{
            "X-Ontogony-Trace-Id"       = $traceId
            "X-Ontogony-Correlation-Id" = "corr_phenom_auth_live_$suffix"
        }) -Body $runBody

    return @{
        runId                    = [string]$run.runId
        mutationId               = $mutationId
        traceId                  = $traceId
        kanonDecisionId          = [string]$evaluate.decisionRecord.decisionId
        expectedAuthorityStatus  = "pending_review"
        expectedProjectionStatus = $null
    }
}

function Get-AuthorityStatus {
    param([string]$MutationId)
    return Invoke-RestMethod -Method Get -Uri "$AisthesisBaseUrl/aisthesis/v1/memory/mutations/$([uri]::EscapeDataString($MutationId))/authority-status"
}

function Get-ProjectionStatus {
    param([string]$RunId)
    try {
        return Invoke-RestMethod -Method Get -Uri "$AllagmaBaseUrl/allagma/v0/runs/$([uri]::EscapeDataString($RunId))/phenomenological-projection" `
            -Headers (Get-AllagmaHeaders)
    }
    catch {
        if ($_.Exception.Response.StatusCode.value__ -eq 404) { return $null }
        throw
    }
}

$checks = @()
$diagnostics = @()
$evidence = @{}

try {
    Invoke-HealthChecks

    $probe = if ($env:ONTOGONY_PHENOM_AUTH_LIVE_TRIGGER_URL) {
        Invoke-ExternalLiveTrigger -TriggerUrl $env:ONTOGONY_PHENOM_AUTH_LIVE_TRIGGER_URL
    }
    else {
        Invoke-BuiltInLiveProbe
    }

    $evidence.runId = $probe.runId
    $evidence.mutationId = $probe.mutationId
    $evidence.traceId = $probe.traceId

    $authority = Wait-Until -Label "Aisthesis authority-status terminal" -Predicate {
        $status = Get-AuthorityStatus -MutationId $probe.mutationId
        if ($status.status -in $terminalAuthorityStatuses) { return $status }
        if ($status.kanon.callbackState -in @("pending_review", "delivered", "applied", "rejected", "policy_missing")) { return $status }
        $null
    }

    $authorityPath = Join-Path $OutDir "authority-status.json"
    $authority | ConvertTo-Json -Depth 20 | Set-Content -Encoding UTF8 $authorityPath
    $evidence.authorityStatusPath = $authorityPath

    if ($authority.kanon) {
        Assert-DecisionRecordUrl -Url $authority.kanon.decisionRecordUrl
        $checks += @{ name = "kanon_decision_route"; status = "PASS" }
    }
    else {
        throw "authority-status response missing kanon block."
    }

    $callbackState = if ($authority.kanon.callbackState) { $authority.kanon.callbackState } else { $authority.status }
    $allowedCallbackStates = @("applied", "rejected", "pending_review", "delivered", "policy_missing")
    if ($callbackState -notin $allowedCallbackStates) {
        throw "Unexpected callback state '$callbackState'."
    }
    if ($probe.expectedAuthorityStatus -and $authority.status -ne $probe.expectedAuthorityStatus -and $callbackState -ne $probe.expectedAuthorityStatus) {
        throw "Expected authority/callback '$($probe.expectedAuthorityStatus)' but got status='$($authority.status)' callback='$callbackState'."
    }
    $checks += @{ name = "pending_review_callback"; status = "PASS" }
    $checks += @{ name = "aisthesis_authority_status"; status = "PASS" }

    $projection = Wait-Until -Label "Allagma phenomenological-projection terminal" -Predicate {
        $status = Get-ProjectionStatus -RunId $probe.runId
        if ($null -eq $status) { return $null }
        $projectionStatus = if ($status.projectionStatus) { $status.projectionStatus } else { $status.status }
        if ($projectionStatus -in $terminalProjectionStatuses) {
            $status | Add-Member -NotePropertyName "_resolvedStatus" -NotePropertyValue $projectionStatus -Force
            return $status
        }
        $null
    }

    $projectionPath = Join-Path $OutDir "projection-status.json"
    $projection | ConvertTo-Json -Depth 20 | Set-Content -Encoding UTF8 $projectionPath
    $evidence.projectionStatusPath = $projectionPath

    $resolvedProjection = if ($projection._resolvedStatus) { $projection._resolvedStatus } `
        elseif ($projection.projectionStatus) { $projection.projectionStatus } `
        else { $projection.status }
    if ($probe.expectedProjectionStatus -and $resolvedProjection -ne $probe.expectedProjectionStatus) {
        throw "Expected projection '$($probe.expectedProjectionStatus)' but got '$resolvedProjection'."
    }
    $checks += @{ name = "allagma_projection_status"; status = "PASS" }

    & "$PSScriptRoot/Test-PhenomenologicalAuthorityRawPayloadLeakage.ps1" -OutDir $OutDir
    $checks += @{ name = "raw_payload_scan"; status = "PASS" }
    $checks += @{ name = "live_e2e"; status = "PASS" }

    Write-LiveSummary -Status "PASS" -Checks $checks -Diagnostics $diagnostics -Evidence $evidence | Out-Null
    exit 0
}
catch {
    $diagnostics += $_.Exception.Message
    Write-LiveSummary -Status "FAIL" -Checks $checks -Diagnostics $diagnostics -Evidence $evidence | Out-Null
    Write-Error $_
    exit 1
}
