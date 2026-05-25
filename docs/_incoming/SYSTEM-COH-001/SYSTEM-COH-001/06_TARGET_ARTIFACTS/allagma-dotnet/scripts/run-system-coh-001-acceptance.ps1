param(
  [string] $DevRoot = "C:\dev",
  [string] $OutputDirectory = "",
  [switch] $IncludeKanonAssistance,
  [switch] $IncludeConexusFallback,
  [switch] $IncludeCorrelationChain,
  [switch] $IncludeEvidenceSpineExport,
  [switch] $IncludePackageMode,
  [switch] $RequireEvidence,
  [switch] $ReleaseMode
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
  $stamp = (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ")
  $OutputDirectory = Join-Path $repoRoot "artifacts/system-coh-001/$stamp"
}
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

$scenarios = New-Object System.Collections.Generic.List[object]
function Add-ScenarioResult($id, $status, $notes = "", $artifact = $null) {
  $scenarios.Add([ordered]@{
    id = $id
    status = $status
    notes = $notes
    artifact = $artifact
  })
}

try {
  & (Join-Path $repoRoot "scripts/validate-runtime-lock.ps1") -ReleaseMode:$ReleaseMode
  & (Join-Path $repoRoot "scripts/validate-system-coh-001.ps1") -ReleaseMode:$ReleaseMode
  Add-ScenarioResult "compatibility_lock" "PASS" "Runtime lock and SYSTEM-COH validator passed."
}
catch {
  Add-ScenarioResult "compatibility_lock" "FAIL" $_.Exception.Message
  if ($ReleaseMode) { throw }
}

# This runner is intentionally a wrapper. Reuse existing smoke scripts when present.
$cohesionSmoke = Join-Path $repoRoot "scripts/run-system-cohesion-smoke.ps1"
if (Test-Path $cohesionSmoke) {
  $args = @("-OutputDirectory", $OutputDirectory)
  if ($IncludeKanonAssistance) { $args += "-IncludeKanonAssistance" }
  if ($IncludeConexusFallback) { $args += "-IncludeConexusFallback" }
  if ($IncludeCorrelationChain) { $args += "-IncludeCorrelationChain" }
  if ($RequireEvidence) { $args += "-RequireEvidence" }
  try {
    & $cohesionSmoke @args
    Add-ScenarioResult "governed_run_complete" "PASS" "System cohesion smoke completed." $OutputDirectory
  }
  catch {
    Add-ScenarioResult "governed_run_complete" "FAIL" $_.Exception.Message
    if ($ReleaseMode) { throw }
  }
}
else {
  Add-ScenarioResult "governed_run_complete" "DEFERRED_WITH_REASON" "run-system-cohesion-smoke.ps1 not found."
}

if ($IncludePackageMode) {
  $packageMode = Join-Path $repoRoot "scripts/run-package-mode-build.ps1"
  if (Test-Path $packageMode) {
    try {
      & $packageMode
      Add-ScenarioResult "package_mode_build" "PASS" "Package-mode build passed."
    }
    catch {
      Add-ScenarioResult "package_mode_build" "FAIL" $_.Exception.Message
      if ($ReleaseMode) { throw }
    }
  }
  else {
    Add-ScenarioResult "package_mode_build" "DEFERRED_WITH_REASON" "Package-mode script missing."
  }
}
else {
  Add-ScenarioResult "package_mode_build" "OPTIONAL_LOCAL_ONLY" "Package-mode not requested."
}

# Real tools are not executed. This scenario should be backed by docs/tests.
Add-ScenarioResult "real_tools_blocked" "DEFERRED_WITH_REASON" "Replace with PASS after docs/tests prove real tools remain blocked."

$summary = [ordered]@{
  schema = "ontogony-system-cohesion-summary-v1"
  package = "SYSTEM-COH-001"
  timestampUtc = (Get-Date).ToUniversalTime().ToString("o")
  repoRoot = $repoRoot.Path
  outputDirectory = $OutputDirectory
  releaseMode = [bool]$ReleaseMode
  scenarios = @($scenarios)
}

$summaryPath = Join-Path $OutputDirectory "summary.json"
$summary | ConvertTo-Json -Depth 20 | Set-Content -Path $summaryPath -Encoding utf8
Write-Host "SYSTEM-COH-001 summary written to $summaryPath"
