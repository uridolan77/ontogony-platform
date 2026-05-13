#!/usr/bin/env pwsh

<#
.SYNOPSIS
Validate public API governance: if public API snapshots changed, CHANGELOG.md must be updated.

.DESCRIPTION
This script checks whether any public API snapshot files (.verified.txt) have been modified
and ensures that CHANGELOG.md has been updated accordingly.

Detection sources, in order:
1. Unstaged worktree changes.
2. Staged index changes.
3. CI pull_request diffs against origin/$GITHUB_BASE_REF when available.
4. CI push diffs against HEAD^ when available.

If no comparable diff is available and no local changes are present, the script exits cleanly.

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
    Write-Host "PASS: $Message" -ForegroundColor Green
}

function Write-CheckWarn {
    param([string]$Message)
    Write-Host "WARN: $Message" -ForegroundColor Yellow
}

function Write-CheckFail {
    param([string]$Message)
    Write-Host "FAIL: $Message" -ForegroundColor Red
}

function Get-ChangedPaths {
    param(
        [Parameter(Mandatory = $true)]
        [string[]]$Commands
    )

    $results = New-Object 'System.Collections.Generic.List[string]'
    foreach ($command in $Commands) {
        try {
            $output = Invoke-Expression $command 2>$null
            foreach ($line in @($output)) {
                if (-not [string]::IsNullOrWhiteSpace($line)) {
                    [void]$results.Add($line.Trim())
                }
            }
        }
        catch {
            Write-CheckWarn "Diff command failed: $command"
        }
    }

    return $results.ToArray() | Sort-Object -Unique
}

# Check if we're in a git repository
try {
    $null = git rev-parse --git-dir
}
catch {
    Write-CheckFail "Not in a git repository"
    exit 2
}

# Get list of changed/added public API snapshot files.
$snapshotPattern = '^tests/Ontogony\.PublicApi\.Tests/.*\.verified\.txt$'

$diffCommands = @(
    'git diff --name-only --diff-filter=AM',
    'git diff --cached --name-only --diff-filter=AM'
)

if ($env:GITHUB_EVENT_NAME -eq 'pull_request' -and -not [string]::IsNullOrWhiteSpace($env:GITHUB_BASE_REF)) {
    $diffCommands += "git diff --name-only --diff-filter=AM origin/$($env:GITHUB_BASE_REF)...HEAD"
}
elseif ($env:GITHUB_EVENT_NAME -eq 'push') {
    try {
        $null = git rev-parse HEAD^ 2>$null
        $diffCommands += 'git diff --name-only --diff-filter=AM HEAD^ HEAD'
    }
    catch {
        Write-CheckWarn 'HEAD^ is unavailable for push diff; using local changes only.'
    }
}

$allChanges = Get-ChangedPaths -Commands $diffCommands
$changedSnapshots = @($allChanges | Where-Object { $_ -match $snapshotPattern })

if ($changedSnapshots.Count -eq 0) {
    Write-CheckPass "No public API snapshot changes detected"
    exit 0
}

Write-Host "`nDetected public API snapshot changes:" -ForegroundColor Cyan
$changedSnapshots | ForEach-Object {
    Write-Host "  - $_"
}

# Check if CHANGELOG.md was modified in the same change set.
$changelogChanged = @($allChanges | Where-Object { $_ -eq 'CHANGELOG.md' }).Count -gt 0

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
    Write-Host "  3. If the diff removes or changes public members, add or update migration documentation"
    Write-Host "  4. Re-run this script to validate"
    Write-Host ""
    Write-Host "Reference: see docs/public-api-review.md for guidance"
    exit 1
}
