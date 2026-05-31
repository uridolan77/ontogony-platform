<#
.SYNOPSIS
  Starts Docker Desktop (if needed) and waits until the Docker daemon accepts commands.

.EXAMPLE
  .\scripts\ensure-docker-desktop.ps1

.EXAMPLE
  .\scripts\ensure-docker-desktop.ps1 -WaitSeconds 180
#>
param(
    [int]$WaitSeconds = 120
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$dockerDesktopExe = "C:\Program Files\Docker\Docker\Docker Desktop.exe"

function Test-DockerDaemonReady {
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

function Start-DockerDesktopApplication {
    if (-not (Test-Path -LiteralPath $dockerDesktopExe)) {
        throw "Docker Desktop not found at: $dockerDesktopExe. Install or repair Docker Desktop, then re-run."
    }

    $running = Get-Process -Name "Docker Desktop", "com.docker.backend" -ErrorAction SilentlyContinue
    if ($running) {
        Write-Host "Docker Desktop process is already running; waiting for daemon ..."
        return
    }

    Write-Host "Starting Docker Desktop ..."
    Start-Process -FilePath $dockerDesktopExe | Out-Null
}

Write-Host "=== ensure-docker-desktop ==="

if (Test-DockerDaemonReady) {
    Write-Host "Docker daemon is already ready."
    exit 0
}

Start-DockerDesktopApplication

$deadline = (Get-Date).AddSeconds($WaitSeconds)
while ((Get-Date) -lt $deadline) {
    if (Test-DockerDaemonReady) {
        Write-Host "Docker daemon is ready."
        exit 0
    }

    Write-Host "Waiting for Docker daemon ..."
    Start-Sleep -Seconds 5
}

throw "Docker daemon did not become ready within ${WaitSeconds}s. Open Docker Desktop from the Start menu, wait until it shows Running, then re-run."
