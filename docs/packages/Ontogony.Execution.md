# Ontogony.Execution

Mechanical execution journal **records**, an append-only **`IExecutionJournal`** port, and a thread-safe **`InMemoryExecutionJournal`** for tests and single-process hosts.

## What this package is

- **DTOs:** `ExecutionRunRecord`, `ExecutionStepRecord`, `ExecutionAttemptRecord`, `ExecutionStateTransitionRecord`, `ExecutionCheckpointRecord` — opaque string vocabulary; optional `Metadata` dictionaries; deterministic JSON via `Ontogony.Hashing.CanonicalJson` in consuming code.
- **Journal port:** `IExecutionJournal` — `Append*` for each line type; `GetRunAsync`; `ListStepsAsync`, `ListAttemptsAsync`, `ListTransitionsAsync`, `ListCheckpointsAsync`.
- **Reference store:** `InMemoryExecutionJournal` — rejects duplicate primary ids with `InvalidOperationException`; list APIs return **append order** (not sorted by `Sequence` or wall clock unless you sort).
- **DI:** `ExecutionJournalServiceCollectionExtensions.AddOntogonyInMemoryExecutionJournal()`.

## What this package is not

- **Not** a workflow engine, agent planner, tool-choice layer, scheduler, or human review policy.
- **Not** a state machine that validates transitions or statuses.
- **Not** a durable provider (no filesystem, Postgres, or cloud blob port in this package).
- **Not** cross-field validation on DTOs: for example, negative `AttemptNumber`, out-of-order `Sequence`, or `CompletedAt` before `StartedAt` are **not** rejected by contracts or the in-memory journal; add host-side validation when you need stricter telemetry.

## Checkpoint payloads and `Ontogony.Artifacts`

Large checkpoint blobs should live in **`Ontogony.Artifacts`** (or another store). Use:

- `ExecutionCheckpointRecord.PayloadHash` — optional fingerprint over bytes or canonical JSON.
- `ExecutionCheckpointRecord.PayloadArtifactId` — optional opaque id, often the same string as `ArtifactRef.ArtifactId` from `Ontogony.Artifacts`.

`Ontogony.Execution` does **not** reference `Ontogony.Artifacts` so the package stays dependency-light and avoids cycles.

## How to use

1. Append a run, then steps, attempts, transitions, and checkpoints as your host observes them.
2. Query with `GetRunAsync` / `List*` for projections, replay, or UI.
3. Optionally emit each append or snapshot through `OntogonyEnvelope<TPayload>` (`Ontogony.Contracts`).

```csharp
using Ontogony.Execution;

var journal = new InMemoryExecutionJournal();
await journal.AppendRunAsync(new ExecutionRunRecord
{
    RunId = "run-1",
    Status = "running",
    CreatedAt = DateTimeOffset.UtcNow
});
```

## Duplicate ids

A second append that reuses an existing `RunId`, `StepId`, `AttemptId`, `TransitionId`, or `CheckpointId` throws **`InvalidOperationException`**. This is an integrity guard, not product policy.

## See also

- `docs/ai-runtime/boundary-guardrails.md`
- [Ontogony.Artifacts](Ontogony.Artifacts.md) — payload-by-reference
- [Package index](index.md)
