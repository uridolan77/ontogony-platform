namespace Ontogony.Redaction;

/// <summary>
/// Outcome of a redaction operation. Only <see cref="Value"/> is retained — never the pre-redaction secret.
/// </summary>
public sealed record RedactionResult(
    string? Value,
    bool WasRedacted,
    RedactionClassification Classification,
    string? RuleName = null);
