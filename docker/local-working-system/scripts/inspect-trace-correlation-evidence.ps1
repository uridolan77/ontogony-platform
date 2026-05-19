# TRACE-CONTRACT-001 — correlation probe + cross-service trace evidence for Docker-local stack.
# Writes redacted JSON under docker/local-working-system/artifacts/.

param(
    [string]$GuidedReportPath = "",
    [string]$SeedReportPath = "",
    [string]$OutputPath = "",
    [string]$AllagmaBaseUrl = "",
    [string]$KanonBaseUrl = "",
    [string]$ConexusBaseUrl = "",
    [string]$AllagmaServiceToken = "",
    [string]$KanonServiceToken = "",
    [string]$ConexusAdminApiKey = "",
    [switch]$SkipGuidedReplay,
    [int]$HttpTimeoutSeconds = 30
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

$composeRoot = Get-DockerLocalComposeRoot
$config = Get-DockerLocalComposeConfig `
    -AllagmaBaseUrl $AllagmaBaseUrl `
    -KanonBaseUrl $KanonBaseUrl `
    -ConexusBaseUrl $ConexusBaseUrl `
    -AllagmaServiceToken $AllagmaServiceToken `
    -KanonServiceToken $KanonServiceToken `
    -ConexusAdminApiKey $ConexusAdminApiKey
$secretPatterns = @(Get-DockerLocalSecretPatterns -ComposeConfig $config)

if ([string]::IsNullOrWhiteSpace($GuidedReportPath)) {
    $GuidedReportPath = Join-Path $composeRoot "artifacts\docker-guided-main-flow-report.json"
}
if ([string]::IsNullOrWhiteSpace($SeedReportPath)) {
    $SeedReportPath = Join-Path $composeRoot "artifacts\env-seed-001-report.json"
}
if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $composeRoot "artifacts\trace-contract-001-evidence-report.json"
}

