# SYSTEM-COH-001 — System cohesion baseline

## Status

`SYSTEM-COH-001` defines the accepted alpha cohesion baseline for the Ontogony governed runtime.

This file is a closeout-level index. It does not replace the detailed matrices:

- `SYSTEM_COMPATIBILITY_MATRIX.md`
- `SYSTEM_ENVIRONMENT_MATRIX.md`
- `SYSTEM_AUTH_MATRIX.md`
- `SYSTEM_ROUTE_MATRIX.md`
- `SYSTEM_TEST_MATRIX.md`
- `SYSTEM_RUNTIME_CORRECTNESS_MATRIX.md`
- `SYSTEM_TRACE_CONTEXT_MATRIX.md`
- `ONTOGONY_RUNTIME_LOCK.md`

## Ownership model

| Repo | Owns | SYSTEM-COH role |
|---|---|---|
| `ontogony-platform` | neutral mechanics/contracts/schemas | validates shared summary/error/context schemas |
| `conexus-dotnet` | model gateway and routing | model aliases, fallback, model-call telemetry |
| `kanon-dotnet` | semantic authority | decisions, policy, gates, provenance, semantic graph, assistance authority |
| `allagma-dotnet` | governed runtime | canonical acceptance matrix, E2E runner, runtime lock |
| `ontogony-frontend` | operator console | Evidence Spine and operator visibility proof |
| `ontogony-ui` | shared UI primitives | optional status/evidence primitives |

## Accepted scenario families

See `system-cohesion-acceptance.matrix.json` for machine-readable details.

| Scenario | Required for alpha | Expected result |
|---|---:|---|
| Compatibility lock/matrix validation | yes | PASS |
| Service health/readiness | yes | PASS |
| Governed run complete | yes | PASS |
| Idempotent run retry | yes | PASS |
| Human gate pause/resume | yes | PASS or deferred with live-stack reason |
| Kanon → Conexus assistance | yes | PASS or deferred with explicit config reason |
| Conexus fallback | yes | PASS or deferred with provider-config reason |
| Correlation chain | yes | PASS |
| Evidence Spine/operator visibility | yes | PASS or deferred with operator proof reason |
| Restart/replay survival | yes | PASS |
| Package-mode build | release proof | PASS or optional local-only with reason |
| Real tools blocked | yes | PASS |
| Observability evidence gate | alpha advisory | PASS or deferred to SYS-OBS package |

## Real tool policy

Real tool execution remains blocked. SYSTEM-COH-001 may validate and document the trust gate; it must not enable real side effects.

## Closeout evidence

Final closeout must be written to:

```text
docs/evidence/SYSTEM_COH_001_CLOSEOUT.md
artifacts/system-coh-001/<timestamp>/summary.json
```
