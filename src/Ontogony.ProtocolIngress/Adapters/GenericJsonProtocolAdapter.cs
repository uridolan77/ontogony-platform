using System.Text.Json;
using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.Primitives;

namespace Ontogony.ProtocolIngress.Adapters;

/// <summary>
/// Adapter for normalizing generic JSON protocol events.
/// Accepts raw JSON and attempts to extract common fields (eventType, source, traceId, etc).
/// </summary>
public sealed class GenericJsonProtocolAdapter : BaseProtocolIngressAdapter, IProtocolIngressAdapter<string>
{
    private const string ProtocolName = "generic-json";

    public GenericJsonProtocolAdapter(
        PayloadHasher payloadHasher,
        IIdGenerator idGenerator,
        IClock clock,
        IEnvelopeValidator envelopeValidator)
        : base(payloadHasher, idGenerator, clock, envelopeValidator)
    {
    }

    public ProtocolIngressResult Normalize(string rawJson, ProtocolIngressContext context)
    {
        if (string.IsNullOrWhiteSpace(rawJson))
            return ProtocolIngressResult.Failure(nameof(rawJson), "Raw JSON payload cannot be empty.");

        JsonElement root;
        try
        {
            using (var doc = JsonDocument.Parse(rawJson))
            {
                root = doc.RootElement.Clone();  // Clone to preserve after doc is disposed
            }
        }
        catch (JsonException ex)
        {
            return ProtocolIngressResult.Failure(nameof(rawJson),
                $"Invalid JSON payload: {ex.Message}");
        }

        // Extract common fields
        var eventType = ExtractString(root, "eventType", "type", "event_type");
        var source = ExtractString(root, "source");
        var traceId = ExtractString(root, "traceId", "trace_id", "correlationId");
        var eventId = ExtractString(root, "eventId", "id", "event_id");

        // Validate required fields
        if (string.IsNullOrWhiteSpace(eventType))
            return ProtocolIngressResult.Failure(nameof(eventType),
                "Event type is required (field: eventType, type, or event_type).");

        if (string.IsNullOrWhiteSpace(source))
            return ProtocolIngressResult.Failure(nameof(source),
                "Source is required.");

        // Validate or generate trace ID
        var finalTraceId = ValidateOrGenerateTraceId(traceId, context, out var traceIdError);
        if (traceIdError != null)
            return traceIdError;

        // Generate event ID if missing
        if (string.IsNullOrWhiteSpace(eventId))
            eventId = IdGenerator.NewGuid().ToString();

        var timestamp = NormalizeTimestamp(null, context);
        var canonicalPayloadHash = ComputeCanonicalPayloadHash(rawJson);

        var rawPayload = new RawProtocolPayload
        {
            Protocol = ProtocolName,
            RawJson = rawJson,
            RawEventType = eventType,
            ParsedObject = root,  // Store the cloned JsonElement
            CanonicalPayloadHash = canonicalPayloadHash
        };

        var envelope = new OntogonyEnvelope<RawProtocolPayload>
        {
            EventId = eventId,
            EventType = NormalizeEnvelopeEventType(ProtocolName),
            Source = NormalizeSourceUri(ProtocolName, source),  // Normalize to absolute URI
            OccurredAt = timestamp,
            TraceId = finalTraceId!,
            SpanId = context.SpanId,
            ParentSpanId = context.ParentSpanId,
            Protocol = ProtocolName,
            Payload = rawPayload,
            PayloadHash = canonicalPayloadHash,
            TenantId = context.Metadata?.TenantId,
            WorkspaceId = context.Metadata?.WorkspaceId,
            ProjectId = context.Metadata?.ProjectId,
            ActorId = context.Metadata?.ActorId,
            SessionId = context.Metadata?.SessionId
        };

        // Validate envelope against platform contracts
        return ValidateAndReturnEnvelope(envelope);
    }

    private static string? ExtractString(JsonElement root, params string[] fieldNames)
    {
        foreach (var fieldName in fieldNames)
        {
            if (root.TryGetProperty(fieldName, out var element) && element.ValueKind == JsonValueKind.String)
            {
                return element.GetString();
            }
        }

        return null;
    }
}
