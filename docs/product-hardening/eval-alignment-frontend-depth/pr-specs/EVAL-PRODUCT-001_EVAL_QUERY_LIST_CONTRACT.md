# EVAL-PRODUCT-001 — Eval query/list contract and dashboard data model

## Repos

allagma-dotnet, ontogony-frontend, ontogony-platform

## Goal

Define the product contract for listing/querying evaluations and dashboard data.

## Boundary

Contract-first. No fake global list.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

Route/DTO decision, OpenAPI/generation if needed, hook/adapter/dashboard model, evidence.

## Validation

Backend tests, OpenAPI check, frontend adapter tests, evidence.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
