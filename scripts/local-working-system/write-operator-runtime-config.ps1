param(
  [string]$OutputPath = (Join-Path $PSScriptRoot "..\..\docker\local-working-system\generated\operator-runtime-config.json"),
  [string]$EnvironmentName = $(if ($env:FRONTEND_RUNTIME_ENVIRONMENT_NAME) { $env:FRONTEND_RUNTIME_ENVIRONMENT_NAME } else { "Local" }),
  [string]$ProfileName = $(if ($env:FRONTEND_RUNTIME_PROFILE_NAME) { $env:FRONTEND_RUNTIME_PROFILE_NAME } else { "docker-local" }),
  [string]$ConexusBaseUrl = $(if ($env:FRONTEND_RUNTIME_CONEXUS_BASE_URL) { $env:FRONTEND_RUNTIME_CONEXUS_BASE_URL } else { "http://localhost:5082" }),
  [string]$KanonBaseUrl = $(if ($env:FRONTEND_RUNTIME_KANON_BASE_URL) { $env:FRONTEND_RUNTIME_KANON_BASE_URL } else { "http://localhost:5081" }),
  [string]$AllagmaBaseUrl = $(if ($env:FRONTEND_RUNTIME_ALLAGMA_BASE_URL) { $env:FRONTEND_RUNTIME_ALLAGMA_BASE_URL } else { "http://localhost:5083" }),
  [string]$KanonOntologyId = $(if ($env:FRONTEND_RUNTIME_KANON_ONTOLOGY_ID) { $env:FRONTEND_RUNTIME_KANON_ONTOLOGY_ID } else { "gaming-core" }),
  [string]$KanonOntologyVersionId = $(if ($env:FRONTEND_RUNTIME_KANON_ONTOLOGY_VERSION_ID) { $env:FRONTEND_RUNTIME_KANON_ONTOLOGY_VERSION_ID } else { "gaming-core@0.1.0" }),
  [string]$ConexusProjectId = $(if ($env:FRONTEND_RUNTIME_CONEXUS_PROJECT_ID) { $env:FRONTEND_RUNTIME_CONEXUS_PROJECT_ID } else { "dev-project" }),
  [string]$ConexusModelAlias = $(if ($env:FRONTEND_RUNTIME_CONEXUS_MODEL_ALIAS) { $env:FRONTEND_RUNTIME_CONEXUS_MODEL_ALIAS } else { "risk-summary-v0" })
)

$ErrorActionPreference = "Stop"
$resolvedOutput = [System.IO.Path]::GetFullPath($OutputPath)
$directory = Split-Path -Parent $resolvedOutput
if (-not (Test-Path $directory)) {
  New-Item -ItemType Directory -Path $directory -Force | Out-Null
}

$config = [ordered]@{
  configSchema = "ontogony.operator-runtime-config.v1"
  version = 1
  environmentName = $EnvironmentName
  profileName = $ProfileName
  generatedAt = (Get-Date).ToUniversalTime().ToString("o")
  build = [ordered]@{
    source = "ontogony-platform/docker/local-working-system"
    gitSha = $(if ($env:FRONTEND_VITE_GIT_SHA) { $env:FRONTEND_VITE_GIT_SHA } else { "local" })
    description = "Docker local working system runtime defaults"
  }
  services = [ordered]@{
    conexus = [ordered]@{ baseUrl = $ConexusBaseUrl }
    kanon = [ordered]@{ baseUrl = $KanonBaseUrl }
    allagma = [ordered]@{ baseUrl = $AllagmaBaseUrl }
  }
  frontend = [ordered]@{
    defaultRoute = "/"
    enableFixtureRoutes = $false
    enableExpertModeDefault = $false
    routeSearchEnabled = $true
  }
  evidence = [ordered]@{
    enableRawPreviewDefault = $false
    exportRedactionMode = "safe"
  }
  localAlpha = [ordered]@{
    allowBrowserCredentialStorage = $true
    showLocalCredentialWarnings = $true
  }
  diagnostics = [ordered]@{
    enableDiagnosticExport = $true
  }
  kanon = [ordered]@{
    ontologyId = $KanonOntologyId
    ontologyVersionId = $KanonOntologyVersionId
  }
  conexus = [ordered]@{
    projectId = $ConexusProjectId
    modelAlias = $ConexusModelAlias
    streamingEnabled = $false
  }
}

$json = $config | ConvertTo-Json -Depth 8
Set-Content -Path $resolvedOutput -Value $json -Encoding utf8
Write-Host "Wrote operator runtime config to $resolvedOutput"
