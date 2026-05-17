#!/usr/bin/env pwsh
# PLATFORM-GOV-003: Kanon.NET Ontogony package union — doc guard + optional sibling drift check.
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$unionPath = Join-Path $repoRoot 'docs\governance\kanon-ontogony-package-union.txt'

if (-not (Test-Path -LiteralPath $unionPath)) { throw "Missing $unionPath" }

$lines = Get-Content -LiteralPath $unionPath | ForEach-Object { $_.Trim() } |
    Where-Object { $_ -ne '' -and -not $_.StartsWith('#') }

$invalid = @($lines | Where-Object { $_ -notmatch '^Ontogony\.[A-Za-z0-9.]+$' })
if ($invalid.Count -gt 0) {
    throw "Invalid package id(s) in kanon-ontogony-package-union.txt: $($invalid -join ', ')"
}

$sorted = $lines | Sort-Object
if ($lines.Count -ne $sorted.Count) { throw 'Internal error: sort length mismatch.' }
for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -ne $sorted[$i]) {
        throw "kanon-ontogony-package-union.txt must be sorted ascending (first drift at index $i : '$($lines[$i])' vs expected '$($sorted[$i])')."
    }
}

$unique = $lines | Sort-Object -Unique
if ($unique.Count -ne $lines.Count) {
    throw 'kanon-ontogony-package-union.txt contains duplicate package ids.'
}

foreach ($pkg in $lines) {
    $csproj = Join-Path $repoRoot "src\$pkg\$pkg.csproj"
    if (-not (Test-Path -LiteralPath $csproj)) {
        throw "Union lists $pkg but missing platform project: $csproj"
    }
}

function Get-OntogonyIdsFromKanonProps {
    param([string]$PropsPath)
    $raw = Get-Content -LiteralPath $PropsPath -Raw
    $fromPackage = @([regex]::Matches($raw, 'PackageReference\s+Include="(Ontogony\.[^"]+)"') |
            ForEach-Object { $_.Groups[1].Value })
    $fromProject = @([regex]::Matches(
            $raw,
            'ProjectReference\s+Include="\$\(OntogonySiblingRoot\)/src/(Ontogony\.[^/\\]+)/'
        ) | ForEach-Object { $_.Groups[1].Value })
    @($fromPackage + $fromProject) | Sort-Object -Unique
}

$kanonRoot = $env:KANON_SIBLING_ROOT
if ([string]::IsNullOrWhiteSpace($kanonRoot)) {
    $kanonRoot = Join-Path $repoRoot '..\kanon-dotnet'
}
$propsPath = Join-Path $kanonRoot 'eng\Ontogony.References.props'

if (Test-Path -LiteralPath $propsPath) {
    $fromProps = Get-OntogonyIdsFromKanonProps -PropsPath $propsPath
    $diff = Compare-Object -ReferenceObject $lines -DifferenceObject $fromProps
    if ($null -ne $diff) {
        $onlyUnion = ($diff | Where-Object SideIndicator -eq '<=').InputObject
        $onlyProps = ($diff | Where-Object SideIndicator -eq '=>').InputObject
        $msg = @(
            "Kanon Ontogony props drift vs docs/governance/kanon-ontogony-package-union.txt ($propsPath)."
            "Only in union file: $($onlyUnion -join ', ')"
            "Only in Kanon props: $($onlyProps -join ', ')"
            'Update kanon-ontogony-package-union.txt or fix Kanon references.'
        ) -join "`n"
        throw $msg
    }
    Write-Host "OK: $($lines.Count) Kanon Ontogony packages; union file matches sibling Kanon props."
}
else {
    Write-Host "OK: $($lines.Count) Kanon Ontogony packages (union file format + platform projects); sibling Kanon props not present - skipped props diff ($propsPath)."
}

exit 0
