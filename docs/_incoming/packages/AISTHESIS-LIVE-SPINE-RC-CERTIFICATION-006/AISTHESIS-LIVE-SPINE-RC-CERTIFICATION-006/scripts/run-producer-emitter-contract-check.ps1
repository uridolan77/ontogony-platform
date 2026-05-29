param(
  [string]$AisthesisRepoRoot = "C:\Dev\aisthesis-dotnet",
  [string]$AllagmaRoot = "C:\Dev\allagma-dotnet",
  [string]$KanonRoot = "C:\Dev\kanon-dotnet",
  [string]$ConexusRoot = "C:\Dev\conexus-dotnet",
  [string]$MetaboleRoot = "C:\Dev\metabole-dotnet",
  [string]$OutDir = ""
)

$ErrorActionPreference = "Stop"
function New-UtcStamp { return (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ") }
if ([string]::IsNullOrWhiteSpace($OutDir)) {
  $OutDir = Join-Path $AisthesisRepoRoot "artifacts\producer-emitter-contract-check\$(New-UtcStamp)"
}
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

$repos = @(
  @{ name="allagma"; root=$AllagmaRoot; expected=@("Aisthesis") },
  @{ name="kanon"; root=$KanonRoot; expected=@("Aisthesis") },
  @{ name="conexus"; root=$ConexusRoot; expected=@("Aisthesis") },
  @{ name="metabole"; root=$MetaboleRoot; expected=@("Aisthesis") }
)

$summary = [ordered]@{
  schemaVersion = "aisthesis.producer-emitter-contract-check.v0"
  status = "PASS"
  repos = @()
  recordedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
}

foreach ($repo in $repos) {
  $exists = Test-Path $repo.root
  $hasAisthesis = $false
  if ($exists) {
    $matches = Get-ChildItem -Path $repo.root -Recurse -File -ErrorAction SilentlyContinue | Where-Object { $_.FullName -match "Aisthesis" } | Select-Object -First 1
    $hasAisthesis = $null -ne $matches
  }
  $status = if ($exists -and $hasAisthesis) { "PASS" } elseif ($exists) { "WARN" } else { "NOT_RUN" }
  if ($status -eq "WARN") { $summary.status = "PARTIAL" }
  $summary.repos += [ordered]@{ name=$repo.name; root=$repo.root; exists=$exists; hasAisthesisFiles=$hasAisthesis; status=$status }
}

$summaryPath = Join-Path $OutDir "summary.json"
($summary | ConvertTo-Json -Depth 30) | Set-Content -Encoding UTF8 $summaryPath
Write-Host "Wrote producer emitter contract check summary: $summaryPath"
