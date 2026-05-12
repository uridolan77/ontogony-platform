namespace Ontogony.Redaction;

public sealed record RedactionResult(
    string? Original,
    string? Value,
    bool WasRedacted,
    RedactionClassification Classification,
    string? RuleName = null);
