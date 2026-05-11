using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

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

    public CloudEventTraceIdPolicy TraceIdPolicy { get; set; } = CloudEventTraceIdPolicy.GenerateWhenMissing;
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
        var payloadHash = ExtractString(extensions, "payloadHash");
        var schemaVersion = ExtractString(extensions, "schemaVersion");

        var metadata = ExtractMetadata(extensions);

        var payload = cloudEvent.Data is TPayload typedPayload
            ? typedPayload
            : cloudEvent.Data is JsonElement elem
                ? JsonSerializer.Deserialize<TPayload>(elem.GetRawText())!
                : (TPayload)(object)cloudEvent.Data!;

        var protocol = protocolFromExtension ?? options.DefaultProtocolWhenMissing;

        return new OntogonyEnvelope<TPayload>
        {
            EventId = cloudEvent.Id,
            EventType = cloudEvent.Type,
            Source = cloudEvent.Source,
            OccurredAt = DateTimeOffset.Parse(cloudEvent.Time ?? DateTimeOffset.UtcNow.ToString("O")),
            TraceId = traceId,
            SpanId = spanId,
            ParentSpanId = parentSpanId,
            TenantId = tenantId,
            WorkspaceId = workspaceId,
            ProjectId = projectId,
            ActorId = actorId,
            SessionId = sessionId,
            Protocol = protocol,
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
    [JsonPropertyName("specversion")]
    public string SpecVersion { get; set; } = "1.0";

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("source")]
    public required string Source { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("datacontenttype")]
    public string? DataContentType { get; set; }

    [JsonPropertyName("time")]
    public string? Time { get; set; }

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

    public CloudEventEnvelope(string id, string source, string type)
    {
        Id = id;
        Source = source;
        Type = type;
    }
}
