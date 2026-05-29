param([Parameter(Mandatory=$true)][string]$SummaryPath)
$summary = Get-Content $SummaryPath -Raw | ConvertFrom-Json
if ($summary.schemaVersion -ne "phenomenological-authority-certification-v2") { throw "Bad schemaVersion" }
if ($summary.status -notin @("PASS","FAIL","NOT_RUN")) { throw "Bad status" }
if ($summary.mode -eq "Live" -and $summary.status -ne "PASS") { throw "Live mode must PASS" }
Write-Host "Summary valid: $SummaryPath"
