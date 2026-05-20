# SYS-OBS-004A — Close B-012 Docker OTLP + Grafana readiness quarantine

**Priority:** P0  
**Repo:** ontogony-platform primary; allagma-dotnet evidence consumer  
**Theme:** Observability / quarantine

## Problem

SYSTEM-ALPHA-005 is cut complete except B-012. The failed live observability rerun reported Grafana not ready on :3000 within 120s while collector/Prometheus/Jaeger were up.

## Scope

Debug docker/local-working-system observability startup order, Grafana health/readiness, datasource provisioning, and verify-system-observability.ps1 timeout/readiness logic. Produce a PASS artifact for the live Docker observability gate.

## Acceptance criteria

verify-system-observability.ps1 -UseExistingServices returns PASS against Docker-local stack; artifact is committed or indexed under evidence; B-012 is marked cleared in Allagma and platform evidence indexes; no production readiness claim is added.

## Source anchors

- `allagma-dotnet/docs/evidence/SYSTEM_ALPHA_005_CLOSEOUT.md`
- `allagma-dotnet/docs/evidence/README.md`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
