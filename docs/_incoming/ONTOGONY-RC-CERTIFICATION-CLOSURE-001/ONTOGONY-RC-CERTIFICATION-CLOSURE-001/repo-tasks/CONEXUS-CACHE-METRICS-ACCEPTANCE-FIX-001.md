# CONEXUS-CACHE-METRICS-ACCEPTANCE-FIX-001

## Owner repo

- `conexus-dotnet`

## Problem

The full Conexus gateway-hardening wrapper fails on `ConexusGatewayMetricsTests.RecordCache_instruments_emit_lookup_hit_and_miss`. Postgres idempotency is green. This is cache metrics instrumentation or assertion drift.

## Required implementation

1. Reproduce only the failing test.
2. Determine whether the issue is instrument emission, metric naming, tags, listener race/flakiness, or cache path behavior.
3. Fix instrumentation or test expectation.
4. Document the stable metric contract.

## Acceptance

```powershell
cd C:\dev\conexus-dotnet
dotnet test .	ests\Conexus.Api.Tests\Conexus.Api.Tests.csproj -c Release --filter "FullyQualifiedName~ConexusGatewayMetricsTests.RecordCache_instruments_emit_lookup_hit_and_miss"
pwsh .\scriptsun-conexus-gateway-hardening-acceptance.ps1 -UsePostgres
```
