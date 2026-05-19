param(
    [switch]$Build,
    [switch]$SkipFrontend
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
$composeFile = Join-Path $composeRoot "docker-compose.yml"
$envFile = Join-Path $composeRoot ".env"
if (-not (Test-Path -LiteralPath $envFile)) {
    $envFile = Join-Path $composeRoot ".env.example"
}

$args = @("compose", "--env-file", $envFile, "-f", $composeFile, "up", "-d")
if ($Build) { $args += "--build" }
if ($SkipFrontend) { $args += @("postgres", "kanon-api", "conexus-api", "allagma-api") }

docker @args
& (Join-Path $PSScriptRoot "wait-local-working-system.ps1") -SkipFrontend:$SkipFrontend
