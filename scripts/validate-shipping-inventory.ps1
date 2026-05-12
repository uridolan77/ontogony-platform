#!/usr/bin/env pwsh
# Ensures src/ has the expected number of Ontogony shipping projects and each ships a README.md.
# Update $expectedCount when packages are added or removed.
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$srcRoot = Join-Path $repoRoot 'src'
$expectedCount = 18

$dirs = @(Get-ChildItem -LiteralPath $srcRoot -Directory | Where-Object { $_.Name -like 'Ontogony.*' } | Sort-Object Name)
$csprojs = [System.Collections.Generic.List[string]]::new()
$missingReadme = [System.Collections.Generic.List[string]]::new()

foreach ($dir in $dirs) {
    $csprojName = "$($dir.Name).csproj"
    $csprojPath = Join-Path $dir.FullName $csprojName
    if (-not (Test-Path -LiteralPath $csprojPath)) {
        continue
    }
    $csprojs.Add($csprojPath)

    $readme = Join-Path $dir.FullName 'README.md'
    if (-not (Test-Path -LiteralPath $readme)) {
        $missingReadme.Add($dir.FullName)
    }
}

if ($csprojs.Count -ne $expectedCount) {
    throw "Shipping inventory: expected $expectedCount Ontogony.* projects under src/, found $($csprojs.Count). Projects:`n$($csprojs -join "`n")"
}

if ($missingReadme.Count -gt 0) {
    throw "Shipping inventory: README.md missing under:`n$($missingReadme -join "`n")"
}

Write-Host "OK: $expectedCount shipping packages under src/ each have README.md."
exit 0
