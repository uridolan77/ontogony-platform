#!/usr/bin/env pwsh
# Validates Ontogony.* ProjectReference edges under src/ against the golden dependency map.
# See docs/architecture/package-levels.md (matrix) and docs/governance/PACKAGE_LEVEL_GOVERNANCE.md (change workflow).
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$srcRoot = Join-Path $repoRoot 'src'

# Golden Ontogony ProjectReference sets (PackageId -> sorted dependency PackageIds).
# When adding a package or edge, update this map and docs/architecture/package-levels.md.
$expectedRefs = [ordered]@{
    'Ontogony.AI.Contracts'        = @('Ontogony.Contracts')
    'Ontogony.Artifacts'           = @('Ontogony.Contracts', 'Ontogony.Hashing', 'Ontogony.Primitives')
    'Ontogony.Configuration'       = @()
    'Ontogony.Contracts'           = @('Ontogony.Primitives')
    'Ontogony.Errors'              = @('Ontogony.Observability')
    'Ontogony.Evaluation.Contracts' = @()
    'Ontogony.Execution'           = @()
    'Ontogony.Topology.Contracts'  = @()
    'Ontogony.Hashing'             = @('Ontogony.Contracts')
    'Ontogony.Hosting'             = @('Ontogony.Errors', 'Ontogony.Observability', 'Ontogony.Security')
    'Ontogony.Http'                = @('Ontogony.Observability', 'Ontogony.Primitives')
    'Ontogony.Idempotency'         = @('Ontogony.Hashing')
    'Ontogony.Logging'             = @('Ontogony.Observability', 'Ontogony.Redaction')
    'Ontogony.Messaging'           = @('Ontogony.Contracts', 'Ontogony.Hashing', 'Ontogony.Observability')
    'Ontogony.Observability'       = @('Ontogony.Contracts')
    'Ontogony.Persistence'         = @('Ontogony.Primitives')
    'Ontogony.Persistence.Postgres' = @('Ontogony.Persistence', 'Ontogony.Primitives')
    'Ontogony.Primitives'          = @()
    'Ontogony.ProtocolIngress'     = @('Ontogony.Contracts', 'Ontogony.Hashing', 'Ontogony.Primitives')
    'Ontogony.Quotas'              = @('Ontogony.Primitives')
    'Ontogony.Redaction'           = @()
    'Ontogony.Replay.Contracts'    = @()
    'Ontogony.Secrets'             = @('Ontogony.Redaction')
    'Ontogony.Secrets.AzureKeyVault' = @('Ontogony.Secrets')
    'Ontogony.Security'            = @('Ontogony.Contracts', 'Ontogony.Http', 'Ontogony.Primitives')
    'Ontogony.SystemCompatibility' = @('Ontogony.Contracts', 'Ontogony.Errors', 'Ontogony.Hashing', 'Ontogony.Http')
    'Ontogony.Testing'             = @(
        'Ontogony.Artifacts',
        'Ontogony.Contracts',
        'Ontogony.Errors',
        'Ontogony.Hashing',
        'Ontogony.Http',
        'Ontogony.Idempotency',
        'Ontogony.Messaging',
        'Ontogony.Observability',
        'Ontogony.Persistence',
        'Ontogony.Primitives',
        'Ontogony.Security'
    )
}

$forbiddenEdges = @(
    @{ Consumer = 'Ontogony.Execution'; Dependency = 'Ontogony.Artifacts' }
)

function Get-PackageId {
    param([string]$CsprojPath)
    $xml = [xml](Get-Content -LiteralPath $CsprojPath -Raw)
    $id = $xml.Project.PropertyGroup | Where-Object { $_.PackageId } | Select-Object -First 1 -ExpandProperty PackageId
    if ([string]::IsNullOrWhiteSpace($id)) {
        throw "PackageId missing in $CsprojPath"
    }
    return $id.Trim()
}

