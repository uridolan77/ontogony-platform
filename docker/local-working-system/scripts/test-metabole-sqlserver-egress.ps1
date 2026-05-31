# Tests TCP reachability to SQL Server from the metabole-api container.
# Usage: .\scripts\test-metabole-sqlserver-egress.ps1 [-SqlHost 185.64.56.157] [-SqlPort 1433]
param(
    [string]$SqlHost = "185.64.56.157",
    [int]$SqlPort = 1433
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
Push-Location $root
try {
    $running = docker compose ps metabole-api --status running -q 2>$null
    if (-not $running) {
        Write-Error "metabole-api is not running. Start: docker compose up -d metabole-api"
    }

    Write-Host "Testing TCP $SqlHost`:$SqlPort from metabole-api container..."
    $cmd = "timeout 5 bash -c 'cat < /dev/null > /dev/tcp/$SqlHost/$SqlPort' 2>/dev/null && echo REACHABLE || echo UNREACHABLE"
    $result = docker compose exec -T metabole-api bash -lc $cmd 2>&1
    Write-Host $result

    $ready = Invoke-RestMethod -Uri "http://localhost:5084/ready" -TimeoutSec 10
    Write-Host "sqlServerExtractor: $($ready.sqlServerExtractor)"

    if ($result -match "REACHABLE") {
        Write-Host "OK: Container can open TCP to SQL. If Test connection still fails, check login, database name, and TLS."
        exit 0
    }

    Write-Host "FAIL: Container cannot reach SQL on $SqlHost`:$SqlPort. Check VPN, firewall, and whether SQL allows your host public IP."
    exit 1
}
finally {
    Pop-Location
}
