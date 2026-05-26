# Six-repo score plans (archived)

**Replaces:** empty stub `ONTOGONY-BACKEND-ABOVE9-PACKAGES` (removed 2026-05-26).

This folder is the canonical archive for **above-9 score-lift** planning across all six repos.

| Document | Purpose |
| --- | --- |
| [`_CURRENT_PLAN.md`](./_CURRENT_PLAN.md) | Full program: PLAT-9-*, FE-9-*, UI-9-*, backend repo plans, execution waves |

## Platform (`ontogony-platform`) — what remains

Use [`_CURRENT_PLAN.md`](./_CURRENT_PLAN.md) § “Plan 1 — ontogony-platform above 9” and cross-check [`CURRENT_STATE.md`](../../../CURRENT_STATE.md).

| Slice | Status (2026-05-26) |
| --- | --- |
| PLAT-9-001 Six-repo compatibility gate | **Done** — [`evidence/PLAT_9_001_SIX_REPO_COMPATIBILITY_GATE_EVIDENCE.md`](../../../evidence/PLAT_9_001_SIX_REPO_COMPATIBILITY_GATE_EVIDENCE.md) |
| PLAT-9-002 Mechanical protocol registry | **Done** — [`contracts/MECHANICAL_PROTOCOL_REGISTRY.md`](../../../contracts/MECHANICAL_PROTOCOL_REGISTRY.md) |
| PLAT-9-006 No-meaning guard | **Done** — `scripts/check-no-product-semantics.ps1`, architecture tests |
| PLAT-DEPTH-001–004 | **Done** — [`reviews/IMPLEMENTATION_DEPTH_OVER9_CLOSEOUT_REPORT.md`](../../../reviews/IMPLEMENTATION_DEPTH_OVER9_CLOSEOUT_REPORT.md) |
| PLAT-9-003 Consumer conformance suite | **Done** (PR-005) — [`evidence/PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md`](../../../evidence/PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md); `*PlatformConformanceTests` on Allagma/Kanon/Conexus `main`. Optional: full-matrix `artifacts/consumer-conformance/` runner (phase 2). |
| PLAT-9-004 Public API hardening | **Partial** — Tier B XML docs still deferred |
| PLAT-9-005 Observability mechanics pack | **Done** — [`docs/observability/`](../../../observability/), [`evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_PHASE2_EVIDENCE.md`](../../../evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_PHASE2_EVIDENCE.md), `run-observability-mechanics-conformance.ps1` |

Frontend, UI, Kanon, Allagma, and Conexus slices in `_CURRENT_PLAN.md` are owned by those repos.
