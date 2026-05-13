# In-memory startup warnings

## Goal

Warn on non-durable mechanics outside Development.

## Acceptance criteria

In-memory services warn/guard in Production.

## Boundary checklist

- [x] Reusable platform mechanics only.
- [x] No Conexus routing/provider/model semantics.
- [x] CI/build/package validation updated where relevant.

## Implementation notes

`AddOntogonyInMemoryArtifactStore`, `AddOntogonyInMemoryExecutionJournal`, `AddOntogonyInMemoryQuotaLedger`, and `AddOntogonyInMemoryOutboxStore` register an `IHostedService` that logs a **warning** on host start when `IHostEnvironment.IsDevelopment()` is false (Staging and Production). No exception is thrown.
