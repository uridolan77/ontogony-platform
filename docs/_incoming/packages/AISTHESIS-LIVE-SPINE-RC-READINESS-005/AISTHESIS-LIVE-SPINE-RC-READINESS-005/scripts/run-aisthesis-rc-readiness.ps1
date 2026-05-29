param(
  [string]$RepoRoot = "",
  [switch]$SkipBuild,
  [switch]$SkipTests,
  [switch]$SkipSmoke
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
  $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
}

function New-UtcStamp { return (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ") }

$stamp = New-UtcStamp
$outDir = Join-Path $RepoRoot "artifacts\aisthesis-rc-readiness\$stamp"
New-Item -ItemType Directory -Force -Path $outDir | Out-Null

$summary = [ordered]@{
  schemaVersion = "aisthesis.rc-readiness.summary.v1"
  package = "AISTHESIS-LIVE-SPINE-RC-READINESS-005"
  status = "PASS"
  startedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
  gates = [ordered]@{}
  artifacts = @()
  failures = @()
}

function Add-Gate($name, $status, $detail = $null) {
  $summary.gates[$name] = [ordered]@{ status = $status; detail = $detail }
  if ($status -eq "FAIL") { $summary.status = "FAIL"; $summary.failures += $name }
}

Push-Location $RepoRoot
try {
  if (-not $SkipBuild) {
    dotnet restore
    if ($LASTEXITCODE -ne 0) { Add-Gate "restore" "FAIL"; throw "restore failed" }
    Add-Gate "restore" "PASS"
    dotnet build Aisthesis.sln -c Release
    if ($LASTEXITCODE -ne 0) { Add-Gate "build" "FAIL"; throw "build failed" }
    Add-Gate "build" "PASS"
  } else { Add-Gate "build" "NOT_RUN" "SkipBuild set" }

  if (-not $SkipTests) {
    dotnet test Aisthesis.sln -c Release --no-build
    if ($LASTEXITCODE -ne 0) { Add-Gate "tests" "FAIL"; throw "tests failed" }
    Add-Gate "tests" "PASS"
  } else { Add-Gate "tests" "NOT_RUN" "SkipTests set" }

  if (-not $SkipSmoke) {
    $fixtureOut = Join-Path $outDir "five-service-fixture"
    & (Join-Path $RepoRoot "scripts\system\run-five-service-aisthesis-live-smoke.ps1") -Mode Fixture -StartApi -OutDir $fixtureOut
    if ($LASTEXITCODE -ne 0) { Add-Gate "fixtureSmoke" "FAIL"; throw "fixture smoke failed" }
    Add-Gate "fixtureSmoke" "PASS" $fixtureOut
    $summary.artifacts += (Join-Path $fixtureOut "summary.json")
  } else { Add-Gate "fixtureSmoke" "NOT_RUN" "SkipSmoke set" }
}
finally {
  Pop-Location
  $summary.completedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
  $summaryPath = Join-Path $outDir "summary.json"
  ($summary | ConvertTo-Json -Depth 30) | Set-Content -Encoding UTF8 $summaryPath
  Write-Host "Wrote RC readiness summary: $summaryPath"
}

if ($summary.status -eq "FAIL") { exit 1 }
