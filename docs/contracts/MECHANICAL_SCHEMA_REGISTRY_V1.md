# Mechanical schema registry v1

**Package:** `ONTOGONY-PLATFORM-MECHANICS-ONLY-CONFORMANCE-001`  
**Status:** promoted (2026-05-29)

## Scope

This registry contains neutral cross-service schema contracts only.

## Schemas

| Schema | Path | Purpose |
| --- | --- | --- |
| error-envelope | `schemas/mechanics/v1/error-envelope.schema.json` | Problem-details/error envelope shape |
| correlation-headers | `schemas/mechanics/v1/correlation-headers.schema.json` | Trace/correlation/header context |
| evidence-reference | `schemas/mechanics/v1/evidence-reference.schema.json` | Reference to evidence without interpreting it |
| idempotency-state | `schemas/mechanics/v1/idempotency-state.schema.json` | Ledger state and replay-safe response mechanics |
| replay-contract | `schemas/mechanics/v1/replay-contract.schema.json` | Replay request/manifest mechanics, not replay engine |
| actor-context | `schemas/mechanics/v1/actor-context.schema.json` | Actor identity propagation |
| observability-meter | `schemas/mechanics/v1/observability-meter.schema.json` | Meter naming descriptor |
| consumer-conformance-report | `schemas/mechanics/v1/consumer-conformance-report.schema.json` | Harness output |
| platform-proposal-gate | `schemas/mechanics/v1/platform-proposal-gate.schema.json` | Mechanics-only proposal record |

## Fixtures

Valid and invalid examples live under `fixtures/mechanics/v1/`.

## Validation

```powershell
.\scripts\conformance\Test-MechanicalSchemaRegistry.ps1 -RepoRoot .
dotnet test tests/Ontogony.SystemCompatibility.Tests --filter FullyQualifiedName~MechanicalSchemaRegistryTests
```

## Versioning

- Schemas live under `schemas/mechanics/v1/`.
- Breaking changes require `v2`.
- Additive optional fields may remain in v1.
- Required-field changes are breaking.
- Product-specific enums are forbidden.

## Related

- [`PLATFORM_MECHANICS_ONLY_PROPOSAL_GATE.md`](../governance/PLATFORM_MECHANICS_ONLY_PROPOSAL_GATE.md)
- [`CONSUMER_CONFORMANCE_SUITE_V1.md`](../conformance/CONSUMER_CONFORMANCE_SUITE_V1.md)
- [`MECHANICAL_PROTOCOL_REGISTRY.md`](./MECHANICAL_PROTOCOL_REGISTRY.md)

