#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Clones or updates sibling repos at commits pinned in system-protocol-registry.json.
.DESCRIPTION
  Used by CI when only ontogony-platform is checked out. Idempotent for local DevRoot
  layouts that already contain siblings (skips existing .git directories).
#>
param(
    [string] $RepoRoot = "",
    [string] $DevRoot = "",
    [string] $GitHubOwner = "",
    [switch] $ShallowClone
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}
if ([string]::IsNullOrWhiteSpace($DevRoot)) {
    $DevRoot = (Resolve-Path (Join-Path $RepoRoot "..")).Path
}

$registryPath = Join-Path $RepoRoot "docs/system/system-protocol-registry.json"
$registry = Get-Content -LiteralPath $registryPath -Raw | ConvertFrom-Json

if ([string]::IsNullOrWhiteSpace($GitHubOwner)) {
    if ($env:GITHUB_REPOSITORY -match '^([^/]+)/') {
        $GitHubOwner = $Matches[1]
    }
    else {
        $GitHubOwner = "uridolan77"
    }
}

New-Item -ItemType Directory -Force -Path $DevRoot | Out-Null

foreach ($r in @($registry.repos)) {
    $name = [string]$r.name
    $commit = [string]$r.commit
    $dest = Join-Path $DevRoot $name
    $gitDir = Join-Path $dest ".git"

    if ($name -eq "ontogony-platform") {
        if (-not (Test-Path -LiteralPath $dest)) {
            if ($IsWindows -or $env:OS -match 'Windows') {
                New-Item -ItemType Junction -Path $dest -Target $RepoRoot -Force | Out-Null
            }
            else {
                New-Item -ItemType SymbolicLink -Path $dest -Target $RepoRoot -Force | Out-Null
            }
            Write-Host "Linked platform checkout: $dest -> $RepoRoot"
        }
        continue
    }

    if (Test-Path -LiteralPath $gitDir) {
        Write-Host "Sibling already present: $dest (skip clone)"
        continue
    }

    $remote = "https://github.com/$GitHubOwner/$name.git"
    Write-Host "Cloning $remote -> $dest @ $commit"
    $cloneArgs = @("clone")
    if ($ShallowClone) { $cloneArgs += @("--filter=blob:none") }
    $cloneArgs += @($remote, $dest)
    & git @cloneArgs
    if ($LASTEXITCODE -ne 0) { throw "git clone failed for $name" }
    Push-Location $dest
    try {
        git checkout $commit
        if ($LASTEXITCODE -ne 0) { throw "git checkout $commit failed in $name" }
    }
    finally { Pop-Location }
}

Write-Host "Registry siblings materialized under $DevRoot"
