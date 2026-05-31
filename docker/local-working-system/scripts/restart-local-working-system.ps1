<#
.SYNOPSIS
  Stops host processes on local-working-system ports, tears down compose, and starts the stack fresh.

.EXAMPLE
  .\scripts\restart-local-working-system.ps1

.EXAMPLE
  .\scripts\restart-local-working-system.ps1 -Build

.EXAMPLE
  .\scripts\restart-local-working-system.ps1 -StartDockerDesktop
    When Docker was stopped (for example by an older script), launch Docker Desktop and wait for the daemon.
#>
param(
    [switch]$Build,
    [switch]$SkipFrontend,
    [switch]$StartDockerDesktop,
    [int]$DockerWaitSeconds = 120
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

# Docker Desktop on Windows owns published ports via com.docker.backend / wslrelay.
# Never stop those processes or Docker will crash.
$script:ProtectedListenerProcessNames = @(
    "com.docker.backend",
    "docker",
    "dockerd",
    "Docker Desktop",
    "wslrelay",
    "wslservice",
    "vmcompute",
    "System",
    "svchost",
    "postgres",
    "nginx"
)

$script:AllowedHostListenerProcessNames = @(
    "dotnet",
    "node",
    "esbuild",
    "vite"
)

function Test-DockerDaemonAvailable {
    $previousPreference = $ErrorActionPreference
    $ErrorActionPreference = "SilentlyContinue"
    try {
        & docker info *> $null
        return ($LASTEXITCODE -eq 0)
    }
    finally {
        $ErrorActionPreference = $previousPreference
    }
}

function Wait-DockerDaemonAvailable {
    param([int]$TimeoutSeconds = 120)

    if (Test-DockerDaemonAvailable) {
        return
    }

    $ensureScript = Join-Path $PSScriptRoot "ensure-docker-desktop.ps1"
    if (-not (Test-Path -LiteralPath $ensureScript)) {
        throw "Docker is not running. Start Docker Desktop manually, then re-run this script."
    }

    & $ensureScript -WaitSeconds $TimeoutSeconds
    if ($LASTEXITCODE -ne 0) {
        throw "Docker daemon did not become ready."
    }
}

function Test-ProcessNameSafeToStop {
    param([string]$ProcessName)

    $normalized = $ProcessName.Trim().ToLowerInvariant()
    foreach ($protectedName in $script:ProtectedListenerProcessNames) {
        if ($normalized -eq $protectedName.ToLowerInvariant()) {
            return $false
        }
    }

    foreach ($allowedName in $script:AllowedHostListenerProcessNames) {
        if ($normalized -eq $allowedName.ToLowerInvariant()) {
            return $true
        }
    }

    return $false
}

function Stop-HostDevListenersOnPorts {
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

    foreach ($processId in ($pids | Select-Object -Unique)) {
        try {
            $process = Get-Process -Id $processId -ErrorAction Stop
            if (-not (Test-ProcessNameSafeToStop -ProcessName $process.ProcessName)) {
                Write-Host "Skipping PID $processId ($($process.ProcessName)) on a stack port (Docker/system listener)."
                continue
            }

            Write-Host "Stopping PID $processId ($($process.ProcessName)) listening on a local-working-system port ..."
            Stop-Process -Id $processId -Force -ErrorAction Stop
        }
        catch {
            Write-Warning "Could not stop PID ${processId}: $($_.Exception.Message)"
        }
    }
}

function Invoke-DockerCompose {
    param([string[]]$ComposeArgs)

    docker @ComposeArgs
    if ($LASTEXITCODE -ne 0) {
        throw "docker $($ComposeArgs -join ' ') failed (exit $LASTEXITCODE)."
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
    Write-Host "`n[1/4] Checking Docker daemon ..."
    if (-not (Test-DockerDaemonAvailable)) {
        if ($StartDockerDesktop) {
            Wait-DockerDaemonAvailable -TimeoutSeconds $DockerWaitSeconds
        }
        else {
            throw @"
Docker is not running (cannot reach //./pipe/dockerDesktopLinuxEngine).

Recovery (pick one):
  1. Start Docker Desktop from the Start menu, wait until it shows Running, then:
       .\scripts\restart-local-working-system.ps1
  2. Or let this script start Docker Desktop and wait:
       .\scripts\restart-local-working-system.ps1 -StartDockerDesktop

If Docker still fails after the old script stopped com.docker.backend, quit Docker Desktop from the tray (Quit), then open it again.
"@
        }
    }

    Write-Host "`n[2/4] docker compose down ..."
    Invoke-DockerCompose -ComposeArgs @(
        "compose",
        "--env-file", $envFileToUse,
        "-f", $composeFile,
        "down",
        "--remove-orphans"
    )

    Write-Host "`n[3/4] Stopping host dev listeners (dotnet/node only; never Docker) ..."
    Stop-HostDevListenersOnPorts -Ports $hostPorts

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

    Invoke-DockerCompose -ComposeArgs $composeArgs

    Write-Host "`nDocker local working system restarted."
    $frontendPort = Get-DotEnvValue -Path $envFileToUse -Key "FRONTEND_HOST_PORT" -DefaultValue "5175"
    if (-not $SkipFrontend) {
        Write-Host "Frontend: http://localhost:$frontendPort"
    }
}
finally {
    Pop-Location
}
