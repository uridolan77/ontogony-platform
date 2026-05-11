$ErrorActionPreference = "Stop"

$version = $env:PACKAGE_VERSION
if ([string]::IsNullOrWhiteSpace($version)) {
  $version = "0.1.0-starter"
}

New-Item -ItemType Directory -Force -Path artifacts/packages | Out-Null

dotnet pack Ontogony.Platform.sln -c Release -p:PackageVersion=$version -o artifacts/packages
