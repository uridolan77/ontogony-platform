#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Validates docs/system/ontogony-system-rc-*.lock.json against the live workspace.
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [string] $RcId = "ONTOGONY-SYSTEM-RC-002",
    [switch] $ReleaseMode
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

$lockFileName = ConvertTo-RcLockFileName $RcId
$lockPath = Join-Path $RepoRoot "docs/system/$lockFileName"
$sixRepoLockPath = Join-Path $RepoRoot "docs/system/ontogony-six-repo-lock.json"

if (-not (Test-Path -LiteralPath $lockPath)) {
    throw "Missing system RC lock: $lockPath"
}

$lock = Get-JsonFromFile $lockPath
$failures = [System.Collections.Generic.List[string]]::new()

function Add-Failure([string]$Message) {
    $script:failures.Add($Message) | Out-Null
}

if ($lock.schema -ne "ontogony-system-rc-lock-v1") {
    Add-Failure "schema must be ontogony-system-rc-lock-v1 (found '$($lock.schema)')"
}

if ($lock.baseline -ne $RcId) {
    Add-Failure "baseline must be $RcId (found '$($lock.baseline)')"
}

$evidenceFolderRel = [string]$lock.evidence.folder
$evidenceDir = Join-Path $RepoRoot ($evidenceFolderRel -replace '/', [IO.Path]::DirectorySeparatorChar)
if (-not (Test-Path -LiteralPath $evidenceDir)) {
    Add-Failure "evidence folder missing: $evidenceDir"
}

$requiredRepos = @(
    "ontogony-platform",
    "conexus-dotnet",
    "kanon-dotnet",
    "allagma-dotnet",
    "ontogony-ui",
    "ontogony-frontend"
)

foreach ($repo in $requiredRepos) {
    $expected = [string]$lock.repos.$repo.commit
    $repoPath = Join-Path $DevRoot $repo
    if (-not (Test-Path -LiteralPath $repoPath)) {
        Add-Failure "repo missing under DevRoot: $repoPath"
        continue
    }
    $actual = Get-RepoGitHead $repoPath
    if ($ReleaseMode -and $actual -ne $expected) {
        Add-Failure "$repo HEAD $actual does not match lock $expected"
    }
}

$actualSixRepoSha = Get-FileSha256Hex $sixRepoLockPath
$expectedSixRepoSha = [string]$lock.rcArtifacts.sixRepoLockSha256
if ($ReleaseMode -and $actualSixRepoSha -ne $expectedSixRepoSha) {
    Add-Failure "six-repo lock hash mismatch (expected $expectedSixRepoSha, actual $actualSixRepoSha)"
}

if ($failures.Count -gt 0) {
    Write-Host "System RC lock validation failed:" -ForegroundColor Red
    $failures | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
    exit 1
}

Write-Host "OK: system RC lock validation passed ($RcId)."
exit 0
