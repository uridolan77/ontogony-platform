#!/usr/bin/env pwsh
# Scans Ontogony.AI.Contracts source for disallowed *product-meaning* phrases.
# Scoped to this package only so planning/docs that explain boundaries are not flagged.
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$scanRoots = @(
    (Join-Path $repoRoot 'src/Ontogony.AI.Contracts')
    (Join-Path $repoRoot 'tests/Ontogony.AI.Contracts.Tests')
)

$forbidden = @(
    'canonization'
    'contradiction resolution'
    'provider ranking'
    'fallback strategy'
    'agent planning'
    'tool selection policy'
    'epistemic status'
    'retrieval relevance'
)

$violations = [System.Collections.Generic.List[string]]::new()

foreach ($root in $scanRoots) {
    if (-not (Test-Path -LiteralPath $root)) { continue }

    Get-ChildItem -LiteralPath $root -Recurse -File -Filter '*.cs' | ForEach-Object {
        $text = Get-Content -LiteralPath $_.FullName -Raw
        foreach ($term in $forbidden) {
            if ($text.IndexOf($term, [System.StringComparison]::OrdinalIgnoreCase) -ge 0) {
                $violations.Add("$($_.FullName): $term")
            }
        }
    }
}

if ($violations.Count -gt 0) {
    Write-Host 'AI runtime boundary validation failed:' -ForegroundColor Red
    $violations | ForEach-Object { Write-Host "  $_" }
    exit 1
}

Write-Host 'OK: AI.Contracts boundary scan passed.'
exit 0
