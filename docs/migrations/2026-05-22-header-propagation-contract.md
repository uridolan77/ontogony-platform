# Migration: header propagation contract (PLATFORM-9-003)

## Summary

Frozen cross-service propagation header names and reusable test helpers for proving outbound HTTP propagation.

## Consumer impact

| Repo | Action | Status (2026-05-22) |
| --- | --- | --- |
| `ontogony-platform` | Use `OntogonyPropagationHeaderContract`, gate checks, `HeaderPropagationConformanceAssertions` | **Done** (PLATFORM-9-003) |
| `allagma-dotnet` | ALLAGMA-PROP-001 — `AllagmaOutboundPropagationConformanceTests` | **Done** |
| `kanon-dotnet` | KANON-PROP-001 — `KanonConexusAssistancePropagationConformanceTests` | **Done** |
| `conexus-dotnet` | CONEXUS-PROP-001 — `ConexusOutboundPropagationConformanceTests` | **Done** |

Closeout index: [`docs/CURRENT_STATE.md`](../CURRENT_STATE.md).

## API

- `Ontogony.Http.OntogonyPropagationHeaderContract` — frozen header list + `AllagmaRunId`
- `Ontogony.Testing.PropagationHeaderScenario` — test scenario DTO
- `Ontogony.Testing.HeaderPropagationConformanceAssertions` — outbound proof helpers

## Idempotency header note

The frozen set uses canonical `X-Ontogony-Idempotency-Key`. Legacy `Idempotency-Key` remains accepted inbound and in `HasIdempotencyKey`; services may continue emitting legacy on inbound interop only.
