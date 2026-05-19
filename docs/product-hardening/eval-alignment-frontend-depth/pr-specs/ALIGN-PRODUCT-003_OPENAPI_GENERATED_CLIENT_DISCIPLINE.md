# ALIGN-PRODUCT-003 — OpenAPI/generated-client discipline

## Repos

backend repos, frontend, platform

## Goal

Tighten OpenAPI snapshot/generated-client discipline.

## Boundary

No broad API redesign.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

Sync/check docs, client status matrix, stale snapshot checks if needed.

## Validation

OpenAPI checks, generated client checks, frontend build/typecheck.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
