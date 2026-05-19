# RP-003A — Resolve Docker provider_transport_error and prove one live real-provider completion.
# Prerequisite: RP-002 gate, RP-003 guided flow script, local CONEXUS_PROVIDER_OPENAI_API_KEY (never commit).
# Rebuilds conexus-api with runtime CA trust (Avast/corporate TLS interception) then runs Allagma real-provider path.

param(
    [string]$DevRoot = "C:\dev",
    [string]$AllagmaRoot = "",
    [switch]$SkipRebuild,
    [switch]$SkipAllagmaGuidedFlow
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
$composeFile = Join-Path $composeRoot "docker-compose.yml"
$envFile = if (Test-Path (Join-Path $composeRoot ".env")) { Join-Path $composeRoot ".env" } else { Join-Path $composeRoot ".env.example" }
$aspnetImage = "mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim"

if ([string]::IsNullOrWhiteSpace($AllagmaRoot)) {
    $AllagmaRoot = Join-Path $DevRoot "allagma-dotnet"
}

function Set-DockerExtraCaFromOpenAiProbe {
    $issuer = docker run --rm $aspnetImage bash -lc `
        "echo | openssl s_client -connect api.openai.com:443 -servername api.openai.com 2>/dev/null | sed -n 's/^issuer=//p' | head -1"
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($issuer)) {
        throw "Could not read TLS issuer from container probe."
    }

    $verify = docker run --rm $aspnetImage bash -lc `
        "echo | openssl s_client -connect api.openai.com:443 -servername api.openai.com 2>&1 | grep 'Verify return code'"
    if ($verify -match "Verify return code: 0 \(ok\)") {
        Write-Host "[PASS] Container already trusts api.openai.com ($verify)"
        return $false
    }

    Write-Host "[INFO] TLS probe before CA inject: $verify"
    $issuerCn = $null
    if ($issuer -match "CN\s*=\s*([^,]+)") { $issuerCn = $Matches[1].Trim() }
    $cert = Get-ChildItem Cert:\CurrentUser\Root, Cert:\LocalMachine\Root | Where-Object {
        $_.Subject -eq $issuer -or ($issuerCn -and $_.Subject -like "*$issuerCn*")
    } | Select-Object -First 1
    if (-not $cert) { throw "Issuer not found in Windows trust store: $issuer" }

    $pemBody = [Convert]::ToBase64String(
        $cert.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Cert),
        [System.Base64FormattingOptions]::InsertLineBreaks)
    $pem = "-----BEGIN CERTIFICATE-----`n$pemBody`n-----END CERTIFICATE-----`n"
    $env:DOCKER_EXTRA_CA_CERT_BASE64 = [Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($pem))
    Write-Host "[PASS] Injected trusted root for Docker build/runtime: $($cert.Subject)"
    return $true
}

function Test-ConexusLiveOpenAi {
    param([string]$BaseUrl)
    $admin = @{ "X-Conexus-Admin-Key" = "cx-conexus-admin-dev" }
    $put = @{
        aliasName = "gpt-4o-mini"
        providerKey = "openai"
        providerModel = "gpt-4o-mini"
        enabled = $true
        priceCatalogVersion = "dev-2026-05"
    } | ConvertTo-Json
    Invoke-RestMethod -Method Put -Uri "$BaseUrl/admin/v0/aliases/gpt-4o-mini" `
        -Headers $admin -ContentType "application/json" -Body $put | Out-Null

    $headers = @{ Authorization = "Bearer cx-dev-key-change-me" }
    $body = '{"model":"gpt-4o-mini","messages":[{"role":"user","content":"Reply with exactly: ok"}],"max_tokens":16}'
    $r = Invoke-RestMethod -Method Post -Uri "$BaseUrl/v1/chat/completions" `
        -Headers $headers -ContentType "application/json" -Body $body
    if ($r.choices[0].message.content -notmatch "ok") {
        throw "Conexus live probe: expected 'ok' in assistant text."
    }
    Write-Host "[PASS] Conexus live OpenAI completion (modelCallId=$($r.id))"
    return $r.id
}

Write-Host ""
Write-Host "=== RP-003A live real-provider validation ==="
Write-Host "Boundary: local only; not production readiness; no secrets logged."
Write-Host ""

if ([string]::IsNullOrWhiteSpace($env:CONEXUS_PROVIDER_OPENAI_API_KEY)) {
    throw "Set CONEXUS_PROVIDER_OPENAI_API_KEY in the local shell only (never commit)."
}

$report = [ordered]@{
    schema = "ontogony-rp-003a-live-validation-v1"
    generatedAt = (Get-Date).ToUniversalTime().ToString("o")
    rootCause = "Docker runtime image did not trust TLS-intercept issuer (build-stage CA only); fixed in conexus-dotnet Dockerfile runtime stage."
    verdict = "PASS"
}

$injected = Set-DockerExtraCaFromOpenAiProbe
$report.caInjectionRequired = $injected

if (-not $SkipRebuild) {
    Push-Location $composeRoot
    try {
        Write-Host "Rebuilding conexus-api (build + runtime CA args) ..."
        docker compose --env-file $envFile build conexus-api
        if ($LASTEXITCODE -ne 0) { throw "docker compose build conexus-api failed" }
    }
    finally { Pop-Location }
}

$key = $env:CONEXUS_PROVIDER_OPENAI_API_KEY
$env:CONEXUS_REAL_PROVIDER_ENABLED = "true"
$env:CONEXUS_REAL_PROVIDER_NAME = "openai"
$env:CONEXUS_REAL_PROVIDER_MODEL = "gpt-4o-mini"
$env:CONEXUS_PROVIDER_OPENAI_API_KEY = $key

Push-Location $composeRoot
try {
    docker compose --env-file $envFile up -d conexus-api
    if ($LASTEXITCODE -ne 0) { throw "docker compose up conexus-api failed" }
}
finally { Pop-Location }

Start-Sleep -Seconds 10
$report.conexusLiveProbeModelCallId = Test-ConexusLiveOpenAi -BaseUrl "http://localhost:5082"

if (-not $SkipAllagmaGuidedFlow) {
    $guided = Join-Path $AllagmaRoot "scripts\env\run-guided-main-flow-real-provider.ps1"
    if (-not (Test-Path -LiteralPath $guided)) { throw "Missing $guided" }
    & $guided -AttemptRealProviderCall -UseDockerConexusRestart
    if ($LASTEXITCODE -ne 0) { throw "run-guided-main-flow-real-provider.ps1 failed" }
    $report.allagmaGuidedFlow = "PASS"
}

$reportPath = Join-Path $composeRoot "artifacts\rp-003a\live-provider-validation-report.json"
$reportDir = Split-Path -Parent $reportPath
if (-not (Test-Path -LiteralPath $reportDir)) { New-Item -ItemType Directory -Path $reportDir -Force | Out-Null }
$report | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $reportPath -Encoding UTF8
Write-Host ""
Write-Host "Report: $reportPath"
Write-Host "RP-003A complete."
