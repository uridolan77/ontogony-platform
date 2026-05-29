param(
    [string]$WorkspaceRoot = "C:\dev",
    [switch]$UseExistingServices
)
$ErrorActionPreference = "Stop"
$timestamp = (Get-Date).ToUniversalTime().ToString("yyyyMMddTHHmmssZ")
$artifactRoot = Join-Path $WorkspaceRoot "artifacts/ontogony-rc-certification/$timestamp"
New-Item -ItemType Directory -Force -Path $artifactRoot | Out-Null
$steps = New-Object System.Collections.Generic.List[object]
function Add-Step($name, $repo, $command, $status, $detail = $null) { $steps.Add([pscustomobject]@{ name=$name; repo=$repo; command=$command; status=$status; detail=$detail }) }
function Run-InRepo($name, $repoPath, $scriptBlockText, [switch]$Required) {
    Push-Location $repoPath
    try {
        Write-Host "=== $name ===" -ForegroundColor Cyan
        pwsh -NoProfile -Command $scriptBlockText
        if ($LASTEXITCODE -ne 0) { throw "Exit code $LASTEXITCODE" }
        Add-Step $name $repoPath $scriptBlockText "PASS"
    } catch {
        Add-Step $name $repoPath $scriptBlockText "FAIL" $_.Exception.Message
        if ($Required) { throw }
    } finally { Pop-Location }
}
$verifyScript = Join-Path $WorkspaceRoot "ontogony-platform/scripts/system/verify-ontogony-runtime-service-identity.ps1"
if ($UseExistingServices) { Run-InRepo "runtime-service-identity" $WorkspaceRoot "pwsh -NoProfile -File '$verifyScript' -WorkspaceRoot '$WorkspaceRoot' -RequireAll -WriteEvidence" -Required }
$repoTestCommands = @(
    @{ repo="allagma-dotnet"; command="dotnet test Allagma.sln -c Release" },
    @{ repo="kanon-dotnet"; command="dotnet test Kanon.sln -c Release" },
    @{ repo="conexus-dotnet"; command="dotnet test Conexus.sln -c Release -p:NoWarn=CS1591 --filter 'Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke&Category!=CapacityBaseline'" },
    @{ repo="metabole-dotnet"; command="dotnet test -c Release" },
    @{ repo="aisthesis-dotnet"; command="dotnet test -c Release" }
)
foreach ($r in $repoTestCommands) {
    $path = Join-Path $WorkspaceRoot $r.repo
    if (Test-Path $path) { Run-InRepo "dotnet-test-$($r.repo)" $path $r.command -Required } else { throw "Missing repo: $path" }
}
Run-InRepo "allagma-package-mode" (Join-Path $WorkspaceRoot "allagma-dotnet") "pwsh .\scripts\run-package-mode-build.ps1" -Required
Run-InRepo "conexus-gateway-hardening" (Join-Path $WorkspaceRoot "conexus-dotnet") "pwsh .\scripts\run-conexus-gateway-hardening-acceptance.ps1 -UsePostgres" -Required
if (-not $UseExistingServices) {
    Run-InRepo "start-five-service-stack" (Join-Path $WorkspaceRoot "allagma-dotnet") "pwsh .\scripts\run-five-service-stack.ps1 -DevRoot '$WorkspaceRoot'" -Required
    Start-Sleep -Seconds 10
}
Run-InRepo "runtime-service-identity-after-stack" $WorkspaceRoot "pwsh -NoProfile -File '$verifyScript' -WorkspaceRoot '$WorkspaceRoot' -RequireAll -WriteEvidence" -Required
$aisthesisLiveCommand = @"
if (-not `$env:AISTHESIS_LIVE_WORKFLOW_TRIGGER_URL) {
  `$env:AISTHESIS_LIVE_WORKFLOW_TRIGGER_URL = 'http://localhost:5083/allagma/v0/runs'
}
if (-not `$env:AISTHESIS_LIVE_WORKFLOW_TRIGGER_PROFILE) {
  `$env:AISTHESIS_LIVE_WORKFLOW_TRIGGER_PROFILE = 'allagma-metabole-orchestration'
}
if (-not `$env:AISTHESIS_ALLAGMA_SERVICE_TOKEN) {
  `$env:AISTHESIS_ALLAGMA_SERVICE_TOKEN = 'allagma-dev-service-token-change-in-production'
}
pwsh .\scripts\system\run-five-service-live-certification.ps1 -Mode Live
"@
Run-InRepo "aisthesis-five-service-live-cert" (Join-Path $WorkspaceRoot "aisthesis-dotnet") $aisthesisLiveCommand -Required
Run-InRepo "allagma-system-cohesion" (Join-Path $WorkspaceRoot "allagma-dotnet") "pwsh .\scripts\system\run-system-cohesion-acceptance.ps1 -UseExistingServices -Quick" -Required
$summary = [pscustomobject]@{ schema="ontogony-rc-certification-rerun/v1"; generatedAtUtc=(Get-Date).ToUniversalTime().ToString("o"); artifactRoot=$artifactRoot; productionReadiness="NOT_CLAIMED"; overallStatus= if (($steps | Where-Object { $_.status -ne "PASS" }).Count -eq 0) { "PASS" } else { "FAIL" }; steps=$steps }
$summary | ConvertTo-Json -Depth 10 | Set-Content -Path (Join-Path $artifactRoot "rc-certification-rerun-summary.json") -Encoding UTF8
$summary | ConvertTo-Json -Depth 10 | Write-Host
if ($summary.overallStatus -ne "PASS") { exit 1 }
