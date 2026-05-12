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

## Conexus.NET use

Use later for request replay/debug bundles and incident reconstruction.
