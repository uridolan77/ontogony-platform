<#
.SYNOPSIS
  REPLAY-RUNTIME-001E — platform wrapper for governed-fake replay smoke + runtime-lock validation.

.DESCRIPTION
  Runs Allagma run-governed-fake-replay-e2e.ps1, mirrors artifacts under
  ontogony-platform/docs/evidence/artifacts/governed-fake-replay-e2e/<timestamp>/,
  and optionally validates the runtime lock when governedFakeReplaySummary is set.

.EXAMPLE
  powershell -NoProfile -File scripts/smoke/run-runtime-lock-governed-fake-replay-e2e.ps1

.EXAMPLE
  powershell -NoProfile -File scripts/smoke/run-runtime-lock-governed-fake-replay-e2e.ps1 -ValidateRuntimeLock
#>
param(
    [string]$DevRoot = "",
    [switch]$ValidateRuntimeLock,
    [switch]$RequireConexusReplayAttempt,
    [string]$AllagmaBaseUrl = "http://localhost:5083"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$platformRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = Split-Path -Parent $platformRoot
}

$allagmaRoot = Join-Path $DevRoot "allagma-dotnet"
$replaySmoke = Join-Path $allagmaRoot "scripts/smoke/run-governed-fake-replay-e2e.ps1"
if (-not (Test-Path -LiteralPath $replaySmoke)) {
    throw "Missing $replaySmoke"
}

$timestamp = (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ")
$platformEvidenceDir = Join-Path $platformRoot "docs/evidence/artifacts/governed-fake-replay-e2e/$timestamp"
New-Item -ItemType Directory -Force -Path $platformEvidenceDir | Out-Null

Write-Host "REPLAY-RUNTIME-001E — platform replay proof"
Write-Host "  evidence: $platformEvidenceDir"

$replayArgs = @{
    OutputDirectory = (Join-Path $platformEvidenceDir "allagma-run")
}
if ($RequireConexusReplayAttempt) {
    $replayArgs.RequireConexusReplayAttempt = $true
}
if (-not [string]::IsNullOrWhiteSpace($AllagmaBaseUrl)) {
    $replayArgs.AllagmaBaseUrl = $AllagmaBaseUrl
}

$replayOutDir = & $replaySmoke @replayArgs
if ($LASTEXITCODE -ne 0) { throw "run-governed-fake-replay-e2e.ps1 failed with exit $LASTEXITCODE" }
$replayOutDir = [string]$replayOutDir.Trim()

foreach ($name in @(
    "governed-fake-replay-summary.json",
    "replay-request.json",
    "replay-result.json",
    "replay-evidence-bundle.json",
    "replay-delta.json",
    "replay-summary.json",
    "replay-summary.md",
    "governed-fake-e2e-result.json",
    "governed-fake-replay-e2e-output.log"
)) {
    $src = Join-Path $replayOutDir $name
    if (Test-Path -LiteralPath $src) {
        Copy-Item -LiteralPath $src -Destination (Join-Path $platformEvidenceDir $name) -Force
    }
}

$summaryPath = Join-Path $platformEvidenceDir "governed-fake-replay-summary.json"
$checkScript = Join-Path $allagmaRoot "scripts/check-governed-fake-replay-summary.ps1"
if (Test-Path -LiteralPath $checkScript) {
    & $checkScript -SummaryPath $summaryPath
}

if ($ValidateRuntimeLock) {
    $validate = Join-Path $allagmaRoot "scripts/validate-runtime-lock.ps1"
    if (-not (Test-Path -LiteralPath $validate)) {
        throw "Missing $validate"
    }
    & $validate -RequireGovernedFakeReplayEvidence
}

Write-Host "PASS: governed-fake-replay platform evidence at $platformEvidenceDir"
Write-Output $platformEvidenceDir
