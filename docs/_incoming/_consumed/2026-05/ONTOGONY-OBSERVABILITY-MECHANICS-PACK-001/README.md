# ONTOGONY-OBSERVABILITY-MECHANICS-PACK-001

**Status:** in_progress (phase 1 landed 2026-05-26)  
**Tracked in:** [`../MANIFEST.md`](../MANIFEST.md)

## Goal

PLAT-9-005 — mechanical observability contracts (metrics, traces, logs) across the six-repo system without product semantics.

## Phase 1 (done)

Canonical docs under [`docs/observability/`](../../observability/) and `SystemCorrelationConventions` in `Ontogony.Observability`. Evidence: [`docs/evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_EVIDENCE.md`](../../evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_EVIDENCE.md).

## Phase 2 (next)

```text
- Grafana starter dashboard JSON or checked-in PromQL
- alerts.prometheus.rules.yml examples
- scripts/run-observability-mechanics-conformance.ps1
- optional full PLAT-9-003 consumer matrix artifact runner
```

## Archive

When phase 2 is done, promote any remaining decisions to `docs/observability/`, record evidence, move this folder to `_consumed/2026-05/`.
