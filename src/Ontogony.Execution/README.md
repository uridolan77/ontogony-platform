# Ontogony.Execution

Mechanical execution journal **DTOs** plus an append-only **`IExecutionJournal`** port and **`InMemoryExecutionJournal`** reference implementation (runs, steps, attempts, state transitions, checkpoints).

## What this package is

- Serialization-friendly records with **opaque string** kinds, statuses, and hashes.
- **`IExecutionJournal`** — append lines and list them by `runId` / `stepId` without workflow or validation policy.
- **`InMemoryExecutionJournal`** — thread-safe, single-process store; lists return **append order**.
- **`AddOntogonyInMemoryExecutionJournal()`** — registers the in-memory journal for tests and examples.

## What this package is not

- Not a workflow engine, agent planner, scheduler, human review policy, or enforced state machine.
- Not a durable or multi-node journal (no Postgres/blob provider here).
- Not lifecycle validation: DTOs and the in-memory journal do **not** require `CompletedAt >= StartedAt`, non-negative `AttemptNumber`, or coherent `Sequence` ordering; hosts add those rules when needed.

## Opaque conventions

Ontogony does not interpret `RunKind`, `Status`, `StepKey`, `FromState` / `ToState`, `ErrorCode`, checkpoint `Label` / `ResumeToken`, or `Metadata` keys.

## Checkpoint payloads and artifacts

`ExecutionCheckpointRecord.PayloadHash` remains an optional opaque fingerprint. **`PayloadArtifactId`** is an optional opaque string that may equal `ArtifactRef.ArtifactId` from **`Ontogony.Artifacts`** when large checkpoint bytes are stored out-of-line; this package stays artifact-type-free (no project reference to `Ontogony.Artifacts`).

## Using with envelopes

Wrap journal lines or aggregates in `OntogonyEnvelope<TPayload>` when emitting on the standard event pipeline (`Ontogony.Contracts`).

## See also

- Repository: `docs/ai-runtime/boundary-guardrails.md`, `docs/packages/Ontogony.Execution.md`, `docs/packages/index.md`.
