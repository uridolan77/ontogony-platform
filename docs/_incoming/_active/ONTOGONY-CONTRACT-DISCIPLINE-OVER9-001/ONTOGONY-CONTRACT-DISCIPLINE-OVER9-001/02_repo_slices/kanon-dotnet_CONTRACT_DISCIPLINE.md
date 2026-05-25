# KANON-CONTRACT-001 to KANON-CONTRACT-007 — Kanon contract-discipline slice

## Objective

Raise Kanon contract discipline from 8.0 to 9.1+ by making generated truth impossible to contradict.

## KANON-CONTRACT-001 — Route/client/server-only count truth fix

Required steps:

1. Treat generated artifacts as authoritative.
2. Regenerate route inventory, client coverage, route doc fragments, OpenAPI baseline, and compatibility manifest.
3. Update narrative docs to use generated fragments or references.
4. Add stale literal tests for route-count tables.

Acceptance:

- `CURRENT_STATE.md`, README, `KNOWN_LIMITATIONS.md`, Allagma matrix, and manifest agree.
- CI fails if a stale literal count remains in known files.

## KANON-CONTRACT-002 — ServerOnly policy exactness

Every server-only route must have route path, reason, owner persona, whether typed client is intentionally blocked or deferred, and downstream impact.

## KANON-CONTRACT-003 — Error contract exactness

Document and test Ontogony envelope mapped errors, endpoint-local `ApiError` normalized errors, and intentional DTO-shaped validation failures.

## KANON-CONTRACT-004 — v0 freeze and v1 guard hardening

No `/ontology/v1` route family. v1 decision remains `stay_v0` until graduation. Breaking changes require migration and downstream impact list.

## KANON-CONTRACT-005 — Compatibility manifest v1.2

Add narrative doc hash/freshness marker, server-only policy hash, route fragment generation time, and Allagma consumed route subset.

## KANON-CONTRACT-006 — Postgres semantic smoke as contract gate

Classify Postgres semantic smoke as a contract discipline gate for semantic persistence.

## KANON-CONTRACT-007 — Conexus assistance contract freeze

Freeze draft_only invariant, redaction requirements, accept/reject/convert shapes, and decision lifecycle linkage.
