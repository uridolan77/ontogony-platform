# Scope and Boundaries

## Platform owns mechanics

```text
trace/correlation propagation
actor context propagation
idempotency key/header conventions
integration HTTP registration
integration error classification
integration metrics
hashing/fingerprints
artifact references/stores
execution journal mechanics
persistence/outbox mechanics
testing fixtures and architecture-test helpers
```

## Platform does not own meaning

```text
Kanon ontology/canonical fact semantics
Agentor planning/execution semantics
Conexus provider routing/model-selection strategy
iGaming/domain rules
human approval business rules
RAG/graph extraction meaning
UI/workbench behavior
```

## Placement rule

If the question is “how do services mechanically call, trace, hash, persist, or measure something?”, it may belong in platform.

If the question is “what does this decision, fact, action, plan, or model output mean?”, it belongs in a product repo.
