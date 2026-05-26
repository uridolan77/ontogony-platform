using System.Text.Json;

namespace Ontogony.Testing.Conformance;

/// <summary>
/// Mechanical checks for generated service compatibility / adoption manifests.
/// </summary>
public static class CompatibilityManifestConformanceAssertions
{
    /// <summary>Asserts the manifest file exists and parses as JSON object.</summary>
    public static JsonElement AssertManifestPresent(string manifestPath)
    {
        if (string.IsNullOrWhiteSpace(manifestPath))
            throw new ArgumentException("manifestPath cannot be empty.", nameof(manifestPath));

        if (!File.Exists(manifestPath))
        {
            throw new InvalidOperationException($"Compatibility manifest not found at '{manifestPath}'.");
        }

        var json = File.ReadAllText(manifestPath);
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Compatibility manifest root must be a JSON object.");
        }

        return doc.RootElement.Clone();
    }

    /// <summary>Asserts top-level JSON properties exist (dot paths supported for one level, e.g. <c>api.ontologyPrefix</c>).</summary>
    public static void AssertRequiredProperties(JsonElement root, params string[] propertyPaths)
    {
        foreach (var path in propertyPaths)
        {
            if (!TryGetProperty(root, path, out var value) || IsEmpty(value))
            {
                throw new InvalidOperationException(
                    $"Compatibility manifest missing required property '{path}'.");
            }
        }
    }

    private static bool TryGetProperty(JsonElement root, string path, out JsonElement value)
    {
        value = default;
        var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var current = root;
        foreach (var segment in segments)
        {
            if (!current.TryGetProperty(segment, out current))
                return false;
        }

        value = current;
        return true;
    }

    private static bool IsEmpty(JsonElement value) =>
        value.ValueKind switch
        {
            JsonValueKind.Null or JsonValueKind.Undefined => true,
            JsonValueKind.String => string.IsNullOrWhiteSpace(value.GetString()),
            JsonValueKind.Array => value.GetArrayLength() == 0,
            JsonValueKind.Object => !value.EnumerateObject().Any(),
            _ => false
        };
}
