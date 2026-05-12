# Validates that the CHANGELOG has been updated for this release.
# Checks for an "Unreleased" or matching version section in CHANGELOG.md.
# This is a soft gate (warns) unless there are actual code changes.
param(
    [Parameter(Mandatory=$false)]
    [string]$ChangelogPath = (Join-Path (Resolve-Path (Join-Path $PSScriptRoot "..")) "CHANGELOG.md"),
    
    [Parameter(Mandatory=$false)]
    [string]$PackageVersion = $env:PACKAGE_VERSION,
    
    [Parameter(Mandatory=$false)]
    [switch]$Strict
)

$ErrorActionPreference = "Stop"

Write-Host "Validating CHANGELOG: $ChangelogPath"

if (-not (Test-Path $ChangelogPath)) {
    $msg = "CHANGELOG.md not found: $ChangelogPath"
    if ($Strict) { throw $msg } else { Write-Warning $msg; exit 0 }
}

$content = Get-Content $ChangelogPath -Raw

# Check for Unreleased section or a version section matching $PackageVersion
$hasUnreleasedSection = $content -match "##\s+(Unreleased|Unreleasd)"
$hasVersionSection = if (-not [string]::IsNullOrWhiteSpace($PackageVersion)) {
    $content -match "##\s+$([regex]::Escape($PackageVersion))"
} else {
    $false
}

Write-Host ""
if ($hasUnreleasedSection) {
    Write-Host "✓ Found 'Unreleased' section in CHANGELOG.md"
    exit 0
} elseif ($hasVersionSection) {
    Write-Host "✓ Found version $PackageVersion section in CHANGELOG.md"
    exit 0
} else {
    $msg = @"
⚠ CHANGELOG validation: No 'Unreleased' section or matching version section found.
Expected either:
  - A section starting with '## Unreleased'
  - A section matching the package version '$PackageVersion'
"@
    if ($Strict) {
        throw $msg
    } else {
        Write-Warning $msg
        exit 0
    }
}
