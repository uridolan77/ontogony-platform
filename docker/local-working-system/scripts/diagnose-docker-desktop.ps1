<#
.SYNOPSIS
  Prints why Docker Desktop is stuck on "Starting the Docker Engine" (read-only checks).

.EXAMPLE
  .\scripts\diagnose-docker-desktop.ps1
#>
$ErrorActionPreference = "Continue"

Write-Host "=== diagnose-docker-desktop ===`n"

Write-Host "[1] docker CLI"
$prev = $ErrorActionPreference
$ErrorActionPreference = "SilentlyContinue"
& docker version 2>&1
Write-Host "exit=$LASTEXITCODE`n"
$ErrorActionPreference = $prev

Write-Host "[2] WSL"
wsl --status 2>&1
wsl -l -v 2>&1
Write-Host ""

Write-Host "[3] Docker-related processes"
Get-Process | Where-Object { $_.ProcessName -match 'docker|wsl|vmmem' } |
    Select-Object Id, ProcessName, @{ N = 'MemMB'; E = { [math]::Round($_.WorkingSet64 / 1MB, 1) } } |
    Format-Table -AutoSize

$logRoot = Join-Path $env:LOCALAPPDATA "Docker\log"
$backendLog = Join-Path $logRoot "host\com.docker.backend.exe.log"
$monitorLog = Join-Path $logRoot "host\monitor.log"

Write-Host "[4] Recent engine bootstrap errors (com.docker.backend.exe.log)"
if (Test-Path $backendLog) {
    Select-String -Path $backendLog -Pattern 'socketforwarder-receive-fds|still waiting for init control API|HTTP 500|enginedependencies' |
        Select-Object -Last 8 |
        ForEach-Object { $_.Line }
}
else {
    Write-Host "  (log not found: $backendLog)"
}

Write-Host "`n[5] Interpretation"
Write-Host @"
  - UI stuck on 'Starting the Docker Engine' + docker version HTTP 500 usually means:
    the Linux VM started but the engine init API never came up.
  - If logs repeat 'socketforwarder-receive-fds.sock: does not exist yet' and
    'still waiting for init control API to respond', the VM bootstrap is broken.
  - This often happens after force-stopping com.docker.backend or wslrelay
    (e.g. an old restart script that killed Docker port listeners).

  Recovery (in order):
    1. Quit Docker Desktop (tray -> Quit).
    2. wsl --shutdown
    3. Start Docker Desktop; wait 3-5 minutes.
    4. If still stuck: Docker Desktop -> Troubleshoot -> Restart Docker Desktop.
    5. Last resort: Troubleshoot -> Reset to factory defaults (removes local images/volumes).
    6. Apply any pending Docker Desktop update after the engine is healthy.

  When 'docker info' works, run:
    .\scripts\restart-local-working-system.ps1
"@
