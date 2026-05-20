# Start the three-node observability stack (Grafana, Jaeger, Prometheus, OTEL collector).
# Compose and dashboard assets live in allagma-dotnet; canonical docs: ontogony-platform/docs/operations/SYSTEM_DASHBOARD_SLO_INDEX.md

param(
    [string]$DevRoot = "C:\dev",
    [string]$AllagmaRoot = "",
    [int]$GrafanaHostPort = 0,
    [switch]$Detach = $true,
    [switch]$Build
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($AllagmaRoot)) {
    $AllagmaRoot = Join-Path $DevRoot "allagma-dotnet"
}
$AllagmaRoot = (Resolve-Path -LiteralPath $AllagmaRoot).Path

$composeFile = Join-Path $AllagmaRoot "docker-compose.observability.yml"
if (-not (Test-Path -LiteralPath $composeFile)) {
    throw "Observability compose not found: $composeFile (set -AllagmaRoot or -DevRoot)."
}

$env:GRAFANA_HOST_PORT = if ($GrafanaHostPort -gt 0) { "$GrafanaHostPort" } else { $env:GRAFANA_HOST_PORT }
if ([string]::IsNullOrWhiteSpace($env:GRAFANA_HOST_PORT)) {
    $env:GRAFANA_HOST_PORT = "3000"
}

$composeArgs = @("compose", "-f", $composeFile)
if ($Build) { $composeArgs += "build" }
$composeArgs += "up"
if ($Detach) { $composeArgs += "-d" }

Write-Host "[INFO] Starting observability stack from: $AllagmaRoot"
Write-Host "[INFO] Grafana: http://localhost:$($env:GRAFANA_HOST_PORT) (folder: Ontogony -> Ontogony Alpha Runtime)"
Write-Host "[INFO] Jaeger:  http://localhost:16686"
Write-Host "[INFO] Prometheus: http://localhost:9090"
Write-Host "[INFO] Docs: ontogony-platform/docs/operations/SYSTEM_DASHBOARD_SLO_INDEX.md"

Push-Location $AllagmaRoot
try {
    & docker @composeArgs
    if ($LASTEXITCODE -ne 0) {
        throw "docker compose failed with exit code $LASTEXITCODE"
    }
}
finally {
    Pop-Location
}
