using System.Text.Json;

namespace Ontogony.Errors;

/// <summary>Serialization and mapping helpers for <see cref="CrossServiceErrorEnvelope"/>.</summary>
public static class CrossServiceErrorEnvelopeExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    /// <summary>Builds a cross-service envelope from an <see cref="ApiError"/> and caller metadata.</summary>
    public static CrossServiceErrorEnvelope FromApiError(
        ApiError apiError,
        string system,
        string? stage = null,
        string? downstreamSystem = null,
        string? correlationId = null,
        bool? retryable = null) =>
        new(
            apiError.Code,
            apiError.Message,
            system,
            stage,
            downstreamSystem,
            apiError.TraceId,
            correlationId,
            retryable,
            apiError.Details);

    /// <summary>Maps a cross-service envelope to <see cref="ApiError"/>.</summary>
    public static ApiError ToApiError(this CrossServiceErrorEnvelope envelope) =>
        new(
            envelope.Code,
            envelope.Message,
            envelope.TraceId,
            envelope.Detail,
            Instance: null);

    /// <summary>Attempts to parse a JSON document into a cross-service envelope.</summary>
    public static bool TryParseJson(string json, out CrossServiceErrorEnvelope? envelope)
    {
        envelope = null;
        if (string.IsNullOrWhiteSpace(json))
        {
            return false;
        }

        try
        {
            using var document = JsonDocument.Parse(json);
            return TryParse(document.RootElement, out envelope);
        }
        catch (JsonException)
        {
            return false;
        }
    }

    /// <summary>Attempts to parse a <see cref="JsonElement"/> into a cross-service envelope.</summary>
    public static bool TryParse(JsonElement element, out CrossServiceErrorEnvelope? envelope)
    {
        envelope = null;
        if (element.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        if (!element.TryGetProperty("code", out var codeElement)
            || codeElement.ValueKind != JsonValueKind.String
            || string.IsNullOrWhiteSpace(codeElement.GetString()))
        {
            return false;
        }

        if (!element.TryGetProperty("message", out var messageElement)
            || messageElement.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        if (!element.TryGetProperty("system", out var systemElement)
            || systemElement.ValueKind != JsonValueKind.String
            || string.IsNullOrWhiteSpace(systemElement.GetString()))
        {
            return false;
        }

        envelope = new CrossServiceErrorEnvelope(
            codeElement.GetString()!,
            messageElement.GetString()!,
            systemElement.GetString()!,
            ReadOptionalString(element, "stage"),
            ReadOptionalString(element, "downstreamSystem"),
            ReadOptionalString(element, "traceId"),
            ReadOptionalString(element, "correlationId"),
            ReadOptionalBool(element, "retryable"),
            ReadOptionalDetail(element, "detail"));
        return true;
    }

    /// <summary>Serializes the envelope to JSON using web defaults.</summary>
    public static string ToJson(this CrossServiceErrorEnvelope envelope) =>
        JsonSerializer.Serialize(envelope, JsonOptions);

    private static string? ReadOptionalString(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

    private static bool? ReadOptionalBool(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var value))
        {
            return null;
        }

        return value.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            _ => null,
        };
    }

    private static object? ReadOptionalDetail(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var value)
            || value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return null;
        }

        return JsonSerializer.Deserialize<object>(value.GetRawText(), JsonOptions);
    }
}
