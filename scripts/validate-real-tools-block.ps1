#!/usr/bin/env pwsh
<#
.SYNOPSIS
  SYS-REAL-TOOLS-BLOCK-VERIFY-001 — platform canonical docs must not overclaim real tool execution.

.DESCRIPTION
  Scans platform canonical docs for overclaim patterns. Allagma runtime proofs live in
  allagma-dotnet (SysRealToolsBlockVerify001Tests + scripts/validate-real-tools-block.ps1).

  Optional: -AllagmaRepoRoot runs the Allagma validator when a sibling checkout exists.

  See docs/evidence/SYS_REAL_TOOLS_BLOCK_VERIFY_001_EVIDENCE.md.
#>
param(
    [string] $RepoRoot = "",
    [string] $PatternsPath = "",
    [string] $AllagmaRepoRoot = "",
    [switch] $ReportOnly
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

if ([string]::IsNullOrWhiteSpace($PatternsPath)) {
    $PatternsPath = Join-Path $RepoRoot "docs/system/real-tools-block-verify-patterns.json"
}

if (-not (Test-Path -LiteralPath $PatternsPath)) {
    throw "Missing patterns inventory: $PatternsPath"
}

$config = Get-Content -LiteralPath $PatternsPath -Raw | ConvertFrom-Json
if ($config.schema -ne "ontogony-real-tools-block-verify-patterns-v1") {
    throw "Unsupported patterns schema: $($config.schema)"
}

$failures = [System.Collections.Generic.List[string]]::new()

function Test-LineMatchesUnless([string]$line, [object]$pattern) {
    if (-not ($pattern.PSObject.Properties.Name -contains "unlessLineRegex")) {
        return $true
    }
    $unless = [string]$pattern.unlessLineRegex
    if ([string]::IsNullOrWhiteSpace($unless)) {
        return $true
    }
    return $line -notmatch $unless
}

foreach ($rel in @($config.canonicalDocGlobs)) {
    $path = Join-Path $RepoRoot $rel
    if (-not (Test-Path -LiteralPath $path)) {
        $failures.Add("Missing canonical doc: $rel")
        continue
    }
    $docRel = $path.Substring($RepoRoot.Length).TrimStart('\', '/').Replace('\', '/')
    $lineNum = 0
    foreach ($line in Get-Content -LiteralPath $path) {
        $lineNum++
        foreach ($p in @($config.overclaimPatterns)) {
            if ($line -match [string]$p.regex) {
                if (Test-LineMatchesUnless $line $p) {
                    $failures.Add("$docRel`:$lineNum [$($p.id)] $($line.Trim())")
                }
            }
        }
    }
}

$evidencePath = Join-Path $RepoRoot "docs/evidence/SYS_REAL_TOOLS_BLOCK_VERIFY_001_EVIDENCE.md"
if (-not (Test-Path -LiteralPath $evidencePath)) {
    $failures.Add("Missing evidence doc: docs/evidence/SYS_REAL_TOOLS_BLOCK_VERIFY_001_EVIDENCE.md")
}

if (-not [string]::IsNullOrWhiteSpace($AllagmaRepoRoot)) {
    $allagmaScript = Join-Path $AllagmaRepoRoot "scripts/validate-real-tools-block.ps1"
    if (Test-Path -LiteralPath $allagmaScript) {
        & $allagmaScript -RepoRoot $AllagmaRepoRoot
        if ($LASTEXITCODE -ne 0) {
            $failures.Add("Allagma validate-real-tools-block.ps1 failed (exit $LASTEXITCODE).")
        }
    }
}

if ($failures.Count -gt 0) {
    Write-Host "real-tools-block-verify FAIL ($($failures.Count) finding(s)):" -ForegroundColor Red
    foreach ($f in $failures) {
        Write-Host "  $f"
    }
    if (-not $ReportOnly) {
        throw "SYS-REAL-TOOLS-BLOCK-VERIFY-001: $($failures.Count) failure(s)."
    }
    exit 1
}

Write-Host "real-tools-block-verify OK (SYS-REAL-TOOLS-BLOCK-VERIFY-001)" -ForegroundColor Green
