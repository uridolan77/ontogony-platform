# DOCKER-LOCAL-VERIFY-001 - rebuild (optional), browser provenance probe, and report validation.
# -Build rebuilds only ontogony-frontend when the stack is already up.

param(
    [switch]$Build,
    [switch]$SkipStart,
    [switch]$NoWait,
    [switch]$DisableAutoCaInjection,
    [string]$FrontendBaseUrl = "",
    [string]$ReportPath = "",
    [string]$ExpectedGitSha = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

$inspectScript = Join-Path $PSScriptRoot "inspect-frontend-browser-provenance.ps1"
$validateScript = Join-Path $PSScriptRoot "validate-frontend-browser-provenance-report.ps1"
$startScript = Join-Path $PSScriptRoot "start-local-working-system.ps1"

if ($Build) {
    $composeRoot = Get-DockerLocalComposeRoot
    $composeFile = Join-Path $composeRoot "docker-compose.yml"
    $envFile = Get-DockerLocalEnvFilePath

    Write-Host "DOCKER-LOCAL-VERIFY-001: frontend-only rebuild (git HEAD -> image -> container -> probe) ..."
    $provenance = Set-FrontendDockerBuildProvenanceEnv
    Write-Host "Expected browser commit: $($provenance.GitSha.Substring(0, [Math]::Min(7, $provenance.GitSha.Length))) ($($provenance.GitSha))"

    if (-not (Test-DockerLocalPostgresRunning)) {
        Write-Host "Postgres not running - starting backend stack once (skip frontend image build) ..."
        $startParams = @{ SkipFrontend = $true }
        if ($DisableAutoCaInjection) { $startParams.DisableAutoCaInjection = $true }
        & $startScript @startParams
        if ($LASTEXITCODE -ne 0) {
            throw "start-local-working-system.ps1 -SkipFrontend failed (exit $LASTEXITCODE)."
        }
    }

    $builtProvenance = Invoke-FrontendDockerImageBuild -DisableAutoCaInjection:$DisableAutoCaInjection
    $ExpectedGitSha = $builtProvenance.GitSha

    $headNow = Get-FrontendExpectedGitHead
    if ($headNow -ne $builtProvenance.GitSha) {
        throw "Repo HEAD changed during build ($headNow vs $($builtProvenance.GitSha)). Commit or stash, then re-run verify -Build."
    }

    Write-Host "Recreating ontogony-frontend container only (backends untouched) ..."
    docker compose --env-file $envFile -f $composeFile up -d --no-deps --force-recreate ontogony-frontend
    if ($LASTEXITCODE -ne 0) {
        throw "docker compose up --force-recreate ontogony-frontend failed (exit $LASTEXITCODE)."
    }

    if (-not $NoWait) {
        Wait-FrontendBrowserHealthy -FrontendBaseUrl $FrontendBaseUrl
    }
}
elseif (-not $SkipStart) {
    Write-Host "DOCKER-LOCAL-VERIFY-001: ensuring stack is up (no rebuild) ..."
    & $startScript
    if ($LASTEXITCODE -ne 0) {
        throw "start-local-working-system.ps1 failed (exit $LASTEXITCODE)."
    }
}

$inspectArgs = @{}
if (-not [string]::IsNullOrWhiteSpace($FrontendBaseUrl)) {
    $inspectArgs.FrontendBaseUrl = $FrontendBaseUrl
}
if (-not [string]::IsNullOrWhiteSpace($ReportPath)) {
    $inspectArgs.OutputPath = $ReportPath
}
if ($Build -and -not [string]::IsNullOrWhiteSpace($ExpectedGitSha)) {
    $inspectArgs.ExpectedGitSha = $ExpectedGitSha
}
elseif (-not [string]::IsNullOrWhiteSpace($ExpectedGitSha)) {
    $inspectArgs.ExpectedGitSha = $ExpectedGitSha
}

& $inspectScript @inspectArgs
if ($LASTEXITCODE -ne 0) {
    throw "inspect-frontend-browser-provenance.ps1 failed (exit $LASTEXITCODE)."
}

$validateArgs = @{}
if (-not [string]::IsNullOrWhiteSpace($ReportPath)) {
    $validateArgs.ReportPath = $ReportPath
}

& $validateScript @validateArgs
if ($LASTEXITCODE -ne 0) {
    throw "validate-frontend-browser-provenance-report.ps1 failed (exit $LASTEXITCODE)."
}

Write-Host "DOCKER-LOCAL-VERIFY-001 verify PASS (rebuild=$Build)."
