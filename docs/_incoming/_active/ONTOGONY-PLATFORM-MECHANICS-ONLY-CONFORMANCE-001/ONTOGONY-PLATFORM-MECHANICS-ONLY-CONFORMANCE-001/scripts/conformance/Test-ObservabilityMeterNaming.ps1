param([string]$RepoRoot=".", [string]$ConsumerName="", [string]$OutputDirectory="artifacts/observability")
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$prefix = "ontogony.$ConsumerName."
$result = [pscustomobject]@{ check="observability-meter-naming"; status="PARTIAL"; expectedPrefix=$prefix; note="Template: scan meter names and validate prefix/mechanical naming." }
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory "observability-meter-naming.json") -Encoding UTF8
exit 2
