#!/usr/bin/env pwsh
<#
.SYNOPSIS
  SYS-STALE-PACKAGE-GUARD-001 — detect stale planning content in docs/_incoming packages.
.DESCRIPTION
  Scans incoming package trees for stale baseline names, closed backlog reopens,
  obsolete route prefixes, Agentor references, and production-readiness overclaims.

  Reconciliation packages must ship CURRENT_BASELINE (or alias), MOVING_MAIN_DELTA,
  and SUPERSEDED_ITEMS. Legacy packages must carry STALE_PACKAGE_QUARANTINE.json and
  still produce detectable stale findings (proving the guard works).

  See docs/system/INCOMING_PACKAGE_RUNBOOK.md.
#>
param(
    [string] $RepoRoot = "",
    [string] $IncomingRoot = "",
    [string] $PatternsPath = "",
    [switch] $ReportOnly
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($IncomingRoot)) {
    $IncomingRoot = Join-Path $RepoRoot "docs/_incoming"
}

if ([string]::IsNullOrWhiteSpace($PatternsPath)) {
    $PatternsPath = Join-Path $RepoRoot "docs/system/stale-incoming-package-patterns.json"
}

if (-not (Test-Path -LiteralPath $PatternsPath)) {
    throw "Missing patterns inventory: $PatternsPath"
}
if (-not (Test-Path -LiteralPath $IncomingRoot)) {
    throw "Missing incoming root: $IncomingRoot"
}

$config = Get-Content -LiteralPath $PatternsPath -Raw | ConvertFrom-Json
if ($config.schema -ne "ontogony-stale-incoming-package-patterns-v1") {
    throw "Unsupported patterns schema: $($config.schema)"
}

