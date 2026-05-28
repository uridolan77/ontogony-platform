param(
  [string]$AisthesisRepo = "C:\dev\aisthesis-dotnet"
)
Write-Host "Template: complete after producer smoke scripts exist."
Write-Host "Probe services on ports 5081-5085, run producer-native scenario, then:"
Push-Location $AisthesisRepo
try {
  ./scripts/system/run-five-service-aisthesis-live-smoke.ps1 -Mode Live
} finally {
  Pop-Location
}
