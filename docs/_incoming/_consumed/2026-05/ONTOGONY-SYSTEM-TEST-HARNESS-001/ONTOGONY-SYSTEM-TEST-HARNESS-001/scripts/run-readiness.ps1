$ErrorActionPreference = "Stop"
Write-Host "Running Ontogony readiness checks..."
$services = @(
  @{ Name="Kanon"; Url=$env:KANON_BASE_URL ?? "http://localhost:5081" },
  @{ Name="Conexus"; Url=$env:CONEXUS_BASE_URL ?? "http://localhost:5082" },
  @{ Name="Allagma"; Url=$env:ALLAGMA_BASE_URL ?? "http://localhost:5083" },
  @{ Name="Metabole"; Url=$env:METABOLE_BASE_URL ?? "http://localhost:5084" },
  @{ Name="Aisthesis"; Url=$env:AISTHESIS_BASE_URL ?? "http://localhost:5085" }
)

foreach ($svc in $services) {
  $health = "$($svc.Url.TrimEnd('/'))/health"
  Write-Host "GET $health"
  try {
    $r = Invoke-WebRequest -Uri $health -UseBasicParsing -TimeoutSec 10
    Write-Host "  $($svc.Name): $($r.StatusCode)"
  } catch {
    Write-Warning "  $($svc.Name) health failed: $($_.Exception.Message)"
    throw
  }
}
