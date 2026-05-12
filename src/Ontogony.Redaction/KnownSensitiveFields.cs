namespace Ontogony.Redaction;

/// <summary>
/// Built-in lowercase field-name fragments used to seed default redaction rules.
/// </summary>
public static class KnownSensitiveFields
{
    /// <summary>Default substring tokens matched case-insensitively against field names.</summary>
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

    /// <summary>Builds default <see cref="RedactionRule"/> rows from <see cref="Defaults"/>.</summary>
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
