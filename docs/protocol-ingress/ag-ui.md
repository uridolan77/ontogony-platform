# AG-UI Protocol Ingress

## Overview

The `AgUiProtocolAdapter` normalizes Agent UI events into `OntogonyEnvelope<RawProtocolPayload>` records. These events capture user interactions and UI state changes without business logic interpretation.

## AG-UI Event DTO

```csharp
public sealed record AgUiEvent
{
    public string? EventId { get; init; }
    public required string Action { get; init; }
    public required string SessionId { get; init; }
    public string? UserId { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
    public string? TraceId { get; init; }
    public string? SpanId { get; init; }
    public string? ParentSpanId { get; init; }
    public object? Data { get; init; }
}
```

## Normalization Mapping

| AG-UI Field | OntogonyEnvelope Field | Notes |
|---|---|---|
| `EventId` | `EventId` | Generated if missing. |
| `Action` | `EventType` | Required (e.g., "click.button", "form.submitted"). |
| `SessionId` | `SessionId` | Required. |
| `UserId` | `ActorId` | Inherited from context if present, otherwise from event. |
| `Timestamp` | `OccurredAt` | Normalized to UTC. Fallback to context or current time. |
| `TraceId` | `TraceId` | Extracted from event or context. Requires generation policy if missing. |
| `SpanId` | `SpanId` | Inherited from context if present, otherwise from event. |
| `ParentSpanId` | `ParentSpanId` | Inherited from context if present, otherwise from event. |
| N/A | `Source` | Set to `"ag-ui"` (constant). |
| N/A | `Protocol` | Set to `"ag-ui"`. |

## Validation Rules

1. **Action** must be present and non-empty.
2. **SessionId** must be present and non-empty.
3. **TraceId** policy applied: use event value, context value, or fail/generate.
4. **EventId** generated if missing (UUID).

## Usage Example

```csharp
var adapter = new AgUiProtocolAdapter(payloadHasher, idGenerator);

var agUiEvent = new AgUiEvent
{
    Action = "button.click",
    SessionId = "session-xyz",
    UserId = "user-456",
    TraceId = "trace-abc",
    Timestamp = DateTimeOffset.UtcNow,
    Data = new { buttonId = "submit-btn", form = "user-profile" }
};

var context = new ProtocolIngressContext();
var result = adapter.Normalize(agUiEvent, context);

if (result.IsSuccess)
{
    var envelope = result.Envelope!;
    // envelope.EventType: "button.click"
    // envelope.SessionId: "session-xyz"
    // envelope.ActorId: "user-456"
    // envelope.Source: "ag-ui"
    // envelope.Protocol: "ag-ui"
}
```

## Session Tracking

AG-UI events are grouped by `SessionId` for session-level analysis:

```csharp
var events = new[]
{
    new AgUiEvent { Action = "page.loaded", SessionId = "session-1", ... },
    new AgUiEvent { Action = "button.click", SessionId = "session-1", ... },
    new AgUiEvent { Action = "form.submitted", SessionId = "session-1", ... }
};

// All events share the same SessionId and TraceId for correlation
```

## User Attribution

AG-UI events capture the user acting on the UI:

```csharp
var agUiEvent = new AgUiEvent
{
    Action = "form.submitted",
    SessionId = "session-xyz",
    UserId = "user-456"  // Captured from session context
};

var result = adapter.Normalize(agUiEvent, context);
// result.Envelope!.ActorId: "user-456"  // User responsible for the action
```

If `UserId` is not provided in the event or context, the adapter will use a placeholder or null.

## Action Naming Conventions

Recommended action naming patterns:

| Category | Examples |
|---|---|
| **Page lifecycle** | `page.loaded`, `page.unloaded`, `page.navigated` |
| **Button interactions** | `button.click`, `button.hover`, `button.focus` |
| **Form interactions** | `form.submitted`, `form.reset`, `input.changed` |
| **Modal/dialog** | `modal.opened`, `modal.closed` |
| **List/table** | `row.selected`, `row.deleted`, `filter.applied` |
| **Custom actions** | `agent.asked_question`, `result.copied`, `tool.invoked` |

Action names follow kebab-case convention for consistency.

## Trace Correlation

AG-UI events can be correlated with backend operations via `TraceId`:

```csharp
// Frontend: user clicks button
var uiEvent = new AgUiEvent
{
    Action = "button.click",
    TraceId = "trace-xyz",
    SessionId = "session-123"
};

// Backend: receives request with same trace ID
var apiEvent = new McpEvent
{
    EventType = "tool.invoked",
    TraceId = "trace-xyz"  // same trace
};

// Both events share trace-xyz for end-to-end visibility
```

## Usage Analytics

AG-UI envelopes support aggregation for analytics:

```csharp
// Group by action
var buttonClicks = envelopes
    .Where(e => e.EventType == "button.click")
    .Count();

// Group by session
var eventsPerSession = envelopes
    .GroupBy(e => e.SessionId)
    .ToDictionary(g => g.Key, g => g.Count());

// Timeline by timestamp
var timeline = envelopes
    .OrderBy(e => e.OccurredAt)
    .ToList();
```

## See Also

- [`overview.md`](overview.md) — Protocol Ingress Overview
