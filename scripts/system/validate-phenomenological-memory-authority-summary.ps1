param([Parameter(Mandatory=$true)][string]$SummaryPath)
$ErrorActionPreference = "Stop"
$summary = Get-Content $SummaryPath -Raw | ConvertFrom-Json
if ($summary.schemaVersion -ne "ontogony-phenomenological-memory-authority-certification-v1") { throw "Wrong schemaVersion" }
if ($summary.status -eq "PASS") {
  foreach ($field in @("runId","traceId","episodeId","mutationId")) {
    if (-not $summary.$field) { throw "PASS missing $field" }
  }
  if (-not $summary.kanon.policyFound) { throw "PASS requires kanon.policyFound=true" }
  if (-not $summary.kanon.decisionId) { throw "PASS requires kanon.decisionId" }
  if (-not $summary.aisthesis.validationOutcomeRecorded) { throw "PASS requires Aisthesis validation outcome" }
  if (-not $summary.frontend.routeCovered) { throw "PASS requires frontend route coverage" }
}
Write-Host "Summary valid: $($summary.status)"
