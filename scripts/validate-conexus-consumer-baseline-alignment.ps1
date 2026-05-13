#!/usr/bin/env pwsh
# Ensures the Conexus baseline package list in docs matches src/Directory.Build.targets (PR-PLAT-006.1 consumer surface / CS1591 strip list).
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$targetsPath = Join-Path $repoRoot 'src\Directory.Build.targets'
$readinessPath = Join-Path $repoRoot 'docs\consumer-blueprints\conexus-dotnet-platform-readiness.md'

if (-not (Test-Path -LiteralPath $targetsPath)) { throw "Missing $targetsPath" }
if (-not (Test-Path -LiteralPath $readinessPath)) { throw "Missing $readinessPath" }

$targetsRaw = Get-Content -LiteralPath $targetsPath -Raw
$fromTargets = [regex]::Matches(
    $targetsRaw,
    "'\$\(MSBuildProjectName\)' == '(Ontogony\.[^']+)'"
) | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique

$readinessRaw = Get-Content -LiteralPath $readinessPath -Raw
$start = $readinessRaw.IndexOf('## Required packages')
if ($start -lt 0) { throw "Readiness doc: missing '## Required packages' section." }
$end = $readinessRaw.IndexOf('## Optional later', $start)
if ($end -lt 0) { throw "Readiness doc: missing '## Optional later' after required packages." }
$section = $readinessRaw.Substring($start, $end - $start)

$fromDoc = [regex]::Matches($section, '\|\s*`(Ontogony\.[^`]+)`\s*\|') |
    ForEach-Object { $_.Groups[1].Value } |
    Sort-Object -Unique

$diff = Compare-Object -ReferenceObject $fromTargets -DifferenceObject $fromDoc
if ($null -ne $diff) {
    $onlyTargets = ($diff | Where-Object SideIndicator -eq '<=').InputObject
    $onlyDoc = ($diff | Where-Object SideIndicator -eq '=>').InputObject
    $msg = @(
        'Conexus consumer baseline drift: src/Directory.Build.targets and conexus-dotnet-platform-readiness.md disagree.'
        "Only in targets: $($onlyTargets -join ', ')"
        "Only in doc: $($onlyDoc -join ', ')"
        "Targets ($($fromTargets.Count)): $($fromTargets -join ', ')"
        "Doc ($($fromDoc.Count)): $($fromDoc -join ', ')"
    ) -join "`n"
    throw $msg
}

Write-Host "OK: $($fromTargets.Count) Conexus baseline packages align between Directory.Build.targets and readiness doc."
exit 0
