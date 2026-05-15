#!/usr/bin/env pwsh
# PLAT-ALLAGMA-001: Allagma consumer blueprint + compile-only skeleton alignment.
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path

$readinessPath = Join-Path $repoRoot 'docs\consumer-blueprints\allagma-dotnet-platform-readiness.md'
$packageModePath = Join-Path $repoRoot 'docs\consumer-blueprints\ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md'
$starterPlanPath = Join-Path $repoRoot 'docs\consumer-blueprints\allagma-dotnet-starter-plan.md'
$skeletonCsproj = Join-Path $repoRoot 'examples\AllagmaDotNetSkeleton\AllagmaDotNetSkeleton.csproj'

foreach ($path in @($readinessPath, $packageModePath, $starterPlanPath, $skeletonCsproj)) {
    if (-not (Test-Path -LiteralPath $path)) { throw "Missing required path: $path" }
}

$readinessRaw = Get-Content -LiteralPath $readinessPath -Raw
$start = $readinessRaw.IndexOf('## Required packages')
if ($start -lt 0) { throw "Readiness doc: missing '## Required packages' section." }
$end = $readinessRaw.IndexOf('## Optional later', $start)
if ($end -lt 0) { throw "Readiness doc: missing '## Optional later' after required packages." }
$section = $readinessRaw.Substring($start, $end - $start)

$fromDoc = [regex]::Matches($section, '\|\s*`(Ontogony\.[^`]+)`\s*\|') |
    ForEach-Object { $_.Groups[1].Value } |
    Sort-Object -Unique

$csprojRaw = Get-Content -LiteralPath $skeletonCsproj -Raw
$fromSkeleton = [regex]::Matches(
    $csprojRaw,
    'ProjectReference Include="\.\.\\\.\.\\src\\(Ontogony\.[^\\]+)\\'
) | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique

$diff = Compare-Object -ReferenceObject $fromDoc -DifferenceObject $fromSkeleton
if ($null -ne $diff) {
    $onlyDoc = ($diff | Where-Object SideIndicator -eq '<=').InputObject
    $onlySkeleton = ($diff | Where-Object SideIndicator -eq '=>').InputObject
    $msg = @(
        'Allagma consumer baseline drift: readiness doc and AllagmaDotNetSkeleton.csproj disagree.'
        "Only in readiness doc: $($onlyDoc -join ', ')"
        "Only in skeleton csproj: $($onlySkeleton -join ', ')"
        "Doc ($($fromDoc.Count)): $($fromDoc -join ', ')"
        "Skeleton ($($fromSkeleton.Count)): $($fromSkeleton -join ', ')"
    ) -join "`n"
    throw $msg
}

$forbiddenFragments = @(
    'OpenAI',
    'Anthropic',
    'Azure.AI.OpenAI',
    'Google.AI',
    'Microsoft.Agents',
    'Kanon.',
    'Conexus.',
    'Agentor.',
    'Allagma.Runtime',
    'Allagma.Application',
    'Allagma.Api'
)

$refHits = [regex]::Matches(
    $csprojRaw,
    '(?:Package|Project)Reference\s+Include="([^"]+)"'
) | ForEach-Object { $_.Groups[1].Value }

foreach ($include in $refHits) {
    foreach ($frag in $forbiddenFragments) {
        if ($include -like "*$frag*") {
            throw "Forbidden reference in AllagmaDotNetSkeleton.csproj: Include=`"$include`" (matched $frag)"
        }
    }
}

Write-Host "Building AllagmaDotNetSkeleton (Release)..."
dotnet build (Join-Path $repoRoot 'examples\AllagmaDotNetSkeleton\AllagmaDotNetSkeleton.csproj') -c Release --no-restore 2>$null
if ($LASTEXITCODE -ne 0) {
    dotnet restore (Join-Path $repoRoot 'examples\AllagmaDotNetSkeleton\AllagmaDotNetSkeleton.csproj')
    if ($LASTEXITCODE -ne 0) { throw 'dotnet restore failed for AllagmaDotNetSkeleton.' }
    dotnet build (Join-Path $repoRoot 'examples\AllagmaDotNetSkeleton\AllagmaDotNetSkeleton.csproj') -c Release --no-restore
    if ($LASTEXITCODE -ne 0) { throw 'dotnet build failed for AllagmaDotNetSkeleton.' }
}

Write-Host "OK: $($fromDoc.Count) Allagma baseline packages align between readiness doc and skeleton; no forbidden references; skeleton builds."
exit 0
