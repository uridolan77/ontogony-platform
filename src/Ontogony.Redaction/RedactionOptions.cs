namespace Ontogony.Redaction;

public sealed class RedactionOptions
{
    public string Replacement { get; set; } = "[redacted]";
    public int RevealPrefixCharacters { get; set; } = 0;
    public int RevealSuffixCharacters { get; set; } = 4;
    public bool IncludeDefaultRules { get; set; } = true;
    public List<RedactionRule> Rules { get; } = [];
}
