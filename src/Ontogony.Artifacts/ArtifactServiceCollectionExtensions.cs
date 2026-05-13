using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ontogony.Hashing;
using Ontogony.Primitives;
using Ontogony.Runtime;

namespace Ontogony.Artifacts;

/// <summary>
/// DI registration for Ontogony artifact stores.
/// </summary>
public static class ArtifactServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="InMemoryArtifactStore"/> as the singleton <see cref="IArtifactStore"/>
    /// for tests, examples, and single-process hosts.
    /// When the host environment is not <see cref="Microsoft.Extensions.Hosting.Environments.Development"/>, registers a startup warning that this store is not durable for production multi-instance use.
    /// </summary>
    public static IServiceCollection AddOntogonyInMemoryArtifactStore(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IClock, SystemClock>();
        services.TryAddSingleton<IIdGenerator, GuidIdGenerator>();
        services.TryAddSingleton<IContentHashService, Sha256ContentHashService>();

        services.AddSingleton<InMemoryArtifactStore>(sp => new InMemoryArtifactStore(
            sp.GetRequiredService<IContentHashService>(),
            sp.GetRequiredService<IClock>(),
            sp.GetRequiredService<IIdGenerator>()));
        services.AddSingleton<IArtifactStore>(sp => sp.GetRequiredService<InMemoryArtifactStore>());

        services.AddOntogonyInMemoryNonDurableStartupWarning(
            "Ontogony.Artifacts: InMemoryArtifactStore",
            "For production or multi-instance hosts, register a durable IArtifactStore (for example blob/object storage or your organization's standard backing store) instead of the in-memory reference store.");

        return services;
    }
}
