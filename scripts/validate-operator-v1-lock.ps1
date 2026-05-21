#!/usr/bin/env pwsh
<#
.SYNOPSIS
  FIRST-VERSION-RC-001 — validate docs/system/ontogony-operator-v1.lock.json.

.DESCRIPTION
  Structural checks, runtime baseline alignment with ontogony-runtime.lock.json,
  evidence path existence under DevRoot, and optional moving-main SHA pins (-ReleaseMode).
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [switch] $RequireEvidence,
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

$lockPath = Join-Path $RepoRoot "docs/system/ontogony-operator-v1.lock.json"
$runtimeLockPath = Join-Path $DevRoot "allagma-dotnet/docs/system/ontogony-runtime.lock.json"

if (-not (Test-Path -LiteralPath $lockPath)) {
    throw "Missing operator v1 lock: $lockPath"
}
if (-not (Test-Path -LiteralPath $runtimeLockPath)) {
    throw "Missing runtime lock: $runtimeLockPath"
}

$lock = Get-Content -LiteralPath $lockPath -Raw | ConvertFrom-Json
$runtimeLock = Get-Content -LiteralPath $runtimeLockPath -Raw | ConvertFrom-Json

function Test-FullGitSha([string]$sha) {
    return $sha -match '^[0-9a-f]{40}$'
}

$failures = [System.Collections.Generic.List[string]]::new()

function Add-Failure([string]$message) {
    $failures.Add($message) | Out-Null
}

if ($lock.schema -ne "ontogony-operator-v1-lock-v1") {
    Add-Failure "schema must be ontogony-operator-v1-lock-v1 (found '$($lock.schema)')"
}

if ($lock.baseline -ne "OPERATOR-V1-001") {
    Add-Failure "baseline must be OPERATOR-V1-001 (found '$($lock.baseline)')"
}

if ($lock.runtimeBaseline -ne $runtimeLock.baseline) {
    Add-Failure "runtimeBaseline '$($lock.runtimeBaseline)' must match ontogony-runtime.lock.json '$($runtimeLock.baseline)'"
}

$requiredRepos = @(
    "allagma-dotnet",
    "kanon-dotnet",
    "conexus-dotnet",
    "ontogony-platform",
    "ontogony-frontend",
    "ontogony-ui"
)

foreach ($repo in $requiredRepos) {
    $sha = [string]$lock.lockedCommits.$repo
    if (-not (Test-FullGitSha $sha)) {
        Add-Failure "lockedCommits.$repo must be a 40-char SHA (found '$sha')"
    }
}

$flows = @($lock.requiredOperatorFlows)
if ($flows.Count -lt 8) {
    Add-Failure "requiredOperatorFlows must list at least 8 demo flows (found $($flows.Count))"
}

if ($RequireEvidence -or $ReleaseMode) {
    foreach ($prop in $lock.evidence.PSObject.Properties) {
        $rel = [string]$prop.Value
        if ($rel -match '^ontogony-frontend/') {
            $full = Join-Path $DevRoot $rel
        }
        elseif ($rel -match '^docs/') {
            $full = Join-Path $RepoRoot $rel
        }
        else {
            $full = Join-Path $RepoRoot $rel
        }
        if (-not (Test-Path -LiteralPath $full)) {
            Add-Failure "evidence.$($prop.Name) missing: $full"
        }
    }
}

if ($ReleaseMode) {
    foreach ($repo in $requiredRepos) {
        $repoRoot = Join-Path $DevRoot $repo
        if (-not (Test-Path -LiteralPath (Join-Path $repoRoot ".git"))) {
            Add-Failure "ReleaseMode: missing git repo $repoRoot"
            continue
        }
        Push-Location $repoRoot
        try {
            $head = (git rev-parse HEAD).Trim()
            $pin = [string]$lock.lockedCommits.$repo
            if ($head.ToLowerInvariant() -ne $pin.ToLowerInvariant()) {
                Add-Failure "ReleaseMode: $repo HEAD $head != lock pin $pin"
            }
        }
        finally {
            Pop-Location
        }
    }
}

if (@($failures).Count -gt 0) {
    Write-Host "operator-v1-lock validation failed:" -ForegroundColor Red
    foreach ($f in $failures) {
        Write-Host "  $f"
    }
    throw "FIRST-VERSION-RC-001: $(@($failures).Count) failure(s)."
}

Write-Host "operator-v1-lock OK (FIRST-VERSION-RC-001, baseline=$($lock.baseline))" -ForegroundColor Green
