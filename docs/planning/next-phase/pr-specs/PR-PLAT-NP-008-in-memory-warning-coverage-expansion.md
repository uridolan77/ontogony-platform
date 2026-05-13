# PR-PLAT-NP-008 — In-memory warning coverage expansion

## Goal

Ensure all non-durable platform in-memory registrations warn outside Development, not only artifact store.

## Scope

- Add tests for:
  - `AddOntogonyInMemoryExecutionJournal`
  - `AddOntogonyInMemoryQuotaLedger`
  - `AddOntogonyInMemoryOutboxStore`
  - any other non-durable in-memory store exposed as a production-tempting registration.
- Confirm warnings include mechanism name and durable replacement guidance.

## Acceptance

- Each **current** warned `AddOntogonyInMemory*` registration has a test for Production warning and Development silence (baseline landed with PLAT-NP-010; expand when new registrations appear).
- No warnings for pure fakes/test helpers that are not intended as service registrations.

## Inventory (PLAT-NP-010 groundwork)

| Implementation | Registration | Warn outside Development? | Notes |
| --- | --- | --- | --- |
| `InMemoryArtifactStore` | `AddOntogonyInMemoryArtifactStore` | **Yes** | Tests in `Ontogony.Artifacts.Tests` |
| `InMemoryExecutionJournal` | `AddOntogonyInMemoryExecutionJournal` | **Yes** | Tests in `Ontogony.Execution.Tests` |
| `InMemoryQuotaLedger` | `AddOntogonyInMemoryQuotaLedger` | **Yes** | Tests in `Ontogony.Quotas.Tests` |
| `InMemoryOutboxStore` | `AddOntogonyInMemoryOutboxStore` | **Yes** | Tests in `Ontogony.Infrastructure.Tests` |
| `InMemoryIdempotencyLedger` | *(none — construct directly / tests)* | **No** | No `AddOntogonyInMemory*` DI sugar; not a startup-registered fake |
| In-process messaging / event publisher | *(varies)* | **N/A** | Out of scope for this warning-host pattern unless a future registration mimics production durability |

## Non-goals

- Do not add durable quota or outbox implementations in this item.
