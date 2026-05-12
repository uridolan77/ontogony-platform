namespace Ontogony.Redaction;

public interface IRedactor
{
    RedactionResult RedactString(
        string? value,
        RedactionClassification classification = RedactionClassification.Secret);

    RedactionResult RedactField(string fieldName, object? value);

    IReadOnlyDictionary<string, object?> RedactFields(IReadOnlyDictionary<string, object?> fields);
}
