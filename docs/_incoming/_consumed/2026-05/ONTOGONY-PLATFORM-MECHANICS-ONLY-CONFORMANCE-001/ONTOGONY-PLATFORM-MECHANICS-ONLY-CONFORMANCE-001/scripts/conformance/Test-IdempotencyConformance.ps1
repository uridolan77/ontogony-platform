param([string]$RepoRoot=".", [string]$ConsumerName="", [string]$OutputDirectory="artifacts/idempotency")
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$result = [pscustomobject]@{ check="idempotency"; status="PARTIAL"; states=@("reserved","running","completed","failed","expired","conflict"); note="Template: validate idempotency state shape and repeated request behavior." }
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory "idempotency.json") -Encoding UTF8
exit 2
