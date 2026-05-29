param(
  [string]$RepoRoot = ".",
  [string]$ProposalPath = "",
  [string]$OutputDirectory = "artifacts/platform-mechanics-conformance/governance"
)

$ErrorActionPreference = "Stop"
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

$forbidden = @(
  "canonical fact",
  "semantic authority",
  "ontology acceptance",
  "model routing",
  "provider fallback policy",
  "human gate policy",
  "workflow orchestration",
  "SLOD mapping algorithm",
  "reconstructability scoring",
  "memory graph semantics",
  "business approval rule"
)

$allowed = @(
  "trace",
  "correlation",
  "header",
  "error envelope",
  "idempotency",
  "outbox",
  "artifact reference",
  "evidence reference",
  "actor context",
  "schema",
  "observability",
  "redaction",
  "secret reference"
)

$files = Get-ChildItem -Path $RepoRoot -Recurse -File -Include *.md,*.cs,*.json,*.props,*.ps1 |
  Where-Object { $_.FullName -notmatch "\\(bin|obj|artifacts|_donors|\.git)\\" }

$hits = @()
foreach ($file in $files) {
  $text = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
  foreach ($term in $forbidden) {
    if ($text -match [regex]::Escape($term)) {
      $hits += [pscustomobject]@{
        file = $file.FullName
        term = $term
        suggestedOwner = "product-repo"
        status = "review"
      }
    }
  }
}

$status = if ($hits.Count -eq 0) { "PASS" } else { "PARTIAL" }
$summary = [pscustomobject]@{
  check = "platform-mechanics-only"
  status = $status
  forbiddenHits = $hits
  note = "PARTIAL means review required; not every lexical hit is a violation."
}
$summary | ConvertTo-Json -Depth 10 | Set-Content (Join-Path $OutputDirectory "mechanics-only-summary.json") -Encoding UTF8

if ($status -eq "PASS") { exit 0 } else { exit 2 }
