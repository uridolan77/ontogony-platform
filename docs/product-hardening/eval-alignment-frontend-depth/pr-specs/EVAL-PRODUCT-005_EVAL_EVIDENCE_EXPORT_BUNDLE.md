# EVAL-PRODUCT-005 — Eval evidence export bundle

## Repos

allagma-dotnet, ontogony-frontend, ontogony-platform

## Goal

Create operator-readable export bundle across eval evidence.

## Boundary

Not compliance archive.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

Bundle schema, export route/command or limitation, validator, evidence.

## Validation

Schema validation, export tests, redaction checks.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
