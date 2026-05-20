# Intake status — Ontogony System Protocols Delta (ALPHA-005)

## Extraction

| Field | Value |
|-------|-------|
| **Extraction date** | 2026-05-20 |
| **Source zip** | `docs/_incoming/Ontogony_System_Protocols_Delta_ALPHA005_2026-05-20.zip` |
| **Destination folder** | `docs/_incoming/Ontogony_System_Protocols_Delta_ALPHA005_2026-05-20/` |
| **Extracted file count** | 35 files |
| **Zip preserved** | Yes (not deleted) |

The archive contained a single top-level folder matching the destination name; extraction into `docs/_incoming/` normalized paths without double nesting.

## Package purpose

Post-**SYSTEM-ALPHA-005** protocol delta and reconciliation package. It supersedes the older `Ontogony_System_Protocols_Package_2026-05-20.zip` as a planning guide for moving `main` ahead of the Alpha-005 cut baseline: observability quarantine closure, lock refresh, protocol registry, drift prevention, and evidence-index reconciliation.

**Baseline posture:** `SYSTEM-ALPHA-005` is the last cut baseline. Current `main` has post-cut deltas and must be revalidated before a new lock.

## Non-claims

This intake does **not** assert:

- Production readiness or production deployment approval
- That moving-main has been re-locked (only after validation gates pass)
- Permission to enable real external tool execution (remains blocked per Alpha-005)
- Closure of B-012 until a new PASS observability artifact exists

## Remaining known quarantine

- **B-012 / SYS-OBS-004A** — Docker OTLP + Grafana readiness (`verify-system-observability.ps1` live gate). Alpha-005 closeout is complete except this item; Grafana readiness on `:3000` failed within 120s in the last live rerun while collector/Prometheus/Jaeger were up.

## Package layout verification

| Expected | Present |
|----------|---------|
| `README.md` | Yes |
| `00_EXECUTIVE_BRIEF.md` … `06_VERIFICATION_CHECKLIST.md` | Yes |
| `issue-cards/` | Yes (12 cards) |
| `repo-plans/` | Yes (6 repos) |
| `data/` | Yes (`refined_backlog.json`, `.csv`) |
| `templates/` | Yes (protocol registry schema + example) |
| `validators/` | **No** — package ships `scripts/validate-incoming-package.ps1` instead |

Additional package files (not in the intake checklist): `07_SOURCE_MAP.md`, `08_SUPERSEDED_ITEMS.md`, `MANIFEST.json`, `source-notes/current-main-observations.md`.

## Next recommended action

1. Read `README.md` and `00_EXECUTIVE_BRIEF.md`.
2. Run through `06_VERIFICATION_CHECKLIST.md` against current repo state (do not treat checklist pass as production readiness).
3. Open PRs from `issue-cards/` only after reconciling each card with `docs/system/ontogony-runtime.lock.json`, current route inventories, and evidence indexes.
4. **Do not implement** package backlog items from this intake note alone — implementation is out of scope for unpack/index.

## Stale-term scan (2026-05-20)

Quick textual scan of extracted content. Hits are **guard/superseded references**, not stale planning contradictions; no package edits applied.

| Term | Hits | Notes |
|------|------|-------|
| `Agentor` | 5 | Listed as a pattern to detect in stale packages (`SYS-STALE-PACKAGE-GUARD-001`, checklist) |
| `SYSTEM-ALPHA-004` as current baseline | 0 as baseline claim | Only in validator regex for stale `pending` wording |
| `production ready` / `production readiness` | Multiple | Explicit **non-claims** and acceptance criteria (“no production readiness claim”) |
| `enable real tool execution` | Multiple | Explicit **Do not** boundaries on issue cards and README |
| `gpt-4o-mini` hard-coded Allagma requirement | 2 | `08_SUPERSEDED_ITEMS.md` marks implemented; validator regex detects stale claims |

## Recommended first issue card

`issue-cards/SYS-OBS-004A.md` — Sprint 0 item #1 in `04_SPRINT_PLAN.md`; closes B-012 before lock refresh and protocol registry work.
