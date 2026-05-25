<#
.SYNOPSIS
  ALLAGMA-MAF-INTEGRATION-DEPTH-001E / MAF-PROOF-LOCK-001 — platform wrapper for governed MAF workflow smoke.

.DESCRIPTION
  Runs Allagma run-governed-maf-e2e.ps1 (optional -Strict), mirrors artifacts under
  ontogony-platform/docs/evidence/artifacts/governed-maf-e2e/<timestamp>/,
  and optionally validates the runtime lock when governedMafE2eSummary is set.

.EXAMPLE
  powershell -NoProfile -File scripts/smoke/run-runtime-lock-governed-maf-e2e.ps1

.EXAMPLE
  powershell -NoProfile -File scripts/smoke/run-runtime-lock-governed-maf-e2e.ps1 -Strict -ValidateRuntimeLock
#>
param(
    [string]$DevRoot = "",
    [switch]$Strict,
    [switch]$ValidateRuntimeLock,
    [string]$AllagmaBaseUrl = "http://localhost:5083"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$platformRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = Split-Path -Parent $platformRoot
}

$allagmaRoot = Join-Path $DevRoot "allagma-dotnet"
$mafSmoke = Join-Path $allagmaRoot "scripts/smoke/run-governed-maf-e2e.ps1"
if (-not (Test-Path -LiteralPath $mafSmoke)) {
    throw "Missing $mafSmoke"
}

$timestamp = (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ")
$platformEvidenceDir = Join-Path $platformRoot "docs/evidence/artifacts/governed-maf-e2e/$timestamp"
New-Item -ItemType Directory -Force -Path $platformEvidenceDir | Out-Null

Write-Host "MAF-PROOF-LOCK-001 — governed MAF workflow proof"
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

$mafOutDir = & powershell @mafArgs
$mafOutDir = [string]$mafOutDir.Trim()
if (-not $mafOutDir -or -not (Test-Path -LiteralPath $mafOutDir)) {
    throw "run-governed-maf-e2e.ps1 did not return a valid output directory."
}

$summaryPath = Join-Path $platformEvidenceDir "governed-maf-e2e-summary.json"
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

Write-Host "PASS: governed-maf-e2e platform evidence at $platformEvidenceDir"
Write-Output $platformEvidenceDir
