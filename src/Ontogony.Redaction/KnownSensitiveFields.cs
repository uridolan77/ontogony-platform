namespace Ontogony.Redaction;

public static class KnownSensitiveFields
{
    public static readonly string[] Defaults =
    [
        "authorization",
        "api_key",
        "apikey",
        "access_token",
        "refresh_token",
        "secret",
        "client_secret",
        "password",
        "credential",
        "provider_secret",
        "provider_api_key",
        "system_prompt",
        "prompt",
        "response"
    ];

    public static IEnumerable<RedactionRule> CreateDefaultRules()
    {
        foreach (var field in Defaults)
        {
            yield return new RedactionRule(
                Name: $"default:{field}",
                MatchKind: RedactionRuleMatchKind.ContainsFieldName,
                Pattern: field,
                Classification: field.Contains("prompt", StringComparison.OrdinalIgnoreCase)
                    ? RedactionClassification.Prompt
                    : field.Contains("response", StringComparison.OrdinalIgnoreCase)
                        ? RedactionClassification.Response
                        : RedactionClassification.Secret);
        }
    }
}
