<#
.SYNOPSIS
  RUNTIME-LOCK-CI-GOVERNED-E2E-001 — one command to prove the governed fake E2E path locally.

.DESCRIPTION
  Stage 2 local proof orchestrator:
    1. Optional system_truth_smoke.ps1 (WARNING allowed)
    2. Allagma run-governed-fake-e2e.ps1 (backend smoke + evidence graph)
    3. Optional Playwright governed-fake-e2e-docker-live (Docker-local frontend)

  Writes a platform mirror under artifacts/runtime-lock-governed-fake-e2e/<timestamp>/ with:
    governed-fake-e2e-summary.json
    governed-fake-e2e-output.log
    evidence-spine-bundle.json (copy)
    playwright-report/ (when Playwright runs)

.EXAMPLE
  pwsh -File scripts/smoke/run-runtime-lock-governed-fake-e2e.ps1

.EXAMPLE
  pwsh -File scripts/smoke/run-runtime-lock-governed-fake-e2e.ps1 -SkipPlaywright -SkipSystemTruth
#>
param(
    [string]$DevRoot = "",
    [switch]$SkipSystemTruth,
    [switch]$SkipPlaywright,
    [switch]$WaitDockerStack,
    [string]$OutputDirectory = "artifacts/runtime-lock-governed-fake-e2e"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$platformRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = Split-Path -Parent $platformRoot
}

$allagmaRoot = Join-Path $DevRoot "allagma-dotnet"
$frontendRoot = Join-Path $DevRoot "ontogony-frontend"
$smokeScript = Join-Path $allagmaRoot "scripts/smoke/run-governed-fake-e2e.ps1"
$truthScript = Join-Path $platformRoot "scripts/smoke/system_truth_smoke.ps1"
$composeWait = Join-Path $platformRoot "docker/local-working-system/scripts/wait-local-working-system.ps1"

foreach ($path in @($allagmaRoot, $smokeScript)) {
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Missing required path: $path"
    }
}

$timestamp = (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ")
$platformOut = Join-Path $platformRoot (Join-Path $OutputDirectory $timestamp)
New-Item -ItemType Directory -Force -Path $platformOut | Out-Null

$logPath = Join-Path $platformOut "governed-fake-e2e-output.log"
Start-Transcript -Path $logPath -Force | Out-Null

$systemTruthStatus = "skipped"
$playwrightStatus = "skipped"
$playwrightReportRelative = ""

try {
    Write-Host "RUNTIME-LOCK-CI-GOVERNED-E2E-001 — output: $platformOut"
    Write-Host "DevRoot: $DevRoot"

    if ($WaitDockerStack -and (Test-Path -LiteralPath $composeWait)) {
        Write-Host "Waiting for Docker local working system ..."
        & $composeWait
    }

    if (-not $SkipSystemTruth -and (Test-Path -LiteralPath $truthScript)) {
        Write-Host ""
        Write-Host "=== system_truth_smoke ==="
        try {
            & $truthScript
            $systemTruthStatus = if ($LASTEXITCODE -eq 0) { "PASS" } else { "WARNING" }
        }
        catch {
            $systemTruthStatus = "WARNING"
            Write-Warning "system_truth_smoke failed or warned: $($_.Exception.Message)"
        }
    }

    Write-Host ""
    Write-Host "=== run-governed-fake-e2e (Allagma) ==="
    $allagmaOutDir = & $smokeScript
    if ($LASTEXITCODE -ne 0) { throw "run-governed-fake-e2e.ps1 failed with exit $LASTEXITCODE" }
    $allagmaOutDir = [string]$allagmaOutDir.Trim()
    if (-not (Test-Path -LiteralPath $allagmaOutDir)) {
        throw "Allagma smoke did not return a valid output directory: '$allagmaOutDir'"
    }

    $summarySrc = Join-Path $allagmaOutDir "governed-fake-e2e-summary.json"
    if (-not (Test-Path -LiteralPath $summarySrc)) {
        throw "Missing $summarySrc — ensure run-governed-fake-e2e.ps1 writes canonical summary."
    }

    Copy-Item -LiteralPath $summarySrc -Destination (Join-Path $platformOut "governed-fake-e2e-summary.json") -Force
    foreach ($name in @("governed-fake-e2e-result.json", "evidence-spine-bundle.json", "evidence-graph.json")) {
        $src = Join-Path $allagmaOutDir $name
        if (Test-Path -LiteralPath $src) {
            Copy-Item -LiteralPath $src -Destination (Join-Path $platformOut $name) -Force
        }
    }
    $allagmaLog = Join-Path $allagmaOutDir "governed-fake-e2e-output.log"
    if (Test-Path -LiteralPath $allagmaLog) {
        Add-Content -LiteralPath $logPath -Value "`n--- allagma smoke log ($allagmaLog) ---`n"
        Get-Content -LiteralPath $allagmaLog | Add-Content -LiteralPath $logPath
    }

    if (-not $SkipPlaywright) {
        if (-not (Test-Path -LiteralPath $frontendRoot)) {
            Write-Warning "Skipping Playwright: ontogony-frontend not found at $frontendRoot"
            $playwrightStatus = "skipped"
        }
        else {
            Write-Host ""
            Write-Host "=== Playwright governed-fake-e2e-docker-live ==="
            Push-Location $frontendRoot
            try {
                npx playwright test -c playwright.docker-local.config.ts governed-fake-e2e-docker-live
                if ($LASTEXITCODE -ne 0) { throw "Playwright governed-fake-e2e-docker-live failed with exit $LASTEXITCODE" }
                $playwrightStatus = "PASS"
                $reportSrc = Join-Path $frontendRoot "playwright-report"
                if (Test-Path -LiteralPath $reportSrc) {
                    $reportDest = Join-Path $platformOut "playwright-report"
                    if (Test-Path -LiteralPath $reportDest) {
                        Remove-Item -LiteralPath $reportDest -Recurse -Force
                    }
                    Copy-Item -LiteralPath $reportSrc -Destination $reportDest -Recurse -Force
                    $playwrightReportRelative = "playwright-report"
                }
            }
            finally {
                Pop-Location
            }
        }
    }

    $summary = Get-Content -LiteralPath (Join-Path $platformOut "governed-fake-e2e-summary.json") -Raw | ConvertFrom-Json
    $summary.systemTruth = [ordered]@{ status = $systemTruthStatus }
    $summary.playwright = [ordered]@{
        status         = $playwrightStatus
        reportRelative = $playwrightReportRelative
    }
    $summary.runtimeLock = [ordered]@{
        stage = "local_proof"
        note  = "Produced by ontogony-platform/scripts/smoke/run-runtime-lock-governed-fake-e2e.ps1"
    }
    $summary.generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    $summary | ConvertTo-Json -Depth 20 | Set-Content -Path (Join-Path $platformOut "governed-fake-e2e-summary.json") -Encoding utf8

    $checkScript = Join-Path $allagmaRoot "scripts/check-governed-fake-e2e-summary.ps1"
    if (Test-Path -LiteralPath $checkScript) {
        & $checkScript -SummaryPath (Join-Path $platformOut "governed-fake-e2e-summary.json")
    }

    Write-Host ""
    Write-Host "PASS: RUNTIME-LOCK-CI-GOVERNED-E2E-001 local proof"
    Write-Host "  platform artifacts: $platformOut"
    Write-Host "  allagma artifacts:  $allagmaOutDir"
    Write-Output $platformOut
}
finally {
    Stop-Transcript | Out-Null
}
