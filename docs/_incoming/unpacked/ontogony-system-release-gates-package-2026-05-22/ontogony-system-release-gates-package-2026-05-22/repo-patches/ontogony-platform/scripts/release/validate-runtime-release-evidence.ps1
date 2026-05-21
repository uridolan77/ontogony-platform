[CmdletBinding()]
param(
  [Parameter(Mandatory=$true)]
  [string]$BundlePath,

  [switch]$ReleaseMode
)

$ErrorActionPreference = "Stop"
if (!(Test-Path $BundlePath)) { throw "Bundle not found: $BundlePath" }

$json = Get-Content $BundlePath -Raw | ConvertFrom-Json
if ($json.schema -ne "ontogony-runtime-release-evidence-v1") { throw "Invalid schema: $($json.schema)" }

$requiredRepos = @("ontogony-platform","kanon-dotnet","conexus-dotnet","allagma-dotnet")
foreach ($repo in $requiredRepos) {
  if (-not $json.repositories.PSObject.Properties.Name.Contains($repo)) { throw "Missing repository state: $repo" }
  $state = $json.repositories.$repo
  foreach ($field in @("expectedRef","lockedCommit","actualCommit","verdict")) {
    if (-not $state.PSObject.Properties.Name.Contains($field)) { throw "Missing repository field $repo.$field" }
  }
}

if ($ReleaseMode) {
  if ($json.mode -ne "Locked") { throw "Release evidence must be Locked mode. Actual: $($json.mode)" }
  if ($json.verdict -ne "PASS") { throw "Release verdict must be PASS. Actual: $($json.verdict)" }

  foreach ($repo in $requiredRepos) {
    $state = $json.repositories.$repo
    if ($state.actualCommit -ne $state.lockedCommit) { throw "$repo actualCommit differs from lockedCommit" }
    if ($state.verdict -ne "PASS") { throw "$repo verdict is $($state.verdict)" }
  }

  if ($null -eq $json.packageMode) { throw "Missing packageMode summary" }
  if ($null -eq $json.systemCohesion) { throw "Missing systemCohesion summary" }
}

$raw = Get-Content $BundlePath -Raw
$secretPatterns = @("sk-[A-Za-z0-9]", "sk-or-", "Bearer\s+[A-Za-z0-9_\-\.]+", "Password=", "ApiKey=", "ServiceToken=")
foreach ($pattern in $secretPatterns) {
  if ($raw -match $pattern) { throw "Potential secret-like value detected by pattern: $pattern" }
}

Write-Host "Runtime release evidence validation PASS: $BundlePath"
