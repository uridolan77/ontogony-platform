using System.Text.Json;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.Primitives;

namespace Ontogony.ProtocolIngress.Adapters;

/// <summary>
/// Adapter for normalizing AG-UI (Agent UI) protocol events.
/// Mechanical normalization without UI state interpretation.
/// </summary>
public sealed class AgUiProtocolAdapter : BaseProtocolIngressAdapter, IProtocolIngressAdapter<AgUiEvent>
{
    private const string ProtocolName = "ag-ui";

    public AgUiProtocolAdapter(PayloadHasher payloadHasher, IIdGenerator idGenerator)
        : base(payloadHasher, idGenerator)
    {
    }

    public ProtocolIngressResult Normalize(AgUiEvent raw, ProtocolIngressContext context)
    {
        if (raw == null)
            return ProtocolIngressResult.Failure(nameof(raw), "AgUiEvent cannot be null.");

        if (string.IsNullOrWhiteSpace(raw.Action))
            return ProtocolIngressResult.Failure(nameof(AgUiEvent.Action),
                "AG-UI action is required.");

        if (string.IsNullOrWhiteSpace(raw.SessionId))
            return ProtocolIngressResult.Failure(nameof(AgUiEvent.SessionId),
                "AG-UI sessionId is required.");

        // Extract trace ID from context or event
        var traceId = raw.TraceId;
        var finalTraceId = ValidateOrGenerateTraceId(traceId, context, out var traceIdError);
        if (traceIdError != null)
            return traceIdError;

        var timestamp = NormalizeTimestamp(raw.Timestamp, context);
        var eventId = raw.EventId ?? IdGenerator.NewGuid().ToString();

        var rawJson = JsonSerializer.Serialize(raw);
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
            EventId = eventId,
            EventType = raw.Action,
            Source = "ag-ui",
            OccurredAt = timestamp,
            TraceId = finalTraceId!,
            SpanId = context.SpanId ?? raw.SpanId,
            ParentSpanId = context.ParentSpanId ?? raw.ParentSpanId,
            Protocol = ProtocolName,
            Payload = rawPayload,
            PayloadHash = payloadHash,
            TenantId = context.Metadata?.TenantId,
            WorkspaceId = context.Metadata?.WorkspaceId,
            ProjectId = context.Metadata?.ProjectId,
            ActorId = context.Metadata?.ActorId ?? raw.UserId,
            SessionId = context.Metadata?.SessionId ?? raw.SessionId
        };

        return ProtocolIngressResult.Success(envelope);
    }
}

/// <summary>
/// DTO representing an AG-UI protocol event.
/// </summary>
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
