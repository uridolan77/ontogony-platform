param(
    [switch]$Build,
    [switch]$NoWait
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
$composeFile = Join-Path $composeRoot "docker-compose.yml"
$defaultEnvFile = Join-Path $composeRoot ".env"
$exampleEnvFile = Join-Path $composeRoot ".env.example"
$waitScript = Join-Path $PSScriptRoot "wait-local-working-system.ps1"

if (-not (Test-Path -LiteralPath $composeFile)) {
    throw "Compose file not found: $composeFile"
}

$envFileToUse = $defaultEnvFile
if (-not (Test-Path -LiteralPath $envFileToUse)) {
    if (-not (Test-Path -LiteralPath $exampleEnvFile)) {
        throw "Missing env files. Expected either $defaultEnvFile or $exampleEnvFile."
    }
    $envFileToUse = $exampleEnvFile
    Write-Warning "'.env' was not found. Using '.env.example' development placeholders."
}

$composeArgs = @(
    "compose",
    "--env-file", $envFileToUse,
    "-f", $composeFile,
    "up",
    "-d"
)
if ($Build) {
    $composeArgs += "--build"
}

Write-Host "Starting Docker local working system ..."
docker @composeArgs
if ($LASTEXITCODE -ne 0) {
    throw "docker compose up failed (exit $LASTEXITCODE)."
}

if (-not $NoWait) {
    & $waitScript
    if ($LASTEXITCODE -ne 0) {
        throw "wait-local-working-system.ps1 failed (exit $LASTEXITCODE)."
    }
}

Write-Host "Docker local working system is up."
