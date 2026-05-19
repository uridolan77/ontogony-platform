param(
    [switch]$StartServices,
    [switch]$SkipFrontend,
    [string]$OutputPath = "docker/local-working-system/artifacts/docker-guided-main-flow-report.json"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ($StartServices) {
    & "$PSScriptRoot\start-local-working-system.ps1" -Build -SkipFrontend:$SkipFrontend
}

& "$PSScriptRoot\wait-local-working-system.ps1" -SkipFrontend:$SkipFrontend

# Reuse accepted ENV-SEED-001 seed verification first.
& "$PSScriptRoot\seed-and-verify-local-working-system.ps1"

# ENV-DOCKER-RUN-001 implementation should extend this stub with:
# - Allagma restart
# - re-fetch evaluation/comparison evidence
# - produce Docker guided machine report
# - validate report schema

Write-Host "ENV-DOCKER-RUN-001 stub complete. Extend in implementation PR."
