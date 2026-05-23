#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Wave 6 - promotes a six-repo system release candidate and writes evidence.

.DESCRIPTION
  Runs platform consumer conformance, six-repo compatibility, UI/frontend RC gates,
  and optional runtime cohesion smoke. With -Promote, refreshes the six-repo lock,
  writes docs/system/ontogony-system-rc-002.lock.json, and copies evidence to
  docs/evidence/ONTOGONY_SYSTEM_RC_002/.

  Without -Promote, runs all gates and writes the evidence bundle using the current
  six-repo lock (dry run). Add -FullWorkspace once sibling docs are green for live
  consumer proofs against DevRoot.

.EXAMPLE
  ./scripts/promote-system-rc.ps1 -DevRoot C:\dev -RcId ONTOGONY-SYSTEM-RC-002

.EXAMPLE
  ./scripts/promote-system-rc.ps1 -DevRoot C:\dev -RcId ONTOGONY-SYSTEM-RC-002 -Promote -FullWorkspace
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [string] $RcId = "ONTOGONY-SYSTEM-RC-002",
    [switch] $Promote,
    [switch] $FullWorkspace,
    [switch] $SkipRuntimeCohesion,
    [switch] $SkipUiRc,
    [switch] $SkipFrontendRc
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = (Resolve-Path (Join-Path $RepoRoot "..")).Path
}

. (Join-Path $PSScriptRoot "lib/system-rc-promotion.ps1")

$uiRoot = Join-Path $DevRoot "ontogony-ui"
$frontendRoot = Join-Path $DevRoot "ontogony-frontend"
$allagmaRoot = Join-Path $DevRoot "allagma-dotnet"
$evidenceFolderName = ConvertTo-EvidenceFolderName $RcId
$evidenceFolderRel = "docs/evidence/$evidenceFolderName"
$evidenceDir = Join-Path $RepoRoot $evidenceFolderRel
$lockFileName = ConvertTo-RcLockFileName $RcId
$systemRcLockPath = Join-Path $RepoRoot "docs/system/$lockFileName"
$sixRepoLockPath = Join-Path $RepoRoot "docs/system/ontogony-six-repo-lock.json"
$postLockDeltasPath = Join-Path $RepoRoot "docs/system/ontogony-six-repo-post-lock-deltas.json"

$gateResults = [System.Collections.Generic.List[object]]::new()
$failed = $false

function Add-GateResult {
    param(
        [string]$Name,
        [string]$Verdict,
        [string]$Detail = ""
    )
    $script:gateResults.Add([ordered]@{
        name = $Name
        verdict = $Verdict
        detail = $Detail
    }) | Out-Null
    if ($Verdict -eq "fail") {
        $script:failed = $true
    }
}

Write-Host "Ontogony system RC promotion - $RcId"
Write-Host "  DevRoot:   $DevRoot"
Write-Host "  Platform:  $RepoRoot"
Write-Host "  Promote:       $($Promote.IsPresent)"
Write-Host "  FullWorkspace: $($FullWorkspace.IsPresent)"
Write-Host ""

# ── Phase 1: UI / frontend RC gates (produce tarball + provenance) ────────────

if (-not $SkipUiRc) {
    Write-Host "=== UI rc:check ==="
    try {
        Invoke-NpmScript $uiRoot "rc:check"
        $uiRc = Read-UiRcArtifacts $uiRoot
        Add-GateResult "ui-rc-check" $uiRc.verdict
    }
    catch {
        Add-GateResult "ui-rc-check" "fail" $_.Exception.Message
        throw
    }
}
else {
    $uiRc = Read-UiRcArtifacts $uiRoot
    Add-GateResult "ui-rc-check" "skipped" "SkipUiRc"
}

