# Observability mechanics contract

**Owner:** `Ontogony.Observability`  
**Program:** PLAT-9-005 (six-repo score lift)  
**Status:** **Done** (phase 1 + phase 2, 2026-05-26)

Platform owns **mechanical** observability: naming, propagation into spans/logs/metrics, and conformance tests. Product repos own dashboards that interpret their own business labels.

---

## Questions this layer answers

```text
Is the system healthy while doing governed work?
Do traces, metrics, and logs use the same identifiers as HTTP and decision events?
Can operators deep-link from a metric spike to trace / run / model call?
```

Reconstructability answers *what happened and whether evidence is complete*. Observability answers *whether the runtime exposes consistent operational signals while that work runs*.

---

## Authority split

| Concern | Owner |
| --- | --- |
| Trace / correlation headers | [`TRACE_CONTEXT_CONTRACT.md`](../contracts/TRACE_CONTEXT_CONTRACT.md), [`HEADER_PROPAGATION_CONTRACT.md`](../contracts/HEADER_PROPAGATION_CONTRACT.md) |
| Header → span → log mapping | `SystemCorrelationConventions` in `Ontogony.Observability` |
| Platform meter + span keys | `OntogonyDiagnostics`, `OntogonySpanAttributes` |
| Service-specific meters | Each product repo (`Allagma.*`, `Conexus.*`, `Kanon.*` prefixes) |
| Operator dashboards | Product repos + shared starter queries in `operations-pack.md` |

---

## Naming rules (frozen)

### Meters

```text
<ServicePrefix>.<domain>     — e.g. Conexus.Gateway, Allagma.Runtime
Ontogony.Platform            — platform middleware + integration instruments only
```

Use `ObservabilityNamingConformanceAssertions.AssertMeterNameUsesPrefix` in consumer tests.

### Instruments (OpenTelemetry logical names)

```text
<service_snake>.<area>.<metric>.<unit?>
ontogony.<area>.<metric>.<unit?>   — platform-only
```

- Use lowercase dotted segments.
- Counters end with `.count`; durations use `.duration.ms`.
- Do not embed PII, prompts, or raw payloads in names or tags.

### Prometheus export

Logical dotted names map to lowercase snake_case with `_total` suffix for counters:

```text
conexus.gateway.request.count → conexus_gateway_request_count_total
```

Use `ObservabilityNamingConformanceAssertions.AssertPrometheusExportName` with the service catalog’s logical name.

### Span attributes

Platform correlation tags use the `ontogony.` prefix — see [trace-attributes.md](./trace-attributes.md). HTTP semantic conventions (`http.request.method`, `url.path`) may coexist.

### Log scopes

Use camelCase scope keys aligned with [STANDARD_LOG_FIELDS.md](./STANDARD_LOG_FIELDS.md). Always include `traceId` and `operationId` on request-scoped logs.

---

## Consumer adoption (minimum)

| Repo | Proof |
| --- | --- |
| Allagma | `AllagmaPlatformConformanceTests` — meter prefix + frozen catalog instrument |
| Conexus | `ConexusPlatformConformanceTests` — meter prefix + Prometheus export names |
| Kanon | Correlation + error baseline; add metric naming when Kanon exports runtime meters |
| Frontend | Displays `traceId` / `correlationId` / run ids consistently (route inventory) |

---

## Phase 1 acceptance (this slice)

```text
docs/observability/* contract set exists
SystemCorrelationConventions ships in Ontogony.Observability
DiagnosticsContractSmokeTests pins platform instrument + span keys
Consumer naming checks remain green (PR-005 kits)
```

## Phase 2 (done)

| Artifact | Location |
| --- | --- |
| Grafana starter | [`dashboards/grafana-dashboard-starter.json`](./dashboards/grafana-dashboard-starter.json) |
| Prometheus alerts | [`alerts/alerts.prometheus.rules.yml`](./alerts/alerts.prometheus.rules.yml) |
| Conformance runner | [`scripts/run-observability-mechanics-conformance.ps1`](../../scripts/run-observability-mechanics-conformance.ps1) |
| Evidence | [`docs/evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_PHASE2_EVIDENCE.md`](../evidence/PLAT_9_005_OBSERVABILITY_MECHANICS_PHASE2_EVIDENCE.md) |

Optional follow-up: full PLAT-9-003 `artifacts/consumer-conformance/` matrix runner (broader than observability-only scope).
