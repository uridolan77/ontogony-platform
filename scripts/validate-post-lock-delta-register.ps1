#!/usr/bin/env pwsh
<#
.SYNOPSIS
  SYS-POSTLOCK-DELTA-REGISTER-001 — validate cross-repo post-lock delta register.

.DESCRIPTION
  Ensures docs/system/post-lock-delta-register.json lists every companion repo,
  uses allowed classification/disposition values, and references exist.

  See docs/evidence/SYS_POSTLOCK_DELTA_REGISTER_001_EVIDENCE.md
#>
param(
    [string] $RepoRoot = "",
    [string] $RegisterPath = "",
    [switch] $ReportOnly
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($RegisterPath)) {
    $RegisterPath = Join-Path $RepoRoot "docs/system/post-lock-delta-register.json"
}

if (-not (Test-Path -LiteralPath $RegisterPath)) {
    throw "Missing post-lock delta register: $RegisterPath"
}

$doc = Get-Content -LiteralPath $RegisterPath -Raw | ConvertFrom-Json
if ($doc.schema -ne "ontogony-post-lock-delta-register-v1") {
    throw "Unsupported register schema: $($doc.schema)"
}

$requiredRepos = @(
    "allagma-dotnet",
    "ontogony-platform",
    "kanon-dotnet",
    "conexus-dotnet",
    "ontogony-frontend",
    "ontogony-ui"
)

$allowedClassifications = [System.Collections.Generic.HashSet[string]]::new(
    [string[]]@(
        "additive_safe_api",
        "operator_doc_only",
        "test_only",
        "runtime_behavior_change",
        "lock_impacting_package",
        "follow_up_needed",
        "validated_alpha_006",
        "moving_main_delta"
    ),
    [StringComparer]::Ordinal
)

$allowedDispositions = [System.Collections.Generic.HashSet[string]]::new(
    [string[]]@("validated_alpha_006", "moving_main_delta", "no_delta"),
    [StringComparer]::Ordinal
)

$failures = [System.Collections.Generic.List[string]]::new()

function Add-Failure([string]$message) {
    $failures.Add($message) | Out-Null
}

function Test-ClassificationValue([object]$value) {
    if ($null -eq $value) {
        return $false
    }
    if ($value -is [System.Array]) {
        foreach ($item in $value) {
            if (-not $allowedClassifications.Contains([string]$item)) {
                return $false
            }
        }
        return $true
    }
    return $allowedClassifications.Contains([string]$value)
}

if ([string]::IsNullOrWhiteSpace([string]$doc.baseline)) {
    Add-Failure "register.baseline is required"
}

if ($doc.repos.Count -lt $requiredRepos.Count) {
    Add-Failure "register.repos must include at least $($requiredRepos.Count) entries"
}

$seenRepos = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
foreach ($entry in @($doc.repos)) {
    $repo = [string]$entry.repo
    if ([string]::IsNullOrWhiteSpace($repo)) {
        Add-Failure "repo entry missing repo name"
        continue
    }
    if (-not $seenRepos.Add($repo)) {
        Add-Failure "duplicate repo entry: $repo"
    }

    $lockDisposition = [string]$entry.lockDisposition
    $alpha006LockSha = [string]$entry.'alpha006LockSha'
    $movingMainHeadSha = [string]$entry.movingMainHeadSha

    if (-not $allowedDispositions.Contains($lockDisposition)) {
        Add-Failure "$repo lockDisposition invalid: $lockDisposition"
    }

    if ([string]::IsNullOrWhiteSpace($alpha006LockSha) -or $alpha006LockSha.Length -lt 8) {
        Add-Failure "$repo alpha006LockSha missing or too short"
    }

    if ($lockDisposition -eq "no_delta") {
        if ($movingMainHeadSha -ne $alpha006LockSha) {
            Add-Failure "$repo no_delta requires movingMainHeadSha == alpha006LockSha"
        }
    }
    elseif ([string]::IsNullOrWhiteSpace($movingMainHeadSha)) {
        Add-Failure "$repo movingMainHeadSha required when lockDisposition is not no_delta"
    }

    if (-not @($entry.groups)) {
        Add-Failure "$repo must declare at least one classified group"
    }

    foreach ($group in @($entry.groups)) {
        if (-not (Test-ClassificationValue $group.classification)) {
            Add-Failure "$repo group '$($group.id)' has invalid classification"
        }
        if ($group.PSObject.Properties.Name -contains "lockDisposition") {
            if (-not $allowedDispositions.Contains([string]$group.lockDisposition)) {
                Add-Failure "$repo group '$($group.id)' lockDisposition invalid"
            }
        }
    }
}

foreach ($required in $requiredRepos) {
    if (-not $seenRepos.Contains($required)) {
        Add-Failure "missing required repo: $required"
    }
}

$lockPath = Join-Path $RepoRoot "../allagma-dotnet/docs/system/ontogony-runtime.lock.json"
if (Test-Path -LiteralPath $lockPath) {
    $lock = Get-Content -LiteralPath $lockPath -Raw | ConvertFrom-Json
    if ([string]$lock.baseline -ne [string]$doc.baseline) {
        Add-Failure "register baseline '$($doc.baseline)' != lock file baseline '$($lock.baseline)'"
    }
    foreach ($entry in @($doc.repos)) {
        $repo = [string]$entry.repo
        $entryLockSha = [string]$entry.'alpha006LockSha'
        if ($lock.lockedCommits.PSObject.Properties.Name -contains $repo) {
            $expected = [string]$lock.lockedCommits.$repo
            if ($expected -ne $entryLockSha) {
                Add-Failure "$repo alpha006LockSha '$entryLockSha' != ontogony-runtime.lock.json '$expected'"
            }
        }
    }
}

if ($failures.Count -gt 0) {
    if ($ReportOnly) {
        Write-Host "post-lock-delta-register REPORT ($($failures.Count) finding(s)):" -ForegroundColor Yellow
        $failures | ForEach-Object { Write-Host "  $_" }
        exit 0
    }
    Write-Host "post-lock-delta-register FAIL ($($failures.Count) finding(s)):" -ForegroundColor Red
    $failures | ForEach-Object { Write-Host "  $_" }
    exit 1
}

Write-Host "post-lock-delta-register OK (SYS-POSTLOCK-DELTA-REGISTER-001)" -ForegroundColor Green
