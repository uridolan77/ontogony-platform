# SYSTEM-RC-001D — Observability PASS

## Owner

`allagma-dotnet` + `ontogony-platform`

## Problem

Observability design exists, but prior PASS wording was retracted. Above-9 observability requires a live PASS artifact.

## Goal

Produce a real observability summary JSON with `verdict: PASS` across Allagma, Kanon, and Conexus.

## Required command

```powershell
cd C:\dev\allagma-dotnet
pwsh ./scripts/verify-system-observability.ps1 -UseExistingServices
```

If the script requires the observability stack to be started:

```powershell
docker compose -f docker-compose.observability.yml up -d
pwsh ./scripts/run-local-stack.ps1 -EnableOtlpExport
pwsh ./scripts/smoke-first-system.ps1
pwsh ./scripts/verify-system-observability.ps1 -UseExistingServices
```

## Required artifact

```text
artifacts/observability/<timestamp>/observability-summary.json
```

## Required evidence doc

```text
docs/evidence/SYSTEM_RC_001D_OBSERVABILITY_PASS.md
```

## PASS requirements

The summary must classify:

```text
allagma_metrics
kanon_metrics
conexus_metrics
allagma_traces
kanon_traces
conexus_traces
cross_service_trace_chain
correlation_id_propagation
model_lifecycle_metrics
human_gate_metrics
gateway_metrics
prometheus_scrape
jaeger_query
grafana_dashboard
```

Each must be:

```text
PASS
DEGRADED
FAIL
```

A final `verdict: PASS` requires no `FAIL` entries and no unclassified `DEGRADED` entries for core app metrics/traces.

## Acceptance criteria

- `observability-summary.json` exists and says `verdict: PASS`.
- Existing docs no longer claim PASS without linking the artifact.
- `RELEASE_EVIDENCE_INDEX.md` links the artifact.
- Degraded Prometheus/collector mismatch is classified explicitly.
- No raw prompts, completions, or secrets appear in metrics/log labels.

## Non-goals

- No production SLO claim.
- No cloud observability deployment.
- No enterprise monitoring integration.
