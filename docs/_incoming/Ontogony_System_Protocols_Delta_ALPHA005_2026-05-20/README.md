# Ontogony System Protocols Delta after SYSTEM-ALPHA-005

Generated: 2026-05-20  
Baseline posture: `SYSTEM-ALPHA-005` is the last cut baseline. Current `main` has post-cut deltas and must be revalidated before a new lock.

## Purpose

This package supersedes the older `Ontogony_System_Protocols_Package_2026-05-20.zip` as an implementation guide. It is not a restatement of the older review package. It is a refined delta package based on the current repo state.

## Package contents

- `00_EXECUTIVE_BRIEF.md` — current decision and recommended next move.
- `01_CURRENT_STATE_ASSESSMENT.md` — what is already implemented vs still real.
- `02_MOVING_MAIN_DELTA.md` — post-Alpha-005 changes that affect planning.
- `03_REFINED_BACKLOG.md` — prioritized up-to-date backlog.
- `04_SPRINT_PLAN.md` — recommended implementation sequence.
- `05_ACCEPTANCE_MATRIX.md` — acceptance criteria by item.
- `06_VERIFICATION_CHECKLIST.md` — manual/CI verification checklist.
- `07_SOURCE_MAP.md` — source/evidence map used to build this package.
- `08_SUPERSEDED_ITEMS.md` — old package/review items that should not be repeated.
- `repo-plans/` — repo-specific execution plans.
- `issue-cards/` — PR-sized issue cards.
- `data/refined_backlog.json` and `.csv` — machine-readable backlog.
- `templates/system-protocol-registry.schema.json` — starter schema for SYS-PROTOCOL-REGISTRY-001.
- `scripts/validate-incoming-package.ps1` — starter stub; canonical validator: `ontogony-platform/scripts/validate-stale-incoming-package.ps1`.

## Operating rule

Do not copy old `_incoming` package content into canonical docs unless it passes reconciliation against:
1. `docs/system/ontogony-runtime.lock.json`;
2. current route inventories/OpenAPI;
3. current evidence indexes;
4. closed/quarantined item status;
5. no obsolete naming.

## Non-claims

- Not production readiness.
- Does not enable real external tool execution.
- Does not claim moving-main is re-locked.
- Does not claim B-012 is closed until a new PASS artifact exists.
