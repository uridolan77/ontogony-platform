# 02 — Target architecture

## Runtime ownership

```text
Client / Operator
  → Frontend operator console
  → Allagma /allagma/v0            governed runtime, run lifecycle, audit bundle
      → Kanon /ontology/v0         semantic plan, action policy, human gates, provenance
      → Conexus /v1/* /admin/v0    model completion, routing, provider evidence, quota/cost
  → Evidence Spine                 cross-service evidence resolver
```

## Authority rules

| Authority | Owner | Must not own |
|---|---|---|
| Runtime orchestration | Allagma | semantic truth, model routing |
| Semantic authority | Kanon | provider routing, workflow execution |
| Model gateway | Conexus | ontology meaning, agent state |
| Shared mechanics | Ontogony.Platform | product semantics |
| Operator presentation | frontend/ui | backend authority |

## Tight integration data chain

A complete operator audit should walk this chain:

```text
Allagma runId
  → Allagma run detail / events / audit bundle
  → topologyAuthorizationDecisionId / planningDecisionId / humanGateId
  → Kanon decision record
  → Kanon provenance / verify / semantic graph / replay bundle
  → Conexus modelCallId / routeDecisionId / model evidence bundle
  → Conexus route decision / provider attempt / usage-cost / quota context
  → Cross-service traceId / correlationId
```

## System artifacts that must agree

| Artifact | Owner | Purpose |
|---|---|---|
| `ontogony-runtime.lock.json` | Allagma | Pinned runtime baseline |
| `SYSTEM_COMPATIBILITY_MATRIX.md` | Allagma | Human-readable runtime matrix |
| `allagma-feature-connection.matrix.json` | Allagma | Endpoint/call/link/gap catalog |
| `KANON_COMPATIBILITY_MANIFEST.json` | Kanon | Frozen semantic authority contract |
| Conexus client/admin contracts | Conexus | Gateway, evidence, routing, quota contracts |
| System protocol registry | Platform | Shared evidence/protocol registry |
| Operator route catalogs | Frontend | UI consumption of backend contracts |

## Non-production alpha boundary

This architecture remains alpha unless all of the following graduate:

- enterprise identity and service-to-service auth,
- production secret and key rotation,
- production SLO monitoring,
- production incident runbooks,
- real tool execution trust model,
- multi-node idempotency classification across all effects,
- external artifact store durability and retention policy.
