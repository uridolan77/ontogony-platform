param(
    [switch]$RemoveVolumes,
    [switch]$Force
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
$composeFile = Join-Path $composeRoot "docker-compose.yml"
$envFile = Join-Path $composeRoot ".env"
if (-not (Test-Path -LiteralPath $envFile)) {
    $envFile = Join-Path $composeRoot ".env.example"
}

if ($RemoveVolumes -and -not $Force) {
    throw "Volume removal is destructive. Re-run with -RemoveVolumes -Force."
}

$args = @("compose", "--env-file", $envFile, "-f", $composeFile, "down")
if ($RemoveVolumes) {
    $args += "-v"
}

docker @args

if ($RemoveVolumes) {
    Write-Host "Docker-local stack stopped and volumes removed."
} else {
    Write-Host "Docker-local stack stopped. Volumes preserved."
}
