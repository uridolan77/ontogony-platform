<# Template validation runner for Ontogony Operator V1. Copy/adapt into the right repo before using as canonical. #>
param([string]$DevRoot="C:\dev", [switch]$SkipDocker)
$ErrorActionPreference="Stop"
function Step($Name,[scriptblock]$Body){ Write-Host "`n=== $Name ===" -ForegroundColor Cyan; & $Body }
Step "Platform" { Push-Location "$DevRoot\ontogony-platform"; try { dotnet test Ontogony.Platform.sln -c Release; ./scripts/validate-system-protocol-registry.ps1; ./scripts/validate-stale-incoming-package.ps1; ./scripts/validate-real-tools-block.ps1 } finally { Pop-Location } }
Step "Allagma" { Push-Location "$DevRoot\allagma-dotnet"; try { dotnet test Allagma.sln -c Release; ./scripts/validate-real-tools-block.ps1; ./scripts/validate-feature-connection-matrix.ps1 } finally { Pop-Location } }
Step "Kanon" { Push-Location "$DevRoot\kanon-dotnet"; try { dotnet test -c Release } finally { Pop-Location } }
Step "Conexus" { Push-Location "$DevRoot\conexus-dotnet"; try { dotnet test -c Release } finally { Pop-Location } }
Step "Frontend" { Push-Location "$DevRoot\ontogony-frontend"; try { npm run openapi:gen; npm run check } finally { Pop-Location } }
Step "UI" { Push-Location "$DevRoot\ontogony-ui"; try { npm run check } finally { Pop-Location } }
if (-not $SkipDocker) { Step "Docker-local placeholder" { Write-Host "Run cohesion, restart, observability, FE smoke, Evidence Spine, demo Playwright." } }
