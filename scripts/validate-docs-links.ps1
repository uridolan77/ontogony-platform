#!/usr/bin/env pwsh
# Validates relative markdown links under docs/ resolve to existing files or directories.
# Skips: http(s)://, mailto:, bare #anchors, empty targets.
# Scans all markdown under docs/.
# Limitation: does not verify in-file anchor targets (#section) exist inside the target file.
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$docsRoot = Join-Path $repoRoot 'docs'

if (-not (Test-Path -LiteralPath $docsRoot)) {
    Write-Error "docs folder not found: $docsRoot"
    exit 1
}

$failures = [System.Collections.Generic.List[string]]::new()

Get-ChildItem -LiteralPath $docsRoot -Recurse -File -Filter '*.md' | ForEach-Object {
    $mdPath = $_.FullName
    $dir = $_.DirectoryName
    $content = Get-Content -LiteralPath $mdPath -Raw

    foreach ($m in [regex]::Matches($content, '(?<prefix>!?)\[[^\]]+\]\(([^)]+)\)')) {
        $target = $m.Groups[2].Value.Trim()
        if ([string]::IsNullOrWhiteSpace($target)) { continue }

        if ($target -match '^(https?://|mailto:|//)') { continue }

        $pathPart = ($target -split '#', 2)[0]
        if ([string]::IsNullOrWhiteSpace($pathPart)) { continue }

        if ($pathPart -match '^(vscode:|file:)') { continue }

        $combined = Join-Path $dir $pathPart
        try {
            $full = [System.IO.Path]::GetFullPath($combined)
        }
        catch {
            $failures.Add("$mdPath : invalid link path '$target'")
            continue
        }

        if (-not (Test-Path -LiteralPath $full)) {
            $failures.Add("$mdPath : '$target' -> $full")
        }
    }
}

if ($failures.Count -gt 0) {
    Write-Host 'Docs link validation failed:' -ForegroundColor Red
    foreach ($f in $failures) { Write-Host "  $f" }
    exit 1
}

Write-Host "OK: all relative links under $docsRoot resolve (anchor targets not checked)."
exit 0
