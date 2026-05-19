# DOCKER-LOCAL-VERIFY-001 — probe served SPA provenance vs ontogony-frontend git HEAD.

param(
    [string]$OutputPath = "",
    [string]$FrontendBaseUrl = "",
    [string]$ExpectedGitSha = "",
    [int]$HttpTimeoutSeconds = 30
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

. "$PSScriptRoot\_docker-local-env.ps1"

$composeRoot = Get-DockerLocalComposeRoot
$frontendRepoRoot = Get-FrontendRepoRoot
$baseUrl = Get-FrontendBrowserBaseUrl -FrontendBaseUrl $FrontendBaseUrl

if ([string]::IsNullOrWhiteSpace($ExpectedGitSha)) {
    $ExpectedGitSha = Get-FrontendExpectedGitHead -FrontendRepoRoot $frontendRepoRoot
}

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $composeRoot "artifacts\docker-local-verify-001-report.json"
}

$outputDir = Split-Path -Parent $OutputPath
if (-not (Test-Path -LiteralPath $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

$expectedNorm = Normalize-GitShaForCompare -Value $ExpectedGitSha
$issues = [System.Collections.Generic.List[string]]::new()

function Add-Issue([string]$Message) {
    $issues.Add($Message) | Out-Null
}

function Invoke-FrontendHttpGet {
    param([string]$Path)
    $uri = if ($Path.StartsWith("http")) { $Path } else { "$baseUrl$Path" }
    return Invoke-WebRequest -Uri $uri -UseBasicParsing -TimeoutSec $HttpTimeoutSeconds
}

$provenance = $null
$provenanceFetchStatus = "missing"
try {
    $provenanceResponse = Invoke-FrontendHttpGet -Path "/provenance.json"
    if ($provenanceResponse.StatusCode -ge 200 -and $provenanceResponse.StatusCode -lt 300) {
        $provenance = $provenanceResponse.Content | ConvertFrom-Json
        $provenanceFetchStatus = "ok"
    }
}
catch {
    $provenanceFetchStatus = "error"
    Add-Issue "GET /provenance.json failed: $($_.Exception.Message)"
}

$indexHtml = $null
$indexFetchStatus = "missing"
try {
    $indexResponse = Invoke-FrontendHttpGet -Path "/"
    if ($indexResponse.StatusCode -ge 200 -and $indexResponse.StatusCode -lt 300) {
        $indexHtml = $indexResponse.Content
        $indexFetchStatus = "ok"
    }
}
catch {
    $indexFetchStatus = "error"
    Add-Issue "GET / (index.html) failed: $($_.Exception.Message)"
}

$metaGitSha = Get-HtmlMetaContent -Html $indexHtml -Name "ontogony:git-sha"
$metaBuildTime = Get-HtmlMetaContent -Html $indexHtml -Name "ontogony:build-time"
$metaAppVersion = Get-HtmlMetaContent -Html $indexHtml -Name "ontogony:app-version"

$bundleHref = Get-FrontendModuleScriptHref -Html $indexHtml
$bundleGitShaPresent = $false
$bundleFetchStatus = "skipped"
$bundleHrefResolved = $null
if (-not [string]::IsNullOrWhiteSpace($bundleHref)) {
    $bundleHrefResolved = if ($bundleHref.StartsWith("http")) { $bundleHref } else { "$baseUrl$bundleHref" }
    try {
        $bundleResponse = Invoke-WebRequest -Uri $bundleHrefResolved -UseBasicParsing -TimeoutSec $HttpTimeoutSeconds
        $bundleFetchStatus = "ok"
        $servedSha = if ($provenance) { [string]$provenance.gitSha } else { $metaGitSha }
        if (-not [string]::IsNullOrWhiteSpace($servedSha) -and $bundleResponse.Content -like "*$servedSha*") {
            $bundleGitShaPresent = $true
        }
        elseif (-not [string]::IsNullOrWhiteSpace($servedSha) -and $servedSha.Length -ge 7 -and $bundleResponse.Content -like "*$($servedSha.Substring(0, 7))*") {
            $bundleGitShaPresent = $true
        }
    }
    catch {
        $bundleFetchStatus = "error"
        Add-Issue "GET bundle $bundleHref failed: $($_.Exception.Message)"
    }
}

$provenanceGitSha = if ($provenance) { [string]$provenance.gitSha } else { "" }
$provenanceNorm = Normalize-GitShaForCompare -Value $provenanceGitSha
$metaNorm = Normalize-GitShaForCompare -Value $metaGitSha

$imageLabelGitSha = $null
$containerId = $null
$containerState = "not_found"
try {
    $envFile = Get-DockerLocalEnvFilePath
    $composeFile = Join-Path $composeRoot "docker-compose.yml"
    $containerId = & docker compose --env-file $envFile -f $composeFile ps -q ontogony-frontend 2>$null
    if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($containerId)) {
        $containerState = (& docker inspect --format '{{.State.Status}}' $containerId 2>$null).Trim()
        $imageName = (& docker inspect --format '{{.Image}}' $containerId 2>$null).Trim()
        if (-not [string]::IsNullOrWhiteSpace($imageName)) {
            $imageLabelGitSha = (& docker image inspect --format '{{ index .Config.Labels "org.ontogony.frontend.git-sha" }}' $imageName 2>$null).Trim()
        }
    }
}
catch {
    Add-Issue "docker inspect ontogony-frontend failed: $($_.Exception.Message)"
}

$matchesExpectedRepo = ($expectedNorm -eq $provenanceNorm) -and ($expectedNorm -eq $metaNorm)
$imageMatchesExpected = $false
if (-not [string]::IsNullOrWhiteSpace($imageLabelGitSha)) {
    $imageMatchesExpected = (Normalize-GitShaForCompare -Value $imageLabelGitSha) -eq $expectedNorm
}

if ($provenanceFetchStatus -ne "ok") {
    Add-Issue "provenance.json is not available from the running frontend container."
}
elseif ($provenanceNorm -ne $expectedNorm) {
    Add-Issue "provenance.json gitSha '$provenanceGitSha' does not match expected repo HEAD '$ExpectedGitSha' (stale image likely)."
}

if ($indexFetchStatus -eq "ok" -and [string]::IsNullOrWhiteSpace($metaGitSha)) {
    Add-Issue "index.html is missing meta name=ontogony:git-sha (stale SPA bundle likely)."
}
elseif ($indexFetchStatus -eq "ok" -and $metaNorm -ne $expectedNorm) {
    Add-Issue "index.html meta ontogony:git-sha '$metaGitSha' does not match expected repo HEAD '$ExpectedGitSha'."
}

if ($provenanceFetchStatus -eq "ok" -and $indexFetchStatus -eq "ok" -and $provenanceNorm -ne $metaNorm) {
    Add-Issue "provenance.json gitSha and index.html meta ontogony:git-sha disagree (partial or corrupted build)."
}

if ($bundleFetchStatus -eq "ok" -and -not $bundleGitShaPresent -and $provenanceNorm -ne "local") {
    Add-Issue "main JS bundle does not contain served git SHA (stale hashed asset likely)."
}

if (-not [string]::IsNullOrWhiteSpace($imageLabelGitSha) -and -not $imageMatchesExpected) {
    Add-Issue "Docker image label org.ontogony.frontend.git-sha '$imageLabelGitSha' does not match expected '$ExpectedGitSha'."
}

$verdict = if ($issues.Count -eq 0) { "PASS" } else { "FAIL" }

$report = [ordered]@{
    schema = "docker-local-verify-001-report-v1"
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    verdict = $verdict
    boundary = "Docker-local frontend freshness only; not production readiness"
    expected = [ordered]@{
        frontendRepoRoot = $frontendRepoRoot
        gitSha = $ExpectedGitSha
        gitShaShort = if ($ExpectedGitSha.Length -ge 7) { $ExpectedGitSha.Substring(0, 7) } else { $ExpectedGitSha }
    }
    browser = [ordered]@{
        baseUrl = $baseUrl
        provenanceFetchStatus = $provenanceFetchStatus
        indexFetchStatus = $indexFetchStatus
        bundleFetchStatus = $bundleFetchStatus
        bundleHref = $bundleHref
        bundleGitShaPresent = $bundleGitShaPresent
    }
    served = [ordered]@{
        provenanceGitSha = $provenanceGitSha
        provenanceBuildTime = if ($provenance) { [string]$provenance.buildTime } else { $null }
        provenanceAppVersion = if ($provenance) { [string]$provenance.version } else { $null }
        provenanceOntogonyUiRef = if ($provenance) { [string]$provenance.ontogonyUiRef } else { $null }
        indexMetaGitSha = $metaGitSha
        indexMetaBuildTime = $metaBuildTime
        indexMetaAppVersion = $metaAppVersion
    }
    docker = [ordered]@{
        containerId = $containerId
        containerState = $containerState
        imageLabelGitSha = $imageLabelGitSha
        imageMatchesExpected = $imageMatchesExpected
    }
    checks = [ordered]@{
        matchesExpectedRepo = $matchesExpectedRepo
        provenanceMatchesMeta = ($provenanceNorm -eq $metaNorm)
        bundleContainsGitSha = $bundleGitShaPresent
    }
    issues = @($issues)
    references = [ordered]@{
        rcqVerify = "ontogony-platform/docs/reviews/RCQ_VERIFY_001_LATEST_FIXES_REVIEW.md"
        startScript = "docker/local-working-system/scripts/start-local-working-system.ps1"
    }
}

$json = $report | ConvertTo-Json -Depth 10
$json | Set-Content -LiteralPath $OutputPath -Encoding UTF8
Write-Host "Wrote frontend browser provenance report: $OutputPath"

if ($verdict -ne "PASS") {
    foreach ($issue in $issues) {
        Write-Warning $issue
    }
    throw "DOCKER-LOCAL-VERIFY-001 inspect FAIL ($($issues.Count) issue(s))."
}

Write-Host "DOCKER-LOCAL-VERIFY-001 inspect PASS (frontend $($ExpectedGitSha.Substring(0, [Math]::Min(7, $ExpectedGitSha.Length))) at $baseUrl)."
