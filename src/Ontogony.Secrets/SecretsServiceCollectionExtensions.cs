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
}
