param(
    [string]$WorkspaceRoot = "C:\dev",
    [switch]$RequireAll,
    [switch]$WriteEvidence
)
$ErrorActionPreference = "Stop"
$services = @(
    @{ Name="kanon"; Port=5081; Ready="/ready"; Health="/health"; Expected="kanon" },
    @{ Name="conexus"; Port=5082; Ready="/ready"; Health="/health"; Expected="conexus" },
    @{ Name="allagma"; Port=5083; Ready="/ready"; Health="/health"; Expected="allagma" },
    @{ Name="metabole"; Port=5084; Ready="/ready"; Health="/health"; Expected="metabole" },
    @{ Name="aisthesis"; Port=5085; Ready="/ready"; Health="/health"; Expected="aisthesis" }
)
$results = @()
function Invoke-JsonOrText($uri) {
    try {
        $response = Invoke-WebRequest -UseBasicParsing -Uri $uri -TimeoutSec 8
        $text = $response.Content
        return @{ ok=$true; status=[int]$response.StatusCode; text=$text }
    } catch { return @{ ok=$false; status=$null; text=$_.Exception.Message } }
}
foreach ($svc in $services) {
    $base = "http://localhost:$($svc.Port)"
    $ready = Invoke-JsonOrText "$base$($svc.Ready)"
    $health = if (-not $ready.ok) { Invoke-JsonOrText "$base$($svc.Health)" } else { $null }
    $body = if ($ready.ok) { $ready.text } elseif ($health -and $health.ok) { $health.text } else { "" }
    $bodyString = ($body | Out-String).Trim()
    $identityOk = ($bodyString.ToLowerInvariant() -match $svc.Expected)
    if (-not $identityOk) {
        if ($svc.Name -eq "conexus" -and (Invoke-JsonOrText "$base/v1/models").status -in @(200,401,403)) { $identityOk = $true }
        if ($svc.Name -eq "allagma" -and (Invoke-JsonOrText "$base/allagma/v0/runs").status -in @(401,405)) { $identityOk = $true }
        if ($svc.Name -eq "metabole" -and (Invoke-JsonOrText "$base/metabole/v0/pipeline-runs/nonexistent").status -in @(401,404,405)) { $identityOk = $true }
        if ($svc.Name -eq "aisthesis" -and (Invoke-JsonOrText "$base/aisthesis/v0").status -in @(200,401,403,404,405)) { $identityOk = $true }
        if ($svc.Name -eq "kanon" -and (Invoke-JsonOrText "$base/ontology/v0/semantic-value-loop/status").status -in @(200,401,403,404,405)) { $identityOk = $true }
    }
    $status = if ($identityOk) { "pass" } elseif ($ready.ok -or ($health -and $health.ok)) { "identity_mismatch" } else { "unreachable" }
    $excerpt = ($bodyString -replace "\s+"," ")
    if ($excerpt.Length -gt 300) { $excerpt = $excerpt.Substring(0,300) }
    $results += [pscustomobject]@{ service=$svc.Name; port=$svc.Port; baseUrl=$base; expectedIdentity=$svc.Expected; status=$status; readyStatus=$ready.status; bodyExcerpt=$excerpt }
}
$overall = if (($results | Where-Object { $_.status -ne "pass" }).Count -eq 0) { "PASS" } elseif ($RequireAll) { "FAIL" } else { "PARTIAL" }
$summary = [pscustomobject]@{ schema="ontogony-runtime-service-identity/v1"; generatedAtUtc=(Get-Date).ToUniversalTime().ToString("o"); overallStatus=$overall; services=$results }
$summary | ConvertTo-Json -Depth 8 | Write-Host
if ($WriteEvidence) {
    $dir = Join-Path $WorkspaceRoot ("artifacts/runtime-port-lock/" + (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ"))
    New-Item -ItemType Directory -Force -Path $dir | Out-Null
    $summary | ConvertTo-Json -Depth 8 | Set-Content -Path (Join-Path $dir "service-identity-summary.json") -Encoding UTF8
}
if ($overall -eq "FAIL") { exit 1 }
