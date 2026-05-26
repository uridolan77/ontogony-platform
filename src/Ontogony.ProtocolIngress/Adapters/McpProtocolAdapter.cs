using System.Text.Json;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.Primitives;

namespace Ontogony.ProtocolIngress.Adapters;

/// <summary>
/// Adapter for normalizing MCP (Model Context Protocol) events.
/// Mechanical normalization without agent orchestration logic.
/// </summary>
public sealed class McpProtocolAdapter : BaseProtocolIngressAdapter, IProtocolIngressAdapter<McpEvent>
{
    private const string ProtocolName = "mcp";

    /// <summary>Creates an MCP ingress adapter.</summary>
    public McpProtocolAdapter(
        PayloadHasher payloadHasher,
        IIdGenerator idGenerator,
        IClock clock,
        IEnvelopeValidator envelopeValidator)
        : base(payloadHasher, idGenerator, clock, envelopeValidator)
    {
    }

    /// <inheritdoc />
    public ProtocolIngressResult Normalize(McpEvent raw, ProtocolIngressContext context)
    {
        if (raw == null)
            return ProtocolIngressResult.Failure(nameof(raw), "McpEvent cannot be null.");

        if (string.IsNullOrWhiteSpace(raw.EventType))
            return ProtocolIngressResult.Failure(nameof(McpEvent.EventType),
                "MCP eventType is required.");

        if (string.IsNullOrWhiteSpace(raw.Source))
            return ProtocolIngressResult.Failure(nameof(McpEvent.Source),
                "MCP source is required.");

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
            RawEventType = raw.EventType,
            ParsedObject = raw,
            RawPayloadHash = rawPayloadHash,
            CanonicalPayloadHash = canonicalPayloadHash
        };

        var envelope = new OntogonyEnvelope<RawProtocolPayload>
        {
            EventId = eventId,
            EventType = NormalizeEnvelopeEventType(ProtocolName),
            Source = NormalizeSourceUri(ProtocolName, raw.Source),  // Normalize to absolute URI
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
            ActorId = context.Metadata?.ActorId ?? raw.ActorId,
            SessionId = context.Metadata?.SessionId ?? raw.SessionId
        };

        // Validate envelope against platform contracts
        return ValidateAndReturnEnvelope(envelope);
    }
}

/// <summary>
/// DTO representing an MCP protocol event.
/// </summary>
public sealed record McpEvent
{
    /// <summary>Optional event identifier.</summary>
    public string? EventId { get; init; }

    /// <summary>MCP event type string.</summary>
    public required string EventType { get; init; }

    /// <summary>MCP source string.</summary>
    public required string Source { get; init; }

    /// <summary>Optional event timestamp.</summary>
    public DateTimeOffset? Timestamp { get; init; }

    /// <summary>Optional trace identifier.</summary>
    public string? TraceId { get; init; }

    /// <summary>Optional span identifier.</summary>
    public string? SpanId { get; init; }

    /// <summary>Optional parent span identifier.</summary>
    public string? ParentSpanId { get; init; }

    /// <summary>Optional actor identifier.</summary>
    public string? ActorId { get; init; }

    /// <summary>Optional session identifier.</summary>
    public string? SessionId { get; init; }

    /// <summary>Optional protocol-specific payload object.</summary>
    public object? Data { get; init; }
}
