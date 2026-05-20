# SYS-STALE-PACKAGE-GUARD-001 — Add stale incoming-package detector before adopting generated ZIPs

**Priority:** P0  
**Repo:** ontogony-platform  
**Theme:** Planning hygiene

## Problem

The older curated review package was directionally useful but became stale after Sprint 3/4/Alpha-005 closures. The repo now contains the old Ontogony_System_Protocols zip as incoming; adopting it without reconciliation would reintroduce closed items.

## Scope

Add a validator for docs/_incoming packages that detects stale baseline names, closed backlog items, obsolete route names, Agentor references, production-readiness overclaims, and tasks superseded by current evidence.

## Acceptance criteria

Validator reports stale items in old packages; new packages must include CURRENT_BASELINE.md, MOVING_MAIN_DELTA.md, and SUPERSEDED_ITEMS.md; CI/documented runbook prevents copying stale package content into canonical docs without reconciliation.

## Source anchors

- `ontogony-platform/docs/_incoming/Ontogony_System_Protocols_Package_2026-05-20.zip`
- `ontogony-platform/docs/_incoming/curated-review-package/...`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
