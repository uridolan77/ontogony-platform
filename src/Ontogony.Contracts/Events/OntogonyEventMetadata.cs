namespace Ontogony.Contracts.Events;

/// <summary>
/// Rich metadata for an envelope, capturing context beyond required fields.
/// </summary>
public sealed record OntogonyEventMetadata(
    string? Subject = null,
    string? DetailLevel = null,
    string? SchemaVersion = null,
    IReadOnlyDictionary<string, string>? CustomProperties = null)
{
    /// <summary>
    /// Convert to a flattened string dictionary for transmission/storage.
    /// </summary>
    public IReadOnlyDictionary<string, string> ToFlatDictionary()
    {
        var dict = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(Subject))
            dict["subject"] = Subject;

        if (!string.IsNullOrWhiteSpace(DetailLevel))
            dict["detailLevel"] = DetailLevel;

        if (!string.IsNullOrWhiteSpace(SchemaVersion))
            dict["schemaVersion"] = SchemaVersion;

        if (CustomProperties is not null)
        {
            foreach (var kvp in CustomProperties)
            {
                dict[kvp.Key] = kvp.Value;
            }
        }

        return dict;
    }

    /// <summary>
    /// Restore from a flattened string dictionary.
    /// </summary>
    public static OntogonyEventMetadata FromFlatDictionary(IReadOnlyDictionary<string, string> dict)
    {
        var subject = dict.TryGetValue("subject", out var s) ? s : null;
        var detailLevel = dict.TryGetValue("detailLevel", out var d) ? d : null;
        var schemaVersion = dict.TryGetValue("schemaVersion", out var sv) ? sv : null;

        var custom = dict
            .Where(kvp => kvp.Key is not ("subject" or "detailLevel" or "schemaVersion"))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return new OntogonyEventMetadata(
            subject,
            detailLevel,
            schemaVersion,
            custom.Count > 0 ? custom : null);
    }
}
