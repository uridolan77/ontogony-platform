# Ontogony.Primitives

Lowest-level **time** and **identifier** abstractions shared across Ontogony packages.

## What this is

- `IClock` / `SystemClock` — injectable time (deterministic tests with `FakeClock` from `Ontogony.Testing`).
- `IIdGenerator` / `GuidIdGenerator` — stable ID generation boundary.

## What this is not

- Not distributed clocks, logical clocks, or snowflake IDs (hosts choose higher-level ID schemes).
