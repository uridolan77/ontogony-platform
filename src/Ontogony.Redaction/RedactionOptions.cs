namespace Ontogony.Redaction;

/// <summary>
/// Options for <see cref="DefaultRedactor"/> and DI registration.
/// </summary>
public sealed class RedactionOptions
{
    /// <summary>Replacement substring for fully masked values.</summary>
    public string Replacement { get; set; } = "[redacted]";

    /// <summary>Number of prefix characters to keep visible when masking.</summary>
    public int RevealPrefixCharacters { get; set; } = 0;

    /// <summary>Number of suffix characters to keep visible when masking.</summary>
    public int RevealSuffixCharacters { get; set; } = 4;

    /// <summary>When true, prepend built-in rules from <see cref="KnownSensitiveFields"/>.</summary>
    public bool IncludeDefaultRules { get; set; } = true;

    /// <summary>Additional custom rules evaluated after defaults.</summary>
    public List<RedactionRule> Rules { get; } = [];
}
