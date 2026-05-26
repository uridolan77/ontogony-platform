# Repo touchpoints

## allagma-dotnet

Expected files to inspect:

```text
src/Allagma.Contracts/RunDecisionEventsContracts.cs
src/Allagma.Application/AllagmaDecisionEventProjector.cs
src/Allagma.Application/GetRunDecisionEventsService.cs
src/Allagma.Api/DecisionEventsEndpoints.cs
tests/Allagma.Tests/RunDecisionEventsTests.cs
docs/generated/ALLAGMA_V0_ROUTE_INVENTORY.json
docs/system/ALLAGMA_FEATURE_CONNECTION_MATRIX.md
docs/system/allagma-feature-connection.matrix.json
docs/CURRENT_STATE.md
docs/KNOWN_LIMITATIONS.md
```

Likely new files:

```text
tests/Allagma.Tests/Reconstructability/AllagmaKanonClassifierIntegrationTests.cs
tests/Allagma.Tests/Fixtures/reconstructability/allagma-decision-events.fixture.json
docs/evidence/ALLAGMA_KANON_RECONSTRUCTABILITY_CLOSURE.md
scripts/system/run-reconstructability-golden-trace.ps1
scripts/system/validate-reconstructability-golden-trace.ps1
```

## kanon-dotnet

Expected files to inspect:

```text
src/Kanon.Application/Reconstructability/*
src/Kanon.Contracts/Reconstructability/*
src/Kanon.Api/*Reconstructability*
tests/Kanon.Tests/*Reconstructability*
docs/CURRENT_STATE.md
docs/KNOWN_LIMITATIONS.md
docs/generated/ONTOLOGY_V0_ROUTE_INVENTORY.json
docs/generated/KANON_COMPATIBILITY_MANIFEST.json
```

Likely new files:

```text
tests/Kanon.Tests/Reconstructability/ExternalDecisionEventFixturesTests.cs
examples/reconstructability/allagma-decision-events.fixture.json
examples/reconstructability/conexus-decision-events.fixture.json
docs/evidence/KANON_EXTERNAL_RECONSTRUCTABILITY_CLASSIFICATION.md
```

## conexus-dotnet

Expected files to inspect:

```text
src/Conexus.Contracts/*
src/Conexus.Application/*
src/Conexus.Api/*
docs/CURRENT_STATE.md
docs/KNOWN_LIMITATIONS.md
docs/API.md
docs/TESTING.md
docs/generated/*
```

Likely new files:

```text
src/Conexus.Contracts/Reconstructability/ConexusDecisionEventContracts.cs
src/Conexus.Application/Reconstructability/ConexusDecisionEventProjector.cs
src/Conexus.Api/ReconstructabilityEndpoints.cs
tests/Conexus.Tests/Reconstructability/ConexusDecisionEventProjectorTests.cs
tests/Conexus.Tests/Reconstructability/ConexusKanonClassifierIntegrationTests.cs
docs/contracts/CONEXUS_DECISION_EVENTS_V1.md
docs/evidence/CONEXUS_RECONSTRUCTABILITY_EMITTERS_EVIDENCE.md
```

## ontogony-platform

Expected files to inspect:

```text
src/Ontogony.*/*
tests/*
docs/packages/*
docs/adoption/*
docs/contracts/*
```

Likely new files:

```text
src/Ontogony.Testing/Conformance/ReconstructabilityConformanceKit.cs
src/Ontogony.Testing/Conformance/CorrelationConformanceKit.cs
src/Ontogony.Testing/Conformance/ErrorEnvelopeConformanceKit.cs
docs/adoption/reconstructability-conformance-kits.md
docs/evidence/PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md
```
