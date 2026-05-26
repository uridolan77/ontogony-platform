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

    /// <summary>Creates an AG-UI ingress adapter.</summary>
    public AgUiProtocolAdapter(
        PayloadHasher payloadHasher,
        IIdGenerator idGenerator,
        IClock clock,
        IEnvelopeValidator envelopeValidator)
        : base(payloadHasher, idGenerator, clock, envelopeValidator)
    {
    }

    /// <inheritdoc />
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
        var rawPayloadHash = ComputeRawPayloadHash(rawJson);
        var canonicalPayloadHash = ComputeCanonicalPayloadHash(rawJson);

        var rawPayload = new RawProtocolPayload
        {
            Protocol = ProtocolName,
            RawJson = rawJson,
            RawEventType = raw.Action,
            ParsedObject = raw,
            RawPayloadHash = rawPayloadHash,
            CanonicalPayloadHash = canonicalPayloadHash
        };

        // For AG-UI, source includes session context for better traceability
        var sessionId = context.Metadata?.SessionId ?? raw.SessionId;
        var source = NormalizeSourceUri(ProtocolName, $"session/{sessionId}");

        var envelope = new OntogonyEnvelope<RawProtocolPayload>
        {
            EventId = eventId,
            EventType = NormalizeEnvelopeEventType(ProtocolName),
            Source = source,  // Normalize to absolute URI
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
            ActorId = context.Metadata?.ActorId ?? raw.UserId,
            SessionId = context.Metadata?.SessionId ?? raw.SessionId
        };

        // Validate envelope against platform contracts
        return ValidateAndReturnEnvelope(envelope);
    }
}

/// <summary>
/// DTO representing an AG-UI protocol event.
/// </summary>
public sealed record AgUiEvent
{
    /// <summary>Optional event identifier.</summary>
    public string? EventId { get; init; }

    /// <summary>AG-UI action name.</summary>
    public required string Action { get; init; }

    /// <summary>AG-UI session identifier.</summary>
    public required string SessionId { get; init; }

    /// <summary>Optional user identifier.</summary>
    public string? UserId { get; init; }

    /// <summary>Optional event timestamp.</summary>
    public DateTimeOffset? Timestamp { get; init; }

    /// <summary>Optional trace identifier.</summary>
    public string? TraceId { get; init; }

    /// <summary>Optional span identifier.</summary>
    public string? SpanId { get; init; }

    /// <summary>Optional parent span identifier.</summary>
    public string? ParentSpanId { get; init; }

    /// <summary>Optional protocol-specific payload object.</summary>
    public object? Data { get; init; }
}
