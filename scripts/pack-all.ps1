# Packs all shipping projects in Ontogony.Platform.sln to artifacts/packages.
# Requires environment variable PACKAGE_VERSION (semantic version for NuGet).
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
    throw "PACKAGE_VERSION is required (e.g. `$env:PACKAGE_VERSION='0.2.0'). Infrastructure packages must not pack under an implicit stale version."
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
    Where-Object { $_.Name -notlike "*.symbols.nupkg" -and $_.Name -like "*.$version.nupkg" })

if ($pkgs.Count -eq 0) {
    throw "Pack produced no .nupkg under $outDir (check build errors and IsPackable)."
}

$expectedShippingPackages = 27
if ($pkgs.Count -ne $expectedShippingPackages) {
    throw "Pack smoke: expected $expectedShippingPackages shipping .nupkg files for version $version, found $($pkgs.Count). If packages were added or removed, update scripts/pack-all.ps1 and scripts/validate-shipping-inventory.ps1."
}

Write-Host ""
Write-Host "Packages ($($pkgs.Count)):"
$pkgs | Sort-Object Name | ForEach-Object { Write-Host "  $($_.Name)" }
