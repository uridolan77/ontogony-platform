using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.Primitives;

namespace Ontogony.ProtocolIngress.Adapters;

/// <summary>
/// Adapter for normalizing CloudEvents protocol events.
/// Mechanical conversion without business logic interpretation.
/// </summary>
public sealed class CloudEventsProtocolAdapter : BaseProtocolIngressAdapter, IProtocolIngressAdapter<CloudEvent>
{
    private const string ProtocolName = "cloudevents";

    public CloudEventsProtocolAdapter(PayloadHasher payloadHasher, IIdGenerator idGenerator)
        : base(payloadHasher, idGenerator)
    {
    }

    public ProtocolIngressResult Normalize(CloudEvent raw, ProtocolIngressContext context)
    {
        if (raw == null)
            return ProtocolIngressResult.Failure(nameof(raw), "CloudEvent cannot be null.");

        // Validate required CloudEvents fields
        if (string.IsNullOrWhiteSpace(raw.Id))
            return ProtocolIngressResult.Failure(nameof(CloudEvent.Id),
                "CloudEvent id is required.");

        if (string.IsNullOrWhiteSpace(raw.Type))
            return ProtocolIngressResult.Failure(nameof(CloudEvent.Type),
                "CloudEvent type is required.");

        if (string.IsNullOrWhiteSpace(raw.Source))
            return ProtocolIngressResult.Failure(nameof(CloudEvent.Source),
                "CloudEvent source is required.");

        // Extract trace ID from context or CloudEvents extensions
        var traceId = raw.Extensions?.ContainsKey("traceid") == true
            ? raw.Extensions["traceid"] as string
            : null;

        var finalTraceId = ValidateOrGenerateTraceId(traceId, context, out var traceIdError);
        if (traceIdError != null)
            return traceIdError;

        var timestamp = NormalizeTimestamp(raw.Time, context);

        // Serialize the entire CloudEvent as RawProtocolPayload
        var rawJson = System.Text.Json.JsonSerializer.Serialize(raw);
        var payloadHash = ComputePayloadHash(rawJson);

        var rawPayload = new RawProtocolPayload
        {
            Protocol = ProtocolName,
            RawJson = rawJson,
            ParsedObject = raw,
            PayloadHash = payloadHash
        };

        var envelope = new OntogonyEnvelope<RawProtocolPayload>
        {
            EventId = raw.Id,
            EventType = raw.Type,
            Source = raw.Source,
            OccurredAt = timestamp,
            TraceId = finalTraceId!,
            SpanId = context.SpanId,
            ParentSpanId = context.ParentSpanId,
            Protocol = ProtocolName,
            Payload = rawPayload,
            PayloadHash = payloadHash,
            TenantId = context.Metadata?.TenantId,
            WorkspaceId = context.Metadata?.WorkspaceId,
            ProjectId = context.Metadata?.ProjectId,
            ActorId = context.Metadata?.ActorId,
            SessionId = context.Metadata?.SessionId
        };

        return ProtocolIngressResult.Success(envelope);
    }
}

/// <summary>
/// DTO representing a CloudEvent for ingress normalization.
/// </summary>
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