if (-not $SkipFrontendRc) {
    Write-Host "`n=== Frontend rc:check ==="
    try {
        $agentPkgDir = Join-Path $RepoRoot "packages/ontogony-agent-interaction"
        $agentDist = Join-Path $agentPkgDir "dist/index.js"
        if (-not (Test-Path -LiteralPath $agentDist)) {
            Write-Host "  Building @ontogony/agent-interaction (dist missing) ..."
            Push-Location $agentPkgDir
            try {
                if (-not (Test-Path -LiteralPath (Join-Path $agentPkgDir "node_modules"))) {
                    npm ci
                    if ($LASTEXITCODE -ne 0) { throw "agent-interaction npm ci failed" }
                }
                npm run build
                if ($LASTEXITCODE -ne 0) { throw "agent-interaction build failed" }
            }
            finally {
                Pop-Location
            }
        }
        Invoke-NpmScript $frontendRoot "rc:check"
        Push-Location $frontendRoot
        try {
            node ./scripts/sync-frontend-release-lock.mjs
            if ($LASTEXITCODE -ne 0) { throw "sync-frontend-release-lock failed" }
        }
        finally {
            Pop-Location
        }
        $frontendRc = Read-FrontendRcArtifacts $frontendRoot
        Add-GateResult "frontend-rc-check" $frontendRc.verdict
    }
    catch {
        Add-GateResult "frontend-rc-check" "fail" $_.Exception.Message
        throw
    }
}
else {
    $frontendRc = Read-FrontendRcArtifacts $frontendRoot
    Add-GateResult "frontend-rc-check" "skipped" "SkipFrontendRc"
}

# ── Phase 2: promote locks (optional) ─────────────────────────────────────────

if ($Promote) {
    Write-Host "`n=== Promote six-repo + system RC locks ==="
    $sixRepoLockDoc = Build-SixRepoLockDocument -DevRoot $DevRoot -RepoRoot $RepoRoot -UiRc $uiRc -FrontendRc $frontendRc
    Write-JsonFile $sixRepoLockPath $sixRepoLockDoc

    $runtimeBaseline = [string](Get-JsonFromFile (Join-Path $allagmaRoot "docs/system/ontogony-runtime.lock.json")).baseline
    $resetDeltas = [ordered]@{
        schema = "ontogony-six-repo-post-lock-deltas-v1"
        baseline = $runtimeBaseline
        description = "Classifies commits added to any of the six repos after the six-repo lock was promoted. Each entry must be present before a repo's HEAD advances past the locked commit. Entries without a classification block the six-repo gate in strict/release mode."
        deltas = @()
    }
    Write-JsonFile $postLockDeltasPath $resetDeltas

    $sixRepoLockSha = Get-FileSha256Hex $sixRepoLockPath
    Write-Host "  six-repo lock: $sixRepoLockPath"
    Write-Host "  post-lock deltas reset"
}

# ── Phase 3: platform gates (release mode) ────────────────────────────────────

Write-Host "`n=== Platform consumer conformance (release) ==="
$consumerArtifactDir = Join-Path $RepoRoot "artifacts/consumer-conformance/system-rc-$((Get-Date).ToUniversalTime().ToString('yyyyMMdd-HHmmss'))"
$consumerArgs = @{
    DevRoot     = $DevRoot
    RepoRoot    = $RepoRoot
    ArtifactDir = $consumerArtifactDir
    ReleaseMode = $true
}
if ($FullWorkspace) {
    $consumerArgs.FullWorkspace = $true
}
& (Join-Path $PSScriptRoot "run-consumer-conformance.ps1") @consumerArgs
if ($LASTEXITCODE -ne 0) {
    Add-GateResult "platform-consumer-conformance" "fail" "exit $LASTEXITCODE"
    throw "Consumer conformance failed."
}
$consumerSummaryPath = Join-Path $consumerArtifactDir "summary.json"
Add-GateResult "platform-consumer-conformance" "pass"

Write-Host "`n=== Six-repo compatibility gate (release) ==="
$sixRepoArtifactDir = Join-Path $RepoRoot "artifacts/six-repo-compat/system-rc-$((Get-Date).ToUniversalTime().ToString('yyyyMMdd-HHmmss'))"
& (Join-Path $PSScriptRoot "run-six-repo-compatibility-gate.ps1") `
    -DevRoot $DevRoot `
    -RepoRoot $RepoRoot `
    -ArtifactDir $sixRepoArtifactDir `
    -ReleaseMode
