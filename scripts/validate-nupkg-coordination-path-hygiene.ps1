#!/usr/bin/env pwsh
# Fails if any shipping .nupkg under artifacts/packages contains zip entries whose paths
# look like coordination/donor/incoming overlay material (PLAT-NP-004).
# Run after pack-all.ps1. See docs/governance/NUGET_SOURCE_MAPPING.md and _donors/README.md (PLAT-NP-004 hygiene).
#
# Matching is substring-based on normalized paths (forward slashes) so we catch odd zip layouts
# without requiring a specific leading slash on every entry.
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$pkgDir = Join-Path $repoRoot 'artifacts/packages'

if (-not (Test-Path -LiteralPath $pkgDir)) {
    throw "artifacts/packages not found at '$pkgDir'. Run ./scripts/pack-all.ps1 from the repo root (set PACKAGE_VERSION if needed)."
}

$nupkgs = @(Get-ChildItem -LiteralPath $pkgDir -Filter '*.nupkg' -File |
    Where-Object { $_.Name -notlike '*.symbols.nupkg' })

if ($nupkgs.Count -eq 0) {
    throw "No non-symbol .nupkg files under '$pkgDir'. Run ./scripts/pack-all.ps1 first."
}

# Case-insensitive substring checks on normalized paths. Keep fragments specific enough to avoid
# matching normal package README paths inside lib/ (e.g. avoid bare "incoming" without context).
$forbiddenFragments = @(
    '_agent_prompts'
    '_issue_bodies'
    'docs/_incoming_packages'
    'docs/_incoming'
    '_incoming_packages'
    'incoming_packages'
    '_incoming/'
    '/_incoming/'
    '.tmp/'
    '/.tmp/'
    '_donor'
    '/_donor/'
    'donors/'
    '/donors/'
)

foreach ($pkg in $nupkgs) {
    $zip = [System.IO.Compression.ZipFile]::OpenRead($pkg.FullName)
    try {
        foreach ($entry in $zip.Entries) {
            $raw = $entry.FullName
            if ([string]::IsNullOrEmpty($raw)) { continue }
            $norm = $raw.Replace('\', '/')
            foreach ($frag in $forbiddenFragments) {
                if ($norm.IndexOf($frag, [StringComparison]::OrdinalIgnoreCase) -ge 0) {
                    throw @"
Forbidden coordination path inside '$($pkg.Name)':
  zip entry: $raw
  matched fragment (case-insensitive): $frag

Fix: ensure packed projects do not glob planning/donor/temp trees (check None/Content items and nuspec file includes).
See docs/governance/NUGET_SOURCE_MAPPING.md and _donors/README.md
"@
                }
            }
        }
    }
    finally {
        $zip.Dispose()
    }
}

Write-Host "OK: coordination-path hygiene - scanned $($nupkgs.Count) .nupkg under artifacts/packages."
exit 0
