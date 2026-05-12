namespace Ontogony.Redaction;

/// <summary>
/// How a <see cref="RedactionRule.Pattern"/> is compared to a field name.
/// </summary>
public enum RedactionRuleMatchKind
{
    /// <summary>Field name equals pattern (ordinal ignore case).</summary>
    ExactFieldName = 0,

    /// <summary>Field name contains pattern as a substring.</summary>
    ContainsFieldName = 1,

    /// <summary>Field name ends with pattern.</summary>
    EndsWithFieldName = 2
}

/// <summary>
/// Named rule: match a field name and apply a redaction classification when masking.
/// </summary>
/// <param name="Name">Stable rule name for diagnostics.</param>
/// <param name="MatchKind">How <paramref name="Pattern"/> is matched.</param>
/// <param name="Pattern">Pattern text (interpretation depends on <paramref name="MatchKind"/>).</param>
/// <param name="Classification">Classification recorded when this rule triggers.</param>
public sealed record RedactionRule(
    string Name,
    RedactionRuleMatchKind MatchKind,
    string Pattern,
    RedactionClassification Classification = RedactionClassification.Secret)
{
    /// <summary>Returns true when this rule applies to <paramref name="fieldName"/>.</summary>
    public bool MatchesField(string fieldName)
    {
        if (string.IsNullOrWhiteSpace(fieldName) || string.IsNullOrWhiteSpace(Pattern))
        {
            return false;
        }

        return MatchKind switch
        {
            RedactionRuleMatchKind.ExactFieldName => string.Equals(fieldName, Pattern, StringComparison.OrdinalIgnoreCase),
            RedactionRuleMatchKind.ContainsFieldName => fieldName.Contains(Pattern, StringComparison.OrdinalIgnoreCase),
            RedactionRuleMatchKind.EndsWithFieldName => fieldName.EndsWith(Pattern, StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }
}
