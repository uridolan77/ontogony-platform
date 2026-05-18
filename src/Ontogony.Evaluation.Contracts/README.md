# Ontogony.Evaluation.Contracts

## What this is

Neutral evaluation DTOs for cross-repo eval runs:

- evaluation run and case records
- metric and score records
- baseline comparison records
- verdict and artifact references

## What this is not

- not an eval harness or scenario runner
- not scoring policy or rubrics with product meaning
- not topology or Kanon policy semantics
- not model routing or provider selection
- not a persistence provider

Use this when a service needs to record or exchange eval evidence in a shared wire format, not when defining how to score or promote topologies.
