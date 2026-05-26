#!/usr/bin/env pwsh
# Validates docs/_incoming hygiene: allowed children, no zips, manifest/path consistency.
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$incomingRoot = Join-Path $repoRoot 'docs\_incoming'
$docsRoot = Join-Path $repoRoot 'docs'
$activeManifest = Join-Path $incomingRoot '_active\MANIFEST.md'
$consumedManifest = Join-Path $incomingRoot '_consumed\MANIFEST.md'

$failures = [System.Collections.Generic.List[string]]::new()

function Add-Failure([string]$Message) {
    $failures.Add($Message) | Out-Null
}

if (-not (Test-Path -LiteralPath $incomingRoot)) {
    Write-Error "docs/_incoming not found: $incomingRoot"
    exit 1
}

# 1. Direct children of docs/_incoming
$allowedIncoming = @('README.md', '_active', '_consumed')
Get-ChildItem -LiteralPath $incomingRoot -Force | ForEach-Object {
    if ($_.Name -notin $allowedIncoming) {
        Add-Failure "_incoming direct child not allowed: $($_.Name)"
    }
}

# 2. No zips under docs/
Get-ChildItem -LiteralPath $docsRoot -Recurse -File -Filter '*.zip' -ErrorAction SilentlyContinue | ForEach-Object {
    Add-Failure "zip file under docs/: $($_.FullName.Substring($repoRoot.Length + 1))"
}

# 3. No loose package markdown directly under docs/_incoming (except README.md)
Get-ChildItem -LiteralPath $incomingRoot -File -Filter '*.md' -ErrorAction SilentlyContinue | ForEach-Object {
    if ($_.Name -ne 'README.md') {
        Add-Failure "loose markdown under _incoming: $($_.Name)"
    }
}

function Get-ManifestPackageNames {
    param(
        [string]$ManifestPath,
        [switch]$ActiveTableOnly
    )

    if (-not (Test-Path -LiteralPath $ManifestPath)) {
        return @()
    }

    $names = [System.Collections.Generic.List[string]]::new()
    $content = Get-Content -LiteralPath $ManifestPath -Raw
    $inTable = $false

    foreach ($line in ($content -split "`n")) {
        if ($line -match '^\|\s*Package\s*\|') {
            $inTable = $true
            continue
        }
        if ($inTable -and $line -match '^\|\s*---') { continue }
        if ($inTable -and $line -notmatch '^\|') {
            if ($ActiveTableOnly) { break }
            $inTable = $false
            continue
        }
        if (-not $inTable) { continue }

        if ($line -match '^\|\s*\[([^\]]+)\]\(\./([^/)]+)/?\)') {
            $names.Add($Matches[2].Trim()) | Out-Null
        }
        elseif ($line -match '^\|\s*([A-Z0-9][A-Z0-9._-]+)\s*\|') {
            $candidate = $Matches[1].Trim()
            if ($candidate -notin @('Package', 'Month', '---')) {
                $names.Add($candidate) | Out-Null
            }
        }
    }

    return @($names | Select-Object -Unique)
}

function Test-ConsumedPackageExists {
    param([string]$PackageName)

    $monthRoot = Join-Path $incomingRoot '_consumed'
    $months = Get-ChildItem -LiteralPath $monthRoot -Directory -ErrorAction SilentlyContinue |
        Where-Object { $_.Name -match '^\d{4}-\d{2}$' }

    foreach ($month in $months) {
        $candidate = Join-Path $month.FullName $PackageName
        if (Test-Path -LiteralPath $candidate) {
            return $true
        }
    }

    return $false
}

function Test-PackageHasStatusDocumentation {
    param([string]$PackageDir)

    if (-not (Test-Path -LiteralPath $PackageDir)) {
        return $false
    }

    foreach ($name in @('README.md', 'IMPLEMENTATION_NOTES.md')) {
        if (Test-Path -LiteralPath (Join-Path $PackageDir $name)) {
            return $true
        }
    }

    foreach ($sub in Get-ChildItem -LiteralPath $PackageDir -Directory -ErrorAction SilentlyContinue) {
        if (Test-Path -LiteralPath (Join-Path $sub.FullName 'README.md')) { return $true }
        if (Test-Path -LiteralPath (Join-Path $sub.FullName 'IMPLEMENTATION_NOTES.md')) { return $true }
        if (Test-Path -LiteralPath (Join-Path $sub.FullName '00_UNPACK_PROMPT.md')) { return $true }
    }

    return $false
}

