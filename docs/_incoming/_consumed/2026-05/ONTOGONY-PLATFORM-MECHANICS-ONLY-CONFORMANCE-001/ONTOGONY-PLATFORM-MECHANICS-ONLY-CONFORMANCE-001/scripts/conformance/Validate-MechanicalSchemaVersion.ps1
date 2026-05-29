param([string]$SchemaPath)
if (-not (Test-Path $SchemaPath)) { throw "Schema not found: $SchemaPath" }
$json = Get-Content $SchemaPath -Raw | ConvertFrom-Json
if (-not $json.'$id') { throw "Schema missing `$id: $SchemaPath" }
if (-not $json.title) { throw "Schema missing title: $SchemaPath" }
if (-not $json.type) { throw "Schema missing type: $SchemaPath" }
Write-Host "Schema version metadata OK: $SchemaPath"
