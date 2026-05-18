param(
  [switch]$UseExistingServices,
  [string]$OutputRoot = "artifacts/eval"
)

$ErrorActionPreference = "Stop"

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$out = Join-Path $OutputRoot $timestamp
New-Item -ItemType Directory -Force -Path $out | Out-Null

# Placeholder scaffold. Wire this after AGM-EVAL-001 lands.
$summary = @{
  schemaVersion = "eval-summary.v0"
  evaluationRunId = "eval_$timestamp"
  generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
  verdict = "degraded"
  qualityScore = 0
  scenarios = @()
  artifacts = @()
  note = "Scaffold only. Implement after AGM-EVAL-001."
}

$summary | ConvertTo-Json -Depth 20 | Set-Content -Encoding UTF8 (Join-Path $out "summary.json")

Write-Host "[INFO] Wrote scaffold eval summary to $out/summary.json"
Write-Host "[DEGRADED] Eval smoke scaffold only until AGM-EVAL-001 implementation."
