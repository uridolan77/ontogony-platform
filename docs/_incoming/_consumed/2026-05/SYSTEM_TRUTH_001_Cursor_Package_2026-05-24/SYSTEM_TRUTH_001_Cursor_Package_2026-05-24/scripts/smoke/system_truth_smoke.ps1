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
    $json = $response.Content | ConvertFrom-Json
    return @{ ok = $true; statusCode = $response.StatusCode; json = $json; error = $null }
  } catch {
    return @{ ok = $false; statusCode = $null; json = $null; error = $_.Exception.Message }
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
  if ($ready.ok -and $ready.json.schemaVersion -eq "ready.v1") { $readyContract = "valid" }
  elseif ($ready.ok) { $readyContract = "warning" }

  $summary += [pscustomobject]@{
    service = $svc.name
    baseUrl = $svc.url
    connectivity = if ($health.ok) { "live" } else { "offline" }
    healthStatus = if ($health.ok) { $health.json.status } else { "unknown" }
    healthContract = $healthContract
    readiness = if ($ready.ok) { $ready.json.status } else { "unknown" }
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
