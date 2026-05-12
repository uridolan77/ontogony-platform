# MCP Protocol Ingress

## Overview

The `McpProtocolAdapter` normalizes Model Context Protocol events into `OntogonyEnvelope<RawProtocolPayload>` records.

## MCP Event DTO

```csharp
public sealed record McpEvent
{
    public string? EventId { get; init; }
    public required string EventType { get; init; }
    public required string Source { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
    public string? TraceId { get; init; }
    public string? SpanId { get; init; }
    public string? ParentSpanId { get; init; }
    public string? ActorId { get; init; }
    public string? SessionId { get; init; }
    public object? Data { get; init; }
}
```

## Normalization Mapping

| MCP Field | OntogonyEnvelope Field | Notes |
|---|---|---|
| `EventId` | `EventId` | Generated if missing. |
| `EventType` | `EventType` | Required. |
| `Source` | `Source` | Required. |
| `Timestamp` | `OccurredAt` | Normalized to UTC. Fallback to context or current time. |
| `TraceId` | `TraceId` | Extracted from event. Requires context or generation policy if missing. |
| `SpanId` | `SpanId` | Inherited from context if present, otherwise from event. |
| `ParentSpanId` | `ParentSpanId` | Inherited from context if present, otherwise from event. |
| `ActorId` | `ActorId` | Inherited from context if present, otherwise from event. |
| `SessionId` | `SessionId` | Inherited from context if present, otherwise from event. |
| N/A | `Protocol` | Set to `"mcp"`. |

## Validation Rules

1. **EventType** must be present and non-empty.
2. **Source** must be present and non-empty.
3. **TraceId** policy applied: use event value, context value, or fail/generate.
4. **EventId** generated if missing (UUID).

## Usage Example

```csharp
var adapter = new McpProtocolAdapter(payloadHasher, idGenerator);

var mcpEvent = new McpEvent
{
    EventType = "tool.called",
    Source = "mcp-agent",
    TraceId = "trace-xyz",
    SpanId = "span-456",
    ActorId = "agent-123",
    SessionId = "session-789",
    Data = new { toolName = "file_read", arguments = new { path = "/tmp/example.txt" } }
};

var context = new ProtocolIngressContext();
var result = adapter.Normalize(mcpEvent, context);

if (result.IsSuccess)
{
    var envelope = result.Envelope!;
    // envelope.EventType: "tool.called"
    // envelope.Source: "mcp-agent"
    // envelope.TraceId: "trace-xyz"
    // envelope.SpanId: "span-456"
    // envelope.ActorId: "agent-123"
    // envelope.SessionId: "session-789"
    // envelope.Protocol: "mcp"
}
```

## Trace ID and Span Context Inheritance

The adapter supports **hierarchical trace context**:

```csharp
var context = new ProtocolIngressContext
{
    TraceId = "trace-parent",
    SpanId = "span-parent",
    ParentSpanId = "span-grandparent"
};

// If the MCP event has its own trace context, it takes precedence
var mcpEvent = new McpEvent
{
    TraceId = "trace-override",  // wins over context.TraceId
    SpanId = "span-event",        // wins over context.SpanId
    // ...
};

var result = adapter.Normalize(mcpEvent, context);
// result.Envelope!.TraceId: "trace-override"
// result.Envelope!.SpanId: "span-event"
```

## Distributed Tracing

MCP events naturally carry distributed tracing metadata:

- **TraceId**: Correlation across service calls.
- **SpanId**: Identifies this event within the trace.
- **ParentSpanId**: Links to the calling span.

The adapter preserves these fields transparently, enabling full trace reconstruction.

## EventId Auto-Generation

If the MCP event does not specify `EventId`, the adapter generates a UUID:

```csharp
var mcpEvent = new McpEvent
{
    // EventId omitted
    EventType = "tool.called",
    Source = "mcp-agent",
    TraceId = "trace-xyz"
};

var result = adapter.Normalize(mcpEvent, context);
// result.Envelope!.EventId: auto-generated UUID
```

## See Also

- [`overview.md`](overview.md) — Protocol Ingress Overview
