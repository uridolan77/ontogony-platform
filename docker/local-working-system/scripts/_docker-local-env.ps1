# Shared Docker-local compose env helpers for operator scripts.
# Dot-source from scripts in this directory: . "$PSScriptRoot\_docker-local-env.ps1"

function Get-DockerLocalComposeRoot {
    Split-Path -Parent $PSScriptRoot
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
        [string]$AllagmaServiceToken = "",
        [string]$KanonServiceToken = ""
    )

    $envFile = Get-DockerLocalEnvFilePath
    $kanonPort = Get-DotEnvValue -Path $envFile -Key "KANON_HOST_PORT" -DefaultValue "5081"
    $allagmaPort = Get-DotEnvValue -Path $envFile -Key "ALLAGMA_HOST_PORT" -DefaultValue "5083"

    if ([string]::IsNullOrWhiteSpace($AllagmaBaseUrl)) {
        $AllagmaBaseUrl = "http://localhost:$allagmaPort"
    }
    if ([string]::IsNullOrWhiteSpace($KanonBaseUrl)) {
        $KanonBaseUrl = "http://localhost:$kanonPort"
    }
    if ([string]::IsNullOrWhiteSpace($AllagmaServiceToken)) {
        $AllagmaServiceToken = Get-DotEnvValue -Path $envFile -Key "ALLAGMA_SERVICE_TOKEN" -DefaultValue "allagma-dev-service-token-change-in-production"
    }
    if ([string]::IsNullOrWhiteSpace($KanonServiceToken)) {
        $KanonServiceToken = Get-DotEnvValue -Path $envFile -Key "KANON_SERVICE_TOKEN" -DefaultValue "kanon-dev-service-token-change-in-production"
    }

    return [pscustomobject]@{
        EnvFilePath = $envFile
        KanonHostPort = $kanonPort
        AllagmaHostPort = $allagmaPort
        AllagmaBaseUrl = $AllagmaBaseUrl
        KanonBaseUrl = $KanonBaseUrl
        AllagmaServiceToken = $AllagmaServiceToken
        KanonServiceToken = $KanonServiceToken
    }
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
