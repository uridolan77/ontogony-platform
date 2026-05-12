namespace Ontogony.Redaction;

public enum RedactionRuleMatchKind
{
    ExactFieldName = 0,
    ContainsFieldName = 1,
    EndsWithFieldName = 2
}

public sealed record RedactionRule(
    string Name,
    RedactionRuleMatchKind MatchKind,
    string Pattern,
    RedactionClassification Classification = RedactionClassification.Secret)
{
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
