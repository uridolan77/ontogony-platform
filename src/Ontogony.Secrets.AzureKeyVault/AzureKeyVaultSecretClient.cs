using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ontogony.Secrets.AzureKeyVault;

/// <summary>
/// Production client using <see cref="SecretClient"/> and <see cref="DefaultAzureCredential"/>.
/// </summary>
public sealed class AzureKeyVaultSecretClient : IAzureKeyVaultSecretClient
{
    private readonly SecretClient _client;
    private readonly ILogger<AzureKeyVaultSecretClient> _logger;

    /// <summary>Creates a client using configured vault URI and default Azure credentials.</summary>
    public AzureKeyVaultSecretClient(IOptions<AzureKeyVaultSecretResolverOptions> options, ILogger<AzureKeyVaultSecretClient> logger)
    {
        _logger = logger;
        var vaultUri = ResolveVaultUri(options.Value);
        if (string.IsNullOrWhiteSpace(vaultUri))
        {
            throw new InvalidOperationException(
                "Azure Key Vault is not configured. Set Ontogony:Secrets:AzureKeyVault:VaultUri or AZURE_KEY_VAULT_URI.");
        }

        if (!Uri.TryCreate(vaultUri.Trim(), UriKind.Absolute, out var uri))
        {
            throw new InvalidOperationException("Azure Key Vault VaultUri must be an absolute URI.");
        }

        _client = new SecretClient(uri, new DefaultAzureCredential());
    }

    /// <inheritdoc />
    public async Task<string?> GetSecretValueAsync(string secretName, string? version, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = string.IsNullOrWhiteSpace(version)
                ? await _client.GetSecretAsync(secretName, cancellationToken: cancellationToken).ConfigureAwait(false)
                : await _client.GetSecretAsync(secretName, version, cancellationToken).ConfigureAwait(false);

            return response.Value.Value;
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            _logger.LogWarning(
                "AzureKeyVault.SecretNotFound secret_name={SecretName} status={Status}",
                secretName,
                ex.Status);
            return null;
        }
    }

    internal static string? ResolveVaultUri(AzureKeyVaultSecretResolverOptions options) =>
        string.IsNullOrWhiteSpace(options.VaultUri)
            ? Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_URI")
            : options.VaultUri;
}
