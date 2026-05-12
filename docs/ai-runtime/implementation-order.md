# Implementation Order

## Phase A — AI mechanical substrate

1. PR36 — `Ontogony.AI.Contracts`
2. PR37 — `Ontogony.Artifacts`
3. PR38 — `Ontogony.Execution`

## Phase B — safety and durability

4. PR39 — `Ontogony.Redaction`
5. PR41 — `Ontogony.Quota`
6. PR42 — `Ontogony.AI.Replay`

## Phase C — knowledge and quality records

7. PR40 — `Ontogony.Knowledge.Contracts`
8. PR43 — `Ontogony.Evaluation.Contracts`
9. PR43 — `Ontogony.Policy.Contracts`

## Consumer pilots

- Conexus: emit `LlmProviderCall`, `LlmUsageRecord`, and `LlmCostRecord`.
- Agentor: attach `ExecutionRun`, `ExecutionStep`, and `ToolCallRecord` to agent runs.
- KB maintainer: attach `DocumentRef`, `ChunkRef`, `EmbeddingRef`, and `ArtifactRef` to ingestion outputs.
