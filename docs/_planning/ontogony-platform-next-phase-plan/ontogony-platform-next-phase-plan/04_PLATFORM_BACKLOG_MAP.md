# Platform Backlog Map

## Immediate

| ID | Title | Why |
|---|---|---|
| PLAT-INT-001 | Service-to-service integration conventions | Kanon/Agentor/Conexus need consistent call mechanics |
| PLAT-OBS-001 | Integration metrics helper | Avoid duplicated integration instrumentation |
| PLAT-TEST-001 | Architecture test helpers | Reuse forbidden dependency checks across repos |

## Conditional extraction

| ID | Title | Trigger |
|---|---|---|
| PLAT-PG-001 | Shared Postgres idempotency ledger | Two services need durable idempotency |
| PLAT-PG-002 | Shared Postgres execution journal | Two services need durable execution journal |
| PLAT-ART-001 | Durable artifact store | Two services need durable artifacts |
| PLAT-HOST-001 | Readiness contributor helper | Readiness pattern repeats |

## Rejected for platform

```text
KanonClient
AgentorClient
ConexusClient
Provider SDKs
Agent runtime framework
Kanon ontology/canonization logic
Conexus routing policy
```
