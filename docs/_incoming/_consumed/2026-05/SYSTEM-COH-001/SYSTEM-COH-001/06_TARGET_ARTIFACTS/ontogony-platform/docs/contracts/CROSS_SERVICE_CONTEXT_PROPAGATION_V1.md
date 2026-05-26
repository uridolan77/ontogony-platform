# Cross-service context propagation v1

## Purpose

Define neutral context propagation fields used by Ontogony services. This contract is product-neutral and belongs in `ontogony-platform`.

## Headers / fields

| Context | Header / field | Required? | Notes |
|---|---|---:|---|
| W3C trace | `traceparent` | recommended | Preserve if supplied |
| Ontogony trace | `X-Ontogony-Trace-Id` | recommended | Compatibility / explicit trace id |
| Correlation | `X-Ontogony-Correlation-Id` | recommended | Legacy `X-Correlation-ID` may be accepted |
| Idempotency | `Idempotency-Key` / `X-Ontogony-Idempotency-Key` | per mutation | Do not blindly forward; derive scoped downstream keys |
| Actor id | `X-Ontogony-Actor-Id` | semantic/policy calls | Must not be leaked to model provider unless explicitly approved |
| Actor type | `X-Ontogony-Actor-Type` | semantic/policy calls | e.g. `human`, `service`, `agent` |
| Actor roles | `X-Ontogony-Actor-Roles` or `X-Ontogony-Roles` | semantic/policy calls | Role delimiter must be documented by consuming service |
| Runtime run id | `X-Allagma-Run-Id` | runtime calls | Allagma-owned execution context |

## Privacy rule

Actor identity and role context are policy/semantic context. They should propagate to Kanon. They should not propagate to Conexus/model providers unless a specific product/privacy review approves it.

## Idempotency derivation

Services should derive operation-scoped downstream keys instead of reusing the caller root key.

Examples:

```text
allagma:{runId}:plan
allagma:{runId}:tool:{toolIntentId}
allagma:{runId}:model:{purpose}
kanon:{decisionId}:assistance:{assistanceType}
```

## Acceptance

SYSTEM-COH-001 must prove these rules are either implemented, documented as transitional, or explicitly deferred.
