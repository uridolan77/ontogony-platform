# Shared helpers for Wave 7 - canonical local operator system scripts.

function Get-LocalOntogonyPaths {
    param(
        [string]$RepoRoot = "",
        [string]$DevRoot = ""
    )

    if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
        $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
    }

    if ([string]::IsNullOrWhiteSpace($DevRoot)) {
        $DevRoot = (Resolve-Path (Join-Path $RepoRoot "..")).Path
    }

    $composeRoot = Join-Path $RepoRoot "docker\local-working-system"
    $dockerScripts = Join-Path $composeRoot "scripts"
    $artifactsDir = Join-Path $composeRoot "artifacts"

    return [ordered]@{
        RepoRoot = $RepoRoot
        DevRoot = $DevRoot
        ComposeRoot = $composeRoot
        DockerScripts = $dockerScripts
        ArtifactsDir = $artifactsDir
        FrontendRoot = Join-Path $DevRoot "ontogony-frontend"
        GuidedReportPath = Join-Path $artifactsDir "docker-guided-main-flow-report.json"
        SeedReportPath = Join-Path $artifactsDir "env-seed-001-report.json"
        DemoIdsPath = Join-Path $artifactsDir "operator-v1-demo-ids.json"
        ValidationReportPath = Join-Path $artifactsDir "local-ontogony-system-validation-report.json"
    }
}

function Test-SixRepoWorkspaceLayout {
    param(
        [string]$DevRoot,
        [switch]$Quiet
    )

    $requiredRepos = @(
        "ontogony-platform",
        "allagma-dotnet",
        "kanon-dotnet",
        "conexus-dotnet",
        "ontogony-frontend",
        "ontogony-ui"
    )

    foreach ($repo in $requiredRepos) {
        $path = Join-Path $DevRoot $repo
        if (-not (Test-Path -LiteralPath $path)) {
            throw "Missing required repo: $path"
        }
        if (-not $Quiet) {
            Write-Host "  OK repo: $path"
        }
    }
}

function Get-LocalOntogonyFrontendBaseUrl {
    param([hashtable]$Paths)

    $envHelpers = Join-Path $Paths.DockerScripts "_docker-local-env.ps1"
    if (Test-Path -LiteralPath $envHelpers) {
        . $envHelpers
        $config = Get-DockerLocalComposeConfig
        $port = Get-DotEnvValue -Path $config.EnvFilePath -Key "FRONTEND_HOST_PORT" -DefaultValue "5175"
        return "http://localhost:$port"
    }

    return "http://localhost:5175"
}

function Invoke-LocalOntogonyScript {
    param(
        [string]$ScriptPath,
        [hashtable]$Arguments = @{}
    )

    if (-not (Test-Path -LiteralPath $ScriptPath)) {
        throw "Script not found: $ScriptPath"
    }

    & $ScriptPath @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "$ScriptPath failed (exit $LASTEXITCODE)."
    }
}

function Test-OperatorFrontendRoute {
    param(
        [string]$Url,
        [string[]]$SecretPatterns = @(),
        [int]$TimeoutSeconds = 30
    )

    $row = [ordered]@{
        url = $Url
        httpStatus = $null
        spaShell = $false
        secretPatternHits = @()
        passed = $false
    }

    try {
        $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec $TimeoutSeconds
        $row.httpStatus = [int]$response.StatusCode
        $body = [string]$response.Content
        $row.spaShell = $body -match 'id="root"'
        foreach ($pattern in $SecretPatterns) {
            if ([string]::IsNullOrWhiteSpace($pattern)) { continue }
            if ($body -match [regex]::Escape($pattern)) {
                $row.secretPatternHits += $pattern
            }
        }
        $row.passed = ($row.httpStatus -eq 200 -and $row.spaShell -and $row.secretPatternHits.Count -eq 0)
    }
    catch {
        $row.error = $_.Exception.Message
        $row.passed = $false
    }

    return [pscustomobject]$row
}
