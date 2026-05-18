param(
    [switch]$Force,
    [switch]$RemoveImages
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if (-not $Force) {
    throw "This removes the local Docker stack and volumes. Re-run with -Force."
}

$composeRoot = Split-Path -Parent $PSScriptRoot
$composeFile = Join-Path $composeRoot "docker-compose.yml"
$defaultEnvFile = Join-Path $composeRoot ".env"
$exampleEnvFile = Join-Path $composeRoot ".env.example"

if (-not (Test-Path -LiteralPath $composeFile)) {
    throw "Compose file not found: $composeFile"
}

$envFileToUse = $defaultEnvFile
if (-not (Test-Path -LiteralPath $envFileToUse)) {
    if (-not (Test-Path -LiteralPath $exampleEnvFile)) {
        throw "Missing env files. Expected either $defaultEnvFile or $exampleEnvFile."
    }
    $envFileToUse = $exampleEnvFile
}

$composeArgs = @(
    "compose",
    "--env-file", $envFileToUse,
    "-f", $composeFile,
    "down",
    "-v",
    "--remove-orphans"
)
if ($RemoveImages) {
    $composeArgs += "--rmi"
    $composeArgs += "local"
}

docker @composeArgs
if ($LASTEXITCODE -ne 0) {
    throw "docker compose down failed (exit $LASTEXITCODE)."
}

Write-Host "Docker local working system reset complete."
