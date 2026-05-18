# Ontogony.Evaluation.Contracts — semantic contract

**Status:** Shipping (pre-1.0).

## Guarantees

- Stable DTOs for evaluation runs, cases, metrics, scores, baseline comparisons, verdicts, and artifact references.
- Temporal instants use `DateTimeOffset`.
- Opaque string vocabularies for verdicts, metric kinds, profiles, and promotion recommendations.
- No eval engine, scoring policy, or storage dependency.

## Does not guarantee

- Scenario execution or baseline harness behavior.
- Topology authorization or Kanon policy semantics.
- Model routing or provider selection.
- Product-specific rubrics or promotion gates.

## Consumer use

Allagma, Conexus, Kanon, and ontogony-frontend should map product eval evidence into these records for cross-repo comparison and release gates. Callers own metric names, thresholds, and verdict vocabulary.
