# 12 — Neo4j Graph Projection

## Purpose

Neo4j is used for graph traversal, visual exploration, evidence-chain navigation, contradiction neighborhoods, and subgraph exports.

It is not the source of truth.

## Source-of-truth rule

```text
PostgreSQL owns canonical writes.
Neo4j receives projections.
```

## What is projected (PR17–PR17.2)

This repo projects two layers:

- **Canonical layer (PR17)**: accepted project state, accepted relations, contradictions, snapshots.
- **Process/event layer (PR17.1)**: review events, resolution events, workflow runs/steps, processing jobs, LLM interactions, tool calls, agent decisions, snapshot synthesis (read-model).

PR17.2 hardens this layer with safe deletions, consistent error policy, and guarded admin endpoints.

## Deterministic identity policy (process/event nodes)

For process/event nodes, identity is based on PostgreSQL ids (not titles/prompt text):

```text
ReviewEvent.neo4jKey       = "review_event:{id}"
ResolutionEvent.neo4jKey   = "resolution_event:{id}"
WorkflowRun.neo4jKey       = "workflow_run:{id}"
WorkflowStep.neo4jKey      = "workflow_step:{id}"
ProcessingJob.neo4jKey     = "processing_job:{id}"
LlmInteraction.neo4jKey    = "llm_interaction:{id}"
Tool.neo4jKey              = "tool:{id}"
ToolCall.neo4jKey          = "tool_call:{id}"
AgentDecision.neo4jKey     = "agent_decision:{id}"
SnapshotSynthesis.neo4jKey = "snapshot_synthesis:{snapshotId}"
```

## Projection hygiene

Neo4j projection must use:

- namespaced assertion edges
- explicit status fields
- required relation classes
- versioned embedding spaces
- clear HEAD-edge authority
- existence validation queries where constraints are unavailable

## Namespaced assertion edges

Use:

```text
EVIDENCE_ASSERTED_BY
RELATION_ASSERTED_BY
REVIEW_PERFORMED_BY
RESOLUTION_PERFORMED_BY
SOURCE_ASSESSED_BY
```

Do not use a generic `ASSERTED_BY`.

## Projection error policy

Neo4j is a read model.

```text
Neo4j enabled + StrictMode=false → log and continue
Neo4j enabled + StrictMode=true  → throw (fail caller)
Neo4j disabled                  → enqueue is a no-op
```

## Projection enqueue: transactional outbox vs direct (PR47 / PR49.1)

When `ProjectionOutbox:Enabled` is **true**, canonical write paths enqueue work through `IGraphProjectionEnqueueService`, which inserts a `projection_outbox` row in the **same PostgreSQL transaction** as the canon mutation. The background worker / `IProjectionOutboxProcessor` applies rows to Neo4j with retries (`pending` / `failed` / stuck `processing` rows with `attempt_count < MaxAttempts`), using `processing` while a projection call is in flight.

**Outbox-backed entity types (when outbox is enabled)** — see `GraphProjectionEnqueueService`: project state, snapshot, review event, resolution event, workflow run/step, processing job, tool call, agent decision, snapshot synthesis, knowledge relation, epistemic ledger entry, source assessment, rebuild-project.

**Always direct to `IGraphProjectionService` (not outbox-backed today)**:

- `EnqueueLlmInteractionAsync` → `ProjectLlmInteractionAsync`
- `EnqueueSourceChunkAsync` → `ProjectSourceChunkAsync`
- `EnqueueEvidenceSpanAsync` → `ProjectEvidenceSpanAsync`

When `ProjectionOutbox:Enabled` is **false**, every enqueue method calls `IGraphProjectionService` directly (subject to `Neo4j:Enabled` and strict-mode error policy). Admin rebuild endpoints and graph tooling continue to call `IGraphProjectionService` / rebuild APIs directly.

## HEAD authority in graph

Inside Neo4j:

```text
(ProjectObjectState)-[:HEAD]->(KnowledgeObjectVersion)
```

is authoritative.

If a scalar `head_version_id` exists, it is a denormalized cache.

## Explicit status vocabulary

Use:

```text
KnowledgeObject.lifecycle_status
KnowledgeObjectVersion.working_status
ProjectObjectState.working_status
KnowledgeRelation.relation_status
ContradictionCase.status
CanonicalSnapshotItem.snapshot_status
```

Do not use generic `status` on multiple node classes where semantics differ.

## Relation classes

Every relation should have:

```text
relation_type
relation_class
relation_status
```

Relation classes:

```text
logical
genealogical
structural
compositional
causal
identity
```

## Embedding projection

Do not store inline embedding vectors directly on knowledge object versions.

Use:

```text
(content node)-[:HAS_EMBEDDING]->(:Embedding)-[:IN_SPACE]->(:EmbeddingSpace)
```

EmbeddingSpace defines:

```text
provider
model_name
dimensions
distance_metric
purpose
```

## Existence guards

If Neo4j Enterprise existence constraints are unavailable, use validation queries in CI or projection validation.

Load-bearing fields:

```text
KnowledgeObject.canonical_key
KnowledgeObjectVersion.title
KnowledgeObjectVersion.created_at
EvidenceSpan.quote
SourceVersion.content_hash
ProjectObjectState HEAD edge
KnowledgeRelation.relation_class
Embedding IN_SPACE edge
```

## Metabolic nodes

Do not project future metabolic nodes until the canonization loop works.

Reserved later:

```text
PredictionRun
SurpriseEvent
RevisionTransaction
EpistemicLedgerEntry
MaintenancePolicyState
```

## Rebuild safety

Project-scoped rebuild must not use unbounded traversal deletes.

- Canonical rebuild deletes only project-scoped canonical nodes (state, accepted relations, contradictions, snapshots + items).
- Event rebuild deletes only event nodes for that project.
- Process rebuild deletes only process nodes for that project.

Actors, sources, source versions, and tools may be shared across projects and should not be deleted by project rebuild.
