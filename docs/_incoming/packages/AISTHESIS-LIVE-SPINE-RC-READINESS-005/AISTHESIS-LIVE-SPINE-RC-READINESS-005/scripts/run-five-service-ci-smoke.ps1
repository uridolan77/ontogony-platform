param(
  [ValidateSet("Preflight", "Fixture", "Live")]
  [string]$Mode = "Preflight",
  [string]$AisthesisBaseUrl = "http://localhost:5084",
  [string]$AllagmaBaseUrl = "http://localhost:5083",
  [string]$KanonBaseUrl = "http://localhost:5081",
  [string]$ConexusBaseUrl = "http://localhost:5082",
  [string]$MetaboleBaseUrl = "http://localhost:5085",
  [string]$RepoRoot = "",
  [string]$OutDir = ""
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
  $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
}
function New-UtcStamp { return (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ") }
if ([string]::IsNullOrWhiteSpace($OutDir)) {
  $OutDir = Join-Path $RepoRoot "artifacts\five-service-ci-smoke\$(New-UtcStamp)"
}
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

function Test-Ready($Name, $BaseUrl) {
  try {
    $r = Invoke-WebRequest -Uri "$BaseUrl/ready" -Method GET -UseBasicParsing -TimeoutSec 5
    return [ordered]@{ name=$Name; baseUrl=$BaseUrl; ready=($r.StatusCode -ge 200 -and $r.StatusCode -lt 300); statusCode=$r.StatusCode; error=$null }
  } catch {
    return [ordered]@{ name=$Name; baseUrl=$BaseUrl; ready=$false; statusCode=$null; error=$_.Exception.Message }
  }
}

$services = @(
  Test-Ready "kanon" $KanonBaseUrl
  Test-Ready "conexus" $ConexusBaseUrl
  Test-Ready "allagma" $AllagmaBaseUrl
  Test-Ready "aisthesis" $AisthesisBaseUrl
  Test-Ready "metabole" $MetaboleBaseUrl
)

$summary = [ordered]@{
  schemaVersion = "aisthesis.five-service-ci-smoke.summary.v1"
  package = "AISTHESIS-LIVE-SPINE-RC-READINESS-005"
  mode = $Mode
  status = "NOT_RUN"
  services = $services
  traceId = $null
  producersObserved = @()
  requiredEdges = [ordered]@{ present=0; missing=0; ambiguous=0; notApplicable=0 }
  reconstructability = [ordered]@{ grade=$null; score=$null; blockingFindings=$null; blockingCodes=@() }
  bundleFingerprintPresent = $false
  redaction = [ordered]@{ tokensIncluded=$false; rawPayloadsIncluded=$false; payloadRefsDereferenced=$false }
  reason = $null
  recordedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
}

if ($Mode -eq "Preflight") {
  $summary.status = "PASS"
  $summary.reason = "Preflight only; readiness results recorded but live workflow not required."
}
elseif ($Mode -eq "Fixture") {
  $fixtureOut = Join-Path $OutDir "fixture"
  & (Join-Path $RepoRoot "scripts\system\run-five-service-aisthesis-live-smoke.ps1") -Mode Fixture -StartApi -OutDir $fixtureOut
  if ($LASTEXITCODE -eq 0) {
    $fixtureSummaryPath = Join-Path $fixtureOut "summary.json"
    if (Test-Path $fixtureSummaryPath) {
      $fx = Get-Content $fixtureSummaryPath -Raw | ConvertFrom-Json
      $summary.status = $fx.status
      $summary.traceId = $fx.traceId
      $summary.producersObserved = @($fx.producerCoverage)
      $summary.requiredEdges.present = [int]$fx.requiredEdges.present
      $summary.requiredEdges.missing = [int]$fx.requiredEdges.missing
      $summary.requiredEdges.notApplicable = [int]$fx.requiredEdges.notApplicable
      $summary.reconstructability.grade = $fx.reconstructabilityGrade
      $summary.reconstructability.score = $fx.reconstructabilityScore
      $summary.bundleFingerprintPresent = -not [string]::IsNullOrWhiteSpace($fx.bundleFingerprint)
    } else { $summary.status = "FAIL"; $summary.reason = "Fixture summary missing." }
  } else { $summary.status = "FAIL"; $summary.reason = "Fixture smoke failed." }
}
elseif ($Mode -eq "Live") {
  $notReady = @($services | Where-Object { -not $_.ready })
  if ($notReady.Count -gt 0) {
    $summary.status = "NOT_RUN"
    $summary.reason = "One or more services not ready."
  } elseif (-not $env:AISTHESIS_LIVE_WORKFLOW_TRIGGER_URL) {
    $summary.status = "NOT_RUN"
    $summary.reason = "All services ready, but AISTHESIS_LIVE_WORKFLOW_TRIGGER_URL is not configured."
  } else {
    $summary.status = "NOT_RUN"
    $summary.reason = "Live trigger URL configured but project-specific trigger is not implemented in template."
  }
}

$summaryPath = Join-Path $OutDir "summary.json"
($summary | ConvertTo-Json -Depth 30) | Set-Content -Encoding UTF8 $summaryPath
Write-Host "Wrote CI smoke summary: $summaryPath"
if ($summary.status -eq "FAIL") { exit 1 }
