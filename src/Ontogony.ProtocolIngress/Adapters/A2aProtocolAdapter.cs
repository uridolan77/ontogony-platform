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

    /// <summary>Creates an A2A ingress adapter.</summary>
    public A2aProtocolAdapter(
        PayloadHasher payloadHasher,
        IIdGenerator idGenerator,
        IClock clock,
        IEnvelopeValidator envelopeValidator)
        : base(payloadHasher, idGenerator, clock, envelopeValidator)
    {
    }

    /// <inheritdoc />
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
        var rawPayloadHash = ComputeRawPayloadHash(rawJson);
        var canonicalPayloadHash = ComputeCanonicalPayloadHash(rawJson);

        var rawPayload = new RawProtocolPayload
        {
            Protocol = ProtocolName,
            RawJson = rawJson,
            RawEventType = raw.MessageType,
            ParsedObject = raw,
            RawPayloadHash = rawPayloadHash,
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
    /// <summary>Optional message identifier.</summary>
    public string? MessageId { get; init; }

    /// <summary>A2A message type string.</summary>
    public required string MessageType { get; init; }

    /// <summary>Sender agent identifier.</summary>
    public required string SenderId { get; init; }

    /// <summary>Receiver agent identifier.</summary>
    public required string ReceiverId { get; init; }

    /// <summary>Optional message timestamp.</summary>
    public DateTimeOffset? Timestamp { get; init; }

    /// <summary>Optional trace identifier.</summary>
    public string? TraceId { get; init; }

    /// <summary>Optional span identifier.</summary>
    public string? SpanId { get; init; }

    /// <summary>Optional parent span identifier.</summary>
    public string? ParentSpanId { get; init; }

    /// <summary>Optional protocol-specific payload object.</summary>
    public object? Payload { get; init; }
}
