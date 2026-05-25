# PLAT-CONTRACT-001 to PLAT-CONTRACT-005 — Platform contract-discipline slice

## Objective

Raise Platform contract discipline above 9 by making mechanical contracts easier to verify across consumers.

## PLAT-CONTRACT-001 — System compatibility gate v2

Extend `Ontogony.SystemCompatibility` to validate runtime lock schema, package version line consistency, required repo roles, error envelope contract references, header propagation frozen set, and post-lock delta schema.

Acceptance:

- machine-readable output;
- Allagma runtime-lock validation can consume it;
- tests cover malformed lock/delta files.

## PLAT-CONTRACT-002 — Header propagation conformance hardening

Make frozen propagation headers testable in all consumers: `traceparent`, `X-Correlation-ID`, actor headers, idempotency header rules, and run/model/decision identifiers where allowed.

## PLAT-CONTRACT-003 — Cross-service error envelope gate v2

Clarify envelope vs intentional DTO-shaped validation errors. Product repos can keep allowed exceptions, but they must be named and tested.

## PLAT-CONTRACT-004 — Package inventory and public API snapshot cleanup

Make package inventory and public API snapshots first-class. Public surface breaks require explicit changelog/migration.

## PLAT-CONTRACT-005 — Consumer blueprint freshness

Update consumer blueprints for Conexus, Kanon, and Allagma package-mode and sibling-source modes.
