param(
  [string]$AllagmaBaseUrl = "http://localhost:5083",
  [string]$ConexusBaseUrl = "http://localhost:5082",
  [string]$KanonBaseUrl = "http://localhost:5081",
  [string]$AllagmaToken = $env:ALLAGMA_SERVICE_TOKEN,
  [string]$ConexusAdminKey = $env:CONEXUS_ADMIN_KEY,
  [string]$OutputDir = "artifacts/evidence-spine-real-001"
)

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

Write-Host "EVIDENCE-SPINE-REAL-001 validation skeleton"
Write-Host "Allagma: $AllagmaBaseUrl"
Write-Host "Conexus: $ConexusBaseUrl"
Write-Host "Kanon: $KanonBaseUrl"

$headers = @{}
if ($AllagmaToken) { $headers["Authorization"] = "Bearer $AllagmaToken" }

# This script is intentionally conservative because local auth/header names vary by repo branch.
# Cursor should adapt it to the actual repo's documented local-stack scripts.

Write-Host "1. Check service health/readiness manually or with repo scripts."
Write-Host "2. Start a governed fake-provider run through Allagma."
Write-Host "3. Capture allagmaRunId, planningDecisionId, conexusModelCallId, routeDecisionId."
Write-Host "4. Resolve Evidence Spine in the frontend/test harness."
Write-Host "5. Assert no generic source failures, no duplicate canonical nodes, and route decision structured result."

@{
  generatedAt = (Get-Date).ToUniversalTime().ToString("o")
  workItem = "EVIDENCE-SPINE-REAL-001"
  allagmaBaseUrl = $AllagmaBaseUrl
  conexusBaseUrl = $ConexusBaseUrl
  kanonBaseUrl = $KanonBaseUrl
  status = "skeleton"
  next = "Adapt to actual local-stack auth and evidence resolver test harness."
} | ConvertTo-Json -Depth 5 | Set-Content -Encoding UTF8 "$OutputDir/validation-skeleton.json"
