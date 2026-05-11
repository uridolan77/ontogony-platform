using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ontogony.Security;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOntogonyHeaderActorContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentActorAccessor, HeaderCurrentActorAccessor>();
        return services;
    }

    public static IServiceCollection AddOntogonyAuthenticationGuards(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = OntogonyAuthenticationOptions.SectionName)
    {
        services
            .AddOptions<OntogonyAuthenticationOptions>()
            .Bind(configuration.GetSection(sectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<OntogonyAuthenticationOptions>, OntogonyAuthenticationOptionsValidator>();
        return services;
    }
}
