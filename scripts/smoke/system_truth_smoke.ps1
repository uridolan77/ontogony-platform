param(
  [string]$ConexusUrl = "http://localhost:5082",
  [string]$KanonUrl = "http://localhost:5081",
  [string]$AllagmaUrl = "http://localhost:5083"
)

$ErrorActionPreference = "Stop"

$services = @(
  @{ name = "conexus"; url = $ConexusUrl },
  @{ name = "kanon"; url = $KanonUrl },
  @{ name = "allagma"; url = $AllagmaUrl }
)

function Invoke-JsonProbe($uri) {
  try {
    $response = Invoke-WebRequest -Uri $uri -UseBasicParsing -TimeoutSec 5
    $json = $null
    if ($response.Content) {
      $json = $response.Content | ConvertFrom-Json
    }
    return @{
      ok = $response.StatusCode -ge 200 -and $response.StatusCode -lt 300
      statusCode = $response.StatusCode
      json = $json
      error = $null
    }
  } catch {
    $statusCode = $null
    $json = $null
  $body = $null
    if ($_.Exception.Response) {
      $statusCode = [int]$_.Exception.Response.StatusCode
      try {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $body = $reader.ReadToEnd()
        if ($body) {
          $json = $body | ConvertFrom-Json
        }
      } catch {
        # keep json null when body is not JSON
      }
    }
    return @{
      ok = $false
      statusCode = $statusCode
      json = $json
      error = if ($json) { $null } else { $_.Exception.Message }
    }
  }
}

$summary = @()
foreach ($svc in $services) {
  $health = Invoke-JsonProbe "$($svc.url)/health"
  $ready = Invoke-JsonProbe "$($svc.url)/ready"

  $healthContract = "invalid"
  if ($health.ok -and $health.json.schemaVersion -eq "health.v1") { $healthContract = "valid" }
  elseif ($health.ok) { $healthContract = "warning" }

  $readyContract = "invalid"
  if ($ready.json -and $ready.json.schemaVersion -eq "ready.v1") { $readyContract = "valid" }
  elseif ($ready.json) { $readyContract = "warning" }

  $readiness = "unknown"
  if ($ready.json -and $ready.json.status) { $readiness = $ready.json.status }
  elseif ($ready.ok) { $readiness = "ready" }
  $summary += [pscustomobject]@{
    service = $svc.name
    baseUrl = $svc.url
    connectivity = if ($health.ok) { "live" } else { "offline" }
    healthStatus = if ($health.ok) { $health.json.status } else { "unknown" }
    healthContract = $healthContract
    readiness = $readiness
    readyContract = $readyContract
    version = if ($health.ok) { $health.json.version } else { $null }
    healthError = $health.error
    readyError = $ready.error
  }
}

$summary | Format-Table -AutoSize

$hasFailure = $summary | Where-Object { $_.connectivity -eq "offline" -or $_.healthContract -eq "invalid" -or $_.readyContract -eq "invalid" }
if ($hasFailure) {
  Write-Host "`nSYSTEM-TRUTH smoke: FAILED" -ForegroundColor Red
  exit 1
}

$hasWarning = $summary | Where-Object { $_.healthContract -eq "warning" -or $_.readyContract -eq "warning" -or $_.readiness -ne "ready" }
if ($hasWarning) {
  Write-Host "`nSYSTEM-TRUTH smoke: WARNING" -ForegroundColor Yellow
  exit 2
}

Write-Host "`nSYSTEM-TRUTH smoke: PASSED" -ForegroundColor Green
exit 0