function Get-OntogonyProjectReferences {
    param([string]$CsprojPath)
    $dir = Split-Path -Parent $CsprojPath
    $xml = [xml](Get-Content -LiteralPath $CsprojPath -Raw)
    $refs = [System.Collections.Generic.List[string]]::new()
    $itemGroups = @($xml.Project.ItemGroup)
    foreach ($ig in $itemGroups) {
        $projectRefNodes = $ig.SelectNodes('ProjectReference')
        if ($null -eq $projectRefNodes -or @($projectRefNodes).Count -eq 0) { continue }
        foreach ($pr in $projectRefNodes) {
            $inc = $pr.Include
            if ([string]::IsNullOrWhiteSpace($inc)) { continue }
            $resolved = [System.IO.Path]::GetFullPath((Join-Path $dir $inc))
            if (-not (Test-Path -LiteralPath $resolved)) { continue }
            $depId = Get-PackageId -CsprojPath $resolved
            if ($depId.StartsWith('Ontogony.')) {
                $refs.Add($depId) | Out-Null
            }
        }
    }
    return ($refs | Sort-Object -Unique)
}

$csprojs = Get-ChildItem -LiteralPath $srcRoot -Directory -Filter 'Ontogony.*' |
    ForEach-Object { Join-Path $_.FullName ($_.Name + '.csproj') } |
    Where-Object { Test-Path -LiteralPath $_ }

$discoveredIds = [System.Collections.Generic.List[string]]::new()
$actualMap = @{}

foreach ($proj in ($csprojs | Sort-Object)) {
    $id = Get-PackageId -CsprojPath $proj
    $discoveredIds.Add($id) | Out-Null
    $actualMap[$id] = @(Get-OntogonyProjectReferences -CsprojPath $proj)
}

$expectedIds = @($expectedRefs.Keys)
$missingInExpected = $discoveredIds | Where-Object { $_ -notin $expectedIds }
$orphanExpected = $expectedIds | Where-Object { $_ -notin $discoveredIds }

$fail = $false

if (@($missingInExpected).Count -gt 0) {
    $fail = $true
    Write-Host 'Package-level validation failed: src/ contains PackageId(s) not in the golden map. Add them to validate-package-levels.ps1 and docs/architecture/package-levels.md:' -ForegroundColor Red
    $missingInExpected | ForEach-Object { Write-Host "  $_" }
}

if (@($orphanExpected).Count -gt 0) {
    $fail = $true
    Write-Host 'Package-level validation failed: golden map contains PackageId(s) with no src/*.csproj:' -ForegroundColor Red
    $orphanExpected | ForEach-Object { Write-Host "  $_" }
}

foreach ($edge in $forbiddenEdges) {
    $deps = $actualMap[$edge.Consumer]
    if ($deps -contains $edge.Dependency) {
        $fail = $true
        Write-Host "Package-level validation failed: forbidden edge $($edge.Consumer) -> $($edge.Dependency)." -ForegroundColor Red
    }
}

foreach ($id in $expectedIds) {
    if (-not $actualMap.ContainsKey($id)) { continue }
    $expected = @($expectedRefs[$id])
    $actual = @($actualMap[$id])
    $expS = $expected -join ';'
    $actS = $actual -join ';'
    if ($expS -ne $actS) {
        $fail = $true
        Write-Host "Package-level validation failed for $id (Ontogony ProjectReferences):" -ForegroundColor Red
        Write-Host "  expected: $expS" -ForegroundColor Yellow
        Write-Host "  actual:   $actS" -ForegroundColor Yellow
    }
}

# Shipping libraries must not reference Ontogony.Testing.
foreach ($kv in $actualMap.GetEnumerator()) {
    if ($kv.Key -eq 'Ontogony.Testing') { continue }
    if ($kv.Value -contains 'Ontogony.Testing') {
        $fail = $true
        Write-Host "Package-level validation failed: $($kv.Key) must not reference Ontogony.Testing." -ForegroundColor Red
    }
}

if ($fail) {
    exit 1
}

Write-Host 'OK: package-level ProjectReference validation passed.'
exit 0
