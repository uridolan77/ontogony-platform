# Ontogony.Replay.Contracts — semantic contract

**Status:** Shipping (pre-1.0).

## Guarantees

- Stable DTOs for replay manifests and replay bundle references.
- `ReplayManifest.CreatedAt` uses `DateTimeOffset` for machine-comparable instants.
- Environment and determinism hint records.
- No execution engine or storage dependency.

## Does not guarantee

- Deterministic model output.
- Replay execution.
- Provider mocking.
- Workflow orchestration.

## Cross-service replay (REPLAY-RUNTIME-001/002)

HTTP vocabulary and DTOs are shared across Allagma orchestration, Kanon eligibility/bundles, and Conexus admin replay. Contract doc: [`docs/contracts/REPLAY_RUNTIME_CONTRACT.md`](../contracts/REPLAY_RUNTIME_CONTRACT.md). Allagma orchestration scope (what is wired vs deferred): `allagma-dotnet/docs/contracts/CROSS_SERVICE_REPLAY.md`.

## Conexus.NET use

Admin replay routes consume these contracts for eligibility and dry-run responses. Chat completion idempotency replay storage is separate — see `conexus-dotnet/docs/IDEMPOTENCY_AND_REPLAY.md`.
