# CloudEvents Protocol Ingress

## Overview

The `CloudEventsProtocolAdapter` normalizes CloudEvents 1.0 compliant events into `OntogonyEnvelope<RawProtocolPayload>` records.

## CloudEvent DTO

```csharp
public sealed record CloudEvent
{
    public required string Id { get; init; }
    public required string Type { get; init; }
    public required string Source { get; init; }
    public DateTimeOffset? Time { get; init; }
    public string? DataContentType { get; init; }
    public object? Data { get; init; }
    public string? DataSchema { get; init; }
    public string? Subject { get; init; }
    public IReadOnlyDictionary<string, object?>? Extensions { get; init; }
}
```

## Normalization Mapping

| CloudEvent Field | OntogonyEnvelope Field | Notes |
|---|---|---|
| `Id` | `EventId` | Required. |
| `Type` | `EventType` | Required. |
| `Source` | `Source` | Required. |
| `Time` | `OccurredAt` | Normalized to UTC. Fallback to context or current time. |
| `Extensions["traceid"]` | `TraceId` | Extracted from extensions if present. Otherwise requires context or generation policy. |
| N/A | `Protocol` | Set to `"cloudevents"`. |

## Validation Rules

1. **Id** must be present and non-empty.
2. **Type** must be present and non-empty.
3. **Source** must be present and non-empty.
4. **TraceId** policy applied: extract from extensions, context, or fail/generate.

## Usage Example

```csharp
var adapter = new CloudEventsProtocolAdapter(payloadHasher, idGenerator);

var cloudEvent = new CloudEvent
{
    Id = "event-123",
    Type = "com.example.user.created",
    Source = "https://user-service/api",
    Time = DateTimeOffset.UtcNow,
    Data = new { userId = "user-456", email = "user@example.com" },
    Extensions = new Dictionary<string, object?>
    {
        { "traceid", "trace-xyz" }
    }
};

var context = new ProtocolIngressContext();
var result = adapter.Normalize(cloudEvent, context);

if (result.IsSuccess)
{
    var envelope = result.Envelope!;
    // envelope.EventId: "event-123"
    // envelope.EventType: "com.example.user.created"
    // envelope.Source: "https://user-service/api"
    // envelope.TraceId: "trace-xyz"
    // envelope.Protocol: "cloudevents"
    // envelope.Payload.RawJson: serialized CloudEvent
}
```

## Trace ID Handling

If the CloudEvent does not carry a trace ID in extensions, ingress will:

1. Use `context.TraceId` if provided.
2. Generate a new ID if `context.IdGenerationPolicy == GenerateIfMissing`.
3. Fail with a structured error if policy is `RequireProvided`.

## Payload Preservation

The entire CloudEvent is serialized as JSON and stored in `RawProtocolPayload.RawJson`. This preserves:
- All extension properties.
- Original data payload.
- Full event metadata.

## See Also

- [`overview.md`](overview.md) — Protocol Ingress Overview
