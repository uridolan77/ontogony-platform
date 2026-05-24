param(
  [string]$Root = "."
)

$forbidden = @(
  "unknown source",
  "Allagma defaults",
  "Kanon trusts headers",
  "secret-live-key"
)

$paths = @(
  "src"
)

$failed = $false
foreach ($phrase in $forbidden) {
  $matches = Get-ChildItem -Path $Root -Recurse -File -Include *.ts,*.tsx,*.md,*.json |
    Where-Object { $_.FullName -notmatch "node_modules|dist|build|coverage" } |
    Select-String -SimpleMatch $phrase

  if ($matches) {
    Write-Host "Forbidden phrase found: $phrase" -ForegroundColor Red
    $matches | ForEach-Object { Write-Host "  $($_.Path):$($_.LineNumber) $($_.Line.Trim())" }
    $failed = $true
  }
}

if ($failed) {
  exit 1
}

Write-Host "No forbidden UI phrases found." -ForegroundColor Green
