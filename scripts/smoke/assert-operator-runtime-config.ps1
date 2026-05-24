param(
  [Parameter(Mandatory = $true)]
  [string]$BaseUrl,
  [string]$ExpectedProfile = $(if ($env:FRONTEND_RUNTIME_PROFILE_NAME) { $env:FRONTEND_RUNTIME_PROFILE_NAME } else { "docker-local" }),
  [string]$ExpectedConexusBaseUrl = $(if ($env:FRONTEND_RUNTIME_CONEXUS_BASE_URL) { $env:FRONTEND_RUNTIME_CONEXUS_BASE_URL } else { "http://localhost:5082" }),
  [string]$ExpectedKanonBaseUrl = $(if ($env:FRONTEND_RUNTIME_KANON_BASE_URL) { $env:FRONTEND_RUNTIME_KANON_BASE_URL } else { "http://localhost:5081" }),
  [string]$ExpectedAllagmaBaseUrl = $(if ($env:FRONTEND_RUNTIME_ALLAGMA_BASE_URL) { $env:FRONTEND_RUNTIME_ALLAGMA_BASE_URL } else { "http://localhost:5083" })
)

$ErrorActionPreference = "Stop"

$SecretKeyPattern = '(?i)secret|token|apikey|api_key|password|credential|bearer|authorization|openai|anthropic|gemini'
$AllowedSecretLikeKeys = @(
  'allowBrowserCredentialStorage',
  'showLocalCredentialWarnings'
)

function Get-ForbiddenSecretKeyViolations {
  param(
    [Parameter(Mandatory = $true)]
    $Value,
    [string]$Path = ""
  )

  $violations = New-Object System.Collections.Generic.List[string]

  function Visit($node, [string]$currentPath) {
    if ($null -eq $node) { return }

    if ($node -is [System.Collections.IEnumerable] -and $node -isnot [string]) {
      $index = 0
      foreach ($item in $node) {
        Visit $item "$currentPath[$index]"
        $index++
      }
      return
    }

    if ($node -is [System.Management.Automation.PSCustomObject]) {
      foreach ($prop in $node.PSObject.Properties) {
        $key = [string]$prop.Name
        $nextPath = if ($currentPath) { "$currentPath.$key" } else { $key }
        if ($key -match $SecretKeyPattern -and $AllowedSecretLikeKeys -notcontains $key) {
          [void]$violations.Add("Forbidden secret-like key: $nextPath")
        }
        Visit $prop.Value $nextPath
      }
    }
  }

  Visit $Value $Path
  return ,$violations
}

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

$rawContent = $response.Content
if ($rawContent.Length -gt 0 -and [int][char]$rawContent[0] -eq 0xFEFF) {
  $rawContent = $rawContent.Substring(1)
}

$config = $rawContent | ConvertFrom-Json -ErrorAction SilentlyContinue
if (-not $config) {
  if ($rawContent -match '(?i)<!doctype html|<html') {
    throw "Runtime config URL returned HTML (SPA fallback). Rebuild/restart ontogony-frontend with nginx runtime-config route and mounted operator-runtime-config.json."
  }
  throw "Runtime config response is not valid JSON."
}
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

$secretViolations = Get-ForbiddenSecretKeyViolations -Value $config
if ($secretViolations.Count -gt 0) {
  throw ("Runtime config contains forbidden secret-like keys:`n  - " + ($secretViolations -join "`n  - "))
}

Write-Host "Runtime config smoke passed for $url"
