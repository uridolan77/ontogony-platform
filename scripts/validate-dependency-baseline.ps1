#!/usr/bin/env pwsh
# Enforces a single Microsoft.Extensions.* / Microsoft.AspNetCore.TestHost version line in Directory.Packages.props
# and basic sanity on global.json SDK. See docs/planning/robustness/DEPENDENCY_BASELINE.md.
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$propsPath = Join-Path $repoRoot 'Directory.Packages.props'
$globalJsonPath = Join-Path $repoRoot 'global.json'

if (-not (Test-Path -LiteralPath $propsPath)) {
    throw "Directory.Packages.props not found: $propsPath"
}

[xml]$propsXml = Get-Content -LiteralPath $propsPath -Raw
$pvs = foreach ($ig in @($propsXml.Project.ItemGroup)) {
    @($ig.PackageVersion)
}
$seen = @{}
foreach ($pv in $pvs) {
    if ($null -eq $pv) { continue }
    if ([string]::IsNullOrWhiteSpace($pv.Include)) { continue }
    $id = $pv.Include.Trim()
    if ($seen.ContainsKey($id)) {
        throw "Duplicate PackageVersion Include='$id' in Directory.Packages.props."
    }
    $seen[$id] = $pv.Version.Trim()
}

$microsoftBaseline = [System.Collections.Generic.List[string]]::new()
foreach ($kv in $seen.GetEnumerator()) {
    $id = $kv.Key
    if ($id -like 'Microsoft.Extensions.*' -or $id -eq 'Microsoft.AspNetCore.TestHost') {
        $microsoftBaseline.Add("$id=$($kv.Value)") | Out-Null
    }
}

$distinctMsVersions = @($seen.GetEnumerator() | Where-Object {
        $_.Key -like 'Microsoft.Extensions.*' -or $_.Key -eq 'Microsoft.AspNetCore.TestHost'
    } | Select-Object -ExpandProperty Value -Unique)

if ($distinctMsVersions.Count -eq 0) {
    throw 'No Microsoft.Extensions.* or Microsoft.AspNetCore.TestHost entries found in Directory.Packages.props.'
}
if ($distinctMsVersions.Count -gt 1) {
    $lines = $microsoftBaseline | Sort-Object
    throw @"
Microsoft baseline version drift in Directory.Packages.props.
All Microsoft.Extensions.* and Microsoft.AspNetCore.TestHost must use the SAME Version.

Distinct versions found: $($distinctMsVersions -join ', ')

Entries:
$($lines -join "`n")
"@
}

Write-Host "OK: Microsoft.Extensions.* / Microsoft.AspNetCore.TestHost aligned on version $($distinctMsVersions[0])."

if (-not (Test-Path -LiteralPath $globalJsonPath)) {
    throw "global.json not found: $globalJsonPath"
}

$gj = Get-Content -LiteralPath $globalJsonPath -Raw | ConvertFrom-Json
$sdkVersion = [string]$gj.sdk.version
if ([string]::IsNullOrWhiteSpace($sdkVersion)) {
    throw 'global.json: sdk.version is missing.'
}
if ($sdkVersion -notmatch '^9\.0\.') {
    throw "global.json: sdk.version '$sdkVersion' is expected to match ^9\.0\. for net9 baseline (see docs/FRAMEWORK_BASELINE.md)."
}

Write-Host "OK: global.json sdk.version=$sdkVersion"
exit 0
