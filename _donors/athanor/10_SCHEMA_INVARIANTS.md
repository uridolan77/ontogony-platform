# 10 — Schema Invariants

## Identity/version invariant

`KnowledgeObject` is stable identity.

`KnowledgeObjectVersion` is append-only content.

Do not update object-version content in place.

## Project-state invariant

`ProjectObjectState` is authoritative for live project-scoped state.

`KnowledgeObject.current_version_id` is only a convenience/default pointer.

## Snapshot invariant

`CanonicalSnapshot` is immutable.

`CanonicalSnapshotItem` must point to exact object versions.

## Source invariant

`SourceVersion` is immutable.

`EvidenceSpan` points to `SourceVersion`, not only to `Source`.

## Evidence invariant

Accepted claims should bind to evidence spans, not only to source chunks.

## Object/version consistency invariant

Whenever a table stores both an object ID and a version ID, the version must belong to that object.

Affected structures:

```text
ProjectObjectState(object_id, current_version_id)
CanonicalSnapshotItem(object_id, object_version_id)
ContradictionParticipant(object_id, object_version_id)
KnowledgeRelation(from_object_id, from_version_id)
KnowledgeRelation(to_object_id, to_version_id)
```

In PostgreSQL, enforce this with composite uniqueness and composite foreign keys.

## Review invariant

Every epistemic action must create a ReviewEvent.

Examples:

- accept
- reject
- supersede
- merge
- defer
- snapshot_created
- confidence_updated
- relation_status_changed

### ReviewEvent target/action mapping (implemented)

Each `ReviewEvent` must target **exactly one** entity.

| Epistemic action | ReviewEvent target |
|---|---|
| Project-state transition (accept/reject/defer/supersede) | `project_object_state_id` |
| Object content edit (new object version creation) | `object_version_id` |
| Relation review (accept/reject) | `relation_id` |
| Contradiction workflow review (create/resolve/dismiss/defer) | `contradiction_id` |
| Snapshot creation | `canonical_snapshot_id` |

## Resolution invariant

ResolutionEvent records mediation.

ReviewEvent records authority and audit.

A single action may create both.

## Contradiction invariant

`KnowledgeRelation.relation_type = contradicts` is not enough.

A material contradiction requires a `ContradictionCase`.

Manual contradiction workflow is allowed in Turtle 2.

Automatic contradiction detection is deferred.

## Blocking contradiction invariant

Blocking contradiction does not prevent snapshot creation.

It must be explicitly carried and exposed.

## Relation-class invariant

Every accepted relation must have a relation class.

Manual relation workflow is allowed in Turtle 2 (candidate → accept/reject).

Automatic relation detection is deferred.

Classes:

```text
logical
genealogical
structural
compositional
causal
identity
```

## Embedding invariant

Embeddings are only comparable within the same `EmbeddingSpace`.

Do not mix vectors from different models or purposes in one search without explicit policy.

## Neo4j projection invariant

Inside Neo4j:

```text
(ProjectObjectState)-[:HEAD]->(KnowledgeObjectVersion)
```

is the authoritative projection edge.

Any scalar `head_version_id` is a cache only.

## PostgreSQL source-of-truth invariant

PostgreSQL owns canonical writes.

Neo4j does not canonize.

## Metabolic invariants (reserved slot; non-runtime in Turtle 2)

Metabolic entities are system-state, not canonical truth:

- `PredictionRun`, `SurpriseEvent`, `RevisionTransaction`, `EpistemicLedgerEntry`, `MaintenancePolicyState` must not directly mutate canonical state.

Required invariants:

- **Non-canonical metabolism**: metabolism may detect/recommend/trigger/group/meter/govern; it may not canonize.
- **RevisionTransaction does not replace ReviewEvent**: grouping revision work is distinct from authoritative review events.
- **Ledger is append-only**: `EpistemicLedgerEntry` is an immutable accounting log (no silent rewrites).
- **Policy cannot auto-canonize**: policy may schedule/priority/freeze/escalate but cannot accept/reject/resolve/snapshot directly.

## Temporal invariant

`created_at` / `recorded_at` is system time.

`observed_at`, `valid_from`, and `valid_to` are world/domain time.

Do not confuse them.

## Snapshot hash invariant (payload compatibility note)

Snapshot hash verification is only meaningful relative to the canonical payload definition used at the time a snapshot was created.

- Snapshots created before the expanded hash payload rules were stabilized may fail verification later.
- The intended fix is to recreate the snapshot (or backfill/recompute hashes under an explicit migration policy).
