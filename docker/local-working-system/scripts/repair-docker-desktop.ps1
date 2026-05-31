<#
.SYNOPSIS
  Repairs Docker Desktop when stuck on "Starting the Docker Engine" (broken VM bootstrap).

.DESCRIPTION
  Use after the old restart script killed com.docker.backend/wslrelay, or when logs repeat:
  socketforwarder-receive-fds.sock does not exist yet / init control API never responds.

  -Default: quit Docker, shutdown WSL, start Docker Desktop, wait for daemon.
  -ResetWslDistro: also unregister docker-desktop WSL distro (Docker recreates it; may remove local images/volumes).

.EXAMPLE
  .\scripts\repair-docker-desktop.ps1

.EXAMPLE
  .\scripts\repair-docker-desktop.ps1 -ResetWslDistro
#>
param(
    [switch]$ResetWslDistro,
    [int]$WaitSeconds = 300
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

function Stop-DockerDesktopProcesses {
    $names = @(
        "Docker Desktop",
        "com.docker.backend",
        "com.docker.build",
        "docker-sandbox",
        "docker-agent"
    )
    foreach ($name in $names) {
        Get-Process -Name $name -ErrorAction SilentlyContinue | ForEach-Object {
            Write-Host "Stopping $($_.ProcessName) PID $($_.Id) ..."
            Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
        }
    }
}

Write-Host "=== repair-docker-desktop ===`n"

Write-Host "[1/5] Quit Docker Desktop and stop backend processes ..."
Stop-DockerDesktopProcesses
Start-Sleep -Seconds 3

Write-Host "[2/5] wsl --shutdown ..."
& wsl --shutdown
Start-Sleep -Seconds 10

if ($ResetWslDistro) {
    Write-Host "[3/5] Unregister docker-desktop WSL distro (Docker will recreate on next start) ..."
    Write-Warning "This can remove local Docker images, containers, and volumes."
    & wsl --unregister docker-desktop 2>&1
    Start-Sleep -Seconds 5
}
else {
    Write-Host "[3/5] Skipping WSL distro unregister (pass -ResetWslDistro to recreate VM)."
}

Write-Host "[4/5] Start Docker Desktop ..."
if (-not (Test-Path -LiteralPath $dockerDesktopExe)) {
    throw "Docker Desktop not found at $dockerDesktopExe"
}
Start-Process -FilePath $dockerDesktopExe | Out-Null

Write-Host "[5/5] Waiting up to ${WaitSeconds}s for docker info ..."
$deadline = (Get-Date).AddSeconds($WaitSeconds)
while ((Get-Date) -lt $deadline) {
    if (Test-DockerDaemonReady) {
        Write-Host "`nDocker daemon is ready."
        & docker version 2>&1
        exit 0
    }
    Write-Host "  still starting... ($(Get-Date -Format 'HH:mm:ss'))"
    Start-Sleep -Seconds 10
}

Write-Host @"

Docker did not become ready in ${WaitSeconds}s.

Next steps (manual):
  1. Docker Desktop -> Troubleshoot (bug icon) -> Restart Docker Desktop
  2. If still stuck: Troubleshoot -> Reset to factory defaults
  3. Apply the pending Docker Desktop update, then reboot Windows once
  4. Re-run: .\scripts\repair-docker-desktop.ps1 -ResetWslDistro

Workaround without Docker: run Metabole on the host:
  cd c:\dev\metabole-dotnet\src\Metabole.Api
  `$env:ASPNETCORE_URLS='http://localhost:5084'
  dotnet run
  Point frontend Settings -> Metabole http://localhost:5084
"@
exit 1
