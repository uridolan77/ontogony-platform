# Ontogony.Topology.Contracts

## What this is

Neutral topology DTOs for cross-repo execution-shape evidence:

- task classification records
- topology selection records
- topology constraint records
- standard topology mode and task classification string constants

## What this is not

- not a topology selector, planner, or orchestrator
- not Kanon policy evaluation or authorization semantics
- not eval harness or baseline comparison logic
- not model routing or provider selection
- not a persistence provider

Use this when a service needs to record or exchange topology metadata in a shared wire format, not when defining which topology is allowed or better for a product domain.
