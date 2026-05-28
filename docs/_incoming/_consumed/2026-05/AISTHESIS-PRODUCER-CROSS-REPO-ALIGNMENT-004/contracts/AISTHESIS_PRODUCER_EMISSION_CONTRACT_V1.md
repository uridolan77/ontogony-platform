# Aisthesis producer emission contract v1

Every producer envelope emitted to Aisthesis must include:

| Field | Requirement |
|---|---|
| `evidenceId` | Stable enough for edges to resolve |
| `traceId` | Required |
| `correlationId` | Required when available |
| `producerSystem` | `allagma`, `kanon`, `conexus`, or `metabole` |
| `evidenceType` | Canonical evidence type |
| `occurredAtUtc` | Producer event time |
| `summary` | Human-readable summary |
| native IDs | Required by producer role |
| `payloadFingerprint` or `payloadRef` | Required for significant evidence |

Edges must reference actual evidence IDs, use canonical relation names, be idempotent, and use the same trace ID.

Batch writes should use stable `batchId` and `idempotencyKey`.
