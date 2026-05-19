# EVAL-PRODUCT-004 — Quality scoring and judge calibration surfaces

## Repos

allagma-dotnet, ontogony-frontend, ontogony-platform

## Goal

Expose richer scoring, judge metadata, confidence, calibration, limitations.

## Boundary

Surface scoped metadata; do not invent claims.

Not production readiness. No real provider keys. No secrets. No broad refactors.

## Files to inspect

- this package
- current repo docs/evidence
- OpenAPI snapshots/generated clients where applicable
- frontend hooks/adapters/pages/tests where applicable

## Deliverables

Scoring metadata contract, UI breakdown, tests/evidence.

## Validation

DTO/API tests if changed, adapter tests, UI wording tests.

## Evidence

Add or update a PR-specific evidence file with commands, results, limitations, and an explicit **not production readiness** statement.

## Non-goals

- production readiness
- real provider mode
- cloud deployment
- unscoped runtime changes
