# PLAT-DEPTH-001 — HTTP resilience v1

**Date:** 2026-05-26  
**Package:** ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001

## Scope

Retry-After aware backoff, exponential backoff with jitter, per-attempt and total timeouts, circuit breaker, and conformance harness without product semantics.

## Implementation

| Component | Path |
| --- | --- |
| Handler | `src/Ontogony.Http/ResilientIntegrationDelegatingHandler.cs` |
| Registry | `src/Ontogony.Http/TransportResilienceRegistry.cs` |
| Options | `src/Ontogony.Http/TransportResilienceOptions.cs` |
| Harness | `src/Ontogony.Testing/HttpResilienceConformanceHarness.cs` |

## Tests

`tests/Ontogony.Infrastructure.Tests/ConformanceKitPr33Tests.cs`:

- `HttpResilience_NoRetryOnSuccess`
- `HttpResilience_RetriesOnTransientFailure`
- `HttpResilience_CircuitOpensAfterThreshold`
- `HttpResilience_RespectsRetryAfterHeader`
- `HttpResilience_TotalTimeoutLimitsAttempts`
- `HttpResilience_BackoffJitterWithinBounds`
- `HttpResilience_RespectsRetryAfterDateHeader`
- `HttpResilience_AttemptTimeoutContext`
- `HttpResilience_ExponentialBackoffLongerThanLinear`

`AdvancedHttpResilienceTests.RetryBypassingBudget_Overrides_Budget_Limit` (budget exhausted, bypass still retries).

Boundary: no product semantics in handler or harness fixtures.

## Validation

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj -c Release --filter "ConformanceKitPr33Tests.HttpResilience"
dotnet test tests/Ontogony.Http.Tests/Ontogony.Http.Tests.csproj -c Release
```
