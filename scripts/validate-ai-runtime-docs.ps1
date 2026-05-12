#!/usr/bin/env pwsh
# Stale API / banned terms in AI substrate *documentation* (packages + ai-runtime guardrails).
# Complements validate-docs-api-names.ps1 (full docs tree) with an explicit file list for PR45.
$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path

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

function Test-GRpcAdapterLine([string]$line) {
    if ($line -notmatch 'gRPC adapter') { return $false }
    if ($line -match '(?i)\bno\s+gRPC\s+adapter') { return $true }
    if ($line -match '(?i)without\s+a\s+gRPC\s+adapter') { return $true }
    if ($line -match '(?i)not\s+.*gRPC\s+adapter') { return $true }
    return $false
}

$requiredFiles = @(
    (Join-Path $repoRoot 'docs/packages/Ontogony.AI.Contracts.md')
    (Join-Path $repoRoot 'docs/packages/Ontogony.Artifacts.md')
    (Join-Path $repoRoot 'docs/packages/Ontogony.Execution.md')
    (Join-Path $repoRoot 'docs/ai-runtime/boundary-guardrails.md')
    (Join-Path $repoRoot 'docs/ai-runtime/implementation-order.md')
)

foreach ($path in $requiredFiles) {
    if (-not (Test-Path -LiteralPath $path)) {
        throw "AI runtime docs gate: required file missing: $path"
    }
}

$failures = [System.Collections.Generic.List[string]]::new()

foreach ($path in $requiredFiles) {
    $lines = Get-Content -LiteralPath $path
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        if ($line -match 'docs-api-allow:') { continue }

        foreach ($term in $banned) {
            if (-not $line.Contains($term)) { continue }
            $failures.Add("${path}:$($i + 1): $term")
        }

        if ((Test-GRpcAdapterLine $line)) { continue }
        if ($line.Contains('gRPC adapter')) {
            $failures.Add("${path}:$($i + 1): gRPC adapter")
        }
    }
}

if ($failures.Count -gt 0) {
    Write-Host 'Banned stale API string in AI runtime docs:' -ForegroundColor Red
    foreach ($f in $failures) { Write-Host "  $f" }
    exit 1
}

Write-Host 'OK: AI runtime doc files pass banned-term scan.'
exit 0
