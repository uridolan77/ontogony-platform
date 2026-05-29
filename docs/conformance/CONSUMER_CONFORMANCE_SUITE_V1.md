# Consumer conformance suite v1

**Package:** `ONTOGONY-PLATFORM-MECHANICS-ONLY-CONFORMANCE-001`  
**Status:** promoted (2026-05-29)

## Purpose

Provide a uniform harness that product repos can run to prove they obey shared mechanical contracts.

## Conformance dimensions

1. Header propagation.
2. Error envelope.
3. Idempotency.
4. Outbox/artifact mechanics.
5. Observability meter naming.
6. No-product-semantics leakage.
7. Actor context propagation (via header contract).
8. Mechanical schema compatibility.

## Run locally (platform fixture mode)

```powershell
.\scripts\conformance\run-consumer-conformance-suite.ps1 -PlatformRoot . -ConsumerName platform -FixtureMode
```

## Run for a product consumer

```powershell
.\scripts\conformance\run-consumer-conformance-suite.ps1 `
  -PlatformRoot C:\dev\ontogony-platform `
  -ConsumerRoot C:\dev\conexus-dotnet `
  -ConsumerName conexus `
  -FixtureMode
```

Per-consumer task cards: [`task-cards/`](./task-cards/).

## Modes

| Mode | Description |
| --- | --- |
| Static | Reads source/docs/config only |
| Fixture | Executes local test fixtures, no services |
| LocalService | Calls local service endpoints (consumer-owned) |
| LiveStack | Optional, only with explicit operator env |

## Required output

Every run writes `summary.json` matching `schemas/mechanics/v1/consumer-conformance-report.schema.json` under:

```text
artifacts/platform-mechanics-conformance/<consumer>/<timestamp>/summary.json
```

## Status semantics

- `PASS`: all required checks passed.
- `PARTIAL`: some checks unavailable but no violation found.
- `FAIL`: at least one required check violated.
- `NOT_RUN`: repo/config unavailable.

## Related

- Existing PLAT-9-003 runner: `scripts/run-consumer-conformance.ps1`
- [`MECHANICAL_SCHEMA_REGISTRY_V1.md`](../contracts/MECHANICAL_SCHEMA_REGISTRY_V1.md)

