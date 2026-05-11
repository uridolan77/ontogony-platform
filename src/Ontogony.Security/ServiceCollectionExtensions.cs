using Microsoft.Extensions.DependencyInjection;

namespace Ontogony.Security;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOntogonyHeaderActorContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentActorAccessor, HeaderCurrentActorAccessor>();
        return services;
    }
}
