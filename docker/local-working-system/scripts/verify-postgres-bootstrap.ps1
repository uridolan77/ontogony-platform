# ENV-DB-001 — verifies Postgres init SQL creates expected DBs, users, and login grants.
# Spins up a disposable postgres:16 container with postgres/init mounted (no compose required).

param(
    [string]$ContainerName = "ontogony-lws-pg-verify",
    [string]$VolumeName = "ontogony_lws_pg_verify_data",
    [int]$HostPort = 55433,
    [switch]$Recreate,
    [switch]$KeepContainer
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
$initDir = Join-Path $composeRoot "postgres\init"
$sqlFile = Join-Path $initDir "001-create-databases-and-users.sql"

if (-not (Test-Path -LiteralPath $sqlFile)) {
    throw "Missing init SQL: $sqlFile"
}

$databases = @("allagma_local", "kanon_local", "conexus_local")
$serviceUsers = @(
    @{ User = "allagma_local"; Password = "allagma_local_pw"; Database = "allagma_local" },
    @{ User = "kanon_local"; Password = "kanon_local_pw"; Database = "kanon_local" },
    @{ User = "conexus_local"; Password = "conexus_local_pw"; Database = "conexus_local" }
)

function Wait-PostgresReady {
    param([int]$TimeoutSeconds = 120)
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        docker exec $ContainerName pg_isready -U ontogony_admin -d postgres *> $null
        if ($LASTEXITCODE -eq 0) { return }
        Start-Sleep -Seconds 1
    }
    throw "Postgres not ready in container $ContainerName within ${TimeoutSeconds}s."
}

function Wait-TcpPort {
    param(
        [string]$HostName = "127.0.0.1",
        [int]$Port,
        [int]$TimeoutSeconds = 120
    )
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        try {
            $client = New-Object System.Net.Sockets.TcpClient
            $client.Connect($HostName, $Port)
            $client.Close()
            return
        }
        catch {
            Start-Sleep -Milliseconds 500
        }
    }
    throw "TCP port $Port on $HostName not ready within ${TimeoutSeconds}s."
}

function Invoke-AdminPsql {
    param([string]$Query)
    docker exec -e PGPASSWORD=ontogony_admin_pw $ContainerName `
        psql -U ontogony_admin -d postgres -tAc $Query
    if ($LASTEXITCODE -ne 0) { throw "Admin psql failed (exit $LASTEXITCODE)." }
}

function Invoke-ServicePsql {
    param(
        [string]$User,
        [string]$Password,
        [string]$Database,
        [string]$Query
    )
    docker exec -e "PGPASSWORD=$Password" $ContainerName `
        psql -U $User -d $Database -tAc $Query
    if ($LASTEXITCODE -ne 0) { throw "Service psql failed for $User@$Database (exit $LASTEXITCODE)." }
}

if ($Recreate) {
    Write-Host "Recreate: removing container $ContainerName and volume $VolumeName (if present) ..."
    $prevEap = $ErrorActionPreference
    $ErrorActionPreference = "SilentlyContinue"
    docker rm -f $ContainerName *> $null
    docker volume rm $VolumeName *> $null
    $ErrorActionPreference = $prevEap
}

$existing = @(docker ps -a --filter "name=^/${ContainerName}$" --format "{{.Names}}" 2>$null)
if ($existing -contains $ContainerName) {
    Write-Host "Starting existing container $ContainerName ..."
    docker start $ContainerName | Out-Null
}
else {
    Write-Host "Creating Postgres container $ContainerName on host port $HostPort ..."
    $initMount = (Resolve-Path -LiteralPath $initDir).Path
    docker run --name $ContainerName `
        -e POSTGRES_USER=ontogony_admin `
        -e POSTGRES_PASSWORD=ontogony_admin_pw `
        -e POSTGRES_DB=postgres `
        -p "${HostPort}:5432" `
        -v "${VolumeName}:/var/lib/postgresql/data" `
        -v "${initMount}:/docker-entrypoint-initdb.d:ro" `
        -d postgres:16 | Out-Null
    if ($LASTEXITCODE -ne 0) { throw "docker run failed (exit $LASTEXITCODE)." }
}

Wait-TcpPort -Port $HostPort -TimeoutSeconds 120
Wait-PostgresReady -TimeoutSeconds 120
Write-Host "Postgres ready in $ContainerName (host port $HostPort)."

$dbList = (Invoke-AdminPsql -Query "SELECT datname FROM pg_database WHERE datname = ANY (ARRAY['allagma_local','kanon_local','conexus_local']) ORDER BY 1;").Trim() -split "`n" | Where-Object { $_ }
$missingDb = @($databases | Where-Object { $_ -notin $dbList })
if ($missingDb.Count -gt 0) {
    throw "Missing databases: $($missingDb -join ', '). Found: $($dbList -join ', '). Use -Recreate if the data volume predates init SQL."
}

$roleList = (Invoke-AdminPsql -Query "SELECT rolname FROM pg_roles WHERE rolname = ANY (ARRAY['allagma_local','kanon_local','conexus_local']) ORDER BY 1;").Trim() -split "`n" | Where-Object { $_ }
$expectedRoles = @($serviceUsers | ForEach-Object { $_.User })
$missingRole = @($expectedRoles | Where-Object { $_ -notin $roleList })
if ($missingRole.Count -gt 0) {
    throw "Missing roles: $($missingRole -join ', '). Found: $($roleList -join ', ')."
}

foreach ($svc in $serviceUsers) {
    $result = (Invoke-ServicePsql -User $svc.User -Password $svc.Password -Database $svc.Database -Query "SELECT 1;").Trim()
    if ($result -ne "1") {
        throw "Login check failed for $($svc.User) on $($svc.Database) (got '$result')."
    }
    Write-Host "PASS login: $($svc.User) -> $($svc.Database)"
}

Write-Host "ENV-DB-001 Postgres bootstrap verification PASS."
Write-Host "  databases: $($dbList -join ', ')"
Write-Host "  roles: $($roleList -join ', ')"

if (-not $KeepContainer) {
    Write-Host "Stopping verification container $ContainerName (data volume retained: $VolumeName)."
    docker stop $ContainerName | Out-Null
}
