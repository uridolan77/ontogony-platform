#!/usr/bin/env pwsh
# Fails if any shipping .nupkg under artifacts/packages contains zip entries whose paths
# look like coordination/donor/incoming overlay material (PLAT-NP-004).
# Run after pack-all.ps1. See docs/planning/next-phase/pr-specs/PR-PLAT-NP-004-donor-incoming-package-hygiene.md
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$pkgDir = Join-Path $repoRoot 'artifacts/packages'

if (-not (Test-Path -LiteralPath $pkgDir)) {
    throw "artifacts/packages not found at $pkgDir - run pack-all.ps1 first."
}

$nupkgs = @(Get-ChildItem -LiteralPath $pkgDir -Filter '*.nupkg' -File |
    Where-Object { $_.Name -notlike '*.symbols.nupkg' })

if ($nupkgs.Count -eq 0) {
    throw "No non-symbol .nupkg files under $pkgDir - run pack-all.ps1 first."
}

# Normalized paths use '/'. Any entry name containing one of these substrings fails CI.
$forbiddenFragments = @(
    '_agent_prompts'
    '_issue_bodies'
    'docs/_incoming_packages/'
    'docs/_incoming/'
    '/.tmp/'
    '/_donor/'
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
Forbidden coordination path inside $($pkg.Name):
  zip entry: $raw
  matched fragment (case-insensitive): $frag
See docs/planning/next-phase/pr-specs/PR-PLAT-NP-004-donor-incoming-package-hygiene.md
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
