param(
  [Parameter(Mandatory = $true)]
  [string]$BaseUrl,
  [string]$ExpectedProfile = $(if ($env:FRONTEND_RUNTIME_PROFILE_NAME) { $env:FRONTEND_RUNTIME_PROFILE_NAME } else { "docker-local" }),
  [string]$ExpectedConexusBaseUrl = $(if ($env:FRONTEND_RUNTIME_CONEXUS_BASE_URL) { $env:FRONTEND_RUNTIME_CONEXUS_BASE_URL } else { "http://localhost:5082" }),
  [string]$ExpectedKanonBaseUrl = $(if ($env:FRONTEND_RUNTIME_KANON_BASE_URL) { $env:FRONTEND_RUNTIME_KANON_BASE_URL } else { "http://localhost:5081" }),
  [string]$ExpectedAllagmaBaseUrl = $(if ($env:FRONTEND_RUNTIME_ALLAGMA_BASE_URL) { $env:FRONTEND_RUNTIME_ALLAGMA_BASE_URL } else { "http://localhost:5083" })
)

$ErrorActionPreference = "Stop"
$url = "$($BaseUrl.TrimEnd('/'))/operator-runtime-config.json"
Write-Host "Fetching $url"
$response = Invoke-WebRequest -Uri $url -UseBasicParsing
if ($response.StatusCode -ne 200) {
  throw "Expected HTTP 200, got $($response.StatusCode)"
}

$cacheControl = $response.Headers["Cache-Control"]
if ($cacheControl -notmatch "no-store|no-cache") {
  throw "Expected no-cache Cache-Control header, got '$cacheControl'"
}

$config = $response.Content | ConvertFrom-Json
if ($config.configSchema -ne "ontogony.operator-runtime-config.v1") {
  throw "Unexpected configSchema: $($config.configSchema)"
}
if ($config.version -ne 1) {
  throw "Unexpected version: $($config.version)"
}
if ($config.profileName -ne $ExpectedProfile) {
  throw "Expected profile '$ExpectedProfile', got '$($config.profileName)'"
}
if ($config.services.conexus.baseUrl -ne $ExpectedConexusBaseUrl) {
  throw "Expected Conexus URL '$ExpectedConexusBaseUrl', got '$($config.services.conexus.baseUrl)'"
}
if ($config.services.kanon.baseUrl -ne $ExpectedKanonBaseUrl) {
  throw "Expected Kanon URL '$ExpectedKanonBaseUrl', got '$($config.services.kanon.baseUrl)'"
}
if ($config.services.allagma.baseUrl -ne $ExpectedAllagmaBaseUrl) {
  throw "Expected Allagma URL '$ExpectedAllagmaBaseUrl', got '$($config.services.allagma.baseUrl)'"
}

$raw = $response.Content
if ($raw -match '(?i)(apiKey|api_key|secret|token|password|credential|openai|anthropic)') {
  throw "Runtime config appears to contain forbidden secret-like keys"
}

Write-Host "Runtime config smoke passed for $url"
