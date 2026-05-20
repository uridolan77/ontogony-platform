# Verify observability stack provisioning (Grafana dashboard + static checks).
# Full live OTLP/metrics gate: allagma-dotnet/scripts/verify-system-observability.ps1

param(
    [string]$DevRoot = "C:\dev",
    [string]$AllagmaRoot = "",
    [string]$GrafanaUrl = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($AllagmaRoot)) {
    $AllagmaRoot = Join-Path $DevRoot "allagma-dotnet"
}
$AllagmaRoot = (Resolve-Path -LiteralPath $AllagmaRoot).Path

$portPreflightLib = Join-Path $AllagmaRoot "scripts\lib\port-preflight.ps1"
if (-not (Test-Path -LiteralPath $portPreflightLib)) {
    throw "Missing port preflight library: $portPreflightLib"
}
. $portPreflightLib

if ([string]::IsNullOrWhiteSpace($GrafanaUrl)) {
    $grafanaPort = if (-not [string]::IsNullOrWhiteSpace($env:GRAFANA_HOST_PORT)) {
        $env:GRAFANA_HOST_PORT.Trim()
    }
    else {
        [string](Get-RecommendedGrafanaHostPort -PreferredPort 3000)
    }
    $env:GRAFANA_HOST_PORT = $grafanaPort
    $GrafanaUrl = "http://localhost:$grafanaPort"
}

$GrafanaUrl = Resolve-GrafanaHealthUrl -GrafanaUrl $GrafanaUrl -BoundParameters @{ GrafanaUrl = $GrafanaUrl }
$env:GRAFANA_HOST_PORT = [string](Get-PortFromUrl -Url $GrafanaUrl)

$verifyScript = Join-Path $AllagmaRoot "scripts\verify-system-observability.ps1"
if (-not (Test-Path -LiteralPath $verifyScript)) {
    throw "Verifier not found: $verifyScript"
}

$args = @(
    "-RepoRoot", $AllagmaRoot,
    "-GrafanaUrl", $GrafanaUrl,
    "-SkipStreamingSmoke",
    "-SkipObservabilityCompose",
    "-UseExistingServices",
    "-NoCleanup"
)

Write-Host "[INFO] Running static observability verification via allagma-dotnet"
Push-Location $AllagmaRoot
try {
    & pwsh -NoProfile -File $verifyScript @args
    if ($LASTEXITCODE -ne 0) {
        throw "verify-system-observability.ps1 failed with exit code $LASTEXITCODE"
    }
}
finally {
    Pop-Location
}

Write-Host "[PASS] Observability stack verification completed (static provisioning; smoke skipped)."
