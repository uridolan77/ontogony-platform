#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Validates PLAT-9-005 observability pack artifacts (dashboard JSON + alert YAML structure).
#>
param(
    [string] $RepoRoot = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
}

$dashboardPath = Join-Path $RepoRoot "docs/observability/dashboards/grafana-dashboard-starter.json"
$alertsPath = Join-Path $RepoRoot "docs/observability/alerts/alerts.prometheus.rules.yml"

if (-not (Test-Path -LiteralPath $dashboardPath)) {
    throw "Missing dashboard JSON: $dashboardPath"
}

if (-not (Test-Path -LiteralPath $alertsPath)) {
    throw "Missing alert rules YAML: $alertsPath"
}

$dashboard = Get-Content -LiteralPath $dashboardPath -Raw | ConvertFrom-Json
if ([string]::IsNullOrWhiteSpace($dashboard.title)) {
    throw "Dashboard JSON missing title."
}
if ($dashboard.panels.Count -lt 1) {
    throw "Dashboard JSON must contain at least one panel."
}
if ($dashboard.uid -ne "ontogony-observability-mechanics") {
    throw "Dashboard uid must be ontogony-observability-mechanics."
}

$yaml = Get-Content -LiteralPath $alertsPath -Raw
$requiredGroups = @(
    "ontogony-platform-mechanics",
    "allagma-mechanics",
    "conexus-mechanics",
    "kanon-mechanics"
)
foreach ($group in $requiredGroups) {
    if ($yaml -notmatch "name:\s+$group") {
        throw "Alert YAML missing group: $group"
    }
}

if ($yaml -notmatch "alert:\s+") {
    throw "Alert YAML must define at least one alert rule."
}

Write-Host "PASS - observability pack artifacts validated"
Write-Host "  Dashboard: $dashboardPath ($($dashboard.panels.Count) panels)"
Write-Host "  Alerts:    $alertsPath"
exit 0
