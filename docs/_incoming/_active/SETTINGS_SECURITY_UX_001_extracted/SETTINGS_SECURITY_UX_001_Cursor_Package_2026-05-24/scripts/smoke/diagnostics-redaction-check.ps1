param(
  [Parameter(Mandatory=$true)]
  [string]$DiagnosticsJsonPath
)

$content = Get-Content $DiagnosticsJsonPath -Raw
$forbidden = @(
  "sk-test",
  "secret-live-key",
  "password123",
  "Bearer abc",
  "Host=localhost;Password="
)

$failed = $false
foreach ($value in $forbidden) {
  if ($content.Contains($value)) {
    Write-Host "Raw secret-like value found in diagnostics export: $value" -ForegroundColor Red
    $failed = $true
  }
}

$json = $content | ConvertFrom-Json
if (-not $json.privacy) {
  Write-Host "Missing privacy metadata." -ForegroundColor Red
  $failed = $true
} else {
  if ($json.privacy.containsRawSecrets -ne $false) {
    Write-Host "containsRawSecrets must be false." -ForegroundColor Red
    $failed = $true
  }
}

if ($failed) { exit 1 }
Write-Host "Diagnostics export redaction check passed." -ForegroundColor Green
