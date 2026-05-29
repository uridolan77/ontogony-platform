param([string]$RepoRoot=".", [string]$ConsumerName="", [string]$OutputDirectory="artifacts/error-envelope")
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$result = [pscustomobject]@{ check="error-envelope"; status="PARTIAL"; required=@("code","message","traceId","correlationId"); note="Template: validate error responses or fixtures against schema." }
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory "error-envelope.json") -Encoding UTF8
exit 2
