using Microsoft.Extensions.Options;

namespace Ontogony.Redaction;

/// <summary>
/// Default <see cref="IRedactor"/> using configured <see cref="RedactionRule"/> entries and optional built-in rules.
/// </summary>
public sealed class DefaultRedactor : IRedactor
{
    private readonly RedactionOptions _options;
    private readonly IReadOnlyList<RedactionRule> _rules;

    /// <summary>Creates a redactor bound to <paramref name="options"/>.</summary>
    /// <param name="options">Options snapshot (uses defaults when null).</param>
    public DefaultRedactor(IOptions<RedactionOptions> options)
    {
        _options = options?.Value ?? new RedactionOptions();
        _rules = BuildRules(_options);
    }

    /// <inheritdoc />
    public RedactionResult RedactString(
        string? value,
        RedactionClassification classification = RedactionClassification.Secret)
    {
        if (value is null)
        {
            return new RedactionResult(null, WasRedacted: false, classification);
        }

        return new RedactionResult(Mask(value), WasRedacted: true, classification);
    }

    /// <inheritdoc />
    public RedactionResult RedactField(string fieldName, object? value)
    {
        var text = value?.ToString();
        var rule = _rules.FirstOrDefault(r => r.MatchesField(fieldName));
        if (rule is null)
        {
            return new RedactionResult(text, WasRedacted: false, RedactionClassification.None);
        }

        return new RedactionResult(Mask(text ?? string.Empty), WasRedacted: true, rule.Classification, rule.Name);
    }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> RedactFields(IReadOnlyDictionary<string, object?> fields)
    {
        ArgumentNullException.ThrowIfNull(fields);

        var output = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var kv in fields)
        {
            var result = RedactField(kv.Key, kv.Value);
            output[kv.Key] = result.WasRedacted ? result.Value : kv.Value;
        }

        return output;
    }

    private string Mask(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return _options.Replacement;
        }

        var prefix = Math.Clamp(_options.RevealPrefixCharacters, 0, value.Length);
        var suffix = Math.Clamp(_options.RevealSuffixCharacters, 0, value.Length - prefix);
        if (prefix == 0 && suffix == 0)
        {
            return _options.Replacement;
        }

        return value[..prefix] + _options.Replacement + value[(value.Length - suffix)..];
    }

    private static IReadOnlyList<RedactionRule> BuildRules(RedactionOptions options)
    {
        var rules = new List<RedactionRule>();
        if (options.IncludeDefaultRules)
        {
            rules.AddRange(KnownSensitiveFields.CreateDefaultRules());
        }

        rules.AddRange(options.Rules);
        return rules;
    }
}
