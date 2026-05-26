# Documentation cleanup report — ONTOGONY-PLATFORM-DOCS-CLEANUP-001

**Date:** 2026-05-26  
**Repo:** `uridolan77/ontogony-platform`  
**Scope:** `docs/` reorganization only (no `src/` or test changes)

---

## Summary

The `docs/` tree had regrown into a **package dump** after the 2026-05-23 cleanup: loose zips, extracted folders, roadmap markdown (`NEXT_*.md`), duplicate `packages/` intake, and an orphan `docs/_incoming_active/` folder. This pass restored a **small, navigable, platform-focused** documentation layout.

| Goal | Result |
| --- | --- |
| `docs/_incoming` contains only `_active`, `_consumed`, `README.md` | **PASS** |
| No zip files under `docs/` | **PASS** |
| Active vs consumed packages explicit | **PASS** — `MANIFEST.md` in both |
| Platform docs ≠ product docs | **PASS** — README/INDEX state boundaries |
| Canonical navigation | **PASS** — `docs/INDEX.md` + updated `docs/README.md` |

---

## Final docs structure (top level)

```text
docs/
  README.md
  INDEX.md
  CURRENT_STATE.md
  ARCHITECTURE.md
  CONTRACTS.md
  DEVELOPMENT.md
  TESTING.md
  INTEGRATION.md
  KNOWN_LIMITATIONS.md
  DOCS_CLEANUP_REPORT.md          # this file
  CODE_ARTIFACT_CLEANUP_REPORT.md
  _incoming/
    README.md
    _active/
      README.md
      MANIFEST.md
      ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/
      ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001/
      ONTOGONY-BACKEND-ABOVE9-PACKAGES/
    _consumed/
      README.md
      MANIFEST.md
      2026-05/                    # all archived packages
  architecture/
  contracts/
  operators/
  packages/
  testing/
  evidence/
  learn/
  migrations/
  adr/
  governance/
  consumer-blueprints/
  adoption/
  quality/
  system/
  schemas/
  environments/
  ai-runtime/
  product-hardening/              # CI-required eval schemas only
  reviews/
```

Empty placeholder categories from the task template (`errors/`, `http/`, `runbooks/` as separate roots) were **not** created; existing `operators/`, `contracts/`, and `packages/` already cover those topics.

---

## Files / folders moved

| From | To |
| --- | --- |
| `_active/REPLAY-RUNTIME-001`, `002`, `RUNTIME-CONFIG-001`, `ONTOGONY-CONTRACT-DISCIPLINE-OVER9-001`, agent/settings extracts | `_consumed/2026-05/<package>/` |
| `_incoming/*-extracted`, `SYSTEM-COH-001`, `EVIDENCE-SPINE-*`, etc. | `_consumed/2026-05/<package>/` |
| `_incoming/packages/*` | `_consumed/2026-05/` (merged) |
| `_incoming/_unzipped-learning` | `_consumed/2026-05/SYSTEM-LEARNING-GUIDE-001/` |
| `docs/_incoming_active/*` | `_consumed/2026-05/` |
| `docs/_CURRENT_PLAN.md` | `_consumed/2026-05/SIX-REPO-SCORE-PLANS/_CURRENT_PLAN.md` |
| Legacy `_consumed/GOVERNED_FAKE_*`, `SYSTEM_TRUTH_*` at root | `_consumed/2026-05/` |

---

## Files / folders deleted

| Item | Rationale |
| --- | --- |
| All `*.zip` under `docs/` (20+) | Unpacked content archived; zips forbidden under `_incoming` |
| `docs/_incoming/NEXT_1.md`, `NEXT_2_CONTRACT.md`, `NEXT_8_CONSOLE_UX_AND.md` | Superseded roadmaps; contract discipline promoted to canonical docs |
| `docs/_incoming/KANON.md`, `REPLAY.md`, `RUNTIME-CONFIG.md` | Duplicate summaries |
| `docs/_incoming/ALLAGMA-MAF-INTEGRATION-DEPTH-001. md` | Stray product note |
| `docs/_incoming/packages/` (after move) | Empty intake subfolder |
| `docs/_incoming_active/` | Orphan folder removed after move |
| `docs/DOCUMENTATION_CLEANUP_REPORT.md` | Superseded by this report |

---

## Merged into canonical docs (no content merge this pass)

