param(
  [string]$WorkspaceRoot = "C:\dev",
  [string]$AisthesisBaseUrl = "http://localhost:5084"
)
$repos = @("aisthesis-dotnet","allagma-dotnet","kanon-dotnet","conexus-dotnet","metabole-dotnet")
$result = foreach ($repo in $repos) {
  [pscustomobject]@{ repo=$repo; path=(Join-Path $WorkspaceRoot $repo); exists=(Test-Path (Join-Path $WorkspaceRoot $repo)) }
}
$result | Format-Table -AutoSize
try {
  Invoke-WebRequest -Uri "$AisthesisBaseUrl/ready" -UseBasicParsing -TimeoutSec 5 | Out-Null
  Write-Host "Aisthesis ready"
} catch {
  Write-Warning "Aisthesis not ready: $($_.Exception.Message)"
}
