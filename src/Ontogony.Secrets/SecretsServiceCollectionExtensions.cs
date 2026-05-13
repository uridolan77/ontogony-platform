using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ontogony.Redaction;

namespace Ontogony.Secrets;

/// <summary>
/// DI registration for Ontogony secret helpers (development protector by default).
/// </summary>
public static class SecretsServiceCollectionExtensions
{
    /// <summary>Registers redaction, development protector, fingerprint service, and <see cref="SecretMasker"/>.</summary>
    public static IServiceCollection AddOntogonySecrets(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOntogonyRedaction();
        services.TryAddSingleton<ISecretProtector, DevelopmentBase64SecretProtector>();
        services.TryAddSingleton<ISecretFingerprintService, Sha256SecretFingerprintService>();
        services.TryAddSingleton<SecretMasker>();

        return services;
    }

    /// <summary>
    /// Registers <see cref="EnvironmentVariableSecretValueResolver"/> as an <see cref="ISecretValueResolver"/> for scheme <c>env</c> (machine/process environment variables).
    /// </summary>
    public static IServiceCollection AddOntogonyEnvironmentSecretValueResolver(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ISecretValueResolver, EnvironmentVariableSecretValueResolver>());

        return services;
    }
}
