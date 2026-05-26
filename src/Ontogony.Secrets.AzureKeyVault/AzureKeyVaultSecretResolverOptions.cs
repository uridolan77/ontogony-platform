namespace Ontogony.Secrets.AzureKeyVault;

/// <summary>
/// Configuration for <see cref="AzureKeyVaultSecretValueResolver"/>.
/// </summary>
public sealed class AzureKeyVaultSecretResolverOptions
{
    /// <summary>Configuration section name for Azure Key Vault options.</summary>
    public const string SectionName = "Ontogony:Secrets:AzureKeyVault";

    /// <summary>
    /// Vault URI (for example <c>https://my-vault.vault.azure.net/</c>).
    /// May also be supplied via environment variable <c>AZURE_KEY_VAULT_URI</c>.
    /// </summary>
    public string? VaultUri { get; init; }
}
