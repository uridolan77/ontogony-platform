# Migration: header propagation contract (PLATFORM-9-003)

## Summary

Frozen cross-service propagation header names and reusable test helpers for proving outbound HTTP propagation.

## Consumer impact

| Repo | Action |
| --- | --- |
| `ontogony-platform` | Use `OntogonyPropagationHeaderContract`, gate checks, `HeaderPropagationConformanceAssertions` |
| `allagma-dotnet` | Add propagation conformance test using `Ontogony.Testing` (optional in follow-up PR) |
| `kanon-dotnet` | Same |
| `conexus-dotnet` | Same |

## API

- `Ontogony.Http.OntogonyPropagationHeaderContract` — frozen header list + `AllagmaRunId`
- `Ontogony.Testing.PropagationHeaderScenario` — test scenario DTO
- `Ontogony.Testing.HeaderPropagationConformanceAssertions` — outbound proof helpers

## Idempotency header note

The frozen set uses canonical `X-Ontogony-Idempotency-Key`. Legacy `Idempotency-Key` remains accepted inbound and in `HasIdempotencyKey`; services may continue emitting legacy on inbound interop only.
