# Durable evaluation and client coverage

## Current risk

If evaluation is launched via fire-and-forget background task after envelope ingestion, evaluation results can be lost on process crash, startup shutdown, unobserved exception, or cancellation mismatch.

## Target

Aisthesis evaluation should become durable enough for RC evidence:

```text
evidence ingested -> evaluation job scheduled -> job runs -> result persisted -> latest query stable
```

## Minimal implementation

- `EvaluationJob` entity.
- `IEvaluationJobStore` abstraction.
- In-memory implementation.
- Optional Postgres implementation/migration.
- Job status: `pending`, `running`, `succeeded`, `failed`, `retry_exhausted`.
- Retry count and last error.
- Explicit scheduling on envelope/batch ingestion.
- Evaluation failure visible in latest trace evaluation route.

## Client coverage

Evaluation routes must either be covered by `Aisthesis.Client` or explicitly marked server-only/internal.

Preferred public client methods:

```csharp
Task<EvaluationRunResponse> StartEvaluationRunAsync(string traceId, CancellationToken ct = default);
Task<EvaluationRunResponse?> GetEvaluationRunAsync(string evaluationRunId, CancellationToken ct = default);
Task<IReadOnlyList<EvaluationRunResponse>> GetTraceEvaluationRunsAsync(string traceId, CancellationToken ct = default);
Task<EvaluationRunResponse?> GetLatestTraceEvaluationRunAsync(string traceId, CancellationToken ct = default);
```
