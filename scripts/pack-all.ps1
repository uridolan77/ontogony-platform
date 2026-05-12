# Packs all shipping projects in Ontogony.Platform.sln to artifacts/packages.
# Run from repo root or any directory (script resolves the solution path).
param(
    [switch]$NoBuild,
    [switch]$IncludeSymbols
)

$ErrorActionPreference = "Stop"

$RepoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
Set-Location $RepoRoot

$version = $env:PACKAGE_VERSION
if ([string]::IsNullOrWhiteSpace($version)) {
    $version = "0.1.0-starter"
}

$outDir = Join-Path $RepoRoot "artifacts/packages"
New-Item -ItemType Directory -Force -Path $outDir | Out-Null

$packArgs = @(
    "pack", "Ontogony.Platform.sln",
    "-c", "Release",
    "-p:PackageVersion=$version",
    "-o", $outDir
)
if ($NoBuild) {
    $packArgs += "--no-build"
}
if ($IncludeSymbols) {
    $packArgs += "-p:IncludeSymbols=true"
}

Write-Host "dotnet $($packArgs -join ' ')"
& dotnet @packArgs

$pkgs = @(Get-ChildItem $outDir -Filter "*.nupkg" -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -notlike "*.symbols.nupkg" })

if ($pkgs.Count -eq 0) {
    throw "Pack produced no .nupkg under $outDir (check build errors and IsPackable)."
}

Write-Host ""
Write-Host "Packages ($($pkgs.Count)):"
$pkgs | Sort-Object Name | ForEach-Object { Write-Host "  $($_.Name)" }
