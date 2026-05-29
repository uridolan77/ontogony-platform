param(
  [string]$PlatformRoot = ".",
  [string]$ConsumerRoot = "",
  [string]$ConsumerName = "",
  [string]$OutputDirectory = "",
  [switch]$FixtureMode
)

$ErrorActionPreference = "Stop"
if ([string]::IsNullOrWhiteSpace($ConsumerName)) { throw "ConsumerName is required." }
if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
  $stamp = Get-Date -Format "yyyyMMddTHHmmssZ"
  $OutputDirectory = Join-Path $PlatformRoot "artifacts/platform-mechanics-conformance/$ConsumerName/$stamp"
}
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

$checks = @()
function Add-Check($name, $status, $details) {
  $script:checks += [pscustomobject]@{ name = $name; status = $status; details = $details }
}

Add-Check "header-propagation" "NOT_RUN" "Implement Test-HeaderPropagationConformance.ps1"
Add-Check "error-envelope" "NOT_RUN" "Implement Test-ErrorEnvelopeConformance.ps1"
Add-Check "idempotency" "NOT_RUN" "Implement Test-IdempotencyConformance.ps1"
Add-Check "outbox-artifact" "NOT_RUN" "Implement Test-OutboxArtifactConformance.ps1"
Add-Check "observability-meter-naming" "NOT_RUN" "Implement Test-ObservabilityMeterNaming.ps1"
Add-Check "no-product-semantics" "NOT_RUN" "Implement Test-NoProductSemantics.ps1"
Add-Check "schema-registry" "NOT_RUN" "Implement Test-MechanicalSchemaRegistry.ps1"

$overall = if ($checks | Where-Object status -eq "FAIL") { "FAIL" } elseif ($checks | Where-Object status -eq "NOT_RUN") { "PARTIAL" } else { "PASS" }

$summary = [pscustomobject]@{
  schema = "ontogony.consumer-conformance-report.v1"
  generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
  consumerName = $ConsumerName
  consumerRoot = $ConsumerRoot
  mode = if ($FixtureMode) { "Fixture" } else { "Static" }
  overallStatus = $overall
  checks = $checks
  nonClaims = @("No production readiness claim", "No product semantic approval")
}
$summary | ConvertTo-Json -Depth 10 | Set-Content (Join-Path $OutputDirectory "summary.json") -Encoding UTF8
Write-Host "Conformance summary: $OutputDirectory/summary.json"
if ($overall -eq "FAIL") { exit 1 } elseif ($overall -eq "PARTIAL") { exit 2 } else { exit 0 }
