namespace Ontogony.Redaction;

/// <summary>
/// Mechanical redactor. <see cref="RedactionResult"/> never carries the pre-redaction value.
/// </summary>
public interface IRedactor
{
    RedactionResult RedactString(
        string? value,
        RedactionClassification classification = RedactionClassification.Secret);

    RedactionResult RedactField(string fieldName, object? value);

    IReadOnlyDictionary<string, object?> RedactFields(IReadOnlyDictionary<string, object?> fields);
}
