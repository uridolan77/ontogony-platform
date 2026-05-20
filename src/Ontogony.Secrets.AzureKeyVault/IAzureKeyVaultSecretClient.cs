namespace Ontogony.Secrets.AzureKeyVault;

/// <summary>
/// Testable port for reading a secret from a configured Azure Key Vault.
/// </summary>
public interface IAzureKeyVaultSecretClient
{
    Task<string?> GetSecretValueAsync(string secretName, string? version, CancellationToken cancellationToken = default);
}
