# OPERATOR-UX-TAXONOMY-001 — executive brief

## One-line goal

Create a shared operator UI taxonomy so Ontogony pages stop inventing local status language and stop overstating truth, readiness, evidence completeness, or live connectivity.

## Why this matters now

The console is now rich enough that wording errors become architecture errors. If a page says `healthy` while readiness is strict-not-ready, or shows `Live with fixture fallback` as a page identity, the operator cannot tell whether the platform is actually connected, partially degraded, or showing demo data.

This sprint turns truth into UI infrastructure.

## Required shared dimensions

The UI must represent these dimensions independently:

| Dimension | Question answered |
|---|---|
| Connectivity | Can the console reach the service? |
| Readiness | Can the service perform the requested workflow now? |
| Contract health | Did the payload match the expected UI/backend contract? |
| Operator usability | Can an operator safely act on this surface? |
| Evidence completeness | Is the evidence graph resolved, partial, unresolved, or not applicable? |
| Data source | Is this live, live-with-fallback, fixture, generated, imported, or unknown? |
| Authority | Is this authoritative, advisory, demo, inferred, or historical? |
| Topology edge state | Is this edge validated, degraded, missing, planned, or blocked? |

## Hard rule

No generic `unknown`. Every unknown must say unknown what and why, for example:

- `Provider: unknown — model-call record did not include provider metadata`
- `Compatibility: unknown — service version metadata is missing from /health`
- `Readiness: unknown — /ready was not queried`
- `Task type: unknown — run metadata did not include task classifier output`

## Package stance

This is not a styling task. It is a truth-contract task implemented through UI taxonomy, adapters, tests, and copy rules.
