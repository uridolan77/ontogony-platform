param(
  [string]$FrontendRoot = "C:\Dev\ontogony-frontend",
  [string]$AisthesisBaseUrl = "http://localhost:5084",
  [string]$OutDir = ""
)

$ErrorActionPreference = "Stop"
function New-UtcStamp { return (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ") }
if ([string]::IsNullOrWhiteSpace($OutDir)) {
  $OutDir = Join-Path $FrontendRoot "artifacts\aisthesis-contract-smoke\$(New-UtcStamp)"
}
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

$summary = [ordered]@{
  schemaVersion = "aisthesis.frontend-contract-smoke.v0"
  status = "NOT_RUN"
  frontendRoot = $FrontendRoot
  aisthesisBaseUrl = $AisthesisBaseUrl
  checks = [ordered]@{}
  reason = $null
  recordedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
}

try {
  $cap = Invoke-WebRequest -Uri "$AisthesisBaseUrl/aisthesis/v0/capabilities" -UseBasicParsing -TimeoutSec 5
  $summary.checks.capabilities = [ordered]@{ status="PASS"; statusCode=$cap.StatusCode }
  $summary.status = "PASS"
} catch {
  $summary.status = "NOT_RUN"
  $summary.reason = $_.Exception.Message
}

$summaryPath = Join-Path $OutDir "summary.json"
($summary | ConvertTo-Json -Depth 30) | Set-Content -Encoding UTF8 $summaryPath
Write-Host "Wrote frontend Aisthesis contract smoke summary: $summaryPath"
