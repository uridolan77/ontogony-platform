# Patch guide — durable evaluation jobs

1. Inspect current `EvaluationService` and endpoint flow.
2. Add an `EvaluationJob` domain/application model only if no equivalent exists.
3. Add `IEvaluationJobStore` with in-memory implementation first.
4. Wire batch/envelope ingestion to schedule jobs instead of `Task.Run` where practical.
5. Add Postgres implementation/migration only if package scope allows.
6. Preserve synchronous manual evaluation endpoint behavior.
7. Add tests for failure visibility and latest evaluation stability.

Do not make evaluation required for evidence ingestion success unless explicitly intended.