function Test-PathExempt([string]$packageRoot, [string]$fullPath, [object]$cfg) {
    $rel = $fullPath.Substring($packageRoot.Length).TrimStart('\', '/').Replace('\', '/')
    foreach ($ex in @($cfg.exemptRelativePaths)) {
        if ($rel -eq $ex -or $rel -like "*$ex") { return $true }
    }
    return $false
}

function Test-LineMatchesUnless([string]$line, [object]$pattern) {
    if (-not ($pattern.PSObject.Properties.Name -contains 'unlessLineRegex')) {
        return $true
    }
    $unless = [string]$pattern.unlessLineRegex
    if ([string]::IsNullOrWhiteSpace($unless)) {
        return $true
    }
    return $line -notmatch $unless
}

function Get-PackageFindings([string]$packageRoot, [object]$cfg) {
    $findings = [System.Collections.Generic.List[object]]::new()
    $exts = @($cfg.scanExtensions)
    Get-ChildItem -Path $packageRoot -Recurse -File |
        Where-Object { $exts -contains $_.Extension.ToLowerInvariant() } |
        ForEach-Object {
        if (Test-PathExempt $packageRoot $_.FullName $cfg) { return }
        $lines = Get-Content -LiteralPath $_.FullName
        $lineNum = 0
        foreach ($line in $lines) {
            $lineNum++
            foreach ($p in @($cfg.patterns)) {
                if ($line -match [string]$p.regex) {
                    if (-not (Test-LineMatchesUnless $line $p)) { continue }
                    $rel = $_.FullName.Substring($packageRoot.Length).TrimStart('\', '/')
                    $findings.Add([pscustomobject]@{
                            file    = $rel
                            line    = $lineNum
                            pattern = [string]$p.id
                            excerpt = if ($line.Length -gt 120) { $line.Substring(0, 117) + "..." } else { $line }
                        }) | Out-Null
                }
            }
        }
    }
    return @($findings)
}

function Test-ReconciliationFilesPresent([string]$packageRoot, [object]$cfg) {
    $missing = @()
    foreach ($set in @($cfg.reconciliationFileSets)) {
        $found = $false
        foreach ($name in @($set.anyOf)) {
            if (Test-Path -LiteralPath (Join-Path $packageRoot $name)) {
                $found = $true
                break
            }
        }
        if (-not $found) {
            $missing += [string]$set.label
        }
    }
    return $missing
}

$failures = [System.Collections.Generic.List[string]]::new()
$reports = [System.Collections.Generic.List[object]]::new()

foreach ($pkg in @($config.reconciliationPackages)) {
    $root = Join-Path $IncomingRoot ([string]$pkg.relativePath)
    if (-not (Test-Path -LiteralPath $root)) {
        $failures.Add("Reconciliation package missing: $($pkg.relativePath)") | Out-Null
        continue
    }

    if ([bool]$pkg.requireReconciliationFiles) {
        $missing = @(Test-ReconciliationFilesPresent $root $config)
        if (@($missing).Count -gt 0) {
            $failures.Add("Package '$($pkg.relativePath)' missing reconciliation file(s): $($missing -join ', ')") | Out-Null
        }
    }

    $findings = @(Get-PackageFindings $root $config)
    $max = [int]$pkg.maxNonExemptFindings
    $reports.Add([pscustomobject]@{
            package  = [string]$pkg.relativePath
            kind     = "reconciliation"
            findings = @($findings).Count
        }) | Out-Null

    if (@($findings).Count -gt $max) {
        $failures.Add("Package '$($pkg.relativePath)' has $(@($findings).Count) stale finding(s) (max $max).") | Out-Null
        $findings | Select-Object -First 12 | Format-Table -AutoSize | Out-String | Write-Host
    }
}

foreach ($legacy in @($config.quarantinedLegacyPackages)) {
    $root = Join-Path $IncomingRoot ([string]$legacy.relativePath)
    $marker = Join-Path $IncomingRoot ([string]$legacy.quarantineMarkerRelativePath)
    if (-not (Test-Path -LiteralPath $root)) {
        $failures.Add("Quarantined legacy package path missing: $($legacy.relativePath)") | Out-Null
        continue
    }
    if (-not (Test-Path -LiteralPath $marker)) {
        $failures.Add("Legacy package '$($legacy.relativePath)' requires quarantine marker: $($legacy.quarantineMarkerRelativePath)") | Out-Null
        continue
    }

    $findings = @(Get-PackageFindings $root $config)
    $min = [int]$legacy.minimumStaleFindings
    $findingCount = @($findings).Count
    $reports.Add([pscustomobject]@{
            package  = [string]$legacy.relativePath
            kind     = "quarantined-legacy"
            findings = $findingCount
        }) | Out-Null

    Write-Host "Quarantined legacy '$($legacy.relativePath)': $findingCount stale finding(s) (detector active)." -ForegroundColor Yellow
    if ($findingCount -lt $min) {
        $failures.Add("Quarantined legacy '$($legacy.relativePath)' expected >= $min stale findings but found $findingCount (detector may have regressed).") | Out-Null
    }
    elseif ($findingCount -gt 0) {
        $findings | Select-Object -First 8 | Format-Table -AutoSize | Out-String | Write-Host
    }
}

# Unmarked package roots: any immediate child dir with README/MANIFEST but not listed above
$known = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
foreach ($pkg in @($config.reconciliationPackages)) { [void]$known.Add([string]$pkg.relativePath) }
foreach ($legacy in @($config.quarantinedLegacyPackages)) { [void]$known.Add([string]$legacy.relativePath) }

Get-ChildItem -LiteralPath $IncomingRoot -Directory | Where-Object {
    -not $_.Name.StartsWith('_', [System.StringComparison]::Ordinal)
} | ForEach-Object {
    $candidates = @($_.FullName)
    $nested = Get-ChildItem -LiteralPath $_.FullName -Directory -ErrorAction SilentlyContinue |
        Where-Object { Test-Path -LiteralPath (Join-Path $_.FullName "README.md") }
    if ($nested) { $candidates = @($nested.FullName) }

    foreach ($dir in $candidates) {
        $rel = $dir.Substring($IncomingRoot.Length).TrimStart('\', '/').Replace('\', '/')
        if ($known.Contains($rel)) { continue }
        $hasMarker = Test-Path -LiteralPath (Join-Path $dir $config.quarantineMarkerFile)
        $hasManifest = (Test-Path -LiteralPath (Join-Path $dir "MANIFEST.json")) -or (Test-Path -LiteralPath (Join-Path $dir "README.md"))
        if (-not $hasManifest) { continue }

        $findings = @(Get-PackageFindings $dir $config)
        $findingCount = @($findings).Count
        if ($findingCount -eq 0) { continue }

        if ($hasMarker) {
            Write-Host "Unlisted package '$rel' has quarantine marker; $findingCount finding(s) (allowed)." -ForegroundColor DarkYellow
            continue
        }

        $failures.Add("Unquarantined incoming package '$rel' has $findingCount stale finding(s). Add STALE_PACKAGE_QUARANTINE.json or reconcile content.") | Out-Null
        $findings | Select-Object -First 6 | Format-Table -AutoSize | Out-String | Write-Host
    }
}

Write-Host ""
Write-Host "Stale incoming-package scan summary (baseline=$($config.currentBaseline)):" -ForegroundColor Cyan
$reports | Format-Table -AutoSize | Out-String | Write-Host

if (@($failures).Count -gt 0) {
    Write-Host "Stale incoming-package validation failed:" -ForegroundColor Red
    foreach ($f in $failures) { Write-Host "  $f" }
    if (-not $ReportOnly) { throw "SYS-STALE-PACKAGE-GUARD-001: $(@($failures).Count) failure(s)." }
    exit 1
}

Write-Host "stale-incoming-package OK (SYS-STALE-PACKAGE-GUARD-001)" -ForegroundColor Green
