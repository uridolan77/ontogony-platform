# Evidence Identifier Contract

**Owner:** `Ontogony.Contracts.References`
**Schema:** `schemas/contracts/evidence-identifier.schema.json`

Evidence identifiers are the linking primitives that connect a trace, an artifact, a subject, and
an actor across the six-repo system.  Platform owns the shapes; product repos own the semantics.

---

## Reference types

### `TraceRef`

Links a unit of work to its distributed trace.

| Field | Type | Description |
| --- | --- | --- |
| `traceId` | string | Distributed trace identifier |
| `spanId` | string? | Optional span identifier |
| `traceparent` | string? | W3C traceparent |

### `ArtifactRef`

Links to a stored artifact produced by execution.

| Field | Type | Description |
| --- | --- | --- |
| `artifactId` | string | Unique artifact identifier |
| `artifactType` | string | Type discriminator (product-defined) |
| `storageKey` | string? | Optional storage-layer key |

### `SubjectRef`

Identifies the entity a decision or action was applied to.

| Field | Type | Description |
| --- | --- | --- |
| `subjectId` | string | Subject identifier |
| `subjectType` | string | Type discriminator (product-defined) |
| `namespace` | string? | Optional namespace qualifier |

### `ActorRef`

Identifies the actor that initiated an action.

| Field | Type | Description |
| --- | --- | --- |
| `actorId` | string | Actor identifier (maps to `X-Ontogony-Actor-Id`) |
| `actorType` | string | Actor type classifier (maps to `X-Ontogony-Actor-Type`) |
| `roles` | string[] | Actor roles (maps to `X-Ontogony-Actor-Roles`) |

---

## Evidence envelope

A complete evidence record links all four reference types plus the platform trace context:

```json
{
  "trace": { "traceId": "4bf92f...", "traceparent": "00-4bf92f...-0-01" },
  "artifact": { "artifactId": "art-001", "artifactType": "run-journal" },
  "subject": { "subjectId": "run-abc123", "subjectType": "allagma.run" },
  "actor": { "actorId": "usr-001", "actorType": "operator", "roles": ["admin"] },
  "correlationId": "a1b2c3d4-...",
  "allagmaRunId": "run-abc123",
  "timestamp": "2026-05-23T10:00:00Z"
}
```

---

## Compliance requirements

- Allagma run events must include `TraceRef` and `ArtifactRef`.
- Kanon decision records must include `TraceRef` and `SubjectRef`.
- Conexus route decisions must include `TraceRef` and `ActorRef`.
- Frontend evidence spine must resolve all four reference types given any starting identifier.
- UI `EvidenceLink` contract (see `ontogony-ui` UI-9-002) must be a neutral rendering of
  `TraceRef`, `ArtifactRef`, `SubjectRef`, and `ActorRef`.
