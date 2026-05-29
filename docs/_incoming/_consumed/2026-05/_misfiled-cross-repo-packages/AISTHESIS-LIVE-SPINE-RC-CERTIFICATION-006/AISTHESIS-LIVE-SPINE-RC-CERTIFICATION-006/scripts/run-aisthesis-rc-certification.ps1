param(
  [string]$RepoRoot = "",
  [switch]$SkipBuild,
  [switch]$SkipTests,
  [switch]$SkipFixture,
  [switch]$SkipLive
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
  $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
}

function New-UtcStamp { return (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ") }
function Add-Gate($summary, $name, $status, $detail = $null) {
  $summary.gates[$name] = [ordered]@{ status = $status; detail = $detail }
  if ($status -eq "FAIL" -or $status -eq "BLOCKER") {
    $summary.status = "FAIL"
    $summary.blockers += $name
  }
}

$stamp = New-UtcStamp
$outDir = Join-Path $RepoRoot "artifacts\aisthesis-rc-certification\$stamp"
New-Item -ItemType Directory -Force -Path $outDir | Out-Null

$summary = [ordered]@{
  schemaVersion = "aisthesis.rc-certification.summary.v1"
  liveSpineSummarySchema = "aisthesis.live-spine.summary.v3"
  package = "AISTHESIS-LIVE-SPINE-RC-CERTIFICATION-006"
  status = "PASS"
  classification = "Not evaluated"
  startedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
  gates = [ordered]@{}
  blockers = @()
  deferrals = @()
  artifacts = @()
}

Push-Location $RepoRoot
try {
  if (-not $SkipBuild) {
    dotnet restore
    if ($LASTEXITCODE -ne 0) { Add-Gate $summary "restore" "FAIL"; throw "restore failed" }
    Add-Gate $summary "restore" "PASS"

    dotnet build Aisthesis.sln -c Release
    if ($LASTEXITCODE -ne 0) { Add-Gate $summary "build" "FAIL"; throw "build failed" }
    Add-Gate $summary "build" "PASS"
  } else { Add-Gate $summary "build" "NOT_RUN" "SkipBuild set" }

  if (-not $SkipTests) {
    dotnet test Aisthesis.sln -c Release --no-build
    if ($LASTEXITCODE -ne 0) { Add-Gate $summary "tests" "FAIL"; throw "tests failed" }
    Add-Gate $summary "tests" "PASS"
  } else { Add-Gate $summary "tests" "NOT_RUN" "SkipTests set" }

  if (-not $SkipFixture) {
    $fixtureOut = Join-Path $outDir "fixture"
    $fixtureScript = Join-Path $RepoRoot "scripts\system
un-five-service-aisthesis-live-smoke.ps1"
    if (Test-Path $fixtureScript) {
      & $fixtureScript -Mode Fixture -StartApi -OutDir $fixtureOut
      if ($LASTEXITCODE -ne 0) { Add-Gate $summary "fixtureSmoke" "FAIL"; throw "fixture smoke failed" }
      Add-Gate $summary "fixtureSmoke" "PASS" $fixtureOut
      $summary.artifacts += (Join-Path $fixtureOut "summary.json")
    } else {
      Add-Gate $summary "fixtureSmoke" "FAIL" "Fixture smoke script not found."
      throw "fixture script missing"
    }
  } else { Add-Gate $summary "fixtureSmoke" "NOT_RUN" "SkipFixture set" }

  if (-not $SkipLive) {
    $liveScript = Join-Path $RepoRoot "scripts\system
un-five-service-live-certification.ps1"
    if (Test-Path $liveScript) {
      $liveOut = Join-Path $outDir "live-or-explain"
      & $liveScript -Mode LiveOrExplain -OutDir $liveOut
      if ($LASTEXITCODE -ne 0) { Add-Gate $summary "fiveServiceLiveCertification" "FAIL"; throw "live certification failed" }
      $liveSummaryPath = Join-Path $liveOut "summary.json"
      if (Test-Path $liveSummaryPath) {
        $live = Get-Content $liveSummaryPath -Raw | ConvertFrom-Json
        Add-Gate $summary "fiveServiceLiveCertification" $live.status $live.reason
        $summary.artifacts += $liveSummaryPath
      } else {
        Add-Gate $summary "fiveServiceLiveCertification" "FAIL" "summary.json missing"
      }
    } else {
      Add-Gate $summary "fiveServiceLiveCertification" "DEFERRED" "Live certification script not present yet."
      $summary.deferrals += "fiveServiceLiveCertification"
    }
  } else { Add-Gate $summary "fiveServiceLiveCertification" "NOT_RUN" "SkipLive set" }

  if ($summary.status -eq "PASS" -and $summary.deferrals.Count -eq 0) {
    $summary.classification = "RC-certification candidate"
  } elseif ($summary.status -eq "PASS") {
    $summary.classification = "RC-certification partial"
  } else {
    $summary.classification = "Blocked"
  }
}
finally {
  Pop-Location
  $summary.completedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
  $summaryPath = Join-Path $outDir "summary.json"
  ($summary | ConvertTo-Json -Depth 40) | Set-Content -Encoding UTF8 $summaryPath
  Write-Host "Wrote RC certification summary: $summaryPath"
}

if ($summary.status -eq "FAIL") { exit 1 }
