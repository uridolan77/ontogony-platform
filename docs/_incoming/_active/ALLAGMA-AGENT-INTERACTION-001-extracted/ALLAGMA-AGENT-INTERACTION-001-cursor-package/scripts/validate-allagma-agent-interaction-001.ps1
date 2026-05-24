param(
  [string]$FrontendRoot = "C:\dev\ontogony-frontend",
  [string]$AllagmaBaseUrl = "http://localhost:5083",
  [string]$RunId = ""
)

$ErrorActionPreference = "Stop"

Write-Host "ALLAGMA-AGENT-INTERACTION-001 validation" -ForegroundColor Cyan
Write-Host "FrontendRoot: $FrontendRoot"
Write-Host "AllagmaBaseUrl: $AllagmaBaseUrl"

if (!(Test-Path $FrontendRoot)) {
  throw "Frontend root not found: $FrontendRoot"
}

Push-Location $FrontendRoot
try {
  Write-Host "Running frontend tests..." -ForegroundColor Cyan
  if (Test-Path "package.json") {
    npm test -- --runInBand
  } else {
    Write-Warning "No package.json found at $FrontendRoot"
  }
} finally {
  Pop-Location
}

Write-Host "Checking Allagma health..." -ForegroundColor Cyan
try {
  $health = Invoke-RestMethod -Uri "$AllagmaBaseUrl/health" -Method Get -TimeoutSec 5
  $health | ConvertTo-Json -Depth 10
} catch {
  Write-Warning "Allagma health check failed: $($_.Exception.Message)"
}

if ($RunId -ne "") {
  Write-Host "Checking live run events for $RunId..." -ForegroundColor Cyan
  try {
    $events = Invoke-RestMethod -Uri "$AllagmaBaseUrl/allagma/v0/runs/$RunId/events" -Method Get -TimeoutSec 10
    $events | ConvertTo-Json -Depth 20 | Out-File -FilePath "allagma-agent-interaction-events-$RunId.json" -Encoding utf8
    Write-Host "Saved allagma-agent-interaction-events-$RunId.json"
  } catch {
    Write-Warning "Run event lookup failed: $($_.Exception.Message)"
  }
}

Write-Host "Manual checks still required:" -ForegroundColor Yellow
Write-Host "- Open Agent Interaction and confirm live_lookup mode for a real run."
Write-Host "- Confirm fixture mode shows: Demo fixture — not live evidence."
Write-Host "- Confirm run list has no raw fake provider response summary or unlabeled unknown."
Write-Host "- Confirm exported bundle has mode/dataSource/sourceAttempts/missing and no duplicate sections."
