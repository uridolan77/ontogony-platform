# SYSTEM-ALPHA-004 prep evidence (platform index)

**Date:** 2026-05-20  
**Status:** IN PROGRESS  
**Canonical prep record:** [`allagma-dotnet/docs/evidence/SYSTEM_ALPHA_004_PREP_EVIDENCE.md`](../../../allagma-dotnet/docs/evidence/SYSTEM_ALPHA_004_PREP_EVIDENCE.md) (runtime lock owner)

Do not treat as baseline PASS until the canonical prep doc shows all CUT gates PASS and `ontogony-runtime.lock.json` is refreshed to `SYSTEM-ALPHA-004`.

## Prerequisites

| Item | Status |
| --- | --- |
| `SYSTEM-SPRINT4-STATUS-RECON-001` | PASS — [SYSTEM_SPRINT4_STATUS_RECON_001_EVIDENCE.md](./SYSTEM_SPRINT4_STATUS_RECON_001_EVIDENCE.md) |
| Sprint 4 closeout addendum | [SYSTEM_SPRINT4_CLOSEOUT_ADDENDUM.md](../_incoming/curated-review-package/ontogony_curated_review_package/SYSTEM_SPRINT4_CLOSEOUT_ADDENDUM.md) |

## Platform validation (2026-05-20)

| Check | Result |
| --- | --- |
| Sprint plan Sprint 4 not deferred | PASS |
| Acceptance matrix Sprint 4 rows closed | PASS |
| Lock unchanged (`SYSTEM-ALPHA-003`) | PASS (intentional) |

## Cross-repo validation summary

See canonical doc for full command log, commit SHAs, blockers, and CUT checklist.

| Area | Result |
| --- | --- |
| Frontend `test:fe-high-value` | PASS |
| Frontend `npm run check` | FAIL (`config:check` resolved; current failure is `format:check` due pre-existing Prettier drift) |
| UI `npm run check` | PASS (lint blocker resolved) |
| Kanon Sprint 4 filters | PASS |
| Kanon full Release | PASS (261/261 on rerun) |
| Allagma scoped filters | PASS |
| Allagma full Release | PASS (714/714 on rerun) |
| Conexus retention persistence | PASS |
| Conexus scoped-keys integration hang | RESOLVED (`AdminScopedKeysIntegrationTests` now PASS; no hang) |
| Conexus full suite | FAIL (3 production-exposure tests fail on local Postgres `127.0.0.1:59990` connection refused) |
| Docker-local system gates | _pending_ |

## Open blockers

Canonical blocker state is now:

- `B-001` runtime lock unchanged until CUT (intentional).
- `B-007` dirty platform/conexus trees and SHA refresh-at-cut requirement.
- `B-010` frontend `format:check` drift (broad pre-existing Prettier mismatches).
- Docker-local CUT gates pending (E2E, restart, observability, evidence spine, composition gates).

Resolved for traceability in canonical prep: `B-002` Allagma vocabulary drift, `B-003` Kanon topology snapshot 409, `B-004` frontend `contracts:audit`, `B-005` UI lint, `B-006` Conexus scoped-keys hang, `B-009` frontend env-catalog drift.

## Next step

`SYSTEM-ALPHA-004-CUT` — [`P0_SYSTEM_ALPHA_004_CUT.md`](../_incoming/ontogony-next-development-phase-package/ontogony-next-development-phase-package/prompts/P0_SYSTEM_ALPHA_004_CUT.md)
