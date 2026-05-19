param(
    [switch]$ExpectConexusReadyAfterSeed
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$checks = @(
    @{ Name = "Kanon health"; Url = "http://localhost:5081/health"; Required = $true },
    @{ Name = "Conexus liveness"; Url = "http://localhost:5082/health/live"; Required = $true },
    @{ Name = "Allagma health"; Url = "http://localhost:5083/health"; Required = $true }
)

foreach ($check in $checks) {
    $r = Invoke-WebRequest -UseBasicParsing -TimeoutSec 5 -Uri $check.Url
    if ($r.StatusCode -lt 200 -or $r.StatusCode -ge 300) {
        throw "$($check.Name) failed: $($r.StatusCode)"
    }
    Write-Host "PASS $($check.Name)"
}

try {
    $ready = Invoke-WebRequest -UseBasicParsing -TimeoutSec 5 -Uri "http://localhost:5082/ready"
    Write-Host "Conexus /ready => $($ready.StatusCode)"
    if ($ExpectConexusReadyAfterSeed -and ($ready.StatusCode -lt 200 -or $ready.StatusCode -ge 300)) {
        throw "Conexus /ready expected ready after seed."
    }
} catch {
    if ($ExpectConexusReadyAfterSeed) {
        throw
    }
    Write-Host "INFO Conexus /ready is not ready before bootstrap; this is expected."
}

Write-Host "Local working system validation PASS."
