# CONEXUS-PERSIST-002 — validate Conexus Docker-local persistence, migrations, and dev bootstrap state.
# Emits JSON report under docker/local-working-system/artifacts/.

param(
    [string]$OutputPath = "",
    [switch]$InvokeBootstrap,
    [switch]$SkipBootstrap,
    [switch]$UseExistingReports = $true,
    [switch]$RequireRouteEvidence,
    [int]$HttpTimeoutSeconds = 10
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
$composeFile = Join-Path $composeRoot "docker-compose.yml"
$defaultEnvFile = Join-Path $composeRoot ".env"
$exampleEnvFile = Join-Path $composeRoot ".env.example"
$envFileToUse = if (Test-Path -LiteralPath $defaultEnvFile) { $defaultEnvFile } else { $exampleEnvFile }

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $composeRoot "artifacts\conexus-persist-002-report.json"
}
$outputDir = Split-Path -Parent $OutputPath
if (-not (Test-Path -LiteralPath $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

$checks = [System.Collections.Generic.List[object]]::new()
$failures = [System.Collections.Generic.List[string]]::new()
$warnings = [System.Collections.Generic.List[string]]::new()

function Add-Check {
    param(
        [string]$Id,
        [string]$Status,
        [string]$Detail,
        [object]$Data = $null
    )
    $entry = [ordered]@{
        id = $Id
        status = $Status
        detail = $Detail
    }
    if ($null -ne $Data) { $entry.data = $Data }
    $checks.Add([pscustomobject]$entry) | Out-Null
    if ($Status -eq "FAIL") { $failures.Add("$Id`: $Detail") | Out-Null }
    if ($Status -eq "WARN") { $warnings.Add("$Id`: $Detail") | Out-Null }
}

function Get-DotEnvValue {
    param(
        [string]$Path,
        [string]$Key,
        [string]$DefaultValue
    )
    $processValue = [Environment]::GetEnvironmentVariable($Key)
    if (-not [string]::IsNullOrWhiteSpace($processValue)) { return $processValue }
    if (-not (Test-Path -LiteralPath $Path)) { return $DefaultValue }
    $line = Select-String -Path $Path -Pattern "^$Key=(.+)$" | Select-Object -First 1
    if ($null -eq $line) { return $DefaultValue }
    return $line.Matches[0].Groups[1].Value.Trim()
}

function Redact-SecretValue {
    param([string]$Value)
    if ([string]::IsNullOrWhiteSpace($Value)) { return "" }
    if ($Value.Length -le 6) { return "***" }
    return "$($Value.Substring(0, 3))***$($Value.Substring($Value.Length - 3))"
}

function Redact-ConnectionString {
    param([string]$Value)
    if ([string]::IsNullOrWhiteSpace($Value)) { return "" }
    return ($Value -replace "Password=[^;]+", "Password=***")
}

function Get-ConnectionStringPart {
    param(
        [string]$ConnectionString,
        [string]$Key,
        [string]$DefaultValue
    )
    if ($ConnectionString -match "${Key}=([^;]+)") {
        return $Matches[1].Trim()
    }
    return $DefaultValue
}

function Get-ComposeContainerId {
    param([string]$Service)
    $rawOutput = docker compose --env-file $envFileToUse -f $composeFile ps -q $Service 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "docker compose ps failed for '$Service' (exit $LASTEXITCODE): $rawOutput"
    }
    $containerId = if ($null -eq $rawOutput) { "" } else { ($rawOutput | Out-String).Trim() }
    if ([string]::IsNullOrWhiteSpace($containerId)) {
        throw "No running container found for service '$Service'."
    }
    return $containerId
}

function Invoke-HttpStatus {
    param([string]$Url)
    try {
        $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec $HttpTimeoutSeconds
        return [ordered]@{ statusCode = [int]$response.StatusCode; error = $null }
    }
    catch {
        if ($_.Exception.Response -and $_.Exception.Response.StatusCode) {
            return [ordered]@{ statusCode = [int]$_.Exception.Response.StatusCode; error = $null }
        }
        return [ordered]@{ statusCode = $null; error = $_.Exception.Message }
    }
}

function Invoke-AdminPsql {
    param([string]$Query)
    $postgresId = Get-ComposeContainerId -Service "postgres"
    $result = docker exec -e "PGPASSWORD=$script:PostgresAdminPassword" $postgresId `
        psql -U $script:PostgresAdminUser -d $script:PostgresAdminDatabase -tAc $Query 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "Admin psql failed (exit $LASTEXITCODE): $result"
    }
    return ($result | Out-String).Trim()
}

$script:PostgresAdminUser = Get-DotEnvValue -Path $envFileToUse -Key "POSTGRES_USER" -DefaultValue "ontogony_admin"
$script:PostgresAdminPassword = Get-DotEnvValue -Path $envFileToUse -Key "POSTGRES_PASSWORD" -DefaultValue "ontogony_admin_pw"
$script:PostgresAdminDatabase = Get-DotEnvValue -Path $envFileToUse -Key "POSTGRES_DB" -DefaultValue "postgres"

Write-Host "=== CONEXUS-PERSIST-002 Conexus persistence/bootstrap validation ==="
Write-Host "Boundary: post-Docker-local hardening; not production readiness."
Write-Host "Output: $OutputPath"
Write-Host ""

# 1) Stack reachable
try {
    $conexusContainerId = Get-ComposeContainerId -Service "conexus-api"
    $postgresContainerId = Get-ComposeContainerId -Service "postgres"
    Add-Check -Id "stack.reachable" -Status "PASS" -Detail "conexus-api and postgres containers are running." -Data @{
        conexusContainerId = $conexusContainerId
        postgresContainerId = $postgresContainerId
    }
}
catch {
    Add-Check -Id "stack.reachable" -Status "FAIL" -Detail $_.Exception.Message
}

$conexusPort = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_HOST_PORT" -DefaultValue "5082"
$conexusBaseUrl = "http://localhost:$conexusPort"
$conexusAdminKey = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_ADMIN_API_KEY" -DefaultValue "cx-conexus-admin-dev"
$conexusDevProjectKey = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_DEV_PROJECT_API_KEY" -DefaultValue "cx-dev-key-change-me"
$conexusAllagmaKey = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_PROJECT_API_KEY_FOR_ALLAGMA" -DefaultValue "cx-dev-key-change-me"
$conexusPostgresConn = Get-DotEnvValue -Path $envFileToUse -Key "CONEXUS_POSTGRES_CONNECTION_STRING" -DefaultValue "Host=postgres;Port=5432;Database=conexus_local;Username=conexus_local;Password=conexus_local_pw"
$conexusDbUser = Get-ConnectionStringPart -ConnectionString $conexusPostgresConn -Key "Username" -DefaultValue "conexus_local"
$conexusDbPassword = Get-ConnectionStringPart -ConnectionString $conexusPostgresConn -Key "Password" -DefaultValue "conexus_local_pw"
$keysAligned = ($conexusDevProjectKey -eq $conexusAllagmaKey)

# 2) /health/live
$live = Invoke-HttpStatus -Url "$conexusBaseUrl/health/live"
if ($live.statusCode -ge 200 -and $live.statusCode -lt 300) {
    Add-Check -Id "conexus.health.live" -Status "PASS" -Detail "GET /health/live returned $($live.statusCode)." -Data $live
}
elseif ($null -ne $live.error) {
    Add-Check -Id "conexus.health.live" -Status "FAIL" -Detail $live.error -Data $live
}
else {
    Add-Check -Id "conexus.health.live" -Status "FAIL" -Detail "GET /health/live returned $($live.statusCode); expected 2xx." -Data $live
}

# 3) /ready captured (not required to pass)
$ready = Invoke-HttpStatus -Url "$conexusBaseUrl/ready"
$readyStatus = if ($ready.statusCode -ge 200 -and $ready.statusCode -lt 300) { "PASS" } else { "INFO" }
$readyDetail = if ($ready.statusCode -eq 503) {
    "GET /ready returned 503 (expected before bootstrap or when strict invariants fail)."
}
elseif ($ready.statusCode -ge 200 -and $ready.statusCode -lt 300) {
    "GET /ready returned $($ready.statusCode)."
}
elseif ($null -ne $ready.error) {
    "GET /ready failed: $($ready.error)"
}
else {
    "GET /ready returned $($ready.statusCode)."
}
Add-Check -Id "conexus.ready.captured" -Status $readyStatus -Detail $readyDetail -Data $ready

# 4) Postgres database present
try {
    $dbExists = Invoke-AdminPsql -Query "SELECT 1 FROM pg_database WHERE datname = 'conexus_local';"
    if ($dbExists -eq "1") {
        Add-Check -Id "postgres.conexus_local" -Status "PASS" -Detail "Database conexus_local exists."
    }
    else {
        Add-Check -Id "postgres.conexus_local" -Status "FAIL" -Detail "Database conexus_local not found."
    }
}
catch {
    Add-Check -Id "postgres.conexus_local" -Status "FAIL" -Detail $_.Exception.Message
}

# 5) Migrations applied (EF history + core routing table)
try {
    $postgresId = Get-ComposeContainerId -Service "postgres"
    $migrationSql = 'SELECT COUNT(*) FROM public."__EFMigrationsHistory";'
    $migrationCount = ($migrationSql | docker exec -i -e "PGPASSWORD=$conexusDbPassword" $postgresId `
        psql -U $conexusDbUser -d conexus_local -tA 2>&1 | Out-String).Trim()
    $aliasTable = (docker exec -e "PGPASSWORD=$conexusDbPassword" $postgresId `
        psql -U $conexusDbUser -d conexus_local -tAc "SELECT 1 FROM information_schema.tables WHERE table_schema = 'public' AND table_name = 'conexus_model_alias';" 2>&1 | Out-String).Trim()

    if ([int]$migrationCount -gt 0 -and $aliasTable -eq "1") {
        Add-Check -Id "postgres.migrations" -Status "PASS" -Detail "EF migrations applied ($migrationCount rows in __EFMigrationsHistory; conexus_model_alias present)." -Data @{
            migrationRowCount = [int]$migrationCount
        }
    }
    elseif ($aliasTable -eq "1") {
        Add-Check -Id "postgres.migrations" -Status "PASS" -Detail "conexus_model_alias present (migration history count=$migrationCount)." -Data @{
            migrationRowCount = [int]$migrationCount
        }
    }
    else {
        Add-Check -Id "postgres.migrations" -Status "FAIL" -Detail "Expected applied migrations and conexus_model_alias table."
    }
}
catch {
    Add-Check -Id "postgres.migrations" -Status "FAIL" -Detail $_.Exception.Message
}

# 6) Compose render includes Conexus Postgres connection
try {
    $rendered = docker compose --env-file $envFileToUse -f $composeFile config 2>&1 | Out-String
    if ($LASTEXITCODE -ne 0) {
        throw "docker compose config failed (exit $LASTEXITCODE)."
    }
    $hasConexusConn = $rendered -match "ConnectionStrings__ConexusPostgres" -or $rendered -match "CONEXUS_POSTGRES_CONNECTION_STRING"
    $hasAllagmaProjectKey = $rendered -match "Conexus__ProjectApiKey" -or $rendered -match "CONEXUS_PROJECT_API_KEY_FOR_ALLAGMA"
    if ($hasConexusConn -and $hasAllagmaProjectKey) {
        Add-Check -Id "compose.render" -Status "PASS" -Detail "Rendered compose includes Conexus Postgres connection and Allagma Conexus__ProjectApiKey." -Data @{
            connectionStringConfigured = $true
            allagmaProjectKeyConfigured = $true
        }
    }
    else {
        Add-Check -Id "compose.render" -Status "FAIL" -Detail "Rendered compose missing Conexus Postgres and/or Allagma project key wiring."
    }
}
catch {
    Add-Check -Id "compose.render" -Status "FAIL" -Detail $_.Exception.Message
}

# 7) Dev key alignment
if ($keysAligned) {
    Add-Check -Id "keys.aligned" -Status "PASS" -Detail "CONEXUS_DEV_PROJECT_API_KEY matches CONEXUS_PROJECT_API_KEY_FOR_ALLAGMA." -Data @{
        devProjectApiKeyConfigured = -not [string]::IsNullOrWhiteSpace($conexusDevProjectKey)
        allagmaProjectApiKeyConfigured = -not [string]::IsNullOrWhiteSpace($conexusAllagmaKey)
        keysAligned = $true
    }
}
else {
    Add-Check -Id "keys.aligned" -Status "FAIL" -Detail "Key mismatch: CONEXUS_DEV_PROJECT_API_KEY and CONEXUS_PROJECT_API_KEY_FOR_ALLAGMA must match unless both are intentionally overridden." -Data @{
        devProjectApiKeyConfigured = -not [string]::IsNullOrWhiteSpace($conexusDevProjectKey)
        allagmaProjectApiKeyConfigured = -not [string]::IsNullOrWhiteSpace($conexusAllagmaKey)
        keysAligned = $false
    }
}

# 8) Bootstrap state (detect or invoke)
$adminHeaders = @{ "X-Conexus-Admin-Key" = $conexusAdminKey }
$bootstrapInvoked = $false
$bootstrapResponse = $null
$fakeProviderPresent = $false
$aliasPresent = $false

try {
    $providers = @(Invoke-RestMethod -Uri "$conexusBaseUrl/admin/v0/providers" -Headers $adminHeaders -TimeoutSec $HttpTimeoutSeconds)
    $aliases = @(Invoke-RestMethod -Uri "$conexusBaseUrl/admin/v0/aliases" -Headers $adminHeaders -TimeoutSec $HttpTimeoutSeconds)

    $fakeProviderPresent = $providers | Where-Object { $_.providerKey -eq "fake" -and $_.enabled -eq $true }
    $aliasPresent = $aliases | Where-Object { $_.alias -eq "gpt-4o-mini" -and $_.enabled -eq $true }

    if ($fakeProviderPresent -and $aliasPresent) {
        Add-Check -Id "bootstrap.state" -Status "PASS" -Detail "Fake provider and gpt-4o-mini alias already present (bootstrap not required)." -Data @{
            bootstrapInvoked = $false
            fakeProviderEnabled = $true
            modelAliasEnabled = $true
        }
    }
    elseif ($SkipBootstrap) {
        Add-Check -Id "bootstrap.state" -Status "FAIL" -Detail "Bootstrap state missing and -SkipBootstrap was set." -Data @{
            fakeProviderEnabled = [bool]$fakeProviderPresent
            modelAliasEnabled = [bool]$aliasPresent
        }
    }
    elseif ($InvokeBootstrap -or (-not $fakeProviderPresent) -or (-not $aliasPresent)) {
        $bootstrapBody = @{
            projectId = "dev-project"
            displayName = "Development Project"
            modelAlias = "gpt-4o-mini"
            providerKey = "fake"
            providerModel = "fake.chat"
            createProjectKey = $true
        } | ConvertTo-Json

        $bootstrapResponse = Invoke-RestMethod `
            -Method Post `
            -Uri "$conexusBaseUrl/admin/v0/dev/bootstrap" `
            -Headers $adminHeaders `
            -ContentType "application/json" `
            -Body $bootstrapBody `
            -TimeoutSec $HttpTimeoutSeconds
        $bootstrapInvoked = $true

        $providers = @(Invoke-RestMethod -Uri "$conexusBaseUrl/admin/v0/providers" -Headers $adminHeaders -TimeoutSec $HttpTimeoutSeconds)
        $aliases = @(Invoke-RestMethod -Uri "$conexusBaseUrl/admin/v0/aliases" -Headers $adminHeaders -TimeoutSec $HttpTimeoutSeconds)
        $fakeProviderPresent = $providers | Where-Object { $_.providerKey -eq "fake" -and $_.enabled -eq $true }
        $aliasPresent = $aliases | Where-Object { $_.alias -eq "gpt-4o-mini" -and $_.enabled -eq $true }

        if ($fakeProviderPresent -and $aliasPresent) {
            Add-Check -Id "bootstrap.state" -Status "PASS" -Detail "Dev bootstrap invoked or verified; fake provider and gpt-4o-mini alias present." -Data @{
                bootstrapInvoked = $true
                projectId = $bootstrapResponse.projectId
                alias = $bootstrapResponse.alias
                providerKey = $bootstrapResponse.providerKey
                warnings = @($bootstrapResponse.warnings)
            }
        }
        else {
            Add-Check -Id "bootstrap.state" -Status "FAIL" -Detail "Bootstrap completed but fake provider or alias still missing."
        }
    }
    else {
        Add-Check -Id "bootstrap.state" -Status "FAIL" -Detail "Bootstrap state missing; re-run with -InvokeBootstrap."
    }
}
catch {
    Add-Check -Id "bootstrap.state" -Status "FAIL" -Detail $_.Exception.Message
}

# Re-capture /ready after bootstrap attempt (informational)
$readyAfter = Invoke-HttpStatus -Url "$conexusBaseUrl/ready"
Add-Check -Id "conexus.ready.afterBootstrap" -Status "INFO" -Detail "GET /ready after bootstrap check: HTTP $($readyAfter.statusCode)." -Data $readyAfter

# 9) Route/model evidence from existing reports
$guidedReportPath = Join-Path $composeRoot "artifacts\docker-guided-main-flow-report.json"
$seedReportPath = Join-Path $composeRoot "artifacts\env-seed-001-report.json"
$routeEvidence = [ordered]@{
    source = $null
    baselineRouteDecisionId = $null
    subjectRouteDecisionId = $null
}

if ($UseExistingReports) {
    if (Test-Path -LiteralPath $guidedReportPath) {
        $guided = Get-Content -Raw -LiteralPath $guidedReportPath | ConvertFrom-Json
        $routeEvidence.source = "docker-guided-main-flow-report.json"
        $routeEvidence.baselineRouteDecisionId = $guided.baselineRouteDecisionId
        $routeEvidence.subjectRouteDecisionId = $guided.subjectRouteDecisionId
    }
    elseif (Test-Path -LiteralPath $seedReportPath) {
        $seed = Get-Content -Raw -LiteralPath $seedReportPath | ConvertFrom-Json
        $routeEvidence.source = "env-seed-001-report.json"

        if ($seed.PSObject.Properties.Name -contains "routeEvidence") {
            $routeEvidence.baselineRouteDecisionId = $seed.routeEvidence.baselineRouteDecisionId
            $routeEvidence.subjectRouteDecisionId = $seed.routeEvidence.subjectRouteDecisionId
        }
        elseif ($seed.PSObject.Properties.Name -contains "runs") {
            if ($seed.runs.PSObject.Properties.Name -contains "baseline") {
                $routeEvidence.baselineRouteDecisionId = $seed.runs.baseline.routeDecisionId
            }
            if ($seed.runs.PSObject.Properties.Name -contains "subject") {
                $routeEvidence.subjectRouteDecisionId = $seed.runs.subject.routeDecisionId
            }
        }
    }
}

$hasRouteIds = -not [string]::IsNullOrWhiteSpace([string]$routeEvidence.baselineRouteDecisionId) `
    -and -not [string]::IsNullOrWhiteSpace([string]$routeEvidence.subjectRouteDecisionId)

if ($hasRouteIds) {
    Add-Check -Id "route.evidence" -Status "PASS" -Detail "Route decision IDs present in $($routeEvidence.source)." -Data $routeEvidence
}
elseif ($RequireRouteEvidence) {
    Add-Check -Id "route.evidence" -Status "FAIL" -Detail "Route evidence required but not found in artifacts. Run seed-and-verify or run-docker-guided-main-flow first." -Data $routeEvidence
}
else {
    Add-Check -Id "route.evidence" -Status "WARN" -Detail "No route evidence in artifacts; run seed or guided flow for full route/model proof." -Data $routeEvidence
}

$verdict = if ($failures.Count -eq 0) { "PASS" } else { "FAIL" }

$report = [ordered]@{
    schema = "conexus-persist-002-report-v1"
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    verdict = $verdict
    boundary = "post-Docker-local hardening; not production readiness"
    services = [ordered]@{
        conexusBaseUrl = $conexusBaseUrl
        envFile = $envFileToUse
    }
    configuration = [ordered]@{
        conexusPostgresConnectionStringConfigured = -not [string]::IsNullOrWhiteSpace($conexusPostgresConn)
        conexusPostgresConnectionStringRedacted = Redact-ConnectionString $conexusPostgresConn
        conexusDevProjectApiKeyConfigured = -not [string]::IsNullOrWhiteSpace($conexusDevProjectKey)
        conexusProjectApiKeyForAllagmaConfigured = -not [string]::IsNullOrWhiteSpace($conexusAllagmaKey)
        keysAligned = $keysAligned
    }
    readiness = [ordered]@{
        beforeBootstrap = $ready
        afterBootstrapCheck = $readyAfter
    }
    bootstrap = [ordered]@{
        invoked = $bootstrapInvoked
        response = if ($null -eq $bootstrapResponse) {
            $null
        }
        else {
            [ordered]@{
                projectId = $bootstrapResponse.projectId
                alias = $bootstrapResponse.alias
                providerKey = $bootstrapResponse.providerKey
                warnings = @($bootstrapResponse.warnings)
                apiKeyIssued = -not [string]::IsNullOrWhiteSpace([string]$bootstrapResponse.apiKey)
            }
        }
    }
    checks = @($checks)
    warnings = @($warnings)
    failures = @($failures)
    safety = [ordered]@{
        realProviderKeys = "no"
        realExternalExecution = "disabled"
        productionReadiness = "not_claimed"
    }
}

$report | ConvertTo-Json -Depth 12 | Set-Content -LiteralPath $OutputPath -Encoding UTF8
Write-Host ""
Write-Host "Wrote report: $OutputPath"
Write-Host "Verdict: $verdict"

if ($warnings.Count -gt 0) {
    Write-Host ""
    Write-Host "Warnings:"
    foreach ($w in $warnings) { Write-Host "  - $w" }
}

if ($failures.Count -gt 0) {
    Write-Host ""
    Write-Host "Failures:"
    foreach ($f in $failures) { Write-Host "  - $f" }
    throw "CONEXUS-PERSIST-002 validation FAIL ($($failures.Count) check(s))."
}

Write-Host ""
Write-Host "CONEXUS-PERSIST-002 validation PASS."
