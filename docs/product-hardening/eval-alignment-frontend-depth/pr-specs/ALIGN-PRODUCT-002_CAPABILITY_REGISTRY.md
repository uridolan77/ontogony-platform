# ALIGN-PRODUCT-002 — Capability registry

## Repos

platform, frontend, backend docs as needed

## Goal

Classify each product surface as live, fixture, degraded, missing, or planned.

## Boundary

Capability-state clarity only.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

Registry doc, UI limitation wording, evidence.

## Validation

Docs checks, frontend tests if UI copy changes.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
