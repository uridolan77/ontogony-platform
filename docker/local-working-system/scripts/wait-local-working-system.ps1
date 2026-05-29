param(
    [int]$TimeoutSeconds = 300,
    [int]$PollIntervalSeconds = 2,
    [switch]$SkipFrontend
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

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

function Get-ComposeContainerId {
    param([string]$Service)

    $rawOutput = docker compose --env-file $envFileToUse -f $composeFile ps -q $Service
    $containerId = if ($null -eq $rawOutput) { "" } else { ($rawOutput | Out-String).Trim() }
    if ([string]::IsNullOrWhiteSpace($containerId)) {
        throw "No running container found for service '$Service'."
    }
    return $containerId
}

function Wait-PostgresHealthy {
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        try {
            $containerId = Get-ComposeContainerId -Service "postgres"
            $status = (docker inspect --format "{{.State.Health.Status}}" $containerId).Trim()
            if ($status -eq "healthy") {
                Write-Host "PASS postgres healthy."
                return
            }
            Write-Host "Waiting postgres health ... current=$status"
        }
        catch {
            Write-Host "Waiting postgres container ..."
        }
        Start-Sleep -Seconds $PollIntervalSeconds
    }

    throw "Postgres did not become healthy within ${TimeoutSeconds}s."
}

function Wait-HttpHealthy {
    param(
        [string]$Name,
        [string]$Url,
        [string]$ServiceName
    )

    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        if (-not [string]::IsNullOrWhiteSpace($ServiceName)) {
            try {
                $containerId = Get-ComposeContainerId -Service $ServiceName
                $containerState = (docker inspect --format "{{.State.Status}}" $containerId).Trim()
                if ($containerState -ne "running") {
                    throw "$Name container is not running (state=$containerState)."
                }
            }
            catch {
                $reason = if ($null -ne $_.Exception -and -not [string]::IsNullOrWhiteSpace($_.Exception.Message)) {
                    $_.Exception.Message
                }
                else {
                    $_.ToString()
                }
                throw "$Name container is not running or missing. $reason"
            }
        }

        try {
            $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 5
            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 300) {
                Write-Host "PASS $Name healthy: $Url"
                return
            }
        }
        catch {
            # retry
        }
        Start-Sleep -Seconds $PollIntervalSeconds
    }

    throw "$Name did not become healthy within ${TimeoutSeconds}s at $Url."
}

function Get-DotEnvValue {
    param(
        [string]$Path,
        [string]$Key,
        [string]$DefaultValue
    )

    $processValue = [Environment]::GetEnvironmentVariable($Key)
    if (-not [string]::IsNullOrWhiteSpace($processValue)) {
        return $processValue
    }

    if (-not (Test-Path -LiteralPath $Path)) {
        return $DefaultValue
    }

    $line = Select-String -Path $Path -Pattern "^$Key=(.+)$" | Select-Object -First 1
    if ($null -eq $line) {
        return $DefaultValue
    }

    return $line.Matches[0].Groups[1].Value.Trim()
}

$kanonPort = Get-DotEnvValue -Path $envFileToUse -Key "KANON_HOST_PORT" -DefaultValue "5081"
$conexusPort = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_HOST_PORT" -DefaultValue "5082"
$allagmaPort = Get-DotEnvValue -Path $envFileToUse -Key "ALLAGMA_HOST_PORT" -DefaultValue "5083"
$aisthesisPort = Get-DotEnvValue -Path $envFileToUse -Key "AISTHESIS_HOST_PORT" -DefaultValue "5085"
$metabolePort = Get-DotEnvValue -Path $envFileToUse -Key "METABOLE_HOST_PORT" -DefaultValue "5084"
$frontendPort = Get-DotEnvValue -Path $envFileToUse -Key "FRONTEND_HOST_PORT" -DefaultValue "5175"

Write-Host "Waiting for Docker local working system health ..."
Wait-PostgresHealthy
Wait-HttpHealthy -Name "kanon-api" -Url "http://localhost:$kanonPort/health" -ServiceName "kanon-api"
Wait-HttpHealthy -Name "conexus-api" -Url "http://localhost:$conexusPort/health/live" -ServiceName "conexus-api"
Wait-HttpHealthy -Name "aisthesis-api" -Url "http://localhost:$aisthesisPort/health" -ServiceName "aisthesis-api"
Wait-HttpHealthy -Name "metabole-api" -Url "http://localhost:$metabolePort/health" -ServiceName "metabole-api"
Wait-HttpHealthy -Name "allagma-api" -Url "http://localhost:$allagmaPort/health" -ServiceName "allagma-api"

if (-not $SkipFrontend) {
    Wait-HttpHealthy -Name "ontogony-frontend" -Url "http://localhost:$frontendPort/" -ServiceName "ontogony-frontend"
}

Write-Host "All requested services are healthy."
