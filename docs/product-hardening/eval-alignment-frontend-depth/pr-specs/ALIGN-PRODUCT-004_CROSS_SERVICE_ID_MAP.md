# ALIGN-PRODUCT-004 — Cross-service ID map

## Repos

platform and service/frontend docs as needed

## Goal

Map run/eval/comparison/trace/correlation/decision/route/model-call IDs.

## Boundary

Docs and UI-link semantics.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

ID map doc, UI link matrix, evidence.

## Validation

Trace proof rerun if needed, frontend tests if UI changes.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
