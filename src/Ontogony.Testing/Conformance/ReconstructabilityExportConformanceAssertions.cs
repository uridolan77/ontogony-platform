using System.Text.Json;

namespace Ontogony.Testing.Conformance;

/// <summary>
/// Mechanical checks for decision-event and evidence JSON exports (redaction + fragment linkage).
/// Product repos use these against frozen fixtures and live golden-trace artifacts.
/// </summary>
public static class ReconstructabilityExportConformanceAssertions
{
    /// <summary>Substrings that must not appear in reconstructability export JSON.</summary>
    public static IReadOnlyList<(string Pattern, string Label)> DefaultForbiddenPatterns { get; } =
    [
        ("sk-proj-", "openai_api_key_material"),
        ("sk-live-", "openai_api_key_material"),
        ("raw_prompt", "raw_prompt"),
        ("raw_completion", "raw_completion"),
        ("\"apiKey\"", "api_key_field"),
        ("Bearer ey", "bearer_jwt"),
        ("connectionString", "connection_string")
    ];

    /// <summary>
    /// Asserts export JSON does not contain known secret or raw-payload leakage patterns.
    /// </summary>
    public static void AssertExportJsonDoesNotLeakSecrets(
        string json,
        IReadOnlyList<(string Pattern, string Label)>? forbiddenPatterns = null)
    {
        if (string.IsNullOrEmpty(json))
            throw new ArgumentException("json cannot be empty.", nameof(json));

        foreach (var (pattern, label) in forbiddenPatterns ?? DefaultForbiddenPatterns)
        {
            if (json.Contains(pattern, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Reconstructability export redaction gate failed: found '{label}' (pattern '{pattern}').");
            }
        }
    }

    /// <summary>
    /// Asserts a single decision-event export has required mechanical fields and resolvable fragment refs.
    /// </summary>
    public static void AssertDecisionEventExportShape(JsonElement root)
    {
        AssertRequiredString(root, "schemaVersion");
        AssertRequiredString(root, "decisionEventId");
        AssertRequiredString(root, "decisionKind");
        AssertRequiredString(root, "severity");
        AssertRequiredString(root, "serviceOfOrigin");

        if (!root.TryGetProperty("evidenceFragments", out var fragments) ||
            fragments.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Decision export must include evidenceFragments array.");
        }

        AssertFragmentRefsResolve(root, fragments);
    }

    /// <summary>
    /// Asserts every entry in inputs.fragmentRefs appears in evidenceFragments[].fragmentId.
    /// </summary>
    public static void AssertFragmentRefsResolve(JsonElement root, JsonElement evidenceFragments)
    {
        if (!root.TryGetProperty("inputs", out var inputs) ||
            !inputs.TryGetProperty("fragmentRefs", out var refs) ||
            refs.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        var fragmentIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var fragment in evidenceFragments.EnumerateArray())
        {
            if (fragment.TryGetProperty("fragmentId", out var id) &&
                id.ValueKind == JsonValueKind.String &&
                !string.IsNullOrWhiteSpace(id.GetString()))
            {
                fragmentIds.Add(id.GetString()!);
            }
        }

        foreach (var reference in refs.EnumerateArray())
        {
            if (reference.ValueKind != JsonValueKind.String)
                continue;

            var refId = reference.GetString();
            if (string.IsNullOrWhiteSpace(refId))
                continue;

            if (!fragmentIds.Contains(refId))
            {
                throw new InvalidOperationException(
                    $"Fragment ref '{refId}' is not listed in evidenceFragments.");
            }
        }
    }

    private static void AssertRequiredString(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var value) ||
            value.ValueKind != JsonValueKind.String ||
            string.IsNullOrWhiteSpace(value.GetString()))
        {
            throw new InvalidOperationException(
                $"Decision export property '{propertyName}' must be a non-empty string.");
        }
    }
}
