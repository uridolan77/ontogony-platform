namespace Ontogony.Configuration;

/// <summary>Startup guards for environment-specific configuration values.</summary>
public static class EnvironmentGuard
{
    /// <summary>Throws when production uses a missing or forbidden secret value.</summary>
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

    /// <summary>Throws when production CORS origins include a wildcard.</summary>
    public static void ThrowIfProductionWildcardCors(string environmentName, IEnumerable<string> origins)
    {
        if (!IsProduction(environmentName)) return;
        if (origins.Any(x => x.Trim() == "*"))
        {
            throw new InvalidOperationException("Production CORS cannot include wildcard '*'.");
        }
    }

    /// <summary>Throws when a dangerous option is enabled outside development-like environments.</summary>
    public static void ThrowIfDangerousOutsideDevelopment(string environmentName, bool isDangerous, string message)
    {
        if (isDangerous && !IsDevelopmentLike(environmentName))
        {
            throw new InvalidOperationException(message);
        }
    }

    /// <summary>Returns whether the environment name represents production.</summary>
    public static bool IsProduction(string environmentName) =>
        string.Equals(environmentName, "Production", StringComparison.OrdinalIgnoreCase)
        || string.Equals(environmentName, "Prod", StringComparison.OrdinalIgnoreCase);

    /// <summary>Returns whether the environment name represents development-like hosting.</summary>
    public static bool IsDevelopmentLike(string environmentName) =>
        string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase)
        || string.Equals(environmentName, "Test", StringComparison.OrdinalIgnoreCase)
        || string.Equals(environmentName, "Testing", StringComparison.OrdinalIgnoreCase)
        || string.Equals(environmentName, "Local", StringComparison.OrdinalIgnoreCase);
}
