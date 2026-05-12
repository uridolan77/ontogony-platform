namespace Ontogony.Redaction;

/// <summary>
/// Mechanical redactor. <see cref="RedactionResult"/> never carries the pre-redaction value.
/// </summary>
public interface IRedactor
{
    /// <summary>Redacts a free-form string using the given classification (no field-name rules).</summary>
    /// <param name="value">Raw value; may be null.</param>
    /// <param name="classification">Classification applied when redacted.</param>
    RedactionResult RedactString(
        string? value,
        RedactionClassification classification = RedactionClassification.Secret);

    /// <summary>Redacts a structured field by name using configured rules.</summary>
    /// <param name="fieldName">Field key as it appears in logs or metadata.</param>
    /// <param name="value">Field value.</param>
    RedactionResult RedactField(string fieldName, object? value);

    /// <summary>Redacts every entry in a field map in place (returns a new dictionary).</summary>
    /// <param name="fields">Name/value pairs (typically log additional fields).</param>
    IReadOnlyDictionary<string, object?> RedactFields(IReadOnlyDictionary<string, object?> fields);
}
