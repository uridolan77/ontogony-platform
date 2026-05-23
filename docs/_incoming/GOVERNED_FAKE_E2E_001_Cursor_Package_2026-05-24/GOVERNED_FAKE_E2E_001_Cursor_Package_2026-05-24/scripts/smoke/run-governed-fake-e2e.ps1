param(
  [string]$AllagmaBaseUrl = "http://localhost:5083",
  [string]$KanonBaseUrl = "http://localhost:5081",
  [string]$ConexusBaseUrl = "http://localhost:5082",
  [string]$AllagmaApiKey = "",
  [string]$KanonToken = "",
  [string]$ConexusAdminKey = "cx-conexus-admin-dev"
)

$ErrorActionPreference = "Stop"

$headers = @{}
if ($AllagmaApiKey.Trim().Length -gt 0) {
  $headers["Authorization"] = "Bearer $AllagmaApiKey"
}

$body = @{
  ontologyVersionId = "gaming-core@0.1.0"
  actorId = "local-operator"
  actorType = "human"
  actorRoles = @("Admin")
  objective = "GOVERNED-FAKE-E2E-001: summarize player risk using local fake provider."
  context = @{
    playerId = "123"
  }
  modelPurpose = "summarize-player-risk"
} | ConvertTo-Json -Depth 10

Write-Host "Starting governed fake run..."
$run = Invoke-RestMethod `
  -Method Post `
  -Uri "$AllagmaBaseUrl/allagma/v0/runs" `
  -Headers $headers `
  -ContentType "application/json" `
  -Body $body

$runId = $run.runId
Write-Host "Run ID: $runId"

Write-Host "Reading run detail..."
$detail = Invoke-RestMethod -Method Get -Uri "$AllagmaBaseUrl/allagma/v0/runs/$runId" -Headers $headers
$detail | ConvertTo-Json -Depth 20

Write-Host "Reading run events..."
$events = Invoke-RestMethod -Method Get -Uri "$AllagmaBaseUrl/allagma/v0/runs/$runId/events" -Headers $headers
$events | ConvertTo-Json -Depth 20

Write-Host "Now paste the runId into Evidence Spine:"
Write-Host $runId