# Parse manifests
$activePackages = Get-ManifestPackageNames -ManifestPath $activeManifest -ActiveTableOnly
$consumedContent = if (Test-Path $consumedManifest) { Get-Content -LiteralPath $consumedManifest -Raw } else { '' }

# Consumed names: table rows only; skip "zip only, removed"
$consumedPackages = [System.Collections.Generic.List[string]]::new()
$inTable = $false
foreach ($line in ($consumedContent -split "`n")) {
    if ($line -match '^\|\s*Package\s*\|') { $inTable = $true; continue }
    if ($inTable -and $line -match '^\|\s*---') { continue }
    if ($inTable -and $line -notmatch '^\|') { $inTable = $false; continue }
    if (-not $inTable) { continue }
    if ($line -match 'zip only,\s*removed') { continue }

    if ($line -match '^\|\s*\[([^\]]+)\]') {
        $consumedPackages.Add($Matches[1].Trim()) | Out-Null
    }
    elseif ($line -match '^\|\s*([A-Z0-9][A-Z0-9._-]+)\s*\|') {
        $candidate = $Matches[1].Trim()
        if ($candidate -ne 'Package') {
            $consumedPackages.Add($candidate) | Out-Null
        }
    }
}

$consumedSet = @($consumedPackages | Select-Object -Unique)

# 4–6. Active packages exist, not duplicated, have status docs
$activeRoot = Join-Path $incomingRoot '_active'
foreach ($pkg in $activePackages) {
    $pkgDir = Join-Path $activeRoot $pkg
    if (-not (Test-Path -LiteralPath $pkgDir)) {
        Add-Failure "active manifest package missing on disk: $pkg"
        continue
    }
    if ($pkg -in $consumedSet) {
        Add-Failure "package listed in both active and consumed manifests: $pkg"
    }
    if (-not (Test-PackageHasStatusDocumentation -PackageDir $pkgDir)) {
        Add-Failure "active package has no README.md, IMPLEMENTATION_NOTES.md, or inner 00_UNPACK_PROMPT/README: $pkg"
    }
}

# 7. Consumed packages exist (unless zip-only removed — already skipped)
foreach ($pkg in $consumedSet) {
    if (-not (Test-ConsumedPackageExists -PackageName $pkg)) {
        Add-Failure "consumed manifest package missing on disk: $pkg (expected under _consumed/<YYYY-MM>/)"
    }
}

# 8. Bidirectional active sync: every _active/<dir> must be in manifest; no extras either way
$onDiskActive = @(Get-ChildItem -LiteralPath $activeRoot -Directory -ErrorAction SilentlyContinue |
    ForEach-Object { $_.Name })
$manifestActiveSet = [System.Collections.Generic.HashSet[string]]::new([string[]]$activePackages)
foreach ($dir in $onDiskActive) {
    if (-not $manifestActiveSet.Contains($dir)) {
        Add-Failure "active directory not listed in _active/MANIFEST.md: $dir"
    }
}
# 9. Consumed month folders listed in manifest (2026-05)
$monthPath = Join-Path (Join-Path $incomingRoot '_consumed') '2026-05'
if (Test-Path -LiteralPath $monthPath) {
    $onDiskConsumed = @(Get-ChildItem -LiteralPath $monthPath -Directory -ErrorAction SilentlyContinue |
        ForEach-Object { $_.Name })
    $manifestConsumedSet = [System.Collections.Generic.HashSet[string]]::new([string[]]$consumedSet)
    foreach ($dir in $onDiskConsumed) {
        if (-not $manifestConsumedSet.Contains($dir)) {
            Add-Failure "consumed/2026-05 directory not listed in _consumed/MANIFEST.md: $dir"
        }
    }
}

if ($failures.Count -gt 0) {
    Write-Host 'Docs incoming hygiene validation failed:' -ForegroundColor Red
    foreach ($f in $failures) { Write-Host "  $f" }
    exit 1
}

Write-Host "OK: docs/_incoming hygiene passed ($($activePackages.Count) active, $($consumedSet.Count) consumed manifest entries)."
exit 0
