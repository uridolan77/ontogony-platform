param(
  [string] $DevRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path,
  [switch] $ReleaseMode
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$requiredFiles = @(
  "docs/system/README.md",
  "docs/system/SYSTEM_COMPATIBILITY_MATRIX.md",
  "docs/system/SYSTEM_ENVIRONMENT_MATRIX.md",
  "docs/system/SYSTEM_AUTH_MATRIX.md",
  "docs/system/SYSTEM_ROUTE_MATRIX.md",
  "docs/system/SYSTEM_TEST_MATRIX.md",
  "docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md",
  "docs/system/ONTOGONY_RUNTIME_LOCK.md",
  "docs/system/ontogony-runtime.lock.json",
  "docs/system/SYSTEM_COHESION_BASELINE.md",
  "docs/system/SYSTEM_COHESION_ACCEPTANCE_MATRIX.md",
  "docs/system/system-cohesion-acceptance.matrix.json",
  "docs/system/SYSTEM_ERROR_COMPATIBILITY_MATRIX.md"
)

$missing = @()
foreach ($file in $requiredFiles) {
  $path = Join-Path $repoRoot $file
  if (-not (Test-Path $path)) { $missing += $file }
}
if ($missing.Count -gt 0) {
  throw "SYSTEM-COH-001 missing required files:`n$($missing -join "`n")"
}

$matrixPath = Join-Path $repoRoot "docs/system/system-cohesion-acceptance.matrix.json"
$matrix = Get-Content $matrixPath -Raw | ConvertFrom-Json
if ($matrix.schema -ne "ontogony-system-cohesion-acceptance-matrix-v1") {
  throw "Unexpected acceptance matrix schema: $($matrix.schema)"
}

$ids = @($matrix.scenarios | ForEach-Object { $_.id })
$dupes = $ids | Group-Object | Where-Object { $_.Count -gt 1 } | ForEach-Object { $_.Name }
if ($dupes.Count -gt 0) { throw "Duplicate scenario ids: $($dupes -join ', ')" }

$required = @(
  "compatibility_lock",
  "service_health_readiness",
  "governed_run_complete",
  "idempotent_run_retry",
  "human_gate_pause_resume",
  "kanon_conexus_assistance",
  "conexus_fallback",
  "correlation_chain",
  "evidence_spine_operator_visibility",
  "restart_replay_survival",
  "real_tools_blocked"
)
foreach ($id in $required) {
  if ($ids -notcontains $id) { throw "Missing required scenario id: $id" }
}

$allowed = @("PASS", "FAIL", "DEFERRED_WITH_REASON", "NOT_APPLICABLE_FOR_ALPHA", "OPTIONAL_LOCAL_ONLY")
foreach ($scenario in $matrix.scenarios) {
  if ($allowed -notcontains $scenario.status) {
    throw "Scenario '$($scenario.id)' has invalid status '$($scenario.status)'"
  }
  if ($scenario.status -eq "DEFERRED_WITH_REASON") {
    if ([string]::IsNullOrWhiteSpace($scenario.reason)) { throw "Scenario '$($scenario.id)' missing deferral reason" }
    if ([string]::IsNullOrWhiteSpace($scenario.owner)) { throw "Scenario '$($scenario.id)' missing owner" }
    if ([string]::IsNullOrWhiteSpace($scenario.nextGate)) { throw "Scenario '$($scenario.id)' missing nextGate" }
  }
}

if ($ReleaseMode) {
  $failed = @($matrix.scenarios | Where-Object { $_.status -eq "FAIL" })
  if ($failed.Count -gt 0) { throw "Release mode forbids FAIL scenarios: $($failed.id -join ', ')" }

  foreach ($id in $required) {
    $scenario = $matrix.scenarios | Where-Object { $_.id -eq $id } | Select-Object -First 1
    if ($scenario.status -eq "OPTIONAL_LOCAL_ONLY") {
      throw "Release mode forbids required scenario '$id' as OPTIONAL_LOCAL_ONLY"
    }
  }
}

Write-Host "SYSTEM-COH-001 validation OK"
