param(
  [string]$OutputPath = "",
  [string]$EnvFilePath = "",
  [string]$ProfileId = "",
  [string]$EnvironmentName = "",
  [string]$ConexusBaseUrl = "",
  [string]$KanonBaseUrl = "",
  [string]$AllagmaBaseUrl = "",
  [string]$KanonOntologyVersionId = "",
  [string]$ConexusProjectId = "",
  [string]$ConexusModelAlias = ""
)

$ErrorActionPreference = "Stop"
. "$PSScriptRoot\_docker-local-env.ps1"

$params = @{}
if (-not [string]::IsNullOrWhiteSpace($OutputPath)) { $params.OutputPath = $OutputPath }
if (-not [string]::IsNullOrWhiteSpace($EnvFilePath)) { $params.EnvFilePath = $EnvFilePath }
if (-not [string]::IsNullOrWhiteSpace($ProfileId)) { $params.ProfileId = $ProfileId }
if (-not [string]::IsNullOrWhiteSpace($EnvironmentName)) { $params.EnvironmentName = $EnvironmentName }
if (-not [string]::IsNullOrWhiteSpace($ConexusBaseUrl)) { $params.ConexusBaseUrl = $ConexusBaseUrl }
if (-not [string]::IsNullOrWhiteSpace($KanonBaseUrl)) { $params.KanonBaseUrl = $KanonBaseUrl }
if (-not [string]::IsNullOrWhiteSpace($AllagmaBaseUrl)) { $params.AllagmaBaseUrl = $AllagmaBaseUrl }
if (-not [string]::IsNullOrWhiteSpace($KanonOntologyVersionId)) { $params.KanonOntologyVersionId = $KanonOntologyVersionId }
if (-not [string]::IsNullOrWhiteSpace($ConexusProjectId)) { $params.ConexusProjectId = $ConexusProjectId }
if (-not [string]::IsNullOrWhiteSpace($ConexusModelAlias)) { $params.ConexusModelAlias = $ConexusModelAlias }

Write-DockerLocalOperatorRuntimeConfig @params
