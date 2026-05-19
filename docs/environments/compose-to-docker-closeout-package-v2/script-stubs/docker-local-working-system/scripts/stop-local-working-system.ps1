$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
$composeFile = Join-Path $composeRoot "docker-compose.yml"
$envFile = Join-Path $composeRoot ".env"
if (-not (Test-Path -LiteralPath $envFile)) {
    $envFile = Join-Path $composeRoot ".env.example"
}

docker compose --env-file $envFile -f $composeFile down
Write-Host "Docker-local stack stopped. Volumes preserved."
