using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ontogony.Secrets.AzureKeyVault;

/// <summary>
/// Resolves <see cref="SecretValueReference"/> values whose scheme is <c>vault</c> or <c>akv</c>.
/// </summary>
public sealed class AzureKeyVaultSecretValueResolver : ISecretValueResolver
{
    private readonly IAzureKeyVaultSecretClient? _client;
    private readonly ILogger<AzureKeyVaultSecretValueResolver> _logger;

    public AzureKeyVaultSecretValueResolver(
        IOptions<AzureKeyVaultSecretResolverOptions> options,
        ILogger<AzureKeyVaultSecretValueResolver> logger,
        IAzureKeyVaultSecretClient? vaultClient = null)
    {
        _logger = logger;
        if (vaultClient is not null)
        {
            _client = vaultClient;
            return;
        }

        if (!string.IsNullOrWhiteSpace(AzureKeyVaultSecretClient.ResolveVaultUri(options.Value)))
        {
            _client = new AzureKeyVaultSecretClient(
                options,
                logger as ILogger<AzureKeyVaultSecretClient>
                    ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<AzureKeyVaultSecretClient>.Instance);
        }
    }

    /// <summary>
    /// Test-only constructor with an injected vault client.
    /// </summary>
    internal AzureKeyVaultSecretValueResolver(IAzureKeyVaultSecretClient client, ILogger<AzureKeyVaultSecretValueResolver> logger)
    {
        _client = client;
        _logger = logger;
    }

    /// <inheritdoc />
    public async ValueTask<SecretValueResolveResult> TryResolveAsync(
        SecretValueReference reference,
        CancellationToken cancellationToken = default)
    {
        if (!IsVaultScheme(reference.Scheme))
        {
            return new SecretValueResolveResult(false, null, "scheme_not_supported");
        }

        if (_client is null)
        {
            return new SecretValueResolveResult(false, null, "vault_not_configured");
        }

        if (!AzureKeyVaultSecretLocator.TryParse(reference.Locator, out var secretName, out var version))
        {
            return new SecretValueResolveResult(false, null, "invalid_locator");
        }

        var value = await _client.GetSecretValueAsync(secretName, version, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrEmpty(value))
        {
            _logger.LogWarning(
                "AzureKeyVault.SecretUnresolved secret_name={SecretName} has_version={HasVersion}",
                secretName,
                version is not null);
            return new SecretValueResolveResult(false, null, "secret_not_found");
        }

        return new SecretValueResolveResult(true, value, null);
    }

    private static bool IsVaultScheme(string scheme) =>
        string.Equals(scheme, "vault", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(scheme, "akv", StringComparison.OrdinalIgnoreCase);
}
