# PR Spec — PLAT-TOPO-001 — Neutral Topology Contracts

## Repo

`ontogony-platform`

## Goal

Add neutral DTOs for task classification and topology selection.

## Add

```text
src/Ontogony.Topology.Contracts/
tests/Ontogony.Topology.Contracts.Tests/
docs/packages/Ontogony.Topology.Contracts.md
```

## Public contracts

```csharp
public sealed record TaskClassificationRecord;
public sealed record TopologySelectionRecord;
public sealed record TopologyConstraintRecord;
public static class StandardTopologyModes;
public static class StandardTaskClassifications;
```

## Standard vocabulary

```text
single_workflow
centralized_orchestrator
parallel_review
decentralized_research
hybrid_validation
deny_or_human_gate_first
```

## Important

These are names, not behavior. Ontogony.Platform must not interpret them.

## Tests

- JSON round-trip.
- static constants remain stable.
- no product dependencies.

## Acceptance

- package docs explain non-goals clearly.
- no evaluator, planner, or orchestrator included.
