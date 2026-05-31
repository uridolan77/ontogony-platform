<#
.SYNOPSIS
  Stops host processes on local-working-system ports, tears down compose, and starts the stack fresh.

.EXAMPLE
  .\scripts\restart-local-working-system.ps1

.EXAMPLE
  .\scripts\restart-local-working-system.ps1 -Build
#>
param(
    [switch]$Build,
    [switch]$SkipFrontend
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

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

function Stop-ListenersOnPorts {
    param([int[]]$Ports)

    $pids = @()
    foreach ($port in $Ports) {
        $connections = Get-NetTCPConnection -LocalPort $port -State Listen -ErrorAction SilentlyContinue
        foreach ($connection in $connections) {
            if ($connection.OwningProcess -gt 0) {
                $pids += $connection.OwningProcess
            }
        }
    }

    foreach ($pid in ($pids | Select-Object -Unique)) {
        try {
            $process = Get-Process -Id $pid -ErrorAction Stop
            Write-Host "Stopping PID $pid ($($process.ProcessName)) listening on a local-working-system port ..."
            Stop-Process -Id $pid -Force -ErrorAction Stop
        }
        catch {
            Write-Warning "Could not stop PID ${pid}: $($_.Exception.Message)"
        }
    }
}

function Stop-HostDotnetProcesses {
    $dotnet = Get-Process dotnet -ErrorAction SilentlyContinue
    if ($null -eq $dotnet) {
        return
    }

    foreach ($process in $dotnet) {
        Write-Host "Stopping dotnet PID $($process.Id) ..."
        Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
    }
}

$hostPorts = @(
    (Get-DotEnvValue -Path $envFileToUse -Key "KANON_HOST_PORT" -DefaultValue "5081"),
    (Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_HOST_PORT" -DefaultValue "5082"),
    (Get-DotEnvValue -Path $envFileToUse -Key "ALLAGMA_HOST_PORT" -DefaultValue "5083"),
    (Get-DotEnvValue -Path $envFileToUse -Key "METABOLE_HOST_PORT" -DefaultValue "5084"),
    (Get-DotEnvValue -Path $envFileToUse -Key "AISTHESIS_HOST_PORT" -DefaultValue "5085"),
    (Get-DotEnvValue -Path $envFileToUse -Key "FRONTEND_HOST_PORT" -DefaultValue "5175"),
    (Get-DotEnvValue -Path $envFileToUse -Key "POSTGRES_HOST_PORT" -DefaultValue "55433"),
    "5173"
) | ForEach-Object { [int]$_ }

Write-Host "=== restart-local-working-system ==="
Write-Host "Compose root: $composeRoot"

Push-Location $composeRoot
try {
    Write-Host "`n[1/4] Stopping host listeners on stack ports ..."
    Stop-ListenersOnPorts -Ports $hostPorts
    Stop-HostDotnetProcesses

    Write-Host "`n[2/4] docker compose down ..."
    docker compose --env-file $envFileToUse -f $composeFile down --remove-orphans
    if ($LASTEXITCODE -ne 0) {
        throw "docker compose down failed (exit $LASTEXITCODE)."
    }

    Write-Host "`n[3/4] Stopping any remaining dotnet processes ..."
    Stop-HostDotnetProcesses

    Write-Host "`n[4/4] docker compose up -d ..."
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
    if ($SkipFrontend) {
        $composeArgs += @(
            "postgres",
            "kanon-api",
            "conexus-api",
            "aisthesis-api",
            "metabole-api",
            "allagma-api"
        )
    }

    docker @composeArgs
    if ($LASTEXITCODE -ne 0) {
        throw "docker compose up failed (exit $LASTEXITCODE)."
    }

    Write-Host "`nDocker local working system restarted."
    $frontendPort = Get-DotEnvValue -Path $envFileToUse -Key "FRONTEND_HOST_PORT" -DefaultValue "5175"
    if (-not $SkipFrontend) {
        Write-Host "Frontend: http://localhost:$frontendPort"
    }
}
finally {
    Pop-Location
}
