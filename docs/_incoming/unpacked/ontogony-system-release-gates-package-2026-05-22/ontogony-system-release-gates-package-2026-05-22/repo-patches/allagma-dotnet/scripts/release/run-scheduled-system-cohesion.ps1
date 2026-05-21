[CmdletBinding()]
param(
  [ValidateSet("Locked","MovingMain","CurrentWorkspace")]
  [string]$Mode = "MovingMain",

  [switch]$IncludeStreaming,
  [switch]$IncludeCapacityBaseline,
  [switch]$IncludeRestartSurvival,

  [string]$OutputRoot = "artifacts/system-cohesion-scheduled"
)

$ErrorActionPreference = "Stop"
$started = (Get-Date).ToUniversalTime().ToString("o")
$timestamp = (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ")
$out = Join-Path $OutputRoot $timestamp
New-Item -ItemType Directory -Force -Path $out | Out-Null
if (Test-Path (Join-Path $OutputRoot "latest")) { Remove-Item -Recurse -Force (Join-Path $OutputRoot "latest") }
New-Item -ItemType Directory -Force -Path (Join-Path $OutputRoot "latest") | Out-Null

$scenarios = New-Object System.Collections.Generic.List[object]

function Add-Scenario {
  param([string]$Id, [string]$Name, [scriptblock]$Body)
  $s = (Get-Date).ToUniversalTime()
  try {
    & $Body
    $verdict = "PASS"; $failure = $null
  }
  catch {
    $verdict = "FAIL"; $failure = @{ message = $_.Exception.Message }
  }
  $e = (Get-Date).ToUniversalTime()
  $scenarios.Add([ordered]@{
    id = $Id
    name = $Name
    verdict = $verdict
    startedAtUtc = $s.ToString("o")
    completedAtUtc = $e.ToString("o")
    durationMs = [int]($e - $s).TotalMilliseconds
    artifacts = @()
    failure = $failure
  })
}

Add-Scenario "completed_run" "Completed governed run + idempotency + human gates + optional assistance/fallback" {
  ./scripts/run-system-cohesion-smoke.ps1 -UseExistingServices -IncludeKanonAssistance -IncludeConexusFallback
}

if ($IncludeRestartSurvival) {
  Add-Scenario "restart_survival" "Postgres restart survival" {
    ./scripts/restart-e2e-first-real-system.ps1
  }
} else {
  $scenarios.Add([ordered]@{ id="restart_survival"; name="Postgres restart survival"; verdict="SKIPPED"; artifacts=@(); failure=$null })
}

if ($IncludeStreaming) {
  Add-Scenario "streaming_smoke" "Conexus streaming model purpose smoke" {
    if (Test-Path "./scripts/run-allagma-stream-001-evidence-smoke.ps1") {
      ./scripts/run-allagma-stream-001-evidence-smoke.ps1
    } elseif (Test-Path "./scripts/smoke-streaming-model-purpose.ps1") {
      ./scripts/smoke-streaming-model-purpose.ps1
    } else {
      throw "No streaming smoke script found."
    }
  }
} else {
  $scenarios.Add([ordered]@{ id="streaming_smoke"; name="Conexus streaming model purpose smoke"; verdict="SKIPPED"; artifacts=@(); failure=$null })
}

if ($IncludeCapacityBaseline) {
  Add-Scenario "conexus_capacity_baseline" "Conexus capacity baseline" {
    $conexus = Join-Path (Split-Path (Get-Location)) "conexus-dotnet"
    if (!(Test-Path $conexus)) { throw "Sibling conexus-dotnet not found at $conexus" }
    Push-Location $conexus
    try {
      $env:CONEXUS_RUN_CAPACITY_BASELINE = "true"
      ./scripts/capacity/run-capacity-baseline.ps1
    }
    finally { Pop-Location }
  }
} else {
  $scenarios.Add([ordered]@{ id="conexus_capacity_baseline"; name="Conexus capacity baseline"; verdict="SKIPPED"; artifacts=@(); failure=$null })
}

$completed = (Get-Date).ToUniversalTime().ToString("o")
$failed = @($scenarios | Where-Object { $_.verdict -eq "FAIL" })
$skippedRequired = @($scenarios | Where-Object { $_.verdict -eq "SKIPPED" })
$verdict = if ($failed.Count -gt 0) { "FAIL" } elseif ($Mode -eq "MovingMain") { "DRIFT_ONLY" } else { "PASS" }

$summary = [ordered]@{
  schema = "ontogony-system-cohesion-scheduled-summary-v1"
  mode = $Mode
  startedAtUtc = $started
  completedAtUtc = $completed
  verdict = $verdict
  scenarios = $scenarios
  artifacts = @()
}

$summaryPath = Join-Path $out "system-cohesion-scheduled-summary.json"
$summary | ConvertTo-Json -Depth 30 | Set-Content -Encoding UTF8 $summaryPath
Copy-Item $summaryPath (Join-Path $OutputRoot "latest/system-cohesion-scheduled-summary.json") -Force

$md = "# System cohesion scheduled summary`n`nMode: $Mode`nVerdict: $verdict`n`n"
foreach ($scenario in $scenarios) {
  $md += "- $($scenario.id): $($scenario.verdict)`n"
}
$md | Set-Content -Encoding UTF8 (Join-Path $out "system-cohesion-scheduled-summary.md")
Copy-Item (Join-Path $out "system-cohesion-scheduled-summary.md") (Join-Path $OutputRoot "latest/system-cohesion-scheduled-summary.md") -Force

if ($failed.Count -gt 0) { throw "System cohesion failed. See $summaryPath" }
Write-Host "System cohesion summary: $summaryPath"
