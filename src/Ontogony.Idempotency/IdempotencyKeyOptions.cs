namespace Ontogony.Idempotency;

/// <summary>
/// Configuration for idempotency key generation.
/// Supports service-specific namespaces and version-aware fingerprints.
/// </summary>
public sealed record IdempotencyKeyOptions
{
    /// <summary>
    /// Service namespace prefix (e.g., "ontogony", "athanor", "agentor", "conexus").
    /// Default: "ontogony"
    /// </summary>
    public string Namespace { get; init; } = "ontogony";

    /// <summary>
    /// Schema version included in key to ensure different versions hash differently.
    /// Default: "v1"
    /// </summary>
    public string Version { get; init; } = "v1";

    /// <summary>
    /// Maximum key length. Keys longer than this are truncated with "..." suffix.
    /// Default: 256
    /// </summary>
    public int MaxKeyLength { get; init; } = 256;

    /// <summary>
    /// Validates that the constructed key meets safe character requirements.
    /// Only alphanumeric, colons, dots, hyphens, and underscores allowed.
    /// </summary>
    public bool ValidateSafeCharacters { get; init; } = true;

    /// <summary>
    /// Validate options on construction.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Namespace))
            throw new ArgumentException("Namespace cannot be empty", nameof(Namespace));

        if (string.IsNullOrWhiteSpace(Version))
            throw new ArgumentException("Version cannot be empty", nameof(Version));

        if (MaxKeyLength < 32)
            throw new ArgumentException("MaxKeyLength must be at least 32", nameof(MaxKeyLength));

        // Validate safe characters in namespace and version
        if (ValidateSafeCharacters)
        {
            ValidateComponent(Namespace, nameof(Namespace));
            ValidateComponent(Version, nameof(Version));
        }
    }

    private static void ValidateComponent(string component, string paramName)
    {
        foreach (var ch in component)
        {
            if (!char.IsLetterOrDigit(ch) && ch != '.' && ch != '-' && ch != '_')
                throw new ArgumentException(
                    $"{paramName} contains unsafe character '{ch}'. Only alphanumeric, dots, hyphens, and underscores allowed.",
                    paramName);
        }
    }
}
