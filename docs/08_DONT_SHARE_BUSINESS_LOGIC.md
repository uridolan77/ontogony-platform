# 08 — Do Not Share Business Logic

Shared infrastructure feels efficient until it becomes the hidden coupling layer that breaks every service at once.

## Rule

A type is allowed here only if it can be used by a completely unrelated service without importing Ontogony's business concepts.

## Examples

Allowed:

```text
TraceContext
ApiError
PayloadHasher
IEventPublisher
CorrelationHeadersDelegatingHandler
IClock
CurrentActor
IdempotencyLedger
```

Not allowed:

```text
CanonicalDecision
KnowledgeObjectVersion
AgentRunPlan
RefundApprovalPolicy
ConexusRoutingRule
ResponsibleGamingAction
SnapshotReadiness
ContradictionResolution
```

## Where those belong

| Concept | Owning repo |
| --- | --- |
| Canonization, snapshots, contradictions | Athanor |
| Agent runs, plans, tools, skills | Agentor |
| Provider routing, model aliases, cost logic | Conexus |
| iGaming workflows | domain app / plugin |
| AG-UI/MCP/A2A normalized payloads | Athanor protocol adapters or future recorder packages |

## Healthy shared contracts

You may define references, not meanings:

```csharp
public sealed record EntityRef(string Type, string Id, string? Version = null);
```

You should not define semantic finality:

```csharp
public sealed record CanonicalKnowledgeTruth(...); // forbidden here
```
