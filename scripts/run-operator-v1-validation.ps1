#!/usr/bin/env pwsh
<#
.SYNOPSIS
  FIRST-VERSION-RC-001 — cross-repo validation runner for Operator V1 RC.

.DESCRIPTION
  Runs focused unit tests, protocol/stale/real-tools validators, frontend check,
  and operator demo Playwright (mocked). Docker cohesion/restart are documented
  as manual gates using existing ALPHA-006 artifacts unless -IncludeDocker is set.
#>
param(
    [string] $DevRoot = "C:\dev",
    [switch] $SkipDocker,
    [switch] $SkipPlaywright,
    [switch] $ReleaseMode
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

function Step($Name, [scriptblock]$Body) {
    Write-Host "`n=== $Name ===" -ForegroundColor Cyan
    & $Body
}

Step "Platform" {
    Push-Location (Join-Path $DevRoot "ontogony-platform")
    try {
        dotnet test Ontogony.Platform.sln -c Release --no-restore 2>$null
        if ($LASTEXITCODE -ne 0) { dotnet test Ontogony.Platform.sln -c Release }
        ./scripts/validate-system-protocol-registry.ps1
        ./scripts/validate-stale-incoming-package.ps1
        ./scripts/validate-real-tools-block.ps1
        ./scripts/validate-post-lock-delta-register.ps1
        $lockArgs = @("./scripts/validate-operator-v1-lock.ps1", "-RequireEvidence")
        if ($ReleaseMode) { $lockArgs += "-ReleaseMode" }
        & @lockArgs
    }
    finally {
        Pop-Location
    }
}

Step "Allagma" {
    Push-Location (Join-Path $DevRoot "allagma-dotnet")
    try {
        dotnet test Allagma.sln -c Release
        ./scripts/validate-real-tools-block.ps1
        ./scripts/validate-feature-connection-matrix.ps1
        ./scripts/validate-runtime-lock.ps1 -RequireEvidence
    }
    finally {
        Pop-Location
    }
}

Step "Kanon" {
    Push-Location (Join-Path $DevRoot "kanon-dotnet")
    try { dotnet test -c Release }
    finally { Pop-Location }
}

Step "Conexus" {
    Push-Location (Join-Path $DevRoot "conexus-dotnet")
    try { dotnet test -c Release }
    finally { Pop-Location }
}

Step "UI pack" {
    Push-Location (Join-Path $DevRoot "ontogony-ui")
    try {
        npm run build 2>&1 | Out-Null
        New-Item -ItemType Directory -Force -Path .tmp-pack | Out-Null
        npm pack --pack-destination .tmp-pack 2>&1 | Out-Null
    }
    finally { Pop-Location }
}

Step "Frontend" {
    Push-Location (Join-Path $DevRoot "ontogony-frontend")
    try {
        npm install 2>&1 | Out-Null
        npm run openapi:sync:allagma 2>&1 | Out-Null
        npm run openapi:gen 2>&1 | Out-Null
        npm run check
        if (-not $SkipPlaywright) {
            Get-NetTCPConnection -LocalPort 5176 -ErrorAction SilentlyContinue |
                ForEach-Object { Stop-Process -Id $_.OwningProcess -Force -ErrorAction SilentlyContinue }
            Start-Sleep -Seconds 1
            npm run test:e2e:demo-flows
        }
    }
    finally { Pop-Location }
}

Step "UI check" {
    Push-Location (Join-Path $DevRoot "ontogony-ui")
    try { npm run check }
    finally { Pop-Location }
}

if (-not $SkipDocker) {
    Step "Docker gates (manual)" {
        Write-Host "Use existing SYSTEM-ALPHA-006 cohesion/restart/observability artifacts when stack is up."
        Write-Host "See docs/operators/OPERATOR_V1_DEMO_GUIDE.md and docker/local-working-system/README.md"
    }
}

Write-Host "`nOperator V1 validation runner finished." -ForegroundColor Green
