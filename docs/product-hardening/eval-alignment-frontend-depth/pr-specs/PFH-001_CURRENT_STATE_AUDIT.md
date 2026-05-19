# PFH-001 — Current-state audit

## Repos

all six repos

## Goal

Produce a precise current-state audit before product changes.

## Boundary

Audit/docs first.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

Updated matrices, inventories, product gap priority list, evidence.

## Validation

Repo searches, docs inspected, no secrets.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
