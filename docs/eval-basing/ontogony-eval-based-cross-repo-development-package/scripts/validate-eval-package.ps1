param(
  [string]$Root = "."
)

$ErrorActionPreference = "Stop"

$required = @(
  "README.md",
  "00_EXECUTIVE_BRIEF.md",
  "03_CROSS_REPO_PHASE_ROADMAP.md",
  "04_ACCEPTANCE_MATRIX.md",
  "pr-specs/PLAT-EVAL-001-neutral-evaluation-contracts.md",
  "pr-specs/AGM-TOPO-001-task-classification-and-topology-selection.md",
  "pr-specs/KANON-TOPO-001-topology-policy-evaluation.md",
  "pr-specs/CX-ROUTE-EVIDENCE-001-route-decision-records.md",
  "schemas/eval-run-summary.schema.json",
  "schemas/topology-selection.schema.json"
)

$missing = @()
foreach ($item in $required) {
  if (-not (Test-Path (Join-Path $Root $item))) {
    $missing += $item
  }
}

if ($missing.Count -gt 0) {
  Write-Error ("Missing required files: " + ($missing -join ", "))
}

Write-Host "[PASS] Eval package structure is complete."