if ($LASTEXITCODE -ne 0) {
    Add-GateResult "six-repo-compatibility" "fail" "exit $LASTEXITCODE"
    throw "Six-repo compatibility gate failed."
}
Add-GateResult "six-repo-compatibility" "pass"

if (-not $SkipRuntimeCohesion) {
    Write-Host "`n=== Runtime system cohesion (quick) ==="
    $cohesionScript = Join-Path $allagmaRoot "scripts/system/run-system-cohesion-acceptance.ps1"
    if (Test-Path -LiteralPath $cohesionScript) {
        & $cohesionScript -DevRoot $DevRoot -Quick -SkipPlatformGate -UseExistingServices
        if ($LASTEXITCODE -ne 0) {
            Add-GateResult "runtime-cohesion" "fail" "exit $LASTEXITCODE"
            throw "System cohesion acceptance failed."
        }
        Add-GateResult "runtime-cohesion" "pass"
    }
    else {
        Add-GateResult "runtime-cohesion" "skipped" "script missing"
    }
}

# ── Phase 4: finalize system RC lock + evidence ───────────────────────────────

$sixRepoLockSha = Get-FileSha256Hex $sixRepoLockPath
$rcArtifacts = @{
    platformConsumerConformanceSummarySha256 = Get-FileSha256Hex $consumerSummaryPath
    sixRepoLockSha256 = $sixRepoLockSha
}

if ($Promote) {
    $sixRepoLockDoc = Get-Content -LiteralPath $sixRepoLockPath -Raw | ConvertFrom-Json
    $systemRcLockDoc = Build-SystemRcLockDocument `
        -RcId $RcId `
        -DevRoot $DevRoot `
        -RepoRoot $RepoRoot `
        -SixRepoLock $sixRepoLockDoc `
        -UiRc $uiRc `
        -FrontendRc $frontendRc `
        -RcArtifacts $rcArtifacts `
        -EvidenceFolderRel $evidenceFolderRel
    Write-JsonFile $systemRcLockPath $systemRcLockDoc
    Write-Host "`n  system RC lock: $systemRcLockPath"
}

$runtimeBaselineFinal = [string](Get-JsonFromFile (Join-Path $allagmaRoot "docs/system/ontogony-runtime.lock.json")).baseline
$sixRepoLockForSummary = Get-JsonFromFile $sixRepoLockPath
$summary = [ordered]@{
    schema = "ontogony-system-rc-summary-v1"
    baseline = $RcId
    runtimeBaseline = $runtimeBaselineFinal
    generatedAt = (Get-Date).ToUniversalTime().ToString("o")
    verdict = if ($failed) { "fail" } else { "pass" }
    devRoot = $DevRoot
    gates = @($gateResults)
    repos = $sixRepoLockForSummary.repos
    rcArtifacts = [ordered]@{
        platformConsumerConformanceSummarySha256 = $rcArtifacts.platformConsumerConformanceSummarySha256
        uiRcTarballSha256 = $uiRc.tarballSha256
        frontendBuildProvenanceSha256 = $frontendRc.buildProvenanceSha256
        frontendRcSummarySha256 = $frontendRc.summarySha256
        sixRepoLockSha256 = $sixRepoLockSha
    }
}

$uiTarballShaPath = Join-Path $uiRc.artifactDir "tarball-sha256.txt"
Write-SystemRcEvidenceBundle -EvidenceDir $evidenceDir -Summary $summary -Paths @{
    platformConsumerConformanceSummary = $consumerSummaryPath
    uiRcSummary = $uiRc.summaryPath
    uiTarballSha256 = $uiTarballShaPath
    frontendRcSummary = $frontendRc.summaryPath
    frontendProvenance = $frontendRc.provenancePath
    sixRepoLock = $sixRepoLockPath
    postLockDeltas = $postLockDeltasPath
}

Write-Host ""
Write-Host "PASS - system RC promotion complete." -ForegroundColor Green
Write-Host "  Evidence: $evidenceDir"
if (Test-Path -LiteralPath $systemRcLockPath) {
    Write-Host "  Lock:     $systemRcLockPath"
}
