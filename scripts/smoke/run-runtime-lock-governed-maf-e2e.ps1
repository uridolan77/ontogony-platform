<#
.SYNOPSIS
  ALLAGMA-MAF-INTEGRATION-DEPTH-001E / MAF-LIVE-GOVERNED-E2E-001 - platform wrapper for governed MAF workflow smoke + Playwright.

.DESCRIPTION
  Runs Allagma run-governed-maf-e2e.ps1 (optional -Strict), mirrors artifacts under
  ontogony-platform/docs/evidence/artifacts/governed-maf-e2e/<timestamp>/,
  optionally runs Playwright governed-maf-e2e-docker-live,
  and optionally validates the runtime lock when governedMafE2eSummary is set.

.EXAMPLE
  powershell -NoProfile -File scripts/smoke/run-runtime-lock-governed-maf-e2e.ps1

.EXAMPLE
  powershell -NoProfile -File scripts/smoke/run-runtime-lock-governed-maf-e2e.ps1 -Strict -ValidateRuntimeLock

.EXAMPLE
  powershell -NoProfile -File scripts/smoke/run-runtime-lock-governed-maf-e2e.ps1 -SkipPlaywright
#>
param(
    [string]$DevRoot = "",
    [switch]$Strict,
    [switch]$ValidateRuntimeLock,
    [switch]$SkipPlaywright,
    [string]$AllagmaBaseUrl = "http://localhost:5083"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$platformRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = Split-Path -Parent $platformRoot
}

$allagmaRoot = Join-Path $DevRoot "allagma-dotnet"
$frontendRoot = Join-Path $DevRoot "ontogony-frontend"
$mafSmoke = Join-Path $allagmaRoot "scripts/smoke/run-governed-maf-e2e.ps1"
if (-not (Test-Path -LiteralPath $mafSmoke)) {
    throw "Missing $mafSmoke"
}

$timestamp = (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ")
$platformEvidenceDir = Join-Path $platformRoot "docs/evidence/artifacts/governed-maf-e2e/$timestamp"
New-Item -ItemType Directory -Force -Path $platformEvidenceDir | Out-Null

$logPath = Join-Path $platformEvidenceDir "governed-maf-e2e-platform.log"
$playwrightStatus = "not_run"
$playwrightReportRelative = ""

Write-Host "MAF-LIVE-GOVERNED-E2E-001 - governed MAF workflow proof"
Write-Host "  evidence: $platformEvidenceDir"
if ($Strict) { Write-Host "  strict: replay_from_checkpoint enabled" }

$mafArgs = @(
    "-NoProfile",
    "-File", $mafSmoke,
    "-NoTimestampSubdirectory",
    "-OutputDirectory", $platformEvidenceDir,
    "-AllagmaBaseUrl", $AllagmaBaseUrl
)
if ($Strict) { $mafArgs += "-Strict" }

$mafOutRaw = & powershell @mafArgs 2>&1
if ($LASTEXITCODE -ne 0) {
    throw "run-governed-maf-e2e.ps1 failed with exit $LASTEXITCODE"
}
$mafOutDir = @($mafOutRaw | Where-Object { $_ -is [string] -and $_.Trim() })[-1]
$mafOutDir = [string]$mafOutDir.Trim()
if (-not $mafOutDir -or -not (Test-Path -LiteralPath $mafOutDir)) {
    throw "run-governed-maf-e2e.ps1 did not return a valid output directory (got '$mafOutDir')."
}

$summaryPath = Join-Path $platformEvidenceDir "governed-maf-e2e-summary.json"
if (-not (Test-Path -LiteralPath $summaryPath)) {
    throw "Missing governed-maf-e2e-summary.json at $summaryPath"
}

if (-not $SkipPlaywright) {
    if (-not (Test-Path -LiteralPath $frontendRoot)) {
        Write-Warning "Skipping Playwright: ontogony-frontend not found at $frontendRoot"
        $playwrightStatus = "skipped"
    }
    else {
        Write-Host ""
        Write-Host "=== Playwright governed-maf-e2e-docker-live ==="
        Push-Location $frontendRoot
        try {
            npx playwright test -c playwright.docker-local.config.ts governed-maf-e2e-docker-live
            if ($LASTEXITCODE -ne 0) { throw "Playwright governed-maf-e2e-docker-live failed with exit $LASTEXITCODE" }
            $playwrightStatus = "PASS"
            $reportSrc = Join-Path $frontendRoot "playwright-report"
            if (Test-Path -LiteralPath $reportSrc) {
                $reportDest = Join-Path $platformEvidenceDir "playwright-report"
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

$summary = Get-Content -LiteralPath $summaryPath -Raw | ConvertFrom-Json
$summary.playwright = [ordered]@{
    status         = $playwrightStatus
    reportRelative = $playwrightReportRelative
}
$summary.runtimeLock = [ordered]@{
    stage = "local_proof"
    note  = "Produced by ontogony-platform/scripts/smoke/run-runtime-lock-governed-maf-e2e.ps1 (MAF-LIVE-GOVERNED-E2E-001)"
}
$summary.generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
$summary | ConvertTo-Json -Depth 20 | Set-Content -Path $summaryPath -Encoding utf8

$checkScript = Join-Path $allagmaRoot "scripts/check-governed-maf-e2e-summary.ps1"
if (Test-Path -LiteralPath $checkScript) {
    & $checkScript -SummaryPath $summaryPath
}

if ($ValidateRuntimeLock) {
    $validate = Join-Path $allagmaRoot "scripts/validate-runtime-lock.ps1"
    if (-not (Test-Path -LiteralPath $validate)) {
        throw "Missing $validate"
    }
    & $validate -RequireGovernedMafE2eEvidence
}

Write-Host ('PASS: governed-maf-e2e platform evidence at ' + $platformEvidenceDir)
Write-Output $platformEvidenceDir
