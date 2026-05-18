# Ontogony.Topology.Contracts — semantic contract

**Status:** Shipping (pre-1.0).

## Guarantees

- Stable DTOs for task classification, topology selection, and topology constraints.
- Well-known string constants for standard topology modes and task classifications (names only).
- Opaque string vocabularies for risk levels, validation modes, and selector versions.
- No planner, orchestrator, or policy engine dependency.

## Does not guarantee

- Task classification or topology selection behavior.
- Kanon topology authorization or domain-pack policy semantics.
- Eval harness, baseline comparison, or promotion gates.
- Model routing or provider selection.

## Consumer use

Allagma, Kanon, Conexus, and ontogony-frontend should map product topology evidence into these records for audit bundles, events, and cross-repo queries. Callers own classifier rules, selector logic, and authorization outcomes.
