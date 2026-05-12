# Generates a package manifest from produced .nupkg files.
# This manifest proves what packages were built and their versions.
# Accepts packages directory path and optionally a git commit hash.
param(
    [Parameter(Mandatory=$false)]
    [string]$PackagesDir = (Join-Path (Resolve-Path (Join-Path $PSScriptRoot "..")) "artifacts/packages"),
    
    [Parameter(Mandatory=$false)]
    [string]$CommitHash = $(git rev-parse HEAD 2>$null || "unknown"),
    
    [Parameter(Mandatory=$false)]
    [string]$OutputPath = (Join-Path (Resolve-Path (Join-Path $PSScriptRoot "..")) "PACKAGE_MANIFEST.json"),
    
    [Parameter(Mandatory=$false)]
    [string]$PackageVersion = $env:PACKAGE_VERSION
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $PackagesDir)) {
    throw "Packages directory not found: $PackagesDir"
}

Write-Host "Generating package manifest from: $PackagesDir"
Write-Host "Commit: $CommitHash"
Write-Host "Package Version: $PackageVersion"

# Find all nupkg files (excluding symbols packages)
$nupkgs = @(Get-ChildItem $PackagesDir -Filter "*.nupkg" -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -notlike "*.symbols.nupkg" } |
    Sort-Object Name)

if ($nupkgs.Count -eq 0) {
    throw "No .nupkg files found in $PackagesDir"
}

# Extract package information
$packages = @()
foreach ($pkg in $nupkgs) {
    # Parse package ID and version from filename (e.g., Ontogony.Contracts.0.2.0.nupkg)
    if ($pkg.Name -match "^(.+?)\.(\d+\.\d+\.\d+)\.nupkg$") {
        $pkgId = $matches[1]
        $pkgVersion = $matches[2]
        
        $packages += @{
            id = $pkgId
            version = $pkgVersion
            filename = $pkg.Name
            path = $pkg.FullName
            size = $pkg.Length
            hash = (Get-FileHash $pkg.FullName -Algorithm SHA256).Hash
        }
    } else {
        Write-Warning "Could not parse package name: $($pkg.Name)"
    }
}

# Quality gates
if ($packages.Count -eq 0) {
    throw "No valid packages found (parsing failed)."
}

# If PackageVersion specified, validate all packages match
if (-not [string]::IsNullOrWhiteSpace($PackageVersion)) {
    $mismatch = $packages | Where-Object { $_.version -ne $PackageVersion }
    if ($mismatch.Count -gt 0) {
        throw @"
Package version mismatch detected.
Expected all packages to be version $PackageVersion, but found:
$(($mismatch | ForEach-Object { "  $($_.id)=$($_.version)" }) -join "`n")
"@
    }
}

# Build manifest
$manifest = @{
    version = if (-not [string]::IsNullOrWhiteSpace($PackageVersion)) { $PackageVersion } else { $packages[0].version }
    generated = [datetime]::UtcNow.ToString("o")
    commit = $CommitHash
    packageCount = $packages.Count
    packages = $packages | ForEach-Object { 
        @{
            id = $_.id
            version = $_.version
            filename = $_.filename
            sizeBytes = $_.size
            sha256 = $_.hash
        }
    }
}

# Write manifest
$json = $manifest | ConvertTo-Json -Depth 10
$json | Out-File -FilePath $OutputPath -Encoding UTF8 -Force

Write-Host ""
Write-Host "✓ Manifest generated: $OutputPath"
Write-Host "  Version: $($manifest.version)"
Write-Host "  Generated: $($manifest.generated)"
Write-Host "  Commit: $($manifest.commit)"
Write-Host "  Packages: $($manifest.packageCount)"
Write-Host ""

# Return manifest object for script consumers
$manifest
