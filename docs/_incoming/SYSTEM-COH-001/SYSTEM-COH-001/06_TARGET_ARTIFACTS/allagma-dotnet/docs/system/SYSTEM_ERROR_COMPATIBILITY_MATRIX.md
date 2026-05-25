# SYSTEM-COH-001 — System error compatibility matrix

## Purpose

This matrix does not force all services to expose the same public error body. It defines how the system classifies and translates service-native failures during governed runtime flows.

## Principles

1. Conexus may expose OpenAI-compatible errors on `/v1/chat/completions`.
2. Kanon may expose Ontogony API errors and typed DTOs on `/ontology/v0`.
3. Allagma must classify downstream failures into stable runtime failure categories.
4. All cross-service failures must retain trace/correlation context where available.
5. Retryability must be explicit.

## Classification fields

| Field | Meaning |
|---|---|
| `system` | Service where the error was observed |
| `stage` | Runtime stage, e.g. `kanon_planning`, `kanon_action_evaluation`, `conexus_model_completion` |
| `nativeCode` | Service-native code where present |
| `cohesionCode` | Stable cross-service classification |
| `retryable` | Whether the orchestrator/client may retry |
| `downstreamSystem` | Downstream service that failed, if any |
| `traceId` | Trace id if known |
| `correlationId` | Correlation id if known |
| `runId` | Allagma run id if known |

## Suggested cohesion codes

| Cohesion code | Stage | Retryable | Notes |
|---|---|---:|---|
| `allagma.kanon.plan_unavailable` | `kanon_planning` | true | Kanon unreachable or transient 5xx during plan compile |
| `allagma.kanon.plan_rejected` | `kanon_planning` | false | Kanon semantically rejects request |
| `allagma.kanon.action_denied` | `kanon_action_evaluation` | false | Policy denial |
| `allagma.kanon.human_gate_required` | `human_gate` | false | Pause, not failure |
| `allagma.kanon.human_gate_denied` | `human_gate` | false | Operator/policy denial |
| `allagma.conexus.provider_unavailable` | `conexus_model_completion` | true | Gateway/provider unavailable |
| `allagma.conexus.model_alias_missing` | `conexus_model_completion` | false | Misconfigured model purpose/alias |
| `allagma.conexus.streaming_idempotency_unsupported` | `conexus_streaming` | false | Allagma should omit idempotency key on streaming |
| `allagma.idempotency.conflict` | `run_start` | false | Same key, different payload |
| `allagma.idempotency.in_progress` | `run_start` | true | Same key in progress |

## Acceptance requirement

SYSTEM-COH-001 must either:

- add tests proving these families are mapped; or
- explicitly mark missing families as `DEFERRED_WITH_REASON` in the acceptance matrix.
