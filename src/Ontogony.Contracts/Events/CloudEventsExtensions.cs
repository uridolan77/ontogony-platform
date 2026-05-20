using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ontogony.Primitives;

namespace Ontogony.Contracts.Events;

/// <summary>
/// Controls trace identifier handling when converting from CloudEvents to <see cref="OntogonyEnvelope{TPayload}"/>.
/// </summary>
public enum CloudEventTraceIdPolicy
{
    /// <summary>When <c>traceId</c> extension is missing, generate a random 32-hex trace id.</summary>
    GenerateWhenMissing = 0,

    /// <summary>When <c>traceId</c> extension is missing or blank, throw <see cref="InvalidOperationException"/>.</summary>
    RejectWhenMissing = 1
}

/// <summary>
/// Options for CloudEvents conversion to <see cref="OntogonyEnvelope{TPayload}"/> via <c>ToOntogonyEnvelope</c> overloads that accept options.
/// </summary>
public sealed class CloudEventConversionOptions
{
    /// <summary>Used when the CloudEvent has no <c>protocol</c> extension.</summary>
    public string DefaultProtocolWhenMissing { get; set; } = ProtocolNames.CloudEvents;

    /// <summary>Controls trace id synthesis or rejection when the CloudEvent omits <c>traceId</c>.</summary>
    public CloudEventTraceIdPolicy TraceIdPolicy { get; set; } = CloudEventTraceIdPolicy.GenerateWhenMissing;

    /// <summary>
    /// When false (default), <see cref="CloudEventEnvelope.Data"/> must not be null when converting to <see cref="OntogonyEnvelope{TPayload}"/>.
    /// When true, null data maps to <c>default(TPayload)</c> for reference types and nullable value types; non-nullable value types still throw.
    /// </summary>
    public bool AllowNullCloudEventData { get; set; }

    /// <summary>
    /// When the CloudEvent omits <c>time</c>, this clock supplies <see cref="OntogonyEnvelope{TPayload}.OccurredAt"/>.
    /// When null, a <see cref="SystemClock"/> is used (wall-clock UTC).
    /// </summary>
    public IClock? Clock { get; set; }
}

/// <summary>
/// Extensions to convert between OntogonyEnvelope and CloudEvents format.
/// </summary>
public static class CloudEventsExtensions
{
    /// <summary>
    /// Convert an OntogonyEnvelope to CloudEvents format (as JSON).
    /// </summary>
    public static string ToCloudEventJson<TPayload>(this OntogonyEnvelope<TPayload> envelope, JsonSerializerOptions? options = null)
    {
        var cloudEvent = envelope.ToCloudEvent();
        return JsonSerializer.Serialize(cloudEvent, options);
    }

    /// <summary>
    /// Convert an OntogonyEnvelope to a CloudEvents-compatible object.
    /// Maps:
    /// - eventId → id
    /// - eventType → type
    /// - source → source
    /// - occurredAt → time
    /// - payload → data
    /// - trace/tenant/project/session/actor → extensions
    /// - protocol → extension <c>protocol</c> (preserved for round-trip)
    /// - schemaVersion → extension <c>schemaVersion</c>
    /// </summary>
    public static CloudEventEnvelope ToCloudEvent<TPayload>(this OntogonyEnvelope<TPayload> envelope)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        var extensions = new Dictionary<string, object>();

        if (!string.IsNullOrWhiteSpace(envelope.TraceId))
            extensions["traceId"] = envelope.TraceId;

        if (!string.IsNullOrWhiteSpace(envelope.SpanId))
            extensions["spanId"] = envelope.SpanId;

        if (!string.IsNullOrWhiteSpace(envelope.ParentSpanId))
            extensions["parentSpanId"] = envelope.ParentSpanId;

        if (!string.IsNullOrWhiteSpace(envelope.TenantId))
            extensions["tenantId"] = envelope.TenantId;

        if (!string.IsNullOrWhiteSpace(envelope.WorkspaceId))
            extensions["workspaceId"] = envelope.WorkspaceId;

        if (!string.IsNullOrWhiteSpace(envelope.ProjectId))
            extensions["projectId"] = envelope.ProjectId;

        if (!string.IsNullOrWhiteSpace(envelope.ActorId))
            extensions["actorId"] = envelope.ActorId;

        if (!string.IsNullOrWhiteSpace(envelope.SessionId))
            extensions["sessionId"] = envelope.SessionId;

        if (!string.IsNullOrWhiteSpace(envelope.Protocol))
            extensions["protocol"] = envelope.Protocol;

        if (!string.IsNullOrWhiteSpace(envelope.ProtocolId))
            extensions["protocolId"] = envelope.ProtocolId;

        if (!string.IsNullOrWhiteSpace(envelope.AuthorityMode))
            extensions["authorityMode"] = envelope.AuthorityMode;

        if (!string.IsNullOrWhiteSpace(envelope.SideEffectLevel))
            extensions["sideEffectLevel"] = envelope.SideEffectLevel;

