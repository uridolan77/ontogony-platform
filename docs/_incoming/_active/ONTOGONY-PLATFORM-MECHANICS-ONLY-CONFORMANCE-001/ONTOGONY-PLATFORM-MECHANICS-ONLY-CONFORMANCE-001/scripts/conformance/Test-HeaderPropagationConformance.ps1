param([string]$RepoRoot=".", [string]$ConsumerName="", [string]$OutputDirectory="artifacts/header-conformance")
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$required = @("trace", "correlation", "causation", "idempotency", "actor")
$result = [pscustomobject]@{ check="header-propagation"; status="PARTIAL"; required=$required; note="Template: implement repo-specific static/live checks." }
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory "header-propagation.json") -Encoding UTF8
exit 2
