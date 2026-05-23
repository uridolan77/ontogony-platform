# SYSTEM-DEMO-FLOWS-001 — extract demo ids from ENV-SEED-001 report for operator walkthroughs.
param(
    [string]$SeedReportPath = (Join-Path $PSScriptRoot "..\artifacts\env-seed-001-report.json"),
    [string]$OutputPath = (Join-Path $PSScriptRoot "..\artifacts\operator-v1-demo-ids.json")
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $SeedReportPath)) {
    Write-Error "Seed report not found: $SeedReportPath. Run seed-and-verify-local-working-system.ps1 first."
}

$seed = Get-Content -LiteralPath $SeedReportPath -Raw | ConvertFrom-Json

$demo = [ordered]@{
    schema          = "ontogony-operator-v1-demo-ids-v1"
    generatedAtUtc  = (Get-Date).ToUniversalTime().ToString("o")
    sourceReport    = (Resolve-Path -LiteralPath $SeedReportPath).Path
    boundary        = "Docker-local operator demo; not production readiness"
    frontendBaseUrl = "http://localhost:5175"
    services        = $seed.services
    flows           = [ordered]@{
        "system-posture"      = [ordered]@{ route = "/system" }
        "simple-governed-run" = [ordered]@{
            route  = "/allagma/runs/start"
            runId  = $seed.runs.subjectRunId
            detail = "/allagma/runs/$($seed.runs.subjectRunId)"
        }
        "human-gate-approve" = [ordered]@{ route = "/allagma/gates" }
        "human-gate-deny"    = [ordered]@{ route = "/allagma/gates" }
        "conexus-fallback"   = [ordered]@{
            route            = "/conexus/observability"
            routeDecisionId  = $seed.routeEvidence.subjectRouteDecisionId
            modelCallId      = $seed.runs.subjectModelCallId
            observabilityUrl = "/conexus/observability?modelCallId=$([uri]::EscapeDataString($seed.runs.subjectModelCallId))"
        }
        "kanon-assistance" = [ordered]@{
            route      = "/kanon/assistance"
            decisionId = $seed.topology.subjectTopologyAuthorizationDecisionId
        }
        "sandbox-posture" = [ordered]@{
            route  = "/allagma/runs/$($seed.runs.baselineRunId)"
            runId  = $seed.runs.baselineRunId
        }
        "evidence-spine-trace" = [ordered]@{
            route           = "/system/evidence-spine"
            lookupRunId     = $seed.runs.baselineRunId
            modelCallId     = $seed.runs.baselineModelCallId
            routeDecisionId = $seed.routeEvidence.baselineRouteDecisionId
            decisionId      = $seed.topology.subjectTopologyAuthorizationDecisionId
        }
    }
}

$dir = Split-Path -Parent $OutputPath
if (-not (Test-Path -LiteralPath $dir)) {
    New-Item -ItemType Directory -Path $dir -Force | Out-Null
}

$demo | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $OutputPath -Encoding UTF8
Write-Host "Wrote operator demo ids: $OutputPath"