Prior passes already promoted:

- Contract discipline → `docs/contracts/CONTRACT_DISCIPLINE_STANDARD.md`
- Replay runtime → `docs/contracts/REPLAY_RUNTIME_CONTRACT.md`
- Platform depth → `docs/evidence/PLAT_DEPTH_*` + `docs/reviews/IMPLEMENTATION_DEPTH_OVER9_CLOSEOUT_REPORT.md`

This pass **updated links** to point at canonical paths and consumed archives instead of deleted `NEXT_*.md` files.

---

## Active packages remaining

See [`_incoming/_active/MANIFEST.md`](./_incoming/_active/MANIFEST.md):

| Package | Status |
| --- | --- |
| ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001 | in_progress |
**Archived this follow-up:** ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001 (platform slice → `_consumed/2026-05/`).

**2026-05-26:** Removed empty `ONTOGONY-BACKEND-ABOVE9-PACKAGES` stub; above-9 backlog tracked in [`_consumed/2026-05/SIX-REPO-SCORE-PLANS/README.md`](./_incoming/_consumed/2026-05/SIX-REPO-SCORE-PLANS/README.md).

---

## Consumed packages archived

See [`_incoming/_consumed/MANIFEST.md`](./_incoming/_consumed/MANIFEST.md) — **22** entries under `_consumed/2026-05/` including replay, runtime-config, contract-discipline, governed-fake-e2e, system-truth, console UX, evidence spine, and score-plan archives.

---

## Links updated

| Area | Change |
| --- | --- |
| `docs/contracts/*` (discipline family) | `NEXT_2_CONTRACT.md` → consumed package path |
| `docs/CONTRACTS.md` | Program archive link |
| `docs/evidence/*` (governed-fake, kanon parity, learning guide) | `_incoming` paths → `_consumed/2026-05/` or MANIFEST |
| `docs/learn/*` | `_incoming_active`, zip refs → consumed |
| `docs/architecture/durability-boundaries.md` | cleanup report link |
| Active package `00_UNPACK_PROMPT.md` | `_incoming_active` → `_active` |

---

## Unresolved ambiguities

| Item | Notes |
| --- | --- |
| `docs/operators/OPERATOR_V1_DEMO_GUIDE.md` | Product-operator demo material; kept for now — consider moving to `ontogony-frontend` in a product-docs pass |
| `docs/product-hardening/` | Retained — `EvalEvidenceExportBundleSchemaTests` requires schemas under `eval-alignment-frontend-depth/` |
| Nested `PACKAGE/PACKAGE/` folders in intake | Cursor export convention; left as-is to avoid breaking in-package relative links |
| `ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001` | Listed active (cross-repo) while platform closeout exists in `reviews/` — archive when all repo slices complete |

---

## Follow-up (2026-05-26, same program)

| Action | Status |
| --- | --- |
| Archive **ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001** (platform slice) | **Done** → `_consumed/2026-05/` + `CONSUMED.md` |
| Write **move to `_consumed`** rule | **Done** — `_incoming/README.md`, `_active/README.md`, `AGENTS.md`, doc standard §8.4 |
| Operator V1 demo guide | Banner added — cross-repo runbook; UX owned by frontend |
| Decision reconstructability | Stays **active** until Task 5 + promotion to `docs/contracts/` |

## Recommended next cleanup pass

1. Promote **decision reconstructability** protocol docs to `docs/contracts/` when Task 5 completes, then **move** spine package to `_consumed` (required by intake policy).
2. Close remaining **PLAT-9-003 / 004 / 005** per [`SIX-REPO-SCORE-PLANS/README.md`](./_incoming/_consumed/2026-05/SIX-REPO-SCORE-PLANS/README.md).
3. Run `scripts/validate-learn-docs.ps1` and a repo-wide link check after the next large intake batch.
4. Consider restoring `validate-stale-incoming-package.ps1` if intake volume regrows.

---

## Validation

```powershell
# _incoming direct children (expect: README.md, _active, _consumed)
Get-ChildItem docs\_incoming -Name

# No zips under docs
Get-ChildItem docs -Filter *.zip -Recurse

# Manifests exist
Test-Path docs\_incoming\_active\MANIFEST.md
Test-Path docs\_incoming\_consumed\MANIFEST.md
Test-Path docs\INDEX.md
```