$outputDir = Split-Path -Parent $OutputPath
if (-not (Test-Path -LiteralPath $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

function Invoke-AllagmaStartRunProbe {
    param(
        [string]$TraceId,
        [string]$CorrelationId
    )
    $body = @{
        ontologyVersionId = "gaming-core@0.1.0"
        actorId = "trace-contract-001-probe"
        actorType = "service"
        actorRoles = @("AgentExecutor", "RiskAnalyst")
        objective = "TRACE-CONTRACT-001 correlation probe for player 421."
        context = @{ playerId = "421" }
    } | ConvertTo-Json -Depth 8

    $headers = @{
        Authorization = "Bearer $($config.AllagmaServiceToken)"
        "X-Ontogony-Trace-Id" = $TraceId
        "X-Ontogony-Correlation-Id" = $CorrelationId
    }

    $response = Invoke-WebRequest `
        -Method Post `
        -Uri "$($config.AllagmaBaseUrl)/allagma/v0/runs" `
        -Headers $headers `
        -ContentType "application/json" `
        -Body $body `
        -UseBasicParsing `
        -TimeoutSec $HttpTimeoutSeconds

    $start = $response.Content | ConvertFrom-Json
    return [pscustomobject]@{
        Response = $response
        Start = $start
        ResponseTraceHeader = (Get-ResponseHeaderValue -Response $response -HeaderName "X-Ontogony-Trace-Id")
    }
}

function Invoke-AllagmaRunEvents {
    param([string]$RunId)
    $headers = @{ Authorization = "Bearer $($config.AllagmaServiceToken)" }
    return @(Invoke-RestMethod `
        -Uri "$($config.AllagmaBaseUrl)/allagma/v0/runs/$RunId/events" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds)
}

function Invoke-KanonDecisionRecord {
    param([string]$DecisionId)
    $headers = @{
        Authorization = "Bearer $($config.KanonServiceToken)"
        "X-Ontogony-Actor-Id" = "trace-contract-001"
        "X-Ontogony-Actor-Type" = "service"
        "X-Ontogony-Roles" = "ProvenanceReader"
    }
    return Invoke-RestMethod `
        -Uri "$($config.KanonBaseUrl)/ontology/v0/decision-records/$DecisionId" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds
}

function Invoke-KanonByTrace {
    param([string]$TraceId)
    $headers = @{
        Authorization = "Bearer $($config.KanonServiceToken)"
        "X-Ontogony-Actor-Id" = "trace-contract-001"
        "X-Ontogony-Actor-Type" = "service"
        "X-Ontogony-Roles" = "ProvenanceReader"
    }
    return Invoke-RestMethod `
        -Uri "$($config.KanonBaseUrl)/ontology/v0/decision-records/by-trace/$([uri]::EscapeDataString($TraceId))" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds
}

function Invoke-ConexusExecutionJournal {
    param([string]$ModelCallId)
    $requestId = $ModelCallId
    if (-not [string]::IsNullOrWhiteSpace($requestId) -and $requestId.StartsWith("chatcmpl-")) {
        $requestId = $requestId.Substring("chatcmpl-".Length)
    }
    $headers = @{ "X-Conexus-Admin-Key" = $config.ConexusAdminApiKey }
    return Invoke-RestMethod `
        -Uri "$($config.ConexusBaseUrl)/admin/v0/diagnostics/execution-runs/by-request-id/$([uri]::EscapeDataString($requestId))" `
        -Headers $headers `
        -TimeoutSec $HttpTimeoutSeconds
}

function Assert-ProbeCorrelationChain {
    param(
        [string]$TraceId,
        [string]$CorrelationId,
        [object]$ProbeStart,
        [string]$ResponseTraceHeader,
        [array]$Events,
        [object]$KanonDecision,
        [object]$KanonByTrace,
        [object]$ConexusJournal
    )

    if ($TraceId -eq $CorrelationId) {
        throw "TRACE-CONTRACT-001: traceId and correlationId must be distinct for probe."
    }
    if ([string]::IsNullOrWhiteSpace($ProbeStart.runId)) {
        throw "TRACE-CONTRACT-001: probe missing runId."
    }
    if ([string]::IsNullOrWhiteSpace($ProbeStart.planningDecisionId)) {
        throw "TRACE-CONTRACT-001: probe missing planningDecisionId."
    }
    if ([string]::IsNullOrWhiteSpace($ResponseTraceHeader)) {
        throw "TRACE-CONTRACT-001: response missing X-Ontogony-Trace-Id."
    }
    if ($ResponseTraceHeader -ne $TraceId) {
        throw "TRACE-CONTRACT-001: response trace header '$ResponseTraceHeader' != probe trace '$TraceId'."
    }

    $created = $Events | Where-Object { $_.eventType -eq "Allagma.RunCreated" } | Select-Object -First 1
    if ($null -eq $created) { throw "TRACE-CONTRACT-001: probe missing Allagma.RunCreated event." }
    $createdPayload = Get-RunEventPayloadObject -Payload $created.payload
    if ($null -ne $createdPayload) {
        $eventTrace = [string](Get-OptionalProperty -Object $createdPayload -Name "traceId")
        $eventCorr = [string](Get-OptionalProperty -Object $createdPayload -Name "correlationId")
        if (-not [string]::IsNullOrWhiteSpace($eventTrace) -and $eventTrace -ne $TraceId) {
            throw "TRACE-CONTRACT-001: RunCreated traceId '$eventTrace' != '$TraceId'."
        }
        if (-not [string]::IsNullOrWhiteSpace($eventCorr) -and $eventCorr -ne $CorrelationId) {
            throw "TRACE-CONTRACT-001: RunCreated correlationId '$eventCorr' != '$CorrelationId'."
        }
        if ($eventCorr -eq $TraceId) {
            throw "TRACE-CONTRACT-001: RunCreated correlationId must differ from traceId."
        }
    }

    if ($KanonDecision.traceId -and $KanonDecision.traceId -ne $TraceId) {
        throw "TRACE-CONTRACT-001: Kanon decision traceId '$($KanonDecision.traceId)' != '$TraceId'."
    }
    if ($KanonDecision.allagmaRunId -and $KanonDecision.allagmaRunId -ne $ProbeStart.runId) {
        throw "TRACE-CONTRACT-001: Kanon allagmaRunId '$($KanonDecision.allagmaRunId)' != '$($ProbeStart.runId)'."
    }
    if ([string]::IsNullOrWhiteSpace($KanonDecision.correlationId)) {
        throw "TRACE-CONTRACT-001: Kanon decision missing correlationId."
    }
    if ($KanonDecision.correlationId -ne $CorrelationId) {
        throw "TRACE-CONTRACT-001: Kanon correlationId '$($KanonDecision.correlationId)' != '$CorrelationId'."
    }
    if ($KanonDecision.correlationId -eq $TraceId) {
        throw "TRACE-CONTRACT-001: Kanon correlationId must differ from traceId."
    }

    $byTraceIds = @($KanonByTrace.decisions | ForEach-Object { $_.decisionId })
    if ($byTraceIds.Count -gt 0 -and $byTraceIds -notcontains $ProbeStart.planningDecisionId) {
        throw "TRACE-CONTRACT-001: Kanon by-trace missing planningDecisionId $($ProbeStart.planningDecisionId)."
    }

    if ([string]::IsNullOrWhiteSpace($ProbeStart.modelCallId)) {
        throw "TRACE-CONTRACT-001: probe missing modelCallId for Conexus journal lookup."
    }

    $meta = $ConexusJournal.metadata
    if ($null -eq $meta) { throw "TRACE-CONTRACT-001: Conexus journal metadata missing." }
    $journalRunId = if ($meta.allagma_run_id) { [string]$meta.allagma_run_id } else { $null }
    if ($journalRunId -ne $ProbeStart.runId) {
        throw "TRACE-CONTRACT-001: Conexus allagma_run_id '$journalRunId' != '$($ProbeStart.runId)'."
    }
    $journalCorr = if ($meta.correlation_id) { [string]$meta.correlation_id } else { $null }
    if ([string]::IsNullOrWhiteSpace($journalCorr)) {
        throw "TRACE-CONTRACT-001: Conexus journal missing correlation_id."
    }
    if ($journalCorr -ne $CorrelationId) {
        throw "TRACE-CONTRACT-001: Conexus correlation_id '$journalCorr' != '$CorrelationId'."
    }
    if ($journalCorr -eq $TraceId) {
        throw "TRACE-CONTRACT-001: Conexus correlation_id must differ from traceId."
    }
}

Write-Host ""
Write-Host "=== TRACE-CONTRACT-001 inspect trace/correlation evidence ==="
Write-Host "Boundary: Docker-local operator contract proof, not production readiness."
Write-Host "Env file: $($config.EnvFilePath)"
Write-Host ""

$traceId = "trace-contract-" + [guid]::NewGuid().ToString("N")
$correlationId = "corr-contract-" + [guid]::NewGuid().ToString("N")

$probeResult = Invoke-AllagmaStartRunProbe -TraceId $traceId -CorrelationId $correlationId
$probeStart = $probeResult.Start
$probeEvents = Invoke-AllagmaRunEvents -RunId $probeStart.runId
$kanonDecision = Invoke-KanonDecisionRecord -DecisionId $probeStart.planningDecisionId
$kanonByTrace = Invoke-KanonByTrace -TraceId $traceId
$conexusJournal = Invoke-ConexusExecutionJournal -ModelCallId $probeStart.modelCallId

Assert-ProbeCorrelationChain `
    -TraceId $traceId `
    -CorrelationId $correlationId `
    -ProbeStart $probeStart `
    -ResponseTraceHeader $probeResult.ResponseTraceHeader `
    -Events $probeEvents `
    -KanonDecision $kanonDecision `
    -KanonByTrace $kanonByTrace `
    -ConexusJournal $conexusJournal

$guidedReplay = [ordered]@{
    attempted = $false
    status = "skipped"
    detail = "SkipGuidedReplay or no guided/seed report."
}

if (-not $SkipGuidedReplay) {
    $reportIds = Read-DockerLocalRunReportIds -GuidedPath $GuidedReportPath -SeedPath $SeedReportPath
    if (-not [string]::IsNullOrWhiteSpace($reportIds.source) -and -not [string]::IsNullOrWhiteSpace($reportIds.subjectRunId)) {
        $guidedReplay.attempted = $true
        try {
            $subjectEvents = Invoke-AllagmaRunEvents -RunId $reportIds.subjectRunId
            $subjectCreated = $subjectEvents | Where-Object { $_.eventType -eq "Allagma.RunCreated" } | Select-Object -First 1
            $subjectPayload = Get-RunEventPayloadObject -Payload $subjectCreated.payload
            $subjectTraceId = [string](Get-OptionalProperty -Object $subjectPayload -Name "traceId")
            if ([string]::IsNullOrWhiteSpace($subjectTraceId)) {
                throw "subject RunCreated missing traceId."
            }
            $subjectByTrace = Invoke-KanonByTrace -TraceId $subjectTraceId
            $subjectIds = @($subjectByTrace.decisions | ForEach-Object { $_.decisionId })
            $guidedReplay = [ordered]@{
                attempted = $true
                status = "PASS"
                sourceReport = $reportIds.source
                subjectRunId = $reportIds.subjectRunId
                subjectTraceId = $subjectTraceId
                kanonDecisionCount = $subjectIds.Count
                detail = "Subject run trace is indexed on Kanon by-trace."
            }
        }
        catch {
            $guidedReplay = [ordered]@{
                attempted = $true
                status = "FAIL"
                sourceReport = $reportIds.source
                subjectRunId = $reportIds.subjectRunId
                detail = (Redact-StringWithPatterns -Value $_.Exception.Message -SecretPatterns $secretPatterns)
            }
            throw
        }
    }
}

$report = [ordered]@{
    schema = "trace-contract-001-evidence-report-v1"
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    verdict = "PASS"
    boundary = "first Dockerized local working system, not production readiness"
    probe = [ordered]@{
        traceId = $traceId
        correlationId = $correlationId
        runId = $probeStart.runId
        planningDecisionId = $probeStart.planningDecisionId
        modelCallId = $probeStart.modelCallId
        responseTraceHeader = $probeResult.ResponseTraceHeader
        runStatus = $probeStart.status
    }
    allagma = [ordered]@{
        runCreatedEventCount = @($probeEvents | Where-Object { $_.eventType -eq "Allagma.RunCreated" }).Count
        eventTypeSample = @($probeEvents | Select-Object -First 8 | ForEach-Object { $_.eventType })
    }
    kanon = [ordered]@{
        planningDecisionType = $kanonDecision.decisionType
        planningTraceId = $kanonDecision.traceId
        planningCorrelationId = $kanonDecision.correlationId
        planningAllagmaRunId = $kanonDecision.allagmaRunId
        byTraceDecisionCount = @($kanonByTrace.decisions).Count
        planningListedByTrace = ($true)
    }
    conexus = [ordered]@{
        journalRunId = $conexusJournal.runId
        metadataAllagmaRunId = $conexusJournal.metadata.allagma_run_id
        metadataCorrelationId = $conexusJournal.metadata.correlation_id
    }
    guidedReplay = $guidedReplay
    services = [ordered]@{
        allagmaBaseUrl = $config.AllagmaBaseUrl
        kanonBaseUrl = $config.KanonBaseUrl
        conexusBaseUrl = $config.ConexusBaseUrl
        envFilePath = (Redact-StringWithPatterns -Value $config.EnvFilePath -SecretPatterns $secretPatterns)
    }
    references = [ordered]@{
        traceStandard = "docs/03_TRACE_CORRELATION_STANDARD.md"
        systemMatrix = "allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md"
        kanonHeaders = "kanon-dotnet/docs/integrations/IDEMPOTENCY_AND_TRACE_HEADERS.md"
    }
    safety = [ordered]@{
        realProviderKeys = "no"
        realExternalExecution = "disabled"
        productionReadiness = "not_claimed"
        secretsRedacted = $true
        tokensLoadedFromEnvFile = $true
    }
}

$json = $report | ConvertTo-Json -Depth 14
Assert-ReportHasNoSecretPatterns -Json $json -SecretPatterns $secretPatterns

$json | Set-Content -LiteralPath $OutputPath -Encoding UTF8
Write-Host "Wrote trace correlation evidence report: $OutputPath"
Write-Host "TRACE-CONTRACT-001 inspect PASS."
