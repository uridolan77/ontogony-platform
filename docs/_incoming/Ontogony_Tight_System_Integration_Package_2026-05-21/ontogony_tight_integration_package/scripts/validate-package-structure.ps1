$ErrorActionPreference = "Stop"

$required = @(
  "README.md",
  "00_EXECUTIVE_VERDICT.md",
  "01_REPO_STATE_REVIEW.md",
  "02_TARGET_ARCHITECTURE.md",
  "03_GAP_REGISTER.md",
  "04_PR_SEQUENCE.md",
  "05_ACCEPTANCE_MATRIX.md",
  "06_RUNTIME_LOCK_AND_COMPATIBILITY_SPEC.md",
  "07_E2E_SCENARIO_SPECS.md",
  "08_OPERATOR_EVIDENCE_SPINE_PLAN.md",
  "09_STREAMING_AND_MODEL_PURPOSE_PLAN.md",
  "10_IDENTITY_AND_SECURITY_ROADMAP.md",
  "11_OBSERVABILITY_AND_SLO_PLAN.md",
  "12_CI_RELEASE_GATES.md",
  "matrices/sys_tight_acceptance_matrix.json",
  "matrices/system_identifier_resolution_matrix.json",
  "templates/EVIDENCE_SUMMARY_TEMPLATE.md",
  "templates/POST_LOCK_DELTA_TEMPLATE.md"
)

foreach ($path in $required) {
  if (-not (Test-Path $path)) {
    throw "Missing required package file: $path"
  }
}

Write-Host "Package structure valid." -ForegroundColor Green
