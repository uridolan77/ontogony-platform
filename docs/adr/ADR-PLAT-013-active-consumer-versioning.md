# ADR-PLAT-013 — Active Consumer Versioning Before 1.0

## Status

Proposed

## Context

Ontogony.Platform began as a source-referenced mechanical substrate. It is now packaged and consumed by Conexus.NET. Pre-1.0 breakage is still allowed, but cannot be silent.

## Decision

Before 1.0, breaking changes are allowed only when they are:
1. captured by public API snapshot diffs where applicable,
2. described in CHANGELOG,
3. accompanied by migration notes when consumer behavior changes,
4. validated against Conexus package-mode consumption.

## Consequences

Positive:
- keeps alpha flexibility,
- prevents silent consumer breakage,
- preserves product/platform boundary.

Negative:
- adds governance overhead.