        if (!string.IsNullOrWhiteSpace(envelope.PayloadHash))
            extensions["payloadHash"] = envelope.PayloadHash;

        if (!string.IsNullOrWhiteSpace(envelope.SchemaVersion))
            extensions["schemaVersion"] = envelope.SchemaVersion;

        if (envelope.Metadata is not null && envelope.Metadata.Count > 0)
        {
            extensions["metadata"] = envelope.Metadata;
        }

        return new CloudEventEnvelope
        {
            SpecVersion = "1.0",
            Id = envelope.EventId,
            Source = envelope.Source,
            Type = envelope.EventType,
            DataContentType = "application/json",
            Time = envelope.OccurredAt.ToString("O"),
            Data = envelope.Payload,
            Extensions = extensions.Count > 0 ? extensions : null
        };
    }

    /// <summary>
    /// Restore an OntogonyEnvelope from CloudEvents format using default conversion options.
    /// </summary>
    public static OntogonyEnvelope<TPayload> ToOntogonyEnvelope<TPayload>(this CloudEventEnvelope cloudEvent) =>
        cloudEvent.ToOntogonyEnvelope<TPayload>((CloudEventConversionOptions?)null);

    /// <summary>
    /// Restore an OntogonyEnvelope from CloudEvents format, supplying the protocol when the CloudEvent omits the extension.
    /// </summary>
    public static OntogonyEnvelope<TPayload> ToOntogonyEnvelope<TPayload>(
        this CloudEventEnvelope cloudEvent,
        string defaultProtocolWhenMissing) =>
        cloudEvent.ToOntogonyEnvelope<TPayload>(new CloudEventConversionOptions
        {
            DefaultProtocolWhenMissing = defaultProtocolWhenMissing
        });

    /// <summary>
    /// Restore an OntogonyEnvelope from CloudEvents format.
    /// Requires CloudEvents <c>specversion</c> 1.0 (case-insensitive).
    /// </summary>
    public static OntogonyEnvelope<TPayload> ToOntogonyEnvelope<TPayload>(
        this CloudEventEnvelope cloudEvent,
        CloudEventConversionOptions? options)
    {
        ArgumentNullException.ThrowIfNull(cloudEvent);
        options ??= new CloudEventConversionOptions();

        EnsureSupportedCloudEventsSpecVersion(cloudEvent);

        var extensions = cloudEvent.Extensions ?? new Dictionary<string, object>();

        var traceFromExt = ExtractString(extensions, "traceId");
        string traceId;
        if (string.IsNullOrWhiteSpace(traceFromExt))
        {
            if (options.TraceIdPolicy == CloudEventTraceIdPolicy.RejectWhenMissing)
            {
                throw new InvalidOperationException(
                    "CloudEvent is missing required extension 'traceId' and TraceIdPolicy is RejectWhenMissing.");
            }

            traceId = ActivityTraceId.CreateRandom().ToHexString();
        }
        else
        {
            traceId = traceFromExt;
        }

        var spanId = ExtractString(extensions, "spanId");
        var parentSpanId = ExtractString(extensions, "parentSpanId");
        var tenantId = ExtractString(extensions, "tenantId");
        var workspaceId = ExtractString(extensions, "workspaceId");
        var projectId = ExtractString(extensions, "projectId");
        var actorId = ExtractString(extensions, "actorId");
        var sessionId = ExtractString(extensions, "sessionId");
        var protocolFromExtension = ExtractString(extensions, "protocol");
        var protocolId = ExtractString(extensions, "protocolId");
        var authorityMode = ExtractString(extensions, "authorityMode");
        var sideEffectLevel = ExtractString(extensions, "sideEffectLevel");
        var payloadHash = ExtractString(extensions, "payloadHash");
        var schemaVersion = ExtractString(extensions, "schemaVersion");

        var metadata = ExtractMetadata(extensions);

        TPayload payload;
        if (cloudEvent.Data is null)
        {
            if (!options.AllowNullCloudEventData)
            {
                throw new InvalidOperationException(
                    "CloudEvent data is required for OntogonyEnvelope payload.");
            }

            if (typeof(TPayload).IsValueType && Nullable.GetUnderlyingType(typeof(TPayload)) is null)
            {
                throw new InvalidOperationException(
                    "CloudEvent data is null but OntogonyEnvelope payload type is a non-nullable value type.");
            }

            payload = default!;
        }
        else if (cloudEvent.Data is TPayload typedPayload)
        {
            payload = typedPayload;
        }
        else if (cloudEvent.Data is JsonElement elem)
        {
            payload = JsonSerializer.Deserialize<TPayload>(elem.GetRawText())!;
        }
        else
        {
            payload = (TPayload)cloudEvent.Data;
        }

        var protocol = protocolFromExtension ?? options.DefaultProtocolWhenMissing;
        var clock = options.Clock ?? new SystemClock();
        var occurredAt = string.IsNullOrWhiteSpace(cloudEvent.Time)
            ? clock.UtcNow
            : DateTimeOffset.Parse(cloudEvent.Time);

        return new OntogonyEnvelope<TPayload>
        {
            EventId = cloudEvent.Id,
            EventType = cloudEvent.Type,
            Source = cloudEvent.Source,
            OccurredAt = occurredAt,
            TraceId = traceId,
            SpanId = spanId,
            ParentSpanId = parentSpanId,
            TenantId = tenantId,
            WorkspaceId = workspaceId,
            ProjectId = projectId,
            ActorId = actorId,
            SessionId = sessionId,
            Protocol = protocol,
            ProtocolId = protocolId,
            AuthorityMode = authorityMode,
            SideEffectLevel = sideEffectLevel,
            SchemaVersion = string.IsNullOrWhiteSpace(schemaVersion) ? "1.0" : schemaVersion,
            Payload = payload,
            PayloadHash = payloadHash,
            Metadata = metadata
        };
    }

    private static void EnsureSupportedCloudEventsSpecVersion(CloudEventEnvelope cloudEvent)
    {
        if (string.IsNullOrWhiteSpace(cloudEvent.SpecVersion) ||
            !string.Equals(cloudEvent.SpecVersion.Trim(), "1.0", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Only CloudEvents specversion 1.0 is supported (received '{cloudEvent.SpecVersion ?? "(null)"}').");
        }
    }

    private static Dictionary<string, string> ExtractMetadata(IDictionary<string, object> extensions)
    {
        if (!extensions.TryGetValue("metadata", out var metaObj) || metaObj is null)
        {
            return new Dictionary<string, string>();
        }

        if (metaObj is Dictionary<string, object> metaDict)
        {
            var metadata = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var kvp in metaDict)
            {
                var value = ExtractObjectAsString(kvp.Value);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    metadata[kvp.Key] = value;
                }
            }

            return metadata;
        }

        if (metaObj is JsonElement metaElement && metaElement.ValueKind == JsonValueKind.Object)
        {
            var metadata = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var property in metaElement.EnumerateObject())
            {
                var value = property.Value.ValueKind == JsonValueKind.String
                    ? property.Value.GetString()
                    : property.Value.GetRawText();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    metadata[property.Name] = value;
                }
            }

            return metadata;
        }

        return new Dictionary<string, string>();
    }

    private static string? ExtractString(IDictionary<string, object> dict, string key)
    {
        if (!dict.TryGetValue(key, out var value) || value is null)
        {
            return null;
        }

        return ExtractObjectAsString(value);
    }

    private static string? ExtractObjectAsString(object value)
    {
        return value switch
        {
            string str => str,
            JsonElement element when element.ValueKind == JsonValueKind.String => element.GetString(),
            JsonElement element when element.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined => null,
            JsonElement element => element.GetRawText(),
            _ => value.ToString()
        };
    }
}

