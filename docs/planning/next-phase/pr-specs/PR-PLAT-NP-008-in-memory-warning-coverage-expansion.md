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

- Each warned registration has a test for Production warning and Development silence.
- No warnings for pure fakes/test helpers that are not intended as service registrations.
