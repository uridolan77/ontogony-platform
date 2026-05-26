namespace Ontogony.Secrets.AzureKeyVault;

/// <summary>
/// Parses vault locators: <c>secret-name</c> or <c>secret-name@version</c> (version is optional).
/// </summary>
public static class AzureKeyVaultSecretLocator
{
    /// <summary>Parses a vault locator into secret name and optional version.</summary>
    public static bool TryParse(string locator, out string secretName, out string? version)
    {
        secretName = "";
        version = null;

        if (string.IsNullOrWhiteSpace(locator))
        {
            return false;
        }

        var trimmed = locator.Trim();
        if (trimmed.StartsWith("@", StringComparison.Ordinal))
        {
            return false;
        }

        var at = trimmed.LastIndexOf('@');
        if (at > 0 && at < trimmed.Length - 1)
        {
            secretName = trimmed[..at].Trim();
            version = trimmed[(at + 1)..].Trim();
            return secretName.Length > 0 && version.Length > 0;
        }

        secretName = trimmed;
        return secretName.Length > 0;
    }
}
