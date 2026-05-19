# EVAL-PRODUCT-002 — Baseline comparison workbench depth

## Repos

allagma-dotnet, ontogony-frontend, ontogony-platform

## Goal

Turn baseline comparison into an operator workbench.

## Boundary

Product depth only.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

Comparison list/history or limitation, drilldown, UI panels, evidence.

## Validation

API/unit tests, adapter tests, fixture/live tests.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
