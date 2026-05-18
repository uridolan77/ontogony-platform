# ENV-SEED-001 — deterministic seed/bootstrap + runtime verification for Docker local working system.
# Uses service APIs/domain flows (no raw schema SQL) to bootstrap Conexus fake provider route,
# verify Kanon topology authorization behavior, and verify Allagma evaluation persistence APIs.

param(
    [string]$AllagmaBaseUrl = "http://localhost:5083",
    [string]$KanonBaseUrl = "http://localhost:5081",
    [string]$ConexusBaseUrl = "http://localhost:5082",
    [string]$ConexusAdminApiKey = "cx-conexus-admin-dev",
    [string]$ConexusProjectApiKey = "cx-dev-key-change-me",
    [string]$AllagmaServiceToken = "allagma-dev-service-token-change-in-production",
    [string]$KanonServiceToken = "kanon-dev-service-token-change-in-production",
    [string]$ScenarioId = "scenario-risk-summary-v0",
    [string]$EvaluationProfileId = "eval-profile-v0",
    [int]$HealthTimeoutSeconds = 120,
    [string]$OutputPath = "",
    [switch]$AllowMissingRouteDecisionId
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $composeRoot "artifacts\env-seed-001-report.json"
}
$outputDir = Split-Path -Parent $OutputPath
if (-not (Test-Path -LiteralPath $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

function Wait-Healthy {
    param(
        [string]$Name,
        [string]$BaseUrl
    )
    $deadline = (Get-Date).AddSeconds($HealthTimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        foreach ($path in @("/health", "/ready")) {
            try {
                $response = Invoke-WebRequest -Uri "$BaseUrl$path" -UseBasicParsing -TimeoutSec 5
                if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 300) {
                    Write-Host "PASS health: $Name at $BaseUrl$path"
                    return
                }
            }
            catch {
                # Retry until timeout.
            }
        }
        Start-Sleep -Seconds 2
    }
    throw "$Name did not become healthy within ${HealthTimeoutSeconds}s ($BaseUrl)."
}

function Start-AllagmaRun {
    param(
        [hashtable]$Context,
        [string]$Label
    )
    $body = @{
        ontologyVersionId = "gaming-core@0.1.0"
        actorId = "env-seed-001-agent"
        actorType = "service"
        actorRoles = @("AgentExecutor", "RiskAnalyst")
        objective = "Summarize risk status for player 123."
        context = $Context
    } | ConvertTo-Json -Depth 8

    Write-Host "POST $Label run ..."
    $response = Invoke-RestMethod `
        -Method Post `
        -Uri "$AllagmaBaseUrl/allagma/v0/runs" `
        -Headers $script:AllagmaHeaders `
        -ContentType "application/json" `
        -Body $body

    if ([string]::IsNullOrWhiteSpace($response.runId)) { throw "$Label runId missing." }
    if ([string]::IsNullOrWhiteSpace($response.planningDecisionId)) { throw "$Label planningDecisionId missing." }
    if ([string]::IsNullOrWhiteSpace($response.modelCallId)) { throw "$Label modelCallId missing." }
    if ($response.status -ne "Completed") {
        throw "$Label expected Completed status but was '$($response.status)'."
    }

    return $response
}

function Assert-Topology {
    param(
        [object]$Topology,
        [string]$ExpectedTopology,
        [bool]$ExpectedAuthorizationRequired,
        [string]$Label
    )

    if ($null -eq $Topology) { throw "$Label topology summary missing." }
    if ($null -eq $Topology.topologySelection) { throw "$Label topologySelection missing." }
    if ($Topology.topologySelection.status -ne "available") { throw "$Label topologySelection.status must be 'available'." }

    $selected = $Topology.topologySelection.value.selectedTopology
    if ($selected -ne $ExpectedTopology) {
        throw "$Label selectedTopology expected '$ExpectedTopology' but was '$selected'."
    }

    $requiresAuth = [bool]$Topology.topologySelection.value.requiresKanonAuthorization
    if ($requiresAuth -ne $ExpectedAuthorizationRequired) {
        throw "$Label requiresKanonAuthorization expected '$ExpectedAuthorizationRequired' but was '$requiresAuth'."
    }
}

try {
    Write-Host "=== ENV-SEED-001 seed + verify ==="
    Write-Host "Boundary: first Dockerized local working system, not production readiness."
    Write-Host "Mode: host-local API verification (Docker compose networking proof is deferred)."
    Write-Host "Output: $OutputPath"

    Wait-Healthy -Name "Kanon" -BaseUrl $KanonBaseUrl
    Wait-Healthy -Name "Conexus" -BaseUrl $ConexusBaseUrl
    Wait-Healthy -Name "Allagma" -BaseUrl $AllagmaBaseUrl

    $script:AllagmaHeaders = @{ Authorization = "Bearer $AllagmaServiceToken" }
    $conexusAdminHeaders = @{ "X-Conexus-Admin-Key" = $ConexusAdminApiKey }
    $conexusProjectHeaders = @{ Authorization = "Bearer $ConexusProjectApiKey" }
    $kanonHeaders = @{
        "X-Ontogony-Actor-Id" = "env-seed-001"
        "X-Ontogony-Actor-Type" = "service"
        "X-Ontogony-Roles" = "ProvenanceReader"
        Authorization = "Bearer $KanonServiceToken"
    }

    $bootstrapBody = @{
        projectId = "dev-project"
        displayName = "Development Project"
        modelAlias = "gpt-4o-mini"
        providerKey = "fake"
        providerModel = "fake.chat"
        createProjectKey = $true
    } | ConvertTo-Json

    $bootstrap = Invoke-RestMethod `
        -Method Post `
        -Uri "$ConexusBaseUrl/admin/v0/dev/bootstrap" `
        -Headers $conexusAdminHeaders `
        -ContentType "application/json" `
        -Body $bootstrapBody

    if ($bootstrap.projectId -ne "dev-project") { throw "Conexus bootstrap projectId mismatch." }
    if ($bootstrap.alias -ne "gpt-4o-mini") { throw "Conexus bootstrap alias mismatch." }
    if ($bootstrap.providerKey -ne "fake") { throw "Conexus bootstrap providerKey mismatch." }
    if (-not [string]::IsNullOrWhiteSpace($bootstrap.apiKey) -and $bootstrap.apiKey -ne $ConexusProjectApiKey) {
        throw "Conexus bootstrap apiKey does not match ConexusProjectApiKey."
    }
    Write-Host "PASS bootstrap: Conexus dev project + fake provider route."

    $baselineRun = Start-AllagmaRun -Context @{ playerId = "123" } -Label "baseline(single_workflow)"
    $subjectRun = Start-AllagmaRun -Context @{ playerId = "123"; topologyOverride = "centralized_orchestrator" } -Label "subject(centralized_orchestrator)"

    $baselineEventsWithTopology = Invoke-RestMethod `
        -Uri "$AllagmaBaseUrl/allagma/v0/runs/$($baselineRun.runId)/events?includeTopologySummary=true" `
        -Headers $script:AllagmaHeaders
    $subjectEventsWithTopology = Invoke-RestMethod `
        -Uri "$AllagmaBaseUrl/allagma/v0/runs/$($subjectRun.runId)/events?includeTopologySummary=true" `
        -Headers $script:AllagmaHeaders

    Assert-Topology -Topology $baselineEventsWithTopology.topology -ExpectedTopology "single_workflow" -ExpectedAuthorizationRequired $false -Label "baseline"
    Assert-Topology -Topology $subjectEventsWithTopology.topology -ExpectedTopology "centralized_orchestrator" -ExpectedAuthorizationRequired $true -Label "subject"

    if ($null -ne $baselineEventsWithTopology.topology.topologyAuthorizationDecisionId) {
        throw "baseline topologyAuthorizationDecisionId must be null by design."
    }
    $subjectTopologyDecisionId = $subjectEventsWithTopology.topology.topologyAuthorizationDecisionId
    if ([string]::IsNullOrWhiteSpace($subjectTopologyDecisionId)) {
        throw "subject topologyAuthorizationDecisionId must be present."
    }

    $subjectTopologyDecision = Invoke-RestMethod `
        -Uri "$KanonBaseUrl/ontology/v0/decision-records/$subjectTopologyDecisionId" `
        -Headers $kanonHeaders
    if ([string]::IsNullOrWhiteSpace($subjectTopologyDecision.decisionId)) {
        throw "Kanon decision record for subject topology authorization was not found."
    }
    Write-Host "PASS topology: baseline null auth ID; subject auth decision recorded."

    $baselineModel = Invoke-RestMethod `
        -Uri "$ConexusBaseUrl/conexus/v0/model-calls/$($baselineRun.modelCallId)" `
        -Headers $conexusProjectHeaders
    $subjectModel = Invoke-RestMethod `
        -Uri "$ConexusBaseUrl/conexus/v0/model-calls/$($subjectRun.modelCallId)" `
        -Headers $conexusProjectHeaders

    $missingRouteDecisionIds = @()
    if ([string]::IsNullOrWhiteSpace($baselineModel.routeDecisionId)) { $missingRouteDecisionIds += "baseline" }
    if ([string]::IsNullOrWhiteSpace($subjectModel.routeDecisionId)) { $missingRouteDecisionIds += "subject" }
    if ($missingRouteDecisionIds.Count -gt 0 -and -not $AllowMissingRouteDecisionId) {
        throw "Missing routeDecisionId for: $($missingRouteDecisionIds -join ', '). Use -AllowMissingRouteDecisionId only if explicitly documenting limitation."
    }

    $baselineRoute = $null
    $subjectRoute = $null
    if (-not [string]::IsNullOrWhiteSpace($baselineModel.routeDecisionId)) {
        $baselineRoute = Invoke-RestMethod `
            -Uri "$ConexusBaseUrl/admin/v0/route-decisions/$($baselineModel.routeDecisionId)" `
            -Headers $conexusAdminHeaders
    }
    if (-not [string]::IsNullOrWhiteSpace($subjectModel.routeDecisionId)) {
        $subjectRoute = Invoke-RestMethod `
            -Uri "$ConexusBaseUrl/admin/v0/route-decisions/$($subjectModel.routeDecisionId)" `
            -Headers $conexusAdminHeaders
    }
    Write-Host "PASS route evidence: routeDecisionId captured on model-call evidence."

    $writeBody = @{
        scenarioId = $ScenarioId
        evaluationProfileId = $EvaluationProfileId
        sourceKind = "runtime_evidence"
    } | ConvertTo-Json

    $baselineEval = Invoke-RestMethod -Method Post `
        -Uri "$AllagmaBaseUrl/allagma/v0/runs/$($baselineRun.runId)/evaluations" `
        -Headers $script:AllagmaHeaders -ContentType "application/json" -Body $writeBody
    $subjectEval = Invoke-RestMethod -Method Post `
        -Uri "$AllagmaBaseUrl/allagma/v0/runs/$($subjectRun.runId)/evaluations" `
        -Headers $script:AllagmaHeaders -ContentType "application/json" -Body $writeBody

    $baselineEvalList = Invoke-RestMethod `
        -Uri "$AllagmaBaseUrl/allagma/v0/runs/$($baselineRun.runId)/evaluations" `
        -Headers $script:AllagmaHeaders
    $subjectEvalList = Invoke-RestMethod `
        -Uri "$AllagmaBaseUrl/allagma/v0/runs/$($subjectRun.runId)/evaluations" `
        -Headers $script:AllagmaHeaders

    if (-not ($baselineEvalList.items | Where-Object { $_.evaluationRunId -eq $baselineEval.evaluationRunId })) {
        throw "baseline evaluation not listed after write."
    }
    if (-not ($subjectEvalList.items | Where-Object { $_.evaluationRunId -eq $subjectEval.evaluationRunId })) {
        throw "subject evaluation not listed after write."
    }
    Write-Host "PASS evaluation persistence APIs: write + list."

    $comparisonBody = @{
        baselineRunId = $baselineRun.runId
        subjectRunId = $subjectRun.runId
        scenarioId = $ScenarioId
    } | ConvertTo-Json

    $comparison = Invoke-RestMethod -Method Post `
        -Uri "$AllagmaBaseUrl/allagma/v0/evaluations/baseline-comparisons" `
        -Headers $script:AllagmaHeaders -ContentType "application/json" -Body $comparisonBody
    $comparisonFetched = Invoke-RestMethod `
        -Uri "$AllagmaBaseUrl/allagma/v0/evaluations/baseline-comparisons/$($comparison.comparisonId)" `
        -Headers $script:AllagmaHeaders

    if ([string]::IsNullOrWhiteSpace($comparisonFetched.comparisonId)) {
        throw "Baseline comparison fetch failed."
    }
    Write-Host "PASS baseline comparison: create + fetch."

    $report = [ordered]@{
        schema = "env-seed-001-report-v1"
        generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
        verdict = "PASS"
        boundary = "first Dockerized local working system, not production readiness"
        verificationMode = "host-local-api"
        dockerComposeNetworkingVerified = $false
        services = [ordered]@{
            allagmaBaseUrl = $AllagmaBaseUrl
            kanonBaseUrl = $KanonBaseUrl
            conexusBaseUrl = $ConexusBaseUrl
        }
        bootstrap = [ordered]@{
            projectId = $bootstrap.projectId
            alias = $bootstrap.alias
            providerKey = $bootstrap.providerKey
            warnings = @($bootstrap.warnings)
        }
        runs = [ordered]@{
            baselineRunId = $baselineRun.runId
            subjectRunId = $subjectRun.runId
            baselineModelCallId = $baselineRun.modelCallId
            subjectModelCallId = $subjectRun.modelCallId
        }
        topology = [ordered]@{
            baselineSelectedTopology = $baselineEventsWithTopology.topology.topologySelection.value.selectedTopology
            baselineTopologyAuthorizationDecisionId = $baselineEventsWithTopology.topology.topologyAuthorizationDecisionId
            subjectSelectedTopology = $subjectEventsWithTopology.topology.topologySelection.value.selectedTopology
            subjectTopologyAuthorizationDecisionId = $subjectTopologyDecisionId
        }
        routeEvidence = [ordered]@{
            baselineRouteDecisionId = $baselineModel.routeDecisionId
            subjectRouteDecisionId = $subjectModel.routeDecisionId
            baselineRouteDecisionFound = ($null -ne $baselineRoute)
            subjectRouteDecisionFound = ($null -ne $subjectRoute)
        }
        evaluations = [ordered]@{
            scenarioId = $ScenarioId
            evaluationProfileId = $EvaluationProfileId
            baselineEvaluationRunId = $baselineEval.evaluationRunId
            subjectEvaluationRunId = $subjectEval.evaluationRunId
            baselineEvaluationCount = @($baselineEvalList.items).Count
            subjectEvaluationCount = @($subjectEvalList.items).Count
            baselineComparisonId = $comparisonFetched.comparisonId
        }
    }

    $report | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $OutputPath -Encoding UTF8
    Write-Host "Wrote seed report: $OutputPath"
    Write-Host "ENV-SEED-001 seed + verify PASS."
    exit 0
}
catch {
    Write-Error "ENV-SEED-001 seed + verify FAIL: $($_.Exception.Message)"
    exit 1
}
