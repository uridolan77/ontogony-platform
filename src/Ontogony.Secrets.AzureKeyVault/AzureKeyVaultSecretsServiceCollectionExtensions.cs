using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ontogony.Secrets.AzureKeyVault;

/// <summary>
/// DI registration for Azure Key Vault secret resolution.
/// </summary>
public static class AzureKeyVaultSecretsServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="AzureKeyVaultSecretValueResolver"/> when a vault URI is configured.
    /// </summary>
    public static IServiceCollection AddOntogonyAzureKeyVaultSecretValueResolver(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<AzureKeyVaultSecretResolverOptions>(
            configuration.GetSection(AzureKeyVaultSecretResolverOptions.SectionName));

        var vaultUri = configuration[AzureKeyVaultSecretResolverOptions.SectionName + ":VaultUri"]
            ?? configuration["AZURE_KEY_VAULT_URI"];

        if (string.IsNullOrWhiteSpace(vaultUri))
        {
            return services;
        }

        services.TryAddSingleton<IAzureKeyVaultSecretClient, AzureKeyVaultSecretClient>();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ISecretValueResolver, AzureKeyVaultSecretValueResolver>());

        return services;
    }
}
