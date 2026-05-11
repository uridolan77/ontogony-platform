using Microsoft.Extensions.DependencyInjection;

namespace Ontogony.Persistence;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddOntogonyPersistencePrimitives(this IServiceCollection services)
    {
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IIdGenerator, GuidIdGenerator>();
        return services;
    }
}
