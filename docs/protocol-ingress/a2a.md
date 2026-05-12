# A2A Protocol Ingress

## Overview

The `A2aProtocolAdapter` normalizes Agent-to-Agent (A2A) protocol messages into `OntogonyEnvelope<RawProtocolPayload>` records.

## A2A Event DTO

```csharp
public sealed record A2aEvent
{
    public string? MessageId { get; init; }
    public required string MessageType { get; init; }
    public required string SenderId { get; init; }
    public required string ReceiverId { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
    public string? TraceId { get; init; }
    public string? SpanId { get; init; }
    public string? ParentSpanId { get; init; }
    public object? Payload { get; init; }
}
```

## Normalization Mapping

| A2A Field | OntogonyEnvelope Field | Notes |
|---|---|---|
| `MessageId` | `EventId` | Generated if missing. |
| `MessageType` | `EventType` | Required. |
| `SenderId` | `Source` | Required. Also populates `ActorId`. |
| `ReceiverId` | (not mapped) | Available in raw payload for filtering/routing. |
| `Timestamp` | `OccurredAt` | Normalized to UTC. Fallback to context or current time. |
| `TraceId` | `TraceId` | Extracted from event or context. Requires generation policy if missing. |
| `SpanId` | `SpanId` | Inherited from context if present, otherwise from event. |
| `ParentSpanId` | `ParentSpanId` | Inherited from context if present, otherwise from event. |
| N/A | `Protocol` | Set to `"a2a"`. |

## Validation Rules

1. **MessageType** must be present and non-empty.
2. **SenderId** must be present and non-empty.
3. **ReceiverId** must be present and non-empty.
4. **TraceId** policy applied: use event value, context value, or fail/generate.
5. **MessageId** generated if missing (UUID).

## Usage Example

```csharp
var adapter = new A2aProtocolAdapter(payloadHasher, idGenerator);

var a2aEvent = new A2aEvent
{
    MessageType = "request",
    SenderId = "agent-coordinator",
    ReceiverId = "agent-executor",
    TraceId = "trace-xyz",
    SpanId = "span-456",
    Timestamp = DateTimeOffset.UtcNow,
    Payload = new { action = "execute_task", taskId = "task-123" }
};

var context = new ProtocolIngressContext();
var result = adapter.Normalize(a2aEvent, context);

if (result.IsSuccess)
{
    var envelope = result.Envelope!;
    // envelope.EventType: "request"
    // envelope.Source: "agent-coordinator"
    // envelope.ActorId: "agent-coordinator"
    // envelope.TraceId: "trace-xyz"
    // envelope.Protocol: "a2a"
}
```

## Source and Actor Mapping

The A2A adapter maps `SenderId` to both `Source` and `ActorId`:

```csharp
// SenderId becomes the agent initiating the message
var a2aEvent = new A2aEvent
{
    SenderId = "agent-service-1",
    ReceiverId = "agent-service-2",
    MessageType = "request",
    // ...
};

var result = adapter.Normalize(a2aEvent, context);
// result.Envelope!.Source: "agent-service-1"
// result.Envelope!.ActorId: "agent-service-1"
```

This enables:
- Filtering events by originating agent.
- Attributing side effects to the sending agent.
- Correlating request/response patterns.

## ReceiverId in Payload

The `ReceiverId` is preserved in the raw protocol payload for routing and choreography decisions:

```csharp
var envelope = result.Envelope!;
var a2aPayload = envelope.Payload;
var rawJson = a2aPayload.RawJson;

// Parse rawJson to extract ReceiverId for routing
// This keeps A2A routing logic independent of OntogonyEnvelope
```

## Choreography without Orchestration

A2A messages support distributed choreography:

1. **Request**: Agent-1 sends to Agent-2.
2. **Response**: Agent-2 sends back to Agent-1 with same TraceId.
3. **Correlation**: TraceId and SpanId link request/response.

The adapter does not interpret choreography rules—each service decides how to handle A2A messages independently.

## Example: Request-Response Pattern

```csharp
// Request
var requestEvent = new A2aEvent
{
    MessageType = "execute_request",
    SenderId = "coordinator",
    ReceiverId = "executor",
    TraceId = "trace-xyz",
    SpanId = "span-1"
};

var requestResult = adapter.Normalize(requestEvent, context);

// Response (same trace, different span)
var responseEvent = new A2aEvent
{
    MessageType = "execute_response",
    SenderId = "executor",
    ReceiverId = "coordinator",
    TraceId = "trace-xyz",        // same trace
    SpanId = "span-2",             // different span
    ParentSpanId = "span-1"        // child of request span
};

var responseResult = adapter.Normalize(responseEvent, context);

// Coordinator can reconstruct conversation by TraceId
```

## See Also

- [`overview.md`](overview.md) — Protocol Ingress Overview
