# SYS-OBS-004A — Docker OTLP + Grafana readiness (platform index)

**Date:** 2026-05-20  
**Status:** **PASS** (B-012 cleared)  
**Canonical:** [`allagma-dotnet/docs/evidence/SYS_OBS_004A_DOCKER_GRAFANA_EVIDENCE.md`](../../../allagma-dotnet/docs/evidence/SYS_OBS_004A_DOCKER_GRAFANA_EVIDENCE.md)

## Summary

Live `verify-system-observability.ps1 -UseExistingServices` PASS against Docker-local working system with Grafana on host port **3001** when **3000** is occupied.

Artifact: `allagma-dotnet/artifacts/observability/20260520T182404Z/observability-summary.json`

## Platform delta

- `docker/local-working-system/docker-compose.yml` — canonical `OTEL_SERVICE_NAME` for Kanon/Conexus/Allagma containers.
- `docker/local-working-system/scripts/start-observability-stack.ps1` — auto-select Grafana host port when `:3000` is not Grafana-ready.
- `docker/local-working-system/scripts/verify-observability-stack.ps1` — resolve Grafana URL before delegating to Allagma verifier.

**Not** a production-readiness claim.
