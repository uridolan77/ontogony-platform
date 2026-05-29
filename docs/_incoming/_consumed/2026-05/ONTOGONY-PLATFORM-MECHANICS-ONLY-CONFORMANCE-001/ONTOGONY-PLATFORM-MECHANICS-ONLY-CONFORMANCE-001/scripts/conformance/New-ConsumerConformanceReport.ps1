param([string]$ConsumerName, [array]$Checks, [string]$OutputPath)
$overall = if ($Checks | Where-Object status -eq "FAIL") { "FAIL" } elseif ($Checks | Where-Object status -eq "NOT_RUN") { "PARTIAL" } else { "PASS" }
[pscustomobject]@{
  schema = "ontogony.consumer-conformance-report.v1"
  generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
  consumerName = $ConsumerName
  overallStatus = $overall
  checks = $Checks
} | ConvertTo-Json -Depth 10 | Set-Content $OutputPath -Encoding UTF8
