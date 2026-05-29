param([string]$RepoRoot=".", [string]$ConsumerName="", [string]$OutputDirectory="artifacts/outbox-artifact")
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$result = [pscustomobject]@{ check="outbox-artifact"; status="PARTIAL"; note="Template: validate outbox/artifact reference mechanics, not product payload semantics." }
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory "outbox-artifact.json") -Encoding UTF8
exit 2
