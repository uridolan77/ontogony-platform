# EventEnvelopeExample

This is a documentation-only example for Ontogony.Contracts + Ontogony.Messaging usage.

Status: Documentation-only (not compiled by Ontogony.Platform.sln).

## Goal

Show how to build and publish a protocol-neutral envelope.

## Sample

```csharp
var envelope = new OntogonyEnvelope<MyPayload>
{
    EventId = "evt_123",
    EventType = "agent.run.started",
    Source = "agentor://api",
    OccurredAt = DateTimeOffset.UtcNow,
    TraceId = "trace_abc",
    Protocol = "mcp",
    Payload = new MyPayload("value")
};

await eventPublisher.PublishAsync(envelope);
```

## Do Not Do This

- Do not embed Athanor/Agentor domain entities in shared envelope contracts.
- Do not omit required envelope fields for emitted events.
