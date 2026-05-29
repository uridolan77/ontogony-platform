param([string]$RepoRoot=".", [string]$ConsumerName="", [string]$OutputDirectory="artifacts/no-product-semantics")
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$result = [pscustomobject]@{ check="no-product-semantics"; status="PARTIAL"; note="Template: run forbidden semantic term scan and route hits to owner repos." }
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory "no-product-semantics.json") -Encoding UTF8
exit 2
