#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Wave 7 - validate the canonical local Ontogony operator system.

.DESCRIPTION
  Proves health, governed runtime evidence, frontend reachability, operator settings
  routes, evidence spine backing data, and trace/correlation visibility.

  Boundary: Docker-local development credentials only - not production readiness.

.EXAMPLE
  ./scripts/validate-local-ontogony-system.ps1

.EXAMPLE
  ./scripts/validate-local-ontogony-system.ps1 -StartServices -Build -IncludeEvidenceSpineLive
#>
param(
    [string]$DevRoot = "",
    [switch]$StartServices,
    [switch]$Build,
    [switch]$SkipFrontend,
    [switch]$SkipGuidedFlow,
    [switch]$IncludeEvidenceSpineLive,
    [string]$OutputPath = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. (Join-Path $PSScriptRoot "lib/local-ontogony-system.ps1")

$paths = Get-LocalOntogonyPaths -DevRoot $DevRoot
$DevRoot = $paths.DevRoot

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = $paths.ValidationReportPath
}

$outputDir = Split-Path -Parent $OutputPath
if (-not (Test-Path -LiteralPath $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

$gateResults = [System.Collections.Generic.List[object]]::new()
$failed = $false

function Add-GateResult {
    param(
        [string]$Name,
        [string]$Verdict,
        [string]$Detail = "",
        [string]$ArtifactPath = ""
    )

    $script:gateResults.Add([ordered]@{
        name = $Name
        verdict = $Verdict
        detail = $Detail
        artifactPath = $ArtifactPath
    }) | Out-Null

    if ($Verdict -eq "fail") {
        $script:failed = $true
    }
}

function Invoke-Gate {
    param(
        [string]$Name,
        [scriptblock]$Body,
        [string]$ArtifactPath = ""
    )

    Write-Host ""
    Write-Host "=== $Name ===" -ForegroundColor Cyan
    try {
        & $Body
        Add-GateResult -Name $Name -Verdict "pass" -ArtifactPath $ArtifactPath
    }
    catch {
        Add-GateResult -Name $Name -Verdict "fail" -Detail $_.Exception.Message -ArtifactPath $ArtifactPath
        Write-Host ("FAIL {0}: {1}" -f $Name, $_.Exception.Message) -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "=== Wave 7 - validate local Ontogony system ===" -ForegroundColor Cyan
Write-Host "Boundary: Docker-local operator system; not production readiness."
Write-Host "  DevRoot:  $DevRoot"
Write-Host "  Platform: $($paths.RepoRoot)"
Write-Host ""

Invoke-Gate -Name "six-repo-workspace" -Body {
    Test-SixRepoWorkspaceLayout -DevRoot $DevRoot -Quiet
}

if ($StartServices) {
    Invoke-Gate -Name "start-local-system" -Body {
        $startArgs = @{ Quick = $true }
        if ($Build) { $startArgs["Build"] = $true }
        if ($SkipFrontend) { $startArgs["SkipFrontend"] = $true }
        Invoke-LocalOntogonyScript `
            -ScriptPath (Join-Path $PSScriptRoot "start-local-ontogony-system.ps1") `
            -Arguments $startArgs
    }
}

Invoke-Gate -Name "runtime-health" -Body {
    $waitArgs = @{}
    if ($SkipFrontend) { $waitArgs["SkipFrontend"] = $true }
    Invoke-LocalOntogonyScript `
        -ScriptPath (Join-Path $paths.DockerScripts "wait-local-working-system.ps1") `
        -Arguments $waitArgs
}

if (-not $SkipGuidedFlow) {
    Invoke-Gate -Name "governed-demo-flow" -ArtifactPath $paths.GuidedReportPath -Body {
        $guidedArgs = @{}
        if ($SkipFrontend) { $guidedArgs["SkipFrontend"] = $true }
        Invoke-LocalOntogonyScript `
            -ScriptPath (Join-Path $paths.DockerScripts "run-docker-guided-main-flow.ps1") `
            -Arguments $guidedArgs
    }
}
else {
    Invoke-Gate -Name "governed-demo-flow-report" -ArtifactPath $paths.GuidedReportPath -Body {
        Invoke-LocalOntogonyScript `
            -ScriptPath (Join-Path $paths.DockerScripts "validate-docker-guided-main-flow.ps1") `
            -Arguments @{ ReportPath = $paths.GuidedReportPath }
    }
}

Invoke-Gate -Name "allagma-run-audit-evidence" -ArtifactPath $paths.GuidedReportPath -Body {
    $guided = Get-Content -Raw -LiteralPath $paths.GuidedReportPath | ConvertFrom-Json
    foreach ($field in @(
        "baselineRunId",
        "subjectRunId",
        "baselineEvaluationRunId",
        "subjectEvaluationRunId",
        "baselineComparisonId"
    )) {
        if ([string]::IsNullOrWhiteSpace([string]$guided.$field)) {
            throw "Guided report missing $field"
        }
    }
}

Invoke-Gate -Name "kanon-decision-provenance" -ArtifactPath $paths.GuidedReportPath -Body {
    Invoke-LocalOntogonyScript `
        -ScriptPath (Join-Path $paths.DockerScripts "inspect-kanon-topology-evidence.ps1")
    Invoke-LocalOntogonyScript `
        -ScriptPath (Join-Path $paths.DockerScripts "validate-kanon-topology-evidence-report.ps1")
}

Invoke-Gate -Name "conexus-model-call-evidence" -ArtifactPath $paths.GuidedReportPath -Body {
    $guided = Get-Content -Raw -LiteralPath $paths.GuidedReportPath | ConvertFrom-Json
    foreach ($field in @("baselineRouteDecisionId", "subjectRouteDecisionId")) {
        if ([string]::IsNullOrWhiteSpace([string]$guided.$field)) {
            throw "Guided report missing $field"
        }
    }
}

Invoke-Gate -Name "trace-correlation-visibility" -ArtifactPath (Join-Path $paths.ArtifactsDir "trace-contract-001-evidence-report.json") -Body {
    Invoke-LocalOntogonyScript `
        -ScriptPath (Join-Path $paths.DockerScripts "inspect-trace-correlation-evidence.ps1")
    Invoke-LocalOntogonyScript `
        -ScriptPath (Join-Path $paths.DockerScripts "validate-trace-correlation-evidence-report.ps1")
}

Invoke-Gate -Name "operator-demo-ids" -ArtifactPath $paths.DemoIdsPath -Body {
    if (-not (Test-Path -LiteralPath $paths.DemoIdsPath)) {
        Invoke-LocalOntogonyScript `
            -ScriptPath (Join-Path $paths.DockerScripts "run-operator-v1-demo-prep.ps1") `
            -Arguments @{ SkipSeed = $true }
    }

    $demo = Get-Content -Raw -LiteralPath $paths.DemoIdsPath | ConvertFrom-Json
    if ($demo.schema -ne "ontogony-operator-v1-demo-ids-v1") {
        throw "Unexpected demo ids schema: $($demo.schema)"
    }
    if ([string]::IsNullOrWhiteSpace([string]$demo.flows.'simple-governed-run'.runId)) {
        throw "Demo ids missing simple-governed-run.runId"
    }
}

if (-not $SkipFrontend) {
    $frontendBaseUrl = Get-LocalOntogonyFrontendBaseUrl -Paths $paths
    $secretPatterns = @()

    $envHelpers = Join-Path $paths.DockerScripts "_docker-local-env.ps1"
    if (Test-Path -LiteralPath $envHelpers) {
        . $envHelpers
        $config = Get-DockerLocalComposeConfig -FrontendBaseUrl $frontendBaseUrl
        $secretPatterns = @(Get-DockerLocalSecretPatterns -ComposeConfig $config)
    }

    Invoke-Gate -Name "operator-settings-route" -Body {
        $settings = Test-OperatorFrontendRoute `
            -Url "$frontendBaseUrl/settings" `
            -SecretPatterns $secretPatterns
        if (-not $settings.passed) {
            throw "Settings route failed at $($settings.url)"
        }
    }

    Invoke-Gate -Name "frontend-service-reachability" -ArtifactPath (Join-Path $paths.ArtifactsDir "fe-harden-001-frontend-evidence-report.json") -Body {
        $inspectScript = Join-Path $paths.FrontendRoot "scripts\docker\inspect-docker-local-operator-frontend.ps1"
        $validateScript = Join-Path $paths.FrontendRoot "scripts\docker\validate-docker-local-operator-frontend-report.ps1"
        Invoke-LocalOntogonyScript `
            -ScriptPath $inspectScript `
            -Arguments @{
                FrontendBaseUrl = $frontendBaseUrl
                GuidedReportPath = $paths.GuidedReportPath
            }
        Invoke-LocalOntogonyScript `
            -ScriptPath $validateScript
    }

    if ($IncludeEvidenceSpineLive) {
        Invoke-Gate -Name "evidence-spine-live" -ArtifactPath (Join-Path $paths.ArtifactsDir "evidence-spine-008a-docker-live-report.json") -Body {
            Invoke-LocalOntogonyScript `
                -ScriptPath (Join-Path $paths.DockerScripts "run-evidence-spine-008a-docker-live-verification.ps1") `
                -Arguments @{
                    Seed = $true
                    FrontendBaseUrl = $frontendBaseUrl
                }
        }
    }
    else {
        Invoke-Gate -Name "evidence-spine-backing-data" -ArtifactPath (Join-Path $paths.ArtifactsDir "evidence-spine-008a-live-api-preflight.json") -Body {
            $seed = Get-Content -Raw -LiteralPath $paths.SeedReportPath | ConvertFrom-Json
            if ($seed.verdict -ne "PASS") {
                throw "Seed report verdict must be PASS."
            }

            . $envHelpers
            $config = Get-DockerLocalComposeConfig -FrontendBaseUrl $frontendBaseUrl
            $allagmaHeaders = @{ Authorization = "Bearer $($config.AllagmaServiceToken)" }
            $kanonHeaders = @{ Authorization = "Bearer $($config.KanonServiceToken)" }
            $conexusAdminHeaders = @{ "X-Conexus-Admin-Key" = $config.ConexusAdminApiKey }

            $rows = @(
                @{ kind = "allagmaRun"; id = $seed.runs.subjectRunId; url = "$($config.AllagmaBaseUrl)/allagma/v0/runs/$($seed.runs.subjectRunId)"; headers = $allagmaHeaders }
                @{ kind = "kanonDecision"; id = $seed.topology.subjectTopologyAuthorizationDecisionId; url = "$($config.KanonBaseUrl)/ontology/v0/decision-records/$($seed.topology.subjectTopologyAuthorizationDecisionId)"; headers = $kanonHeaders }
                @{ kind = "conexusRouteDecision"; id = $seed.routeEvidence.subjectRouteDecisionId; url = "$($config.ConexusBaseUrl)/admin/v0/route-decisions/$($seed.routeEvidence.subjectRouteDecisionId)"; headers = $conexusAdminHeaders }
            )

            foreach ($row in $rows) {
                if ([string]::IsNullOrWhiteSpace([string]$row.id)) {
                    throw "Missing $($row.kind) id in seed report."
                }
                $response = Invoke-WebRequest -Uri $row.url -Headers $row.headers -UseBasicParsing
                if ($response.StatusCode -lt 200 -or $response.StatusCode -ge 300) {
                    throw "$($row.kind) lookup failed with HTTP $($response.StatusCode)"
                }
            }

            $workbench = Test-OperatorFrontendRoute -Url "$frontendBaseUrl/system/evidence-spine" -SecretPatterns $secretPatterns
            if (-not $workbench.passed) {
                throw "Evidence spine workbench route failed."
            }

            $preflightPath = Join-Path $paths.ArtifactsDir "evidence-spine-008a-live-api-preflight.json"
            $preflight = [ordered]@{
                schema = "evidence-spine-008a-live-api-preflight-v1"
                recordedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
                seedReportPath = $paths.SeedReportPath
                frontendBaseUrl = $frontendBaseUrl
                verdict = "PASS"
                kinds = @($rows | ForEach-Object { $_.kind })
            }
            $preflight | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $preflightPath -Encoding UTF8
        }
    }
}

$verdict = if ($failed) { "FAIL" } else { "PASS" }
$report = [ordered]@{
    schema = "local-ontogony-system-validation-report-v1"
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    verdict = $verdict
    boundary = "Docker-local operator system; not production readiness"
    devRoot = $DevRoot
    platformRoot = $paths.RepoRoot
    guidedReportPath = $paths.GuidedReportPath
    demoIdsPath = $paths.DemoIdsPath
    includeEvidenceSpineLive = [bool]$IncludeEvidenceSpineLive
    gates = $gateResults
    safety = [ordered]@{
        realProviderKeys = "no"
        realExternalExecution = "disabled"
        productionReadiness = "not_claimed"
    }
}

$report | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $OutputPath -Encoding UTF8

Write-Host ""
if ($failed) {
    Write-Host "Local Ontogony system validation FAIL." -ForegroundColor Red
    Write-Host "Report: $OutputPath"
    exit 1
}

Write-Host "Local Ontogony system validation PASS." -ForegroundColor Green
Write-Host "Report: $OutputPath"
Write-Host "Guide:  docs/operators/OPERATOR_V1_DEMO_GUIDE.md"
