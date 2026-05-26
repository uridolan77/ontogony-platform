using Ontogony.Contracts.Events;
using Ontogony.Hashing;
using Ontogony.Primitives;

namespace Ontogony.ProtocolIngress.Adapters;

/// <summary>
/// Adapter for normalizing CloudEvents protocol events.
/// Mechanical conversion without business logic interpretation.
/// </summary>
public sealed class CloudEventsProtocolAdapter : BaseProtocolIngressAdapter, IProtocolIngressAdapter<CloudEventEnvelope>
{
    private const string ProtocolName = "cloudevents";

    /// <summary>Creates a CloudEvents ingress adapter.</summary>
    public CloudEventsProtocolAdapter(
        PayloadHasher payloadHasher,
        IIdGenerator idGenerator,
        IClock clock,
        IEnvelopeValidator envelopeValidator)
        : base(payloadHasher, idGenerator, clock, envelopeValidator)
    {
    }

    /// <inheritdoc />
    public ProtocolIngressResult Normalize(CloudEventEnvelope raw, ProtocolIngressContext context)
    {
        if (raw == null)
            return ProtocolIngressResult.Failure(nameof(raw), "CloudEventEnvelope cannot be null.");

        // Validate required CloudEvents fields
        if (string.IsNullOrWhiteSpace(raw.Id))
            return ProtocolIngressResult.Failure(nameof(CloudEventEnvelope.Id),
                "CloudEvent id is required.");

        if (string.IsNullOrWhiteSpace(raw.Type))
            return ProtocolIngressResult.Failure(nameof(CloudEventEnvelope.Type),
                "CloudEvent type is required.");

        if (string.IsNullOrWhiteSpace(raw.Source))
            return ProtocolIngressResult.Failure(nameof(CloudEventEnvelope.Source),
                "CloudEvent source is required.");

        // Extract trace ID from context or CloudEvents extensions
        var traceId = ExtractExtensionAsString(raw.Extensions, "traceId")
            ?? ExtractExtensionAsString(raw.Extensions, "traceid");

        var finalTraceId = ValidateOrGenerateTraceId(traceId, context, out var traceIdError);
        if (traceIdError != null)
            return traceIdError;

        DateTimeOffset? providedTimestamp = null;
        if (!string.IsNullOrWhiteSpace(raw.Time) && DateTimeOffset.TryParse(raw.Time, out var parsedTime))
        {
            providedTimestamp = parsedTime;
        }

        var timestamp = NormalizeTimestamp(providedTimestamp, context);

        // Serialize the entire CloudEvent as RawProtocolPayload
        var rawJson = System.Text.Json.JsonSerializer.Serialize(raw);
        var rawPayloadHash = ComputeRawPayloadHash(rawJson);
        var canonicalPayloadHash = ComputeCanonicalPayloadHash(rawJson);

        var traceParent = ExtractExtensionAsString(raw.Extensions, "traceparent");
        var traceState = ExtractExtensionAsString(raw.Extensions, "tracestate");
        var metadata = new Dictionary<string, string>(StringComparer.Ordinal);
        if (!string.IsNullOrWhiteSpace(traceParent))
            metadata["traceparent"] = traceParent;
        if (!string.IsNullOrWhiteSpace(traceState))
            metadata["tracestate"] = traceState;

        var rawPayload = new RawProtocolPayload
        {
            Protocol = ProtocolName,
            RawJson = rawJson,
            RawEventType = raw.Type,
            ParsedObject = raw,
            RawPayloadHash = rawPayloadHash,
            CanonicalPayloadHash = canonicalPayloadHash
        };

        var envelope = new OntogonyEnvelope<RawProtocolPayload>
        {
            EventId = raw.Id,
            EventType = NormalizeEnvelopeEventType(ProtocolName),
            Source = NormalizeSourceUri(ProtocolName, raw.Source),  // Normalize to absolute URI
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
            SessionId = context.Metadata?.SessionId,
            Metadata = metadata
        };

        // Validate envelope against platform contracts
        return ValidateAndReturnEnvelope(envelope);
    }

    private static string? ExtractExtensionAsString(Dictionary<string, object>? extensions, string key)
    {
        if (extensions is null || !extensions.TryGetValue(key, out var value) || value is null)
            return null;

        return value switch
        {
            string s => s,
            System.Text.Json.JsonElement { ValueKind: System.Text.Json.JsonValueKind.String } e => e.GetString(),
            System.Text.Json.JsonElement { ValueKind: System.Text.Json.JsonValueKind.Null or System.Text.Json.JsonValueKind.Undefined } => null,
            System.Text.Json.JsonElement e => e.GetRawText(),
            _ => value.ToString()
        };
    }
}
