# EVAL-PRODUCT-003 — Scenario dataset surfaces

## Repos

allagma-dotnet, ontogony-frontend, ontogony-platform

## Goal

Make scenario datasets/suites/scenario labels visible to operators.

## Boundary

Dataset surfaces only.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

Dataset metadata, labels, dashboard filters, tests/evidence.

## Validation

DTO/API tests if changed, frontend tests.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
