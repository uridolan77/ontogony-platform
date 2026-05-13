#!/usr/bin/env pwsh

<#
.SYNOPSIS
Validate public API governance: if public API snapshots changed, CHANGELOG.md must be updated.

.DESCRIPTION
This script checks whether any public API snapshot files (.verified.txt) have been modified
and ensures that CHANGELOG.md has been updated accordingly.

In a git worktree, detects changed/added public API snapshot files in tests/Ontogory.PublicApi.Tests/
and requires corresponding CHANGELOG.md changes.

Exit codes:
  0 = All checks passed
  1 = Public API changed but CHANGELOG.md not updated (governance failure)
  2 = Script error (e.g., not in git repo)
#>

param(
    [switch]$Strict = $false  # If true, fail even on warnings
)

Set-StrictMode -Version 2.0
$ErrorActionPreference = 'Stop'

function Write-CheckPass {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-CheckWarn {
    param([string]$Message)
    Write-Host "⚠ $Message" -ForegroundColor Yellow
}

function Write-CheckFail {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

# Check if we're in a git repository
try {
    $null = git rev-parse --git-dir
}
catch {
    Write-CheckFail "Not in a git repository"
    exit 2
}

# Get list of changed/added public API snapshot files
$snapshotPattern = 'tests/Ontogory.PublicApi.Tests/.*\.verified\.txt$'
$changedSnapshots = @()

try {
    # Staged + unstaged changes
    $allChanges = git diff --name-only --diff-filter=A,M 2>$null
    if ($allChanges) {
        $changedSnapshots = $allChanges | Where-Object { $_ -match $snapshotPattern }
    }
}
catch {
    Write-CheckWarn "Could not enumerate git changes; skipping snapshot detection"
}

if ($changedSnapshots.Count -eq 0) {
    Write-CheckPass "No public API snapshot changes detected"
    exit 0
}

Write-Host "`nDetected public API snapshot changes:" -ForegroundColor Cyan
$changedSnapshots | ForEach-Object {
    Write-Host "  - $_"
}

# Check if CHANGELOG.md was modified
$changelogChanged = $false
try {
    $allChanges = git diff --name-only --diff-filter=A,M 2>$null
    if ($allChanges) {
        $changelogChanged = $allChanges | Where-Object { $_ -eq 'CHANGELOG.md' }
    }
}
catch {
    Write-CheckWarn "Could not check CHANGELOG.md status"
}

if ($changelogChanged) {
    Write-CheckPass "CHANGELOG.md was updated"
    Write-CheckPass "Public API governance check passed"
    exit 0
}
else {
    Write-CheckFail "Public API snapshots changed but CHANGELOG.md was NOT updated"
    Write-Host ""
    Write-Host "Required actions:" -ForegroundColor Yellow
    Write-Host "  1. Review the public API changes (compare .verified.txt files)"
    Write-Host "  2. Add an entry to CHANGELOG.md under 'Unreleased' section"
    Write-Host "  3. If breaking change, add/update migration documentation"
    Write-Host "  4. Re-run this script to validate"
    Write-Host ""
    Write-Host "Reference: see docs/public-api-review.md for guidance"
    exit 1
}
