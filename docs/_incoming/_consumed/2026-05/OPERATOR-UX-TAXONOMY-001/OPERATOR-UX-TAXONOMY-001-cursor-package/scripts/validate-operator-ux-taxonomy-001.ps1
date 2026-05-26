param(
  [string]$Root = "."
)

$ErrorActionPreference = "Stop"
Write-Host "== OPERATOR-UX-TAXONOMY-001 validation scan =="
Write-Host "Root: $Root"

$fail = $false

function Scan-For($Label, $Pattern) {
  Write-Host "-- scanning: $Label"
  $matches = Get-ChildItem -Path $Root -Recurse -File |
    Where-Object { $_.FullName -notmatch "node_modules|\.git" -and $_.Extension -ne ".zip" -and $_.Name -ne "package-lock.json" } |
    Select-String -Pattern $Pattern -SimpleMatch

  if ($matches) {
    $matches | ForEach-Object { Write-Host "$($_.Path):$($_.LineNumber): $($_.Line)" }
    Write-Host "FOUND: $Label"
    $script:fail = $true
  } else {
    Write-Host "ok: $Label"
  }
}

Scan-For "page-headline fixture fallback" "Live with fixture fallback"
Scan-For "ambiguous failed runs sample" "Failed runs (sample)"
Scan-For "developer placeholder copy" "Backend-waiting list APIs"
Scan-For "raw leaked parameter error" "The parameter already belongs to a collection"
Scan-For "generic unexpected error copy" "An unexpected error occurred"

Write-Host "-- candidate bare unknown occurrences"
Get-ChildItem -Path $Root -Recurse -File |
  Where-Object { $_.FullName -notmatch "node_modules|\.git" -and $_.Extension -ne ".zip" -and $_.Name -ne "package-lock.json" } |
  Select-String -Pattern "\bunknown\b" |
  ForEach-Object { Write-Host "$($_.Path):$($_.LineNumber): $($_.Line)" }

Write-Host "-- recommended repo commands"
Write-Host "npm test"
Write-Host "npm run lint"
Write-Host "npm run build"

if ($fail) {
  throw "Validation found forbidden high-confidence strings. Review and migrate them."
}

Write-Host "Validation scan completed. Review bare unknown candidates manually."
