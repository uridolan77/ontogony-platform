# Shared Docker-local compose env helpers for operator scripts.
# Dot-source from scripts in this directory: . "$PSScriptRoot\_docker-local-env.ps1"

function Get-DockerLocalComposeRoot {
    Split-Path -Parent $PSScriptRoot
}

function Get-FrontendRepoRoot {
    $composeRoot = Get-DockerLocalComposeRoot
    $candidate = Join-Path $composeRoot "..\..\..\ontogony-frontend"
    return (Resolve-Path -LiteralPath $candidate).Path
}

function Get-FrontendExpectedGitHead {
    param([string]$FrontendRepoRoot = "")

    if ([string]::IsNullOrWhiteSpace($FrontendRepoRoot)) {
        $FrontendRepoRoot = Get-FrontendRepoRoot
    }
    if (-not (Test-Path -LiteralPath $FrontendRepoRoot)) {
        throw "ontogony-frontend repo not found at: $FrontendRepoRoot"
    }

    $sha = & git -C $FrontendRepoRoot rev-parse HEAD 2>$null
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($sha)) {
        throw "git rev-parse HEAD failed in ontogony-frontend ($FrontendRepoRoot)."
    }
    return $sha.Trim()
}

function Get-FrontendPackageVersion {
    param([string]$FrontendRepoRoot = "")

    if ([string]::IsNullOrWhiteSpace($FrontendRepoRoot)) {
        $FrontendRepoRoot = Get-FrontendRepoRoot
    }
    $pkgPath = Join-Path $FrontendRepoRoot "package.json"
    if (-not (Test-Path -LiteralPath $pkgPath)) {
        return ""
    }
    $pkg = Get-Content -Raw -LiteralPath $pkgPath | ConvertFrom-Json
    return [string]$pkg.version
}

function Set-FrontendDockerBuildProvenanceEnv {
    param([string]$FrontendRepoRoot = "")

    if ([string]::IsNullOrWhiteSpace($FrontendRepoRoot)) {
        $FrontendRepoRoot = Get-FrontendRepoRoot
    }

    $gitSha = Get-FrontendExpectedGitHead -FrontendRepoRoot $FrontendRepoRoot
    $version = Get-FrontendPackageVersion -FrontendRepoRoot $FrontendRepoRoot
    if ([string]::IsNullOrWhiteSpace($version)) {
        $version = "0.0.0"
    }

    $env:FRONTEND_VITE_GIT_SHA = $gitSha
    $env:FRONTEND_VITE_APP_VERSION = $version
    $env:FRONTEND_VITE_BUILD_TIME = (Get-Date).ToUniversalTime().ToString("o")

    return [pscustomobject]@{
        FrontendRepoRoot = $FrontendRepoRoot
        GitSha = $gitSha
        AppVersion = $version
        BuildTime = $env:FRONTEND_VITE_BUILD_TIME
    }
}

function Ensure-DockerLocalBuildCaInjection {
    param([switch]$DisableAutoCaInjection)

    if ($DisableAutoCaInjection) { return }
    if (-not [string]::IsNullOrWhiteSpace($env:DOCKER_EXTRA_CA_CERT_BASE64)) { return }

    $dotnetSdkImage = "mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim"
    $nodeImage = "node:20-bookworm-slim"

    & docker run --rm $dotnetSdkImage bash -lc "curl -fsS https://api.nuget.org/v3/index.json -o /dev/null" | Out-Null
    $nugetTlsOk = $LASTEXITCODE -eq 0
    & docker run --rm $nodeImage node -e "fetch('https://registry.npmjs.org').then(()=>process.exit(0)).catch(()=>process.exit(1))" | Out-Null
    $npmTlsOk = $LASTEXITCODE -eq 0

    if ($nugetTlsOk -and $npmTlsOk) { return }

    $issuer = & docker run --rm $dotnetSdkImage bash -lc "echo | openssl s_client -connect api.nuget.org:443 -servername api.nuget.org 2>/dev/null | sed -n 's/^issuer=//p'"
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($issuer)) {
        $issuer = & docker run --rm $dotnetSdkImage bash -lc "echo | openssl s_client -connect registry.npmjs.org:443 -servername registry.npmjs.org 2>/dev/null | sed -n 's/^issuer=//p'"
    }
    $issuer = ($issuer | Select-Object -First 1).Trim()
    if ([string]::IsNullOrWhiteSpace($issuer)) {
        Write-Warning "TLS-intercept issuer could not be detected; Docker frontend build may fail on npm ci."
        return
    }

    $issuerCn = $null
    if ($issuer -match "CN\s*=\s*([^,]+)") { $issuerCn = $Matches[1].Trim() }
    $cert = Get-ChildItem Cert:\CurrentUser\Root, Cert:\LocalMachine\Root | Where-Object {
        $_.Subject -eq $issuer -or ($issuerCn -and $_.Subject -like "*CN=$issuerCn*")
    } | Select-Object -First 1
    if (-not $cert) {
        Write-Warning "Issuer '$issuer' is not in Windows trust stores; Docker frontend build may fail."
        return
    }

    $certBytes = $cert.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Cert)
    $pemBody = [Convert]::ToBase64String($certBytes, [System.Base64FormattingOptions]::InsertLineBreaks)
    $pem = "-----BEGIN CERTIFICATE-----`n$pemBody`n-----END CERTIFICATE-----`n"
    $env:DOCKER_EXTRA_CA_CERT_BASE64 = [Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($pem))
    Write-Host "Detected TLS-intercept issuer '$($cert.Subject)'. Injecting trusted root CA into Docker build stages."
}

