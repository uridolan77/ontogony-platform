#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$packages = @(
  'Ontogony.Logging',
  'Ontogony.Redaction',
  'Ontogony.Secrets',
  'Ontogony.Quotas',
  'Ontogony.Replay.Contracts'
)

foreach ($pkg in $packages) {
  $project = Join-Path $repoRoot "src/$pkg/$pkg.csproj"
  $readme = Join-Path $repoRoot "src/$pkg/README.md"
  if (-not (Test-Path $project)) { throw "Missing package project: $project" }
  if (-not (Test-Path $readme)) { throw "Missing package README: $readme" }
}

$tests = @(
  'Ontogony.Logging.Tests',
  'Ontogony.Redaction.Tests',
  'Ontogony.Secrets.Tests',
  'Ontogony.Quotas.Tests',
  'Ontogony.Replay.Contracts.Tests'
)

foreach ($test in $tests) {
  $project = Join-Path $repoRoot "tests/$test/$test.csproj"
  if (-not (Test-Path $project)) { throw "Missing test project: $project" }
}

Write-Host 'OK: PR48-PR52 overlay packages and tests are present.'