/// <summary>
/// CloudEvents 1.0 wire shape used with <see cref="CloudEventsExtensions"/>. Unknown attributes deserialize into <see cref="Extensions"/>.
/// Only <see cref="SpecVersion"/> 1.0 is accepted when converting to <see cref="OntogonyEnvelope{TPayload}"/>.
/// </summary>
[SuppressMessage("Usage", "CA1711:Identifiers should not have incorrect suffix")]
public sealed class CloudEventEnvelope
{
    /// <summary>CloudEvents spec version (must be 1.0 for conversion helpers).</summary>
    [JsonPropertyName("specversion")]
    public string SpecVersion { get; set; } = "1.0";

    /// <summary>CloudEvents id (maps to <see cref="OntogonyEnvelope{TPayload}.EventId"/>).</summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>CloudEvents source URI.</summary>
    [JsonPropertyName("source")]
    public required string Source { get; set; }

    /// <summary>CloudEvents type (maps to <see cref="OntogonyEnvelope{TPayload}.EventType"/>).</summary>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    /// <summary>Optional content type of <see cref="Data"/>.</summary>
    [JsonPropertyName("datacontenttype")]
    public string? DataContentType { get; set; }

    /// <summary>ISO 8601 occurrence time when present.</summary>
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    /// <summary>Event payload (object or JSON element depending on serializer).</summary>
    [JsonPropertyName("data")]
    public object? Data { get; set; }

    /// <summary>
    /// Optional extension attributes (serialized as top-level properties).
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, object>? Extensions { get; set; }

    /// <summary>
    /// Initialize with required fields.
    /// </summary>
    public CloudEventEnvelope()
    {
    }

    /// <summary>Creates an envelope with required CloudEvents identity fields.</summary>
    /// <param name="id">CloudEvents id.</param>
    /// <param name="source">CloudEvents source.</param>
    /// <param name="type">CloudEvents type.</param>
    public CloudEventEnvelope(string id, string source, string type)
    {
        Id = id;
        Source = source;
        Type = type;
    }
}