function Invoke-FrontendDockerImageBuild {
    param(
        [switch]$NoCache,
        [switch]$DisableAutoCaInjection
    )

    Ensure-DockerLocalBuildCaInjection -DisableAutoCaInjection:$DisableAutoCaInjection

    $composeRoot = Get-DockerLocalComposeRoot
    $composeFile = Join-Path $composeRoot "docker-compose.yml"
    $envFile = Get-DockerLocalEnvFilePath
    $provenance = Set-FrontendDockerBuildProvenanceEnv

    $shortSha = $provenance.GitSha.Substring(0, [Math]::Min(7, $provenance.GitSha.Length))
    Write-Host "Building ontogony-frontend image (git $shortSha, v$($provenance.AppVersion)) ..."

    $buildArgs = @(
        "compose",
        "--env-file", $envFile,
        "-f", $composeFile,
        "build",
        "ontogony-frontend",
        "--build-arg", "VITE_GIT_SHA=$($provenance.GitSha)",
        "--build-arg", "VITE_BUILD_TIME=$($provenance.BuildTime)",
        "--build-arg", "VITE_APP_VERSION=$($provenance.AppVersion)"
    )
    if (-not [string]::IsNullOrWhiteSpace($env:DOCKER_EXTRA_CA_CERT_BASE64)) {
        $buildArgs += @("--build-arg", "EXTRA_CA_CERT_BASE64=$($env:DOCKER_EXTRA_CA_CERT_BASE64)")
    }
    if ($NoCache) {
        $buildArgs += "--no-cache"
    }

    docker @buildArgs | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "docker compose build ontogony-frontend failed (exit $LASTEXITCODE)."
    }

    return $provenance
}

function Test-DockerLocalPostgresRunning {
    $composeRoot = Get-DockerLocalComposeRoot
    $composeFile = Join-Path $composeRoot "docker-compose.yml"
    $envFile = Get-DockerLocalEnvFilePath
    $id = docker compose --env-file $envFile -f $composeFile ps -q postgres 2>$null
    return ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace(($id | Out-String).Trim()))
}

function Wait-FrontendBrowserHealthy {
    param(
        [int]$TimeoutSeconds = 90,
        [int]$PollIntervalSeconds = 2,
        [string]$FrontendBaseUrl = ""
    )

    $baseUrl = Get-FrontendBrowserBaseUrl -FrontendBaseUrl $FrontendBaseUrl
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        try {
            $response = Invoke-WebRequest -Uri "$baseUrl/" -UseBasicParsing -TimeoutSec 5
            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 300) {
                Write-Host "PASS ontogony-frontend healthy: $baseUrl/"
                return
            }
        }
        catch {
            # retry
        }
        Start-Sleep -Seconds $PollIntervalSeconds
    }

    throw "ontogony-frontend did not become healthy within ${TimeoutSeconds}s at $baseUrl/"
}

function Get-FrontendBrowserBaseUrl {
    param([string]$FrontendBaseUrl = "")

    if (-not [string]::IsNullOrWhiteSpace($FrontendBaseUrl)) {
        return $FrontendBaseUrl.TrimEnd("/")
    }

    $envFile = Get-DockerLocalEnvFilePath
    $port = Get-DotEnvValue -Path $envFile -Key "FRONTEND_HOST_PORT" -DefaultValue "5175"
    return "http://localhost:$port"
}

