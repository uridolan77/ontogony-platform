# Current-state assessment

## Current cut baseline

`SYSTEM-ALPHA-005` is cut complete for Docker-local governed runtime scope, with one remaining quarantine: **B-012 / Docker OTLP + Grafana readiness**.

Alpha-005 explicitly does not claim production readiness and keeps real external tool execution blocked.

## Already implemented system primitives

### Compatibility and runtime lock

Allagma owns the canonical system matrices:

- `docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`
- `docs/system/SYSTEM_ENVIRONMENT_MATRIX.md`
- `docs/system/SYSTEM_AUTH_MATRIX.md`
- `docs/system/SYSTEM_ROUTE_MATRIX.md`
- `docs/system/SYSTEM_TEST_MATRIX.md`
- `docs/system/ontogony-runtime.lock.json`

### Model routing boundary

Allagma now routes through model-purpose configuration:

- Allagma owns model purpose.
- Conexus owns alias-to-provider routing.
- Kanon owns semantic/policy authority.

Current code uses `route.ConexusModelAlias`, not a hard-coded provider model string.

### Error contract

Ontogony.Platform includes:

- `CrossServiceErrorEnvelope`
- `CrossServiceErrorCodes`
- `CrossServiceErrorStage`

Allagma documents how downstream Kanon/Conexus failures map into governed runtime failures.

### Context propagation

Allagma propagates:

- actor/idempotency/run context to Kanon;
- run id and derived idempotency to Conexus non-streaming calls;
- run id only for Conexus streaming calls because streaming idempotency keys are rejected;
- trace/correlation into event payloads when available.

### Conexus contract maturity

Conexus current contracts include:

- `tools`
- `tool_choice`
- `response_format`
- message `tool_calls`
- `tool_call_id`
- `function_call`

Conexus also now has an operator model-call evidence flow tying `modelCallId`, `routeDecisionId`, usage/cost, route evidence, admin evidence links, and evidence bundles.

### Kanon connection maturity

Kanon exposes a broad `/ontology/v0` route surface and connection evidence has advanced through KANON-CONNECT 001–007, including feature mapping, settings consistency, Allagma semantic links, Conexus assistance observability, semantic graph, route parity, and Docker cross-service smoke evidence.

## Still real / not done

- B-012 observability quarantine remains open.
- Current `main` has moved beyond the Alpha-005 lock in several repos.
- Incoming package/stale plan hygiene is not yet enforceable.
- Protocol registry is still distributed across many docs rather than one machine-readable graph.
- Some handwritten matrices must be audited against generated route inventories and typed client calls.
- Production readiness is still not claimed.
- Real external tool execution is still blocked and should remain blocked.
