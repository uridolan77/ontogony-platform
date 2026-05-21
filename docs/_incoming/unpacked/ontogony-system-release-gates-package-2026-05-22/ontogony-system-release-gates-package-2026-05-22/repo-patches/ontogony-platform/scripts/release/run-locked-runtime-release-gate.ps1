[CmdletBinding()]
param(
  [Parameter(Mandatory=$true)]
  [string]$ReleaseId,

  [ValidateSet("Locked","ExpectedRefs","CurrentWorkspace")]
  [string]$Mode = "Locked",

  [string]$WorkspaceRoot = (Join-Path (Get-Location) "artifacts/release-workspace"),

  [string]$RuntimeLockPath = "allagma-dotnet/docs/system/ontogony-runtime.lock.json",

  [switch]$SkipCohesion,
  [switch]$SkipPackageMode
)

$ErrorActionPreference = "Stop"
$started = (Get-Date).ToUniversalTime().ToString("o")
$timestamp = (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ")
$outRoot = Join-Path (Get-Location) "artifacts/releases/$ReleaseId/$timestamp"
New-Item -ItemType Directory -Force -Path $outRoot | Out-Null

function Write-Step($m) { Write-Host "[release-gate] $m" }

function Invoke-Logged {
  param([string]$Name, [string]$WorkingDirectory, [string]$Command)
  $log = Join-Path $outRoot "logs/$Name.log"
  New-Item -ItemType Directory -Force -Path (Split-Path $log) | Out-Null
  Write-Step "$Name"
  Push-Location $WorkingDirectory
  try {
    pwsh -NoProfile -Command $Command *>&1 | Tee-Object -FilePath $log
    if ($LASTEXITCODE -ne 0) { throw "$Name failed with exit code $LASTEXITCODE" }
  }
  finally { Pop-Location }
}

# Locate or clone repos.
$repos = @("ontogony-platform","kanon-dotnet","conexus-dotnet","allagma-dotnet")
New-Item -ItemType Directory -Force -Path $WorkspaceRoot | Out-Null

# In CurrentWorkspace mode, assume repos already exist under WorkspaceRoot.
# In Locked/ExpectedRefs mode, the implementation should clone/fetch and checkout runtime-lock pins.
# This stub intentionally keeps clone details explicit for implementers to wire token/remote policy.

$lockFile = Join-Path $WorkspaceRoot $RuntimeLockPath
if (!(Test-Path $lockFile)) {
  throw "Runtime lock not found at $lockFile. Checkout allagma-dotnet first or pass -RuntimeLockPath."
}
$lock = Get-Content $lockFile -Raw | ConvertFrom-Json

$repoStates = @{}
foreach ($repo in $repos) {
  $repoPath = Join-Path $WorkspaceRoot $repo
  if (!(Test-Path $repoPath)) { throw "Repo path missing: $repoPath" }

  Push-Location $repoPath
  try {
    if ($Mode -eq "Locked") {
      $sha = $lock.lockedCommits.$repo
      git fetch --all --tags
      git checkout $sha
    }
    elseif ($Mode -eq "ExpectedRefs") {
      $ref = $lock.expectedRefs.$repo
      git fetch origin $ref
      git checkout FETCH_HEAD
    }

    $actual = (git rev-parse HEAD).Trim()
    $locked = [string]$lock.lockedCommits.$repo
    $expectedRef = [string]$lock.expectedRefs.$repo
    $verdict = if ($actual -eq $locked) { "PASS" } elseif ($Mode -eq "ExpectedRefs") { "DRIFT" } else { "FAIL" }
    $repoStates[$repo] = [ordered]@{
      expectedRef = $expectedRef
      lockedCommit = $locked
      actualCommit = $actual
      verdict = $verdict
    }
    if ($Mode -eq "Locked" -and $actual -ne $locked) { throw "$repo checked out $actual but lock requires $locked" }
  }
  finally { Pop-Location }
}

# Repo-local gates. These commands are intentionally conservative and should be tightened per repo.
Invoke-Logged -Name "platform-build-test" -WorkingDirectory (Join-Path $WorkspaceRoot "ontogony-platform") -Command "dotnet restore Ontogony.Platform.sln; dotnet build Ontogony.Platform.sln --no-restore -c Release; dotnet test Ontogony.Platform.sln --no-build -c Release"

Invoke-Logged -Name "kanon-contract-gates" -WorkingDirectory (Join-Path $WorkspaceRoot "kanon-dotnet") -Command "./scripts/bootstrap-solution.ps1; dotnet restore Kanon.sln; dotnet build Kanon.sln -c Release --no-restore; dotnet test tests/Kanon.Tests/Kanon.Tests.csproj -c Release --no-build --filter `"FullyQualifiedName~OntologyV0RouteInventoryTests|FullyQualifiedName~OpenApi|FullyQualifiedName~KanonCompatibilityManifest|FullyQualifiedName~KanonV0ContractFreeze|FullyQualifiedName~KanonEvidenceSpineHandoff`""

Invoke-Logged -Name "conexus-default-gate" -WorkingDirectory (Join-Path $WorkspaceRoot "conexus-dotnet") -Command "dotnet restore Conexus.sln; dotnet build Conexus.sln --no-restore -c Release -p:NoWarn=CS1591; dotnet test Conexus.sln --no-build -c Release -p:NoWarn=CS1591 --filter `"Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke&Category!=CapacityBaseline`""

Invoke-Logged -Name "allagma-default-gate" -WorkingDirectory (Join-Path $WorkspaceRoot "allagma-dotnet") -Command "./scripts/bootstrap-solution.ps1; dotnet restore Allagma.sln; dotnet build Allagma.sln -c Release --no-restore; dotnet test Allagma.sln -c Release --no-build --filter `"Category!=CrossRepo&Category!=PersistenceSmoke`"; ./scripts/architecture-conformance/run-cross-repo-conformance.ps1 -AllowPartial"

$packageSummary = $null
if (!$SkipPackageMode) {
  Invoke-Logged -Name "package-mode-release-train" -WorkingDirectory (Join-Path $WorkspaceRoot "allagma-dotnet") -Command "./scripts/release/run-package-mode-release-train.ps1 -WorkspaceRoot `"$WorkspaceRoot`" -RuntimeLockPath `"docs/system/ontogony-runtime.lock.json`""
  $packageSummaryPath = Join-Path $WorkspaceRoot "allagma-dotnet/artifacts/package-mode-release/latest/package-mode-release-summary.json"
  if (Test-Path $packageSummaryPath) { $packageSummary = Get-Content $packageSummaryPath -Raw | ConvertFrom-Json }
}

