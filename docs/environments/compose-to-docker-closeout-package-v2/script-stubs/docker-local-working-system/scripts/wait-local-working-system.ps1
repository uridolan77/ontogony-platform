param(
    [int]$TimeoutSeconds = 300,
    [int]$PollIntervalSeconds = 2,
    [switch]$SkipFrontend
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
$envFile = Join-Path $composeRoot ".env"
if (-not (Test-Path -LiteralPath $envFile)) {
    $envFile = Join-Path $composeRoot ".env.example"
}

function Get-EnvValue([string]$Key, [string]$Default) {
    $process = [Environment]::GetEnvironmentVariable($Key)
    if (-not [string]::IsNullOrWhiteSpace($process)) { return $process }
    if (Test-Path -LiteralPath $envFile) {
        $line = Select-String -Path $envFile -Pattern "^$Key=(.+)$" | Select-Object -First 1
        if ($null -ne $line) { return $line.Matches[0].Groups[1].Value.Trim() }
    }
    return $Default
}

function Wait-Http([string]$Name, [string]$Url) {
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        try {
            $r = Invoke-WebRequest -UseBasicParsing -TimeoutSec 5 -Uri $Url
            if ($r.StatusCode -ge 200 -and $r.StatusCode -lt 300) {
                Write-Host "PASS $Name: $Url"
                return
            }
        } catch {
            Start-Sleep -Seconds $PollIntervalSeconds
        }
    }
    throw "$Name did not become healthy at $Url"
}

$kanonPort = Get-EnvValue "KANON_HOST_PORT" "5081"
$conexusPort = Get-EnvValue "CONEXUS_HOST_PORT" "5082"
$allagmaPort = Get-EnvValue "ALLAGMA_HOST_PORT" "5083"
$frontendPort = Get-EnvValue "FRONTEND_HOST_PORT" "5175"

Write-Host "Waiting for Docker-local services..."
Wait-Http "Kanon /health" "http://localhost:$kanonPort/health"
Wait-Http "Conexus /health/live" "http://localhost:$conexusPort/health/live"
Wait-Http "Allagma /health" "http://localhost:$allagmaPort/health"

if (-not $SkipFrontend) {
    Wait-Http "Frontend root" "http://localhost:$frontendPort/"
}

Write-Host "Docker-local service wait PASS."
