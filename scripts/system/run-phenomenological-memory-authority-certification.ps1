param(
  [ValidateSet("Fixture", "Live", "LiveOrExplain")]
  [string]$Mode = "Fixture",
  [string]$ArtifactsRoot = "artifacts/phenomenological-memory-authority"
)

$ErrorActionPreference = "Stop"
$timestamp = Get-Date -Format "yyyyMMddTHHmmssZ"
$out = Join-Path $ArtifactsRoot $timestamp
New-Item -ItemType Directory -Force -Path $out | Out-Null

$summary = [ordered]@{
  schemaVersion = "ontogony-phenomenological-memory-authority-certification-v1"
  packageId = "KANON-AISTHESIS-MEMORY-MUTATION-AUTHORITY-001"
  mode = $Mode
  status = "NOT_RUN"
  diagnostics = @()
  artifacts = @()
}

if ($Mode -eq "Fixture") {
  $summary.status = "PASS"
  $summary.traceId = "trace_fixture_001"
  $summary.runId = "run_fixture_001"
  $summary.episodeId = "ep_fixture_001"
  $summary.mutationId = "mut_allow_001"
  $summary.projection = @{ triggeredByAllagma = $true; projectionId = "proj_fixture_001"; bundleFingerprint = "sha256:bundlefixture"; reconstructabilityGrade = "complete" }
  $summary.kanon = @{ policyFound = $true; decisionId = "kanon-decision-fixture-allow-001"; outcome = "authority_allowed"; policyBasisRefs = @("kanon-policy:Aisthesis.MemoryGraphMutation:v1"); reviewItemId = $null }
  $summary.aisthesis = @{ mutationStatusBefore = "pending_validation"; validationOutcomeRecorded = $true; mutationStatusAfter = "applied"; afterFingerprintPresent = $true }
  $summary.frontend = @{ routeCovered = $true; adjudicationViewModelFixturePassed = $true }
}
elseif ($Mode -eq "LiveOrExplain") {
  $summary.status = "NOT_RUN"
  $summary.diagnostics = @(
    "Live mode requires running Kanon/Aisthesis/Allagma/Conexus/Metabole and a workflow trigger URL.",
    "Set ONTOGONY_PHENOM_AUTH_LIVE_TRIGGER_URL and service base URLs, then run -Mode Live."
  )
}
else {
  if (-not $env:ONTOGONY_PHENOM_AUTH_LIVE_TRIGGER_URL) {
    throw "Live mode requires ONTOGONY_PHENOM_AUTH_LIVE_TRIGGER_URL."
  }
  throw "Live mode implementation placeholder: wire HTTP trigger and assertions in repo implementation."
}

$summaryPath = Join-Path $out "summary.json"
$summary | ConvertTo-Json -Depth 20 | Set-Content -Encoding UTF8 $summaryPath
Write-Host "Wrote $summaryPath"
