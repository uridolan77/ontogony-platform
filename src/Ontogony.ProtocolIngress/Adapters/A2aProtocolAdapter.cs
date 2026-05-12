using System.Text.Json;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.Primitives;

namespace Ontogony.ProtocolIngress.Adapters;

/// <summary>
/// Adapter for normalizing A2A (Agent-to-Agent) protocol events.
/// Mechanical normalization without choreography semantics.
/// </summary>
public sealed class A2aProtocolAdapter : BaseProtocolIngressAdapter, IProtocolIngressAdapter<A2aEvent>
{
    private const string ProtocolName = "a2a";

    public A2aProtocolAdapter(
        PayloadHasher payloadHasher,
        IIdGenerator idGenerator,
        IClock clock,
        IEnvelopeValidator envelopeValidator)
        : base(payloadHasher, idGenerator, clock, envelopeValidator)
    {
    }

    public ProtocolIngressResult Normalize(A2aEvent raw, ProtocolIngressContext context)
    {
        if (raw == null)
            return ProtocolIngressResult.Failure(nameof(raw), "A2aEvent cannot be null.");

        if (string.IsNullOrWhiteSpace(raw.MessageType))
            return ProtocolIngressResult.Failure(nameof(A2aEvent.MessageType),
                "A2A messageType is required.");

        if (string.IsNullOrWhiteSpace(raw.SenderId))
            return ProtocolIngressResult.Failure(nameof(A2aEvent.SenderId),
                "A2A senderId is required.");

        if (string.IsNullOrWhiteSpace(raw.ReceiverId))
            return ProtocolIngressResult.Failure(nameof(A2aEvent.ReceiverId),
                "A2A receiverId is required.");

        // Extract trace ID from context or event
        var traceId = raw.TraceId;
        var finalTraceId = ValidateOrGenerateTraceId(traceId, context, out var traceIdError);
        if (traceIdError != null)
            return traceIdError;

        var timestamp = NormalizeTimestamp(raw.Timestamp, context);
        var messageId = raw.MessageId ?? IdGenerator.NewGuid().ToString();

        var rawJson = JsonSerializer.Serialize(raw);
        var canonicalPayloadHash = ComputeCanonicalPayloadHash(rawJson);

        var rawPayload = new RawProtocolPayload
        {
            Protocol = ProtocolName,
            RawJson = rawJson,
            RawEventType = raw.MessageType,
            ParsedObject = raw,
            CanonicalPayloadHash = canonicalPayloadHash
        };

        var envelope = new OntogonyEnvelope<RawProtocolPayload>
        {
            EventId = messageId,
            EventType = NormalizeEnvelopeEventType(ProtocolName),
            Source = NormalizeSourceUri(ProtocolName, raw.SenderId),  // Normalize to absolute URI
            OccurredAt = timestamp,
            TraceId = finalTraceId!,
            SpanId = context.SpanId ?? raw.SpanId,
            ParentSpanId = context.ParentSpanId ?? raw.ParentSpanId,
            Protocol = ProtocolName,
            Payload = rawPayload,
            PayloadHash = canonicalPayloadHash,
            TenantId = context.Metadata?.TenantId,
            WorkspaceId = context.Metadata?.WorkspaceId,
            ProjectId = context.Metadata?.ProjectId,
            ActorId = context.Metadata?.ActorId ?? raw.SenderId,
            SessionId = context.Metadata?.SessionId
        };

        // Validate envelope against platform contracts
        return ValidateAndReturnEnvelope(envelope);
    }
}

/// <summary>
/// DTO representing an A2A protocol event.
/// </summary>
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
