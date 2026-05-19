# PFH-000 — Product hardening package setup

## Repos

ontogony-platform

## Goal

Unpack/register this package as the canonical control package.

## Boundary

Docs/package setup only.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

Package files, evidence, pointer docs.

## Validation

Manifest parse, file inventory, no runtime changes.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
