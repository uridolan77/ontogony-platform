using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ontogony.Redaction;

namespace Ontogony.Secrets;

public static class SecretsServiceCollectionExtensions
{
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
