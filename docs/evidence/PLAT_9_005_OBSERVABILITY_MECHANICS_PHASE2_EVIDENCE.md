# PLAT-9-005 — Observability mechanics pack (phase 2)

**Date:** 2026-05-26  
**Program:** Six-repo score lift — closes PLAT-9-005 after [phase 1](PLAT_9_005_OBSERVABILITY_MECHANICS_EVIDENCE.md).

## Deliverables

| Artifact | Location |
| --- | --- |
| Grafana starter dashboard | [`docs/observability/dashboards/grafana-dashboard-starter.json`](../observability/dashboards/grafana-dashboard-starter.json) |
| Dashboard import guide | [`docs/observability/dashboards/dashboard-import.md`](../observability/dashboards/dashboard-import.md) |
| Prometheus alert rules | [`docs/observability/alerts/alerts.prometheus.rules.yml`](../observability/alerts/alerts.prometheus.rules.yml) |
| Alert documentation | [`docs/observability/alerts/alert-rules.md`](../observability/alerts/alert-rules.md) |
| Artifact validator | [`scripts/validate-observability-pack-artifacts.ps1`](../../scripts/validate-observability-pack-artifacts.ps1) |
| Conformance aggregator | [`scripts/run-observability-mechanics-conformance.ps1`](../../scripts/run-observability-mechanics-conformance.ps1) |
| CI artifact tests | `tests/Ontogony.Observability.Tests/ObservabilityPackArtifactTests.cs` |

## Alert groups

```text
ontogony-platform-mechanics
allagma-mechanics
conexus-mechanics
kanon-mechanics
```

## Verification

```powershell
cd C:\dev\ontogony-platform
.\scripts\run-observability-mechanics-conformance.ps1
```

Produces `artifacts/observability-mechanics/<timestamp>/summary.json` (gitignored).

**Live run 2026-05-26:**

| Suite | Result |
| --- | --- |
| Pack artifact validation | PASS |
| `Ontogony.Observability.Tests` (smoke + conventions + pack artifacts) | **8/8** |
| `AllagmaPlatformConformanceTests` | **6/6** |
| `ConexusPlatformConformanceTests` | **6/6** |
| `KanonPlatformConformanceTests` | **5/5** |

## Status

| Gate | Result |
| --- | --- |
| Dashboard JSON parses | **Done** — `ObservabilityPackArtifactTests` + validator script |
| Alert YAML with four service groups | **Done** |
| Platform observability tests | **Done** |
| Aggregator script + summary JSON | **Done** |
| PLAT-9-005 program slice | **Done** — see six-repo score plan README |
