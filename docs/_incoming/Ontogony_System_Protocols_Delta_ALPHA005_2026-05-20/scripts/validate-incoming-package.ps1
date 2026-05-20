param(
  [Parameter(Mandatory=$true)]
  [string]$PackageRoot
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $PackageRoot)) {
  throw "Package root not found: $PackageRoot"
}

$patterns = @(
  @{ Name = "obsolete-agentor"; Regex = "\bAgentor\b" },
  @{ Name = "stale-alpha004-pending"; Regex = "SYSTEM-ALPHA-004.*pending|runtime baseline promotion.*pending SYSTEM-ALPHA-004" },
  @{ Name = "hardcoded-old-allagma-model-claim"; Regex = "Allagma.*hard-?codes.*gpt-4o-mini|Model\s*=\s*`"gpt-4o-mini`"" },
  @{ Name = "closed-b013-open-claim"; Regex = "B-013.*(open|quarantine|blocked|missing)" },
  @{ Name = "production-overclaim"; Regex = "\bproduction[- ]ready\b|\bproduction readiness\b" }
)

$findings = @()

Get-ChildItem -Path $PackageRoot -Recurse -File -Include *.md,*.txt,*.json,*.csv | ForEach-Object {
  $path = $_.FullName
  $content = Get-Content -Raw -Path $path
  foreach ($p in $patterns) {
    if ($content -match $p.Regex) {
      $findings += [pscustomobject]@{
        file = $path
        finding = $p.Name
        pattern = $p.Regex
      }
    }
  }
}

if ($findings.Count -gt 0) {
  $findings | Format-Table -AutoSize | Out-String | Write-Host
  throw "Incoming package appears stale or overclaims current state. Reconcile before adoption."
}

Write-Host "Incoming package passed basic staleness checks."
