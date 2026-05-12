# CloudEvents Protocol Ingress

## Overview

The `CloudEventsProtocolAdapter` normalizes CloudEvents 1.0 compliant events into `OntogonyEnvelope<RawProtocolPayload>` records.

## CloudEvent Model

```csharp
public sealed class CloudEventEnvelope
{
    public string SpecVersion { get; set; } = "1.0";
    public required string Id { get; set; }
    public required string Source { get; set; }
    public required string Type { get; set; }
    public string? Time { get; set; }
    public object? Data { get; set; }
    public Dictionary<string, object>? Extensions { get; set; }
}
```

The adapter now uses the canonical CloudEvents shape from `Ontogony.Contracts.Events`.

## Normalization Mapping

| CloudEvent Field | OntogonyEnvelope Field | Notes |
|---|---|---|
| `Id` | `EventId` | Required. |
| `Type` | `Payload.RawEventType` | Preserved raw protocol type. |
| N/A | `EventType` | Mechanical ingress type: `cloudevents.ingress.normalized`. |
| `Source` | `Source` | Normalized to absolute URI as `cloudevents://{source}` when needed. |
| `Time` | `OccurredAt` | Parsed if valid; fallback to context or clock. |
| `Extensions["traceId"]` or `Extensions["traceid"]` | `TraceId` | Extracted from extensions if present. Otherwise uses context or generation policy. |
| `Extensions["traceparent"]` | `Metadata["traceparent"]` | Preserved for W3C trace propagation continuity. |
| `Extensions["tracestate"]` | `Metadata["tracestate"]` | Preserved for W3C trace propagation continuity. |
| N/A | `Protocol` | Set to `"cloudevents"`. |

## Validation Rules

1. **Id** must be present and non-empty.
2. **Type** must be present and non-empty.
3. **Source** must be present and non-empty.
4. **TraceId** policy applied: extract from extensions, context, or fail/generate.

## Usage Example

```csharp
var adapter = new CloudEventsProtocolAdapter(payloadHasher, idGenerator, clock, envelopeValidator);

var cloudEvent = new CloudEventEnvelope
{
    Id = "event-123",
    Type = "com.example.user.created",
    Source = "https://user-service/api",
    Time = DateTimeOffset.UtcNow.ToString("O"),
    Data = new { userId = "user-456", email = "user@example.com" },
    Extensions = new Dictionary<string, object>
    {
        { "traceId", "trace-xyz" }
    }
};

var context = new ProtocolIngressContext();
var result = adapter.Normalize(cloudEvent, context);

if (result.IsSuccess)
{
    var envelope = result.Envelope!;
    // envelope.EventId: "event-123"
    // envelope.EventType: "cloudevents.ingress.normalized"
    // envelope.Payload.RawEventType: "com.example.user.created"
    // envelope.Source: "https://user-service/api"  // absolute URIs are preserved
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
