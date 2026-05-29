param(
  [ValidateSet("Preflight", "Fixture", "Live", "LiveOrExplain")]
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
  $OutDir = Join-Path $RepoRoot "artifacts\five-service-live-certification\$(New-UtcStamp)"
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

$services = [ordered]@{
  kanon = (Test-Ready "kanon" $KanonBaseUrl)
  conexus = (Test-Ready "conexus" $ConexusBaseUrl)
  allagma = (Test-Ready "allagma" $AllagmaBaseUrl)
  aisthesis = (Test-Ready "aisthesis" $AisthesisBaseUrl)
  metabole = (Test-Ready "metabole" $MetaboleBaseUrl)
}

$summary = [ordered]@{
  schemaVersion = "aisthesis.live-spine.summary.v3"
  package = "AISTHESIS-LIVE-SPINE-RC-CERTIFICATION-006"
  mode = $Mode
  status = "NOT_RUN"
  classification = "Not evaluated"
  traceId = $null
  workflowProfile = $null
  services = $services
  producersObserved = @()
  requiredEdges = [ordered]@{
    v1 = [ordered]@{ present=0; missing=0; ambiguous=0; notApplicable=0 }
    v2 = [ordered]@{ present=0; missing=0; ambiguous=0; notApplicable=0 }
  }
  reconstructability = [ordered]@{ grade=$null; score=$null; blockingFindings=0; blockingCodes=@(); diagnostics=@() }
  bundle = [ordered]@{ bundleFingerprintPresent=$false; bundleFingerprint=$null; exportedAtUtc=$null }
  redaction = [ordered]@{ tokensIncluded=$false; rawPayloadsIncluded=$false; payloadRefsDereferenced=$false }
  gates = [ordered]@{}
  deferrals = @()
  reason = $null
  artifacts = @()
  recordedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
}

if ($Mode -eq "Preflight") {
  $summary.status = "PASS"
  $summary.classification = "RC-certification partial"
  $summary.reason = "Preflight records readiness/config only; live workflow not required."
}
elseif ($Mode -eq "Fixture") {
  $fixtureScript = Join-Path $RepoRoot "scripts\system
un-five-service-aisthesis-live-smoke.ps1"
  if (-not (Test-Path $fixtureScript)) {
    $summary.status = "FAIL"
    $summary.classification = "Blocked"
    $summary.reason = "Fixture smoke script missing."
  } else {
    $fixtureOut = Join-Path $OutDir "fixture"
    & $fixtureScript -Mode Fixture -StartApi -OutDir $fixtureOut
    if ($LASTEXITCODE -ne 0) {
      $summary.status = "FAIL"
      $summary.classification = "Blocked"
      $summary.reason = "Fixture smoke failed."
    } else {
      $fixtureSummaryPath = Join-Path $fixtureOut "summary.json"
      $summary.artifacts += $fixtureSummaryPath
      if (Test-Path $fixtureSummaryPath) {
        $fx = Get-Content $fixtureSummaryPath -Raw | ConvertFrom-Json
        $summary.status = $fx.status
        $summary.classification = if ($fx.status -eq "PASS") { "RC-certification partial" } else { "Blocked" }
        $summary.traceId = $fx.traceId
        $summary.producersObserved = @($fx.producerCoverage)
        $summary.requiredEdges.v1.present = [int]$fx.requiredEdges.present
        $summary.requiredEdges.v1.missing = [int]$fx.requiredEdges.missing
        $summary.requiredEdges.v1.notApplicable = [int]$fx.requiredEdges.notApplicable
        $summary.reconstructability.grade = $fx.reconstructabilityGrade
        $summary.reconstructability.score = $fx.reconstructabilityScore
        $summary.bundle.bundleFingerprintPresent = -not [string]::IsNullOrWhiteSpace($fx.bundleFingerprint)
        $summary.bundle.bundleFingerprint = $fx.bundleFingerprint
      } else {
        $summary.status = "FAIL"
        $summary.classification = "Blocked"
        $summary.reason = "Fixture summary missing."
      }
    }
  }
}
else {
  $notReady = @($services.Values | Where-Object { -not $_.ready })
  if ($notReady.Count -gt 0) {
    $summary.status = "NOT_RUN"
    $summary.classification = "RC-certification partial"
    $summary.reason = "One or more services not ready."
  } elseif (-not $env:AISTHESIS_LIVE_WORKFLOW_TRIGGER_URL) {
    $summary.status = "NOT_RUN"
    $summary.classification = "RC-certification partial"
    $summary.reason = "All services ready, but AISTHESIS_LIVE_WORKFLOW_TRIGGER_URL is not configured."
  } else {
    # Project-specific trigger hook. Keep honest: this template does not know the exact live workflow contract.
    $summary.status = "NOT_RUN"
    $summary.classification = "RC-certification partial"
    $summary.reason = "Live workflow trigger URL is configured, but package template requires repo-specific trigger implementation before PASS is allowed."
    $summary.deferrals += "Implement live workflow trigger call and Aisthesis trace verification."
  }
}

$summaryPath = Join-Path $OutDir "summary.json"
($summary | ConvertTo-Json -Depth 50) | Set-Content -Encoding UTF8 $summaryPath
Write-Host "Wrote five-service live certification summary: $summaryPath"
if ($summary.status -eq "FAIL") { exit 1 }
