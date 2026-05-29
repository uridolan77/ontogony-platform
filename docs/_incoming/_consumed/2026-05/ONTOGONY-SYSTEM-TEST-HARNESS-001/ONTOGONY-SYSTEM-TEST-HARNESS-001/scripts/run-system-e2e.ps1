$ErrorActionPreference = "Stop"
Write-Host "Running ONTOGONY-SYSTEM-TEST-HARNESS-001 .NET system tests..."
Push-Location "$PSScriptRoot/../tests/dotnet"
try {
  dotnet test ./Ontogony.SystemTestHarness.sln --logger "trx;LogFileName=ontogony-system-tests.trx"
} finally {
  Pop-Location
}
