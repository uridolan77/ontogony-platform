# PR32 — Advanced HTTP Resilience Policies

## Goal

Upgrade `Ontogony.Http` from useful retry/circuit mechanics to a strong service-to-service integration layer.

## Scope

Add:

- named per-client policy options
- retry budget
- total timeout and per-attempt timeout
- richer retry metrics
- improved redaction hooks
- retry classification extension point

Suggested APIs:

```csharp
public interface IRetryClassifier
{
    RetryDecision ShouldRetry(HttpRequestMessage request, HttpResponseMessage? response, Exception? exception);
}
```

Options:

```csharp
public int RetryBudgetPerMinute { get; set; }
public TimeSpan? TotalTimeout { get; set; }
public TimeSpan? AttemptTimeout { get; set; }
public bool EmitAttemptMetrics { get; set; } = true;
```

## Must not do

- Do not implement provider routing.
- Do not add LLM-specific behavior.
- Do not retry unsafe requests without idempotency safeguards.

## Tests

- Retry budget stops further retries.
- Total timeout bounds full operation.
- Attempt timeout handles slow attempts.
- Classifier overrides defaults.
- Metrics emitted for attempts/retries/circuit-open.
- Sensitive headers/body fragments are redacted.

## Acceptance

A named integration client can be configured with explicit safe retry behavior and observable retry metrics.
