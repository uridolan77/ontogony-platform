using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Athanor.Infrastructure.Hashing;

/// <summary>Deterministic JSON for hashing: sorted object keys at every object level; arrays preserve order.</summary>
internal static class CanonicalJson
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = false,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string Serialize(object? payload)
    {
        if (payload is null)
        {
            return "null";
        }

        var node = JsonSerializer.SerializeToNode(payload, SerializerOptions)
            ?? throw new ArgumentException("Payload serialized to null JsonNode.", nameof(payload));

        var canonical = Canonicalize(node)
            ?? throw new InvalidOperationException("Canonical JSON produced null.");

        return canonical.ToJsonString(WriteOptions);
    }

    /// <summary>Parses JSON (object, array, or scalar) and returns canonical text (sorted object keys at each level).</summary>
    public static string CanonicalizeJsonString(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException("JSON is required.", nameof(json));
        }

        var node = JsonNode.Parse(json.Trim())
            ?? throw new ArgumentException("JSON root is null.", nameof(json));

        var canonical = Canonicalize(node)
            ?? throw new InvalidOperationException("Canonical JSON produced null.");

        return canonical.ToJsonString(WriteOptions);
    }

    private static JsonNode? Canonicalize(JsonNode? node)
    {
        switch (node)
        {
            case JsonObject o:
                var sorted = new JsonObject();
                foreach (var p in o.OrderBy(p => p.Key, StringComparer.Ordinal))
                {
                    sorted[p.Key] = Canonicalize(p.Value);
                }

                return sorted;
            case JsonArray a:
                return new JsonArray(a.Select(Canonicalize).ToArray<JsonNode?>());
            default:
                return node?.DeepClone();
        }
    }
}
