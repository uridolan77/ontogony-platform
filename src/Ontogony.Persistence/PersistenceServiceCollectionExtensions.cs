using Microsoft.Extensions.DependencyInjection;

namespace Ontogony.Persistence;

#pragma warning disable CS0618 // Intentional legacy alias registrations for compatibility.
public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddOntogonyPersistencePrimitives(this IServiceCollection services)
    {
        services.AddSingleton<Ontogony.Primitives.IClock, Ontogony.Primitives.SystemClock>();
        services.AddSingleton<Ontogony.Primitives.IIdGenerator, Ontogony.Primitives.GuidIdGenerator>();

        // Legacy compatibility aliases for early adopters of Ontogony.Persistence.
        services.AddSingleton<IClock>(sp => new SystemClock());
        services.AddSingleton<IIdGenerator>(sp => new GuidIdGenerator());

        return services;
    }
}
#pragma warning restore CS0618