$cohesionSummary = $null
if (!$SkipCohesion) {
  Invoke-Logged -Name "scheduled-system-cohesion" -WorkingDirectory (Join-Path $WorkspaceRoot "allagma-dotnet") -Command "./scripts/release/run-scheduled-system-cohesion.ps1 -Mode $Mode -IncludeStreaming -IncludeCapacityBaseline -IncludeRestartSurvival"
  $cohesionSummaryPath = Join-Path $WorkspaceRoot "allagma-dotnet/artifacts/system-cohesion-scheduled/latest/system-cohesion-scheduled-summary.json"
  if (Test-Path $cohesionSummaryPath) { $cohesionSummary = Get-Content $cohesionSummaryPath -Raw | ConvertFrom-Json }
}

$completed = (Get-Date).ToUniversalTime().ToString("o")
$bundle = [ordered]@{
  schema = "ontogony-runtime-release-evidence-v1"
  releaseId = $ReleaseId
  mode = $Mode
  startedAtUtc = $started
  completedAtUtc = $completed
  verdict = if ($Mode -eq "Locked") { "PASS" } else { "DRIFT_ONLY" }
  runtimeLock = $lock
  repositories = $repoStates
  repoGates = [ordered]@{
    "ontogony-platform" = @{ verdict = "PASS"; log = "logs/platform-build-test.log" }
    "kanon-dotnet" = @{ verdict = "PASS"; log = "logs/kanon-contract-gates.log" }
    "conexus-dotnet" = @{ verdict = "PASS"; log = "logs/conexus-default-gate.log" }
    "allagma-dotnet" = @{ verdict = "PASS"; log = "logs/allagma-default-gate.log" }
  }
  packageMode = $packageSummary
  systemCohesion = $cohesionSummary
  capacityBaseline = $null
  restartSurvival = $null
  streamingSmoke = $null
  artifacts = @()
  failure = $null
}

$bundlePath = Join-Path $outRoot "release-evidence-bundle.json"
$bundle | ConvertTo-Json -Depth 100 | Set-Content -Encoding UTF8 $bundlePath

$md = @"
# $ReleaseId release evidence

Mode: $Mode
Verdict: $($bundle.verdict)
Started: $started
Completed: $completed

See: release-evidence-bundle.json
"@
$md | Set-Content -Encoding UTF8 (Join-Path $outRoot "release-evidence-bundle.md")

./scripts/release/validate-runtime-release-evidence.ps1 -BundlePath $bundlePath -ReleaseMode:($Mode -eq "Locked")
Write-Step "Evidence bundle: $bundlePath"
