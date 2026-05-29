param([string]$RepoRoot=".", [string]$OutputDirectory="artifacts/schema-registry")
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$schemaPath = Join-Path $RepoRoot "schemas/mechanics/v1"
$schemas = if (Test-Path $schemaPath) { Get-ChildItem $schemaPath -Filter *.schema.json } else { @() }
$status = if ($schemas.Count -ge 6) { "PASS" } else { "FAIL" }
$result = [pscustomobject]@{ check="mechanical-schema-registry"; status=$status; schemaCount=$schemas.Count; path=$schemaPath }
$result | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory "schema-registry.json") -Encoding UTF8
if ($status -eq "PASS") { exit 0 } else { exit 1 }
