# Migration Note — PR32 Advanced HTTP Resilience Policies

Date: 2026-05-12
PR: PR32

## Summary

PR32 upgrades `Ontogony.Http` resilience from basic retry/circuit mechanics to an advanced service-to-service integration layer with retry budgets, timeouts, and custom retry classification.

**Breaking changes**: None. All new features are opt-in; existing code continues to work unchanged.

## What was added

### Core abstractions

- `IRetryClassifier` interface — custom retry decision logic
- `RetryDecision` enum — `DoNotRetry`, `Retry`, `RetryBypassingBudget`
- `DefaultRetryClassifier` — standard implementation for backward compatibility
- `RetryBudgetTracker` — tracks per-client retry budgets per minute

### Extended options in `TransportResilienceOptions`

- `RetryBudgetPerMinute` (int) — max retries per client per minute; 0 disables
- `TotalTimeout` (TimeSpan?) — total time for all attempts + backoff; null disables
- `AttemptTimeout` (TimeSpan?) — per-attempt time limit; null disables
- `EmitAttemptMetrics` (bool) — default true; indicates metrics emission intent

### Enhanced registry API

- `TransportResilienceRegistry.TryConsumeRetryBudget(clientName, options)` — consume one retry from per-minute budget

### Enhanced handler

- `ResilientIntegrationDelegatingHandler` now accepts optional `IRetryClassifier` parameter
- Enforces total timeout via `CancellationTokenSource.CancelAfter()`
- Enforces per-attempt timeout with linked cancellation tokens
- Checks retry budget before each retry decision
- Classifies retry decisions via `IRetryClassifier.ShouldRetry()`

## API changes (backward compatible)

### Constructor

Old:
```csharp
new ResilientIntegrationDelegatingHandler(
    clientName, registry, options, clock)
```

New (optional classifier):
```csharp
new ResilientIntegrationDelegatingHandler(
    clientName, registry, options, clock, customClassifier)
```

The classifier parameter is optional; `DefaultRetryClassifier` is used if omitted.

## Migration path for consuming services

### No action required

Existing code using `Ontogony.Http` continues to work unchanged:
- Retry logic works as before (status codes, exceptions)
- Retry budget is disabled by default (budget = 0)
- Timeouts are disabled by default (null)
- Metrics flag defaults to true

### Recommended adoption

To leverage advanced resilience:

1. **Define retry budget** — set `RetryBudgetPerMinute` to limit retry storms:
   ```csharp
   options.RetryBudgetPerMinute = 10; // 10 retries/min/client
   ```

2. **Set timeouts** — protect against slow/hung requests:
   ```csharp
   options.TotalTimeout = TimeSpan.FromSeconds(30);
   options.AttemptTimeout = TimeSpan.FromSeconds(10);
   ```

3. **Custom retry logic** (optional) — implement domain-specific retry decisions:
   ```csharp
   public class MyRetryClassifier : IRetryClassifier
   {
       public RetryDecision ShouldRetry(
           HttpRequestMessage request,
           HttpResponseMessage? response,
           Exception? exception)
       {
           // Custom logic here
       }
   }

   var handler = new ResilientIntegrationDelegatingHandler(
       "my-client", registry, options, clock,
       new MyRetryClassifier());
   ```

### Athanor

- Apply `RetryBudgetPerMinute` to service-to-service clients to prevent thundering herd on canonicalization or snapshot queries.
- Use `TotalTimeout` + `AttemptTimeout` for external LLM provider calls.

### Agentor

- Apply budgets to plan/tool invocation HTTP clients.
- Consider custom `IRetryClassifier` for 4xx vs 5xx handling (e.g., don't retry 400 from plan validation).

### Conexus

- Apply budgets to routing/pricing queries.
- Use per-attempt timeout for model routing decisions under SLA.

## Test coverage

New test class `AdvancedHttpResilienceTests` covers:

- Retry budget exhaustion stops further retries
- Total timeout bounds operation lifecycle
- Per-attempt timeout treats slow requests as transient
- Custom classifier overrides default behavior
- `RetryBypassingBudget` decision bypasses budget limits
- Metrics flag can be queried for observability intent

## Breaking change assessment

- **API changes**: None (backward compatible)
- **Behavior changes**: Only when new options are set; default behavior unchanged
- **Schema changes**: None
- **Wire format**: None

## Non-breaking implementation notes

- `DefaultRetryClassifier` replicates old behavior so existing code sees no change
- Retry budget defaults to 0 (disabled) so no change in retry frequency
- Timeouts default to null (disabled) so no change in timeout behavior
- New handler constructor parameter is optional with sensible default
- Classifier decision `RetryBypassingBudget` is new; old code never used it

This PR is a pure addition of advanced features with full backward compatibility.
