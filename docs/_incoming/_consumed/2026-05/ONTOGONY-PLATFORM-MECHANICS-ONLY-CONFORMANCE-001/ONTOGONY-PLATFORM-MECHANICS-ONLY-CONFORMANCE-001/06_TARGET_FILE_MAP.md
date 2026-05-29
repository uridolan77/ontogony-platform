# Target file map

## Platform docs

| Target | Purpose |
|---|---|
| `docs/governance/PLATFORM_MECHANICS_ONLY_PROPOSAL_GATE.md` | Mechanics-only proposal policy |
| `docs/conformance/CONSUMER_CONFORMANCE_SUITE_V1.md` | Consumer harness guide |
| `docs/contracts/MECHANICAL_SCHEMA_REGISTRY_V1.md` | Registry of neutral schemas |
| `docs/contracts/OBSERVABILITY_METER_NAMING_CONTRACT.md` | Cross-service meter naming rules |
| `docs/contracts/NO_PRODUCT_SEMANTICS_CONTRACT.md` | Semantic leak guard |
| `docs/status/ONTOGONY_PLATFORM_MECHANICS_ONLY_CONFORMANCE_001_STATUS.md` | Status update |
| `docs/evidence/ONTOGONY_PLATFORM_MECHANICS_ONLY_CONFORMANCE_001_CLOSEOUT.md` | Final evidence |

## Platform schemas

| Target | Purpose |
|---|---|
| `schemas/mechanics/v1/error-envelope.schema.json` | Cross-service error envelope |
| `schemas/mechanics/v1/correlation-headers.schema.json` | Trace/correlation/actor headers |
| `schemas/mechanics/v1/evidence-reference.schema.json` | Neutral reference to evidence |
| `schemas/mechanics/v1/idempotency-state.schema.json` | Idempotency ledger state |
| `schemas/mechanics/v1/replay-contract.schema.json` | Replay request/manifest contract |
| `schemas/mechanics/v1/actor-context.schema.json` | Actor context transfer |
| `schemas/mechanics/v1/observability-meter.schema.json` | Meter descriptor |
| `schemas/mechanics/v1/consumer-conformance-report.schema.json` | Harness output |

## Scripts

| Target | Purpose |
|---|---|
| `scripts/governance/check-platform-mechanics-only.ps1` | Proposal + repo semantic-leak gate |
| `scripts/conformance/run-consumer-conformance-suite.ps1` | Main conformance runner |
| `scripts/conformance/Test-HeaderPropagationConformance.ps1` | Header propagation |
| `scripts/conformance/Test-ErrorEnvelopeConformance.ps1` | Error envelopes |
| `scripts/conformance/Test-IdempotencyConformance.ps1` | Idempotency |
| `scripts/conformance/Test-OutboxArtifactConformance.ps1` | Outbox/artifact mechanics |
| `scripts/conformance/Test-ObservabilityMeterNaming.ps1` | Meters |
| `scripts/conformance/Test-NoProductSemantics.ps1` | Semantic leak scan |
| `scripts/conformance/Test-MechanicalSchemaRegistry.ps1` | Schema validation |
| `scripts/conformance/New-ConsumerConformanceReport.ps1` | Summary generator |

## Tests

| Target | Purpose |
|---|---|
| `tests/Ontogony.SystemCompatibility.Tests/MechanicsOnlyProposalGateTests.cs` | Proposal gate |
| `tests/Ontogony.SystemCompatibility.Tests/MechanicalSchemaRegistryTests.cs` | Schema registry |
| `tests/Ontogony.Testing.Tests/ConsumerConformanceHarnessTests.cs` | Harness behavior |
| `tests/Ontogony.Observability.Tests/MeterNamingConformanceTests.cs` | Meter naming |