function Get-HtmlMetaContent {
    param(
        [string]$Html,
        [string]$Name
    )
    if ([string]::IsNullOrWhiteSpace($Html)) { return $null }
    $pattern = "name=`"$([regex]::Escape($Name))`"\s+content=`"([^`"]+)`""
    if ($Html -match $pattern) {
        return $Matches[1]
    }
    return $null
}

function Get-FrontendModuleScriptHref {
    param([string]$Html)
    if ([string]::IsNullOrWhiteSpace($Html)) { return $null }
    if ($Html -match '<script[^>]+type="module"[^>]+src="([^"]+)"') {
        return $Matches[1]
    }
    if ($Html -match '<script[^>]+src="([^"]+)"[^>]+type="module"') {
        return $Matches[1]
    }
    return $null
}

function Normalize-GitShaForCompare {
    param([string]$Value)
    if ([string]::IsNullOrWhiteSpace($Value)) { return "" }
    return $Value.Trim().ToLowerInvariant()
}

function Get-DockerLocalEnvFilePath {
    $composeRoot = Get-DockerLocalComposeRoot
    $defaultEnvFile = Join-Path $composeRoot ".env"
    $exampleEnvFile = Join-Path $composeRoot ".env.example"
    if (Test-Path -LiteralPath $defaultEnvFile) { return $defaultEnvFile }
    return $exampleEnvFile
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
    $line = Select-String -Path $Path -Pattern "^$([regex]::Escape($Key))=(.+)$" | Select-Object -First 1
    if ($null -eq $line) { return $DefaultValue }
    return $line.Matches[0].Groups[1].Value.Trim()
}

function Get-DockerLocalComposeConfig {
    param(
        [string]$AllagmaBaseUrl = "",
        [string]$KanonBaseUrl = "",
        [string]$ConexusBaseUrl = "",
        [string]$AllagmaServiceToken = "",
        [string]$KanonServiceToken = "",
        [string]$ConexusAdminApiKey = ""
    )

    $envFile = Get-DockerLocalEnvFilePath
    $kanonPort = Get-DotEnvValue -Path $envFile -Key "KANON_HOST_PORT" -DefaultValue "5081"
    $allagmaPort = Get-DotEnvValue -Path $envFile -Key "ALLAGMA_HOST_PORT" -DefaultValue "5083"
    $conexusPort = Get-DotEnvValue -Path $envFile -Key "CONEXUS_HOST_PORT" -DefaultValue "5082"

    if ([string]::IsNullOrWhiteSpace($AllagmaBaseUrl)) {
        $AllagmaBaseUrl = "http://localhost:$allagmaPort"
    }
    if ([string]::IsNullOrWhiteSpace($KanonBaseUrl)) {
        $KanonBaseUrl = "http://localhost:$kanonPort"
    }
    if ([string]::IsNullOrWhiteSpace($ConexusBaseUrl)) {
        $ConexusBaseUrl = "http://localhost:$conexusPort"
    }
    if ([string]::IsNullOrWhiteSpace($AllagmaServiceToken)) {
        $AllagmaServiceToken = Get-DotEnvValue -Path $envFile -Key "ALLAGMA_SERVICE_TOKEN" -DefaultValue "allagma-dev-service-token-change-in-production"
    }
    if ([string]::IsNullOrWhiteSpace($KanonServiceToken)) {
        $KanonServiceToken = Get-DotEnvValue -Path $envFile -Key "KANON_SERVICE_TOKEN" -DefaultValue "kanon-dev-service-token-change-in-production"
    }
    if ([string]::IsNullOrWhiteSpace($ConexusAdminApiKey)) {
        $ConexusAdminApiKey = Get-DotEnvValue -Path $envFile -Key "CONEXUS_ADMIN_API_KEY" -DefaultValue "cx-conexus-admin-dev"
    }

    return [pscustomobject]@{
        EnvFilePath = $envFile
        KanonHostPort = $kanonPort
        AllagmaHostPort = $allagmaPort
        ConexusHostPort = $conexusPort
        AllagmaBaseUrl = $AllagmaBaseUrl
        KanonBaseUrl = $KanonBaseUrl
        ConexusBaseUrl = $ConexusBaseUrl
        AllagmaServiceToken = $AllagmaServiceToken
        KanonServiceToken = $KanonServiceToken
        ConexusAdminApiKey = $ConexusAdminApiKey
    }
}

function Get-RunEventPayloadObject {
    param($Payload)
    if ($null -eq $Payload) { return $null }
    if ($Payload -is [System.Collections.IDictionary]) { return $Payload }
    if ($Payload -is [System.Collections.IEnumerable] -and $Payload -isnot [string]) {
        $items = @($Payload)
        if ($items.Count -lt 1) { return $null }
        return $items[-1]
    }
    return $Payload
}

function Get-ResponseHeaderValue {
    param(
        [Microsoft.PowerShell.Commands.WebResponseObject]$Response,
        [string]$HeaderName
    )
    if ($null -eq $Response -or $null -eq $Response.Headers) { return $null }
    foreach ($key in $Response.Headers.Keys) {
        if ($key -ieq $HeaderName) {
            $values = $Response.Headers[$key]
            if ($values -is [string]) { return $values.Trim() }
            if ($null -ne $values -and $values.Count -gt 0) { return [string]$values[0] }
        }
    }
    return $null
}

function Get-DockerLocalSecretPatterns {
    param([object]$ComposeConfig)

    $envFile = $ComposeConfig.EnvFilePath
    $patterns = [System.Collections.Generic.List[string]]::new()
    $staticPatterns = @(
        "cx-dev-key-change-me",
        "cx-conexus-admin-dev",
        "allagma_local_pw",
        "kanon_local_pw",
        "conexus_local_pw",
        "ontogony_admin_pw"
    )
    foreach ($p in $staticPatterns) { $patterns.Add($p) | Out-Null }

    foreach ($key in @(
            "ALLAGMA_SERVICE_TOKEN",
            "KANON_SERVICE_TOKEN",
            "CONEXUS_DEV_PROJECT_API_KEY",
            "CONEXUS_ADMIN_API_KEY",
            "CONEXUS_PROJECT_API_KEY_FOR_ALLAGMA",
            "POSTGRES_PASSWORD"
        )) {
        $value = Get-DotEnvValue -Path $envFile -Key $key -DefaultValue ""
        if (-not [string]::IsNullOrWhiteSpace($value)) {
            $patterns.Add($value) | Out-Null
        }
    }

    if (-not [string]::IsNullOrWhiteSpace($ComposeConfig.AllagmaServiceToken)) {
        $patterns.Add($ComposeConfig.AllagmaServiceToken) | Out-Null
    }
    if (-not [string]::IsNullOrWhiteSpace($ComposeConfig.KanonServiceToken)) {
        $patterns.Add($ComposeConfig.KanonServiceToken) | Out-Null
    }

    return $patterns | Select-Object -Unique
}

function Redact-StringWithPatterns {
    param(
        [string]$Value,
        [string[]]$SecretPatterns
    )
    if ([string]::IsNullOrWhiteSpace($Value)) { return $null }
    $redacted = $Value
    foreach ($pattern in $SecretPatterns) {
        if (-not [string]::IsNullOrWhiteSpace($pattern)) {
            $redacted = $redacted.Replace($pattern, "***")
        }
    }
    return $redacted
}

function Redact-ObjectWithPatterns {
    param(
        [object]$InputObject,
        [string[]]$SecretPatterns
    )
    if ($null -eq $InputObject) { return $null }
    $json = $InputObject | ConvertTo-Json -Depth 12 -Compress
    foreach ($pattern in $SecretPatterns) {
        if (-not [string]::IsNullOrWhiteSpace($pattern)) {
            $json = $json.Replace($pattern, "***")
        }
    }
    return ($json | ConvertFrom-Json)
}

function Assert-ReportHasNoSecretPatterns {
    param(
        [string]$Json,
        [string[]]$SecretPatterns
    )
    foreach ($pattern in $SecretPatterns) {
        if ([string]::IsNullOrWhiteSpace($pattern)) { continue }
        if ($json -match [regex]::Escape($pattern)) {
            throw "Report still contains raw secret pattern: $pattern"
        }
    }
}

function Get-OptionalProperty {
    param(
        [object]$Object,
        [string]$Name
    )
    if ($null -eq $Object) { return $null }
    $prop = $Object.PSObject.Properties[$Name]
    if ($null -eq $prop) { return $null }
    return $prop.Value
}

function Read-DockerLocalRunReportIds {
    param(
        [string]$GuidedPath,
        [string]$SeedPath
    )

    $ids = [ordered]@{
        source = $null
        baselineRunId = $null
        subjectRunId = $null
        subjectTopologyAuthorizationDecisionId = $null
    }

    if (Test-Path -LiteralPath $GuidedPath) {
        $guided = Get-Content -Raw -LiteralPath $GuidedPath | ConvertFrom-Json
        $ids.source = "docker-guided-main-flow-report.json"
        $ids.baselineRunId = [string]$guided.baselineRunId
        $ids.subjectRunId = [string]$guided.subjectRunId
        $ids.subjectTopologyAuthorizationDecisionId = [string]$guided.subjectTopologyAuthorizationDecisionId
    }
    elseif (Test-Path -LiteralPath $SeedPath) {
        $seed = Get-Content -Raw -LiteralPath $SeedPath | ConvertFrom-Json
        $ids.source = "env-seed-001-report.json"
        $ids.baselineRunId = [string]$seed.runs.baselineRunId
        $ids.subjectRunId = [string]$seed.runs.subjectRunId
        $ids.subjectTopologyAuthorizationDecisionId = [string]$seed.topology.subjectTopologyAuthorizationDecisionId
    }

    return [pscustomobject]$ids
}
