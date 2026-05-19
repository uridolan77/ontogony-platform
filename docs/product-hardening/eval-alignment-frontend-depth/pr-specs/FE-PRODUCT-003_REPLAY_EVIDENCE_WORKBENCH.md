# FE-PRODUCT-003 — Replay evidence workbench

## Repos

ontogony-frontend, allagma if needed

## Goal

Create a limitation-aware replay evidence workbench.

## Boundary

Do not fake unsupported replay triggers.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

Lookup modes, limitation actions, export/cross-links if supported.

## Validation

replay:check, targeted e2e, adapter tests.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
