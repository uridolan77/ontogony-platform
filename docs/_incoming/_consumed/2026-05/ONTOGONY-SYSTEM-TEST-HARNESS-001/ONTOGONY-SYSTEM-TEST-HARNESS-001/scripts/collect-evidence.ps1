$ErrorActionPreference = "Stop"
$target = "$PSScriptRoot/../evidence/archive/$(Get-Date -Format yyyyMMdd-HHmmss)"
New-Item -ItemType Directory -Force -Path $target | Out-Null
Copy-Item "$PSScriptRoot/../evidence/local/*" $target -Recurse -Force -ErrorAction SilentlyContinue
Write-Host "Evidence archived to $target"
