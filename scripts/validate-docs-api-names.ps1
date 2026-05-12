#!/usr/bin/env pwsh
# Fails if banned (obsolete or misleading) API strings appear in current docs.
# Excludes: CHANGELOG.md, docs/migrations/**, docs/planning/**, docs/_incoming/**
# Per-line allow: <!-- docs-api-allow: ExactTerm -->
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$docsRoot = Join-Path $repoRoot 'docs'

function Test-ExcludedPath([string]$fullPath) {
    $rel = $fullPath.Substring($repoRoot.Length).TrimStart([char[]]@('\', '/'))
    $norm = $rel -replace '\\', '/'
    if ($norm -ieq 'CHANGELOG.md') { return $true }
    if ($norm.StartsWith('docs/migrations/', [System.StringComparison]::OrdinalIgnoreCase)) { return $true }
    if ($norm.StartsWith('docs/planning/', [System.StringComparison]::OrdinalIgnoreCase)) { return $true }
    if ($norm.StartsWith('docs/_incoming/', [System.StringComparison]::OrdinalIgnoreCase)) { return $true }
    return $false
}

# Substrings or terms that should not appear in instructional / reference docs.
$banned = @(
    'UseRequestTracingMiddleware'
    'AddIntegrationHttpClient<'
    'ConfigureHttpClientDefaults'
    'MaxAttempts'
    'OpenCircuitAfterConsecutiveFailures'
    'CircuitBreakerResetTimeout'
    'ProtocolNames.Internal'
    'OntogonySecurityOptions'
    'OntogonyClaimsValidator'
    'UseOntogonySignatureValidation'
    'IEventPublisher<'
    'IEventSubscriber<'
    'CanonicalJsonHasher'
    'AddPostgresContext'
    'ProtocolIngress.ToProto'
    'grpcClient.PublishAsync'
)

# "gRPC adapter" is often used correctly as "No gRPC adapter"; allow that negation.
function Test-GRpcAdapterLine([string]$line) {
    if ($line -notmatch 'gRPC adapter') { return $false }
    if ($line -match '(?i)\bno\s+gRPC\s+adapter') { return $true }
    if ($line -match '(?i)without\s+a\s+gRPC\s+adapter') { return $true }
    if ($line -match '(?i)not\s+.*gRPC\s+adapter') { return $true }
    return $false
}

$failures = [System.Collections.Generic.List[string]]::new()

Get-ChildItem -LiteralPath $docsRoot -Recurse -File -Filter '*.md' | ForEach-Object {
    $file = $_
    $full = $file.FullName
    if (Test-ExcludedPath $full) { return }

    $lines = Get-Content -LiteralPath $full
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        if ($line -match 'docs-api-allow:') { continue }

        foreach ($term in $banned) {
            if (-not $line.Contains($term)) { continue }
            $failures.Add("$($file.FullName):$($i + 1): $term")
        }

        if ((Test-GRpcAdapterLine $line)) { continue }
        if ($line.Contains('gRPC adapter')) {
            $failures.Add("$($file.FullName):$($i + 1): gRPC adapter")
        }
    }
}

if ($failures.Count -gt 0) {
    Write-Host 'Banned API name or stale term in docs:' -ForegroundColor Red
    foreach ($f in $failures) { Write-Host "  $f" }
    exit 1
}

Write-Host 'OK: no banned stale API strings in scanned docs.'
exit 0
