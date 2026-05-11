namespace Ontogony.Configuration;

public static class EnvironmentGuard
{
    public static void ThrowIfProductionDefaultSecret(string environmentName, string optionName, string? value, params string[] forbiddenValues)
    {
        if (!IsProduction(environmentName)) return;
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Production requires non-empty {optionName}.");
        }

        if (forbiddenValues.Any(x => string.Equals(x, value, StringComparison.Ordinal)))
        {
            throw new InvalidOperationException($"Production cannot use default or unsafe value for {optionName}.");
        }
    }

    public static void ThrowIfProductionWildcardCors(string environmentName, IEnumerable<string> origins)
    {
        if (!IsProduction(environmentName)) return;
        if (origins.Any(x => x.Trim() == "*"))
        {
            throw new InvalidOperationException("Production CORS cannot include wildcard '*'.");
        }
    }

    public static void ThrowIfDangerousOutsideDevelopment(string environmentName, bool isDangerous, string message)
    {
        if (isDangerous && !IsDevelopmentLike(environmentName))
        {
            throw new InvalidOperationException(message);
        }
    }

    public static bool IsProduction(string environmentName) =>
        string.Equals(environmentName, "Production", StringComparison.OrdinalIgnoreCase)
        || string.Equals(environmentName, "Prod", StringComparison.OrdinalIgnoreCase);

    public static bool IsDevelopmentLike(string environmentName) =>
        string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase)
        || string.Equals(environmentName, "Test", StringComparison.OrdinalIgnoreCase)
        || string.Equals(environmentName, "Testing", StringComparison.OrdinalIgnoreCase)
        || string.Equals(environmentName, "Local", StringComparison.OrdinalIgnoreCase);
}
