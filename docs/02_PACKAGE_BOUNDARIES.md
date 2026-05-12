# 02 — Package Boundaries

## Package map

| Package | Owns | Must not own |
| --- | --- | --- |
| Ontogony.Contracts | protocol-neutral envelope, refs, common DTO primitives | Athanor/Agentor/Conexus domain logic |
| Ontogony.Observability | trace ID, correlation context, Activity/Meter hooks, outgoing correlation headers | business audit decisions |
| Ontogony.Configuration | startup guards, options validation helpers | service-specific settings meaning |
| Ontogony.Errors | API error contract and exception middleware | domain exception classes from product repos |
| Ontogony.Http | resilient integration client mechanics | integration-specific API semantics |
| Ontogony.Hashing | SHA-256, canonical JSON, payload hash | source truth semantics |
| Ontogony.Idempotency | idempotency ledger abstractions/fingerprints | domain deduplication rules |
| Ontogony.Messaging | in-process event publish/dispatch (`IEventPublisher`, handlers) | business event meaning; durable outbox rows |
| Ontogony.Security | current actor, tenant/project context primitives | final authorization decisions |
| Ontogony.Persistence | outbox / processed-message / dead-letter contracts, clocks, ID helpers | domain repositories; general-purpose ORM |
| Ontogony.Testing | fake clock, event recorder, fixtures | product demo scenarios |

## Dependency direction

```text
Contracts has no internal dependencies.
Observability depends on Contracts.
Errors depends on Observability.
Http depends on Observability.
Hashing depends on Contracts only when necessary.
Idempotency depends on Hashing.
Messaging depends on Contracts.
Testing may depend on all packages.
```

## Smell checklist

A proposed shared type is suspicious if its name includes:

```text
Canonical
KnowledgeObject
AgentRun
ToolPolicy
ProviderRouting
Bonus
Refund
Snapshot
Contradiction
ReviewDecision
```

Those may be valid in product repos. They are usually invalid here.
