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

    public McpProtocolAdapter(PayloadHasher payloadHasher, IIdGenerator idGenerator)
        : base(payloadHasher, idGenerator)
    {
    }

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
            EventType = raw.EventType,
            Source = raw.Source,
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
            ActorId = context.Metadata?.ActorId ?? raw.ActorId,
            SessionId = context.Metadata?.SessionId ?? raw.SessionId
        };

        return ProtocolIngressResult.Success(envelope);
    }
}

/// <summary>
/// DTO representing an MCP protocol event.
/// </summary>
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
