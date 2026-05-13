namespace Ontogony.Secrets;

/// <summary>
/// Parses opaque <c>scheme:locator</c> strings into <see cref="SecretValueReference"/> without validating scheme-specific rules.
/// </summary>
public static class SecretValueReferenceParser
{
    /// <summary>
    /// Attempts to parse <paramref name="value"/> as <c>scheme:locator</c> (first colon separates scheme from the rest).
    /// </summary>
    /// <param name="value">The raw reference string (for example <c>env:MY_API_KEY</c>).</param>
    /// <param name="reference">The parsed reference when this method returns <c>true</c>; otherwise <c>default</c>.</param>
    /// <param name="error">A stable, non-secret diagnostic when parsing fails; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if both scheme and locator are non-blank after trimming; otherwise <c>false</c>.</returns>
    /// <remarks>
    /// Does not trim or reinterpret inner locator content beyond leading and trailing whitespace on scheme and locator segments.
    /// Does not validate whether a resolver supports the scheme.
    /// </remarks>
    public static bool TryParse(string? value, out SecretValueReference reference, out string? error)
    {
        reference = default;

        if (string.IsNullOrWhiteSpace(value))
        {
            error = "Secret value reference is missing.";
            return false;
        }

        var trimmed = value.Trim();
        var separator = trimmed.IndexOf(':');
        if (separator < 0)
        {
            error = "Secret value reference must use scheme:locator form.";
            return false;
        }

        var scheme = trimmed[..separator].Trim();
        var locator = trimmed[(separator + 1)..].Trim();

        if (scheme.Length == 0)
        {
            error = "Secret value reference scheme must not be blank.";
            return false;
        }

        if (locator.Length == 0)
        {
            error = "Secret value reference locator must not be blank.";
            return false;
        }

        error = null;
        reference = new SecretValueReference(scheme, locator);
        return true;
    }
}
