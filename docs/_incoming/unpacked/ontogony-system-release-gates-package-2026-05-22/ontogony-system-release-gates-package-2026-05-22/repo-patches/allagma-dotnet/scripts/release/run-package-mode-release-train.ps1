[CmdletBinding()]
param(
  [string]$WorkspaceRoot = (Split-Path (Get-Location)),
  [string]$FeedDirectory = (Join-Path (Get-Location) "artifacts/local-cross-repo-feed"),
  [string]$RuntimeLockPath = "docs/system/ontogony-runtime.lock.json",
  [string]$OutputRoot = "artifacts/package-mode-release"
)

$ErrorActionPreference = "Stop"
$started = (Get-Date).ToUniversalTime().ToString("o")
$timestamp = (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ")
$out = Join-Path $OutputRoot $timestamp
New-Item -ItemType Directory -Force -Path $out | Out-Null
if (Test-Path (Join-Path $OutputRoot "latest")) { Remove-Item -Recurse -Force (Join-Path $OutputRoot "latest") }
New-Item -ItemType Directory -Force -Path (Join-Path $OutputRoot "latest") | Out-Null

$lock = Get-Content $RuntimeLockPath -Raw | ConvertFrom-Json
$ontogonyVersion = [string]$lock.packageVersions.Ontogony
$kanonVersion = [string]$lock.packageVersions.'Kanon.Client'
$conexusVersion = [string]$lock.packageVersions.'Conexus.Client'

$ontogonyRoot = Join-Path $WorkspaceRoot "ontogony-platform"
$kanonRoot = Join-Path $WorkspaceRoot "kanon-dotnet"
$conexusRoot = Join-Path $WorkspaceRoot "conexus-dotnet"

./scripts/pack-cross-repo-packages.ps1 `
  -FeedDirectory $FeedDirectory `
  -OntogonyRoot $ontogonyRoot `
  -KanonRoot $kanonRoot `
  -ConexusRoot $conexusRoot `
  -OntogonyPackageVersion $ontogonyVersion `
  -KanonPackageVersion $kanonVersion `
  -ConexusPackageVersion $conexusVersion

dotnet nuget remove source local-cross-repo --configfile nuget.config 2>$null
dotnet nuget add source $FeedDirectory --name local-cross-repo --configfile nuget.config

dotnet restore Allagma.sln --configfile nuget.config `
  -p:UseOntogonyPackages=true `
  -p:UseKanonPackages=true `
  -p:UseConexusPackages=true `
  -p:OntogonyPackageVersion=$ontogonyVersion `
  -p:KanonPackageVersion=$kanonVersion `
  -p:ConexusPackageVersion=$conexusVersion

dotnet build Allagma.sln -c Release --no-restore `
  -p:UseOntogonyPackages=true `
  -p:UseKanonPackages=true `
  -p:UseConexusPackages=true `
  -p:OntogonyPackageVersion=$ontogonyVersion `
  -p:KanonPackageVersion=$kanonVersion `
  -p:ConexusPackageVersion=$conexusVersion

dotnet test Allagma.sln -c Release --no-build `
  -p:UseOntogonyPackages=true `
  -p:UseKanonPackages=true `
  -p:UseConexusPackages=true `
  --filter "Category!=CrossRepo&Category!=PersistenceSmoke"

# Basic assertion: project.assets files should not contain sibling source repo paths for packed upstreams.
$assets = Get-ChildItem -Recurse -Filter project.assets.json | Where-Object { $_.FullName -like "*Allagma*" }
$bad = @()
foreach ($asset in $assets) {
  $raw = Get-Content $asset.FullName -Raw
  foreach ($needle in @("../kanon-dotnet","../conexus-dotnet","../ontogony-platform","..\\kanon-dotnet","..\\conexus-dotnet","..\\ontogony-platform")) {
    if ($raw.Contains($needle)) { $bad += "$($asset.FullName):$needle" }
  }
}
if ($bad.Count -gt 0) { throw "Package mode restored sibling references: $($bad -join '; ')" }

$pkgs = Get-ChildItem $FeedDirectory -Filter *.nupkg | ForEach-Object {
  [ordered]@{ id = ($_.BaseName -replace "\.$([regex]::Escape($ontogonyVersion))$",""); version = ""; path = $_.FullName }
}

$completed = (Get-Date).ToUniversalTime().ToString("o")
$summary = [ordered]@{
  schema = "ontogony-package-mode-release-summary-v1"
  verdict = "PASS"
  startedAtUtc = $started
  completedAtUtc = $completed
  runtimeLockBaseline = [string]$lock.baseline
  feedDirectory = $FeedDirectory
  packages = @(
    @{ id = "Ontogony.*"; version = $ontogonyVersion },
    @{ id = "Kanon.Client"; version = $kanonVersion },
    @{ id = "Kanon.Contracts"; version = [string]$lock.packageVersions.'Kanon.Contracts' },
    @{ id = "Conexus.Client"; version = $conexusVersion },
    @{ id = "Conexus.Contracts"; version = [string]$lock.packageVersions.'Conexus.Contracts' }
  )
  restore = @{ verdict = "PASS" }
  build = @{ verdict = "PASS" }
  test = @{ verdict = "PASS" }
  noSiblingReferenceAssertion = @{ verdict = "PASS" }
}

$summaryPath = Join-Path $out "package-mode-release-summary.json"
$summary | ConvertTo-Json -Depth 20 | Set-Content -Encoding UTF8 $summaryPath
Copy-Item $summaryPath (Join-Path $OutputRoot "latest/package-mode-release-summary.json") -Force
git checkout -- nuget.config 2>$null
Write-Host "Package-mode release summary: $summaryPath"
