# TASK-005 — Home Truth Model

## Goal

Update Operator Home to render truth dimensions.

## Requirements

- No "All services healthy" if any readiness is not ready.
- No page-level "Live with fixture fallback".
- Show service cards with connectivity/readiness/contract/version.
- Show compatibility artifact status separately.
- Show execution safety separately.

## Acceptance

Home is no longer contradictory when Conexus is live but not ready.
