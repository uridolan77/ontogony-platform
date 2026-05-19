param(
    [switch]$Build,
    [switch]$NoWait,
    [switch]$SkipFrontend,
    [switch]$DisableAutoCaInjection
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$composeRoot = Split-Path -Parent $PSScriptRoot
$composeFile = Join-Path $composeRoot "docker-compose.yml"
$defaultEnvFile = Join-Path $composeRoot ".env"
$exampleEnvFile = Join-Path $composeRoot ".env.example"
$waitScript = Join-Path $PSScriptRoot "wait-local-working-system.ps1"
$dotnetSdkImage = "mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim"
$nodeImage = "node:20-bookworm-slim"

function Test-NuGetTlsFromContainer {
    param([string]$Image)

    & docker run --rm $Image bash -lc "curl -fsS https://api.nuget.org/v3/index.json -o /dev/null"
    return ($LASTEXITCODE -eq 0)
}

function Test-NpmRegistryTlsFromContainer {
    param([string]$Image)

    & docker run --rm $Image node -e "fetch('https://registry.npmjs.org').then(()=>process.exit(0)).catch(()=>process.exit(1))"
    return ($LASTEXITCODE -eq 0)
}

function Get-TlsIssuerFromContainer {
    param(
        [string]$Image,
        [string]$HostName
    )

    $issuer = & docker run --rm $Image bash -lc "echo | openssl s_client -connect $HostName`:443 -servername $HostName 2>/dev/null | sed -n 's/^issuer=//p'"
    if ($LASTEXITCODE -ne 0) {
        return $null
    }

    return ($issuer | Select-Object -First 1)
}

function Set-DockerExtraCaFromWindowsTrustStore {
    param([string]$Issuer)

    if ([string]::IsNullOrWhiteSpace($Issuer)) {
        Write-Warning "NuGet issuer could not be detected from container TLS probe."
        return $false
    }

    $issuerCn = $null
    if ($Issuer -match "CN\s*=\s*([^,]+)") {
        $issuerCn = $Matches[1].Trim()
    }

    $candidateCerts = Get-ChildItem Cert:\CurrentUser\Root, Cert:\LocalMachine\Root
    $cert = $candidateCerts | Where-Object {
        $_.Subject -eq $Issuer -or
        ($issuerCn -and $_.Subject -like "*CN=$issuerCn*")
    } | Select-Object -First 1

    if (-not $cert) {
        Write-Warning "Issuer '$Issuer' is not present in Windows root trust stores."
        return $false
    }

    $certBytes = $cert.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Cert)
    $pemBody = [Convert]::ToBase64String($certBytes, [System.Base64FormattingOptions]::InsertLineBreaks)
    $pem = "-----BEGIN CERTIFICATE-----`n$pemBody`n-----END CERTIFICATE-----`n"
    $env:DOCKER_EXTRA_CA_CERT_BASE64 = [Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($pem))

    Write-Host "Detected TLS-intercept issuer '$($cert.Subject)'. Injecting local trusted root CA into Docker build stages."
    return $true
}

if (-not (Test-Path -LiteralPath $composeFile)) {
    throw "Compose file not found: $composeFile"
}

$envFileToUse = $defaultEnvFile
if (-not (Test-Path -LiteralPath $envFileToUse)) {
    if (-not (Test-Path -LiteralPath $exampleEnvFile)) {
        throw "Missing env files. Expected either $defaultEnvFile or $exampleEnvFile."
    }
    $envFileToUse = $exampleEnvFile
    Write-Warning "'.env' was not found. Using '.env.example' development placeholders."
}

$composeArgs = @(
    "compose",
    "--env-file", $envFileToUse,
    "-f", $composeFile,
    "up",
    "-d"
)
if ($Build) {
    if (-not $DisableAutoCaInjection -and [string]::IsNullOrWhiteSpace($env:DOCKER_EXTRA_CA_CERT_BASE64)) {
        $nugetTlsOk = Test-NuGetTlsFromContainer -Image $dotnetSdkImage
        $npmTlsOk = Test-NpmRegistryTlsFromContainer -Image $nodeImage

        if (-not $nugetTlsOk -or -not $npmTlsOk) {
            $issuer = Get-TlsIssuerFromContainer -Image $dotnetSdkImage -Host "api.nuget.org"
            if ([string]::IsNullOrWhiteSpace($issuer)) {
                $issuer = Get-TlsIssuerFromContainer -Image $dotnetSdkImage -Host "registry.npmjs.org"
            }
            $null = Set-DockerExtraCaFromWindowsTrustStore -Issuer $issuer
        }
    }

    $composeArgs += "--build"
}

if ($SkipFrontend) {
    $composeArgs += @("postgres", "kanon-api", "conexus-api", "allagma-api")
}

Write-Host "Starting Docker local working system ..."
docker @composeArgs
if ($LASTEXITCODE -ne 0) {
    throw "docker compose up failed (exit $LASTEXITCODE)."
}

if (-not $NoWait) {
    if ($SkipFrontend) {
        & $waitScript -SkipFrontend
    }
    else {
        & $waitScript
    }
    if ($LASTEXITCODE -ne 0) {
        throw "wait-local-working-system.ps1 failed (exit $LASTEXITCODE)."
    }
}

Write-Host "Docker local working system is up."
