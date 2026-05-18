param([string]$DevRoot="C:\dev")
$ErrorActionPreference="Stop"
foreach($repo in @("ontogony-platform","allagma-dotnet","kanon-dotnet","conexus-dotnet","ontogony-frontend","ontogony-ui")){
  $path=Join-Path $DevRoot $repo
  if(!(Test-Path $path)){ throw "Missing repo: $path" }
  Write-Host "OK repo: $path"
}
foreach($url in @("http://localhost:5081/health","http://localhost:5082/health","http://localhost:5083/health")){
  $r=Invoke-WebRequest -UseBasicParsing -TimeoutSec 5 -Uri $url
  Write-Host "OK $url -> $($r.StatusCode)"
}
