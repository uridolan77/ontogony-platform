# Observability mechanics (platform)

Mechanical contracts for traces, metrics, logs, and operator correlation across the six-repo system. **No product semantics** — dashboards describe operational signals, not canonization or routing policy.

| Document | Purpose |
| --- | --- |
| [OBSERVABILITY_MECHANICS_CONTRACT.md](./OBSERVABILITY_MECHANICS_CONTRACT.md) | Authority, naming rules, consumer obligations |
| [STANDARD_SPAN_NAMES.md](./STANDARD_SPAN_NAMES.md) | Activity sources and span naming |
| [STANDARD_LOG_FIELDS.md](./STANDARD_LOG_FIELDS.md) | Structured logging scope keys |
| [STANDARD_METRIC_TAGS.md](./STANDARD_METRIC_TAGS.md) | Metric instrument and tag conventions |
| [metrics-catalog.md](./metrics-catalog.md) | Frozen `Ontogony.Platform` instruments |
| [trace-attributes.md](./trace-attributes.md) | Frozen `ontogony.*` span attribute keys |
| [operations-pack.md](./operations-pack.md) | Local collector, OTLP, burn-in checklist |
| [dashboards/dashboard-import.md](./dashboards/dashboard-import.md) | Grafana starter dashboard import |
| [alerts/alert-rules.md](./alerts/alert-rules.md) | Prometheus starter alert groups |

Related:

- [`docs/contracts/TRACE_CONTEXT_CONTRACT.md`](../contracts/TRACE_CONTEXT_CONTRACT.md)
- [`docs/operators/TRACE_CORRELATION_CONTRACT.md`](../operators/TRACE_CORRELATION_CONTRACT.md)
- [`docs/adoption/reconstructability-conformance-kits.md`](../adoption/reconstructability-conformance-kits.md) — includes `ObservabilityNamingConformanceAssertions`

Evidence:

- Phase 1: [`docs/evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_EVIDENCE.md`](../evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_EVIDENCE.md)
- Phase 2: [`docs/evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_PHASE2_EVIDENCE.md`](../evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_PHASE2_EVIDENCE.md)

Conformance: `.\scripts\run-observability-mechanics-conformance.ps1`
