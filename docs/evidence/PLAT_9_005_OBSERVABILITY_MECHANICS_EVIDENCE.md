# PLAT-9-005 — Observability mechanics pack (phase 1)

**Date:** 2026-05-26  
**Program:** Six-repo score lift — [`SIX-REPO-SCORE-PLANS`](../_incoming/_consumed/2026-05/SIX-REPO-SCORE-PLANS/README.md)

## Phase 1 deliverables

| Artifact | Location |
| --- | --- |
| Mechanics contract index | [`docs/observability/README.md`](../observability/README.md) |
| Observability mechanics contract | [`docs/observability/OBSERVABILITY_MECHANICS_CONTRACT.md`](../observability/OBSERVABILITY_MECHANICS_CONTRACT.md) |
| Span / log / tag standards | `STANDARD_SPAN_NAMES.md`, `STANDARD_LOG_FIELDS.md`, `STANDARD_METRIC_TAGS.md` |
| Frozen catalogs | `metrics-catalog.md`, `trace-attributes.md` |
| Operations pack | `operations-pack.md` |
| Header → span → log helper | `SystemCorrelationConventions` in `Ontogony.Observability` |

## Verification

```powershell
cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Observability.Tests/Ontogony.Observability.Tests.csproj -c Release --filter "FullyQualifiedName~DiagnosticsContractSmokeTests|FullyQualifiedName~SystemCorrelationConventionsTests"

# Consumer naming (PR-005 kits, includes observability assertions)
dotnet test tests/Conexus.Application.Tests/Conexus.Application.Tests.csproj -c Release --filter ConexusPlatformConformanceTests
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj -c Release --filter AllagmaPlatformConformanceTests
```

## Status

| Gate | Result |
| --- | --- |
| `docs/observability/*` contract set | **Done** (phase 1) |
| `SystemCorrelationConventions` | **Done** |
| `DiagnosticsContractSmokeTests` | **Existing** — pins platform instruments |
| Starter Grafana JSON / alert rules | **Deferred** (phase 2) |
| `run-observability-mechanics-conformance.ps1` aggregator | **Deferred** (phase 2) |

## Relation to PLAT-9-003

PR-005 (`Ontogony.Testing.Conformance`) closed the **consumer conformance suite** used by Allagma, Kanon, and Conexus — including `ObservabilityNamingConformanceAssertions`. The broader PLAT-9-003 matrix artifact (`artifacts/consumer-conformance/<timestamp>/summary.json`) remains optional phase 2 work if a single cross-repo runner is required.
