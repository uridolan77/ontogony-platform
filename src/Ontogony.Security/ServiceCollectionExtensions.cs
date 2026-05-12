using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

    public static IServiceCollection AddOntogonyClaimsActorContext(
        this IServiceCollection services,
        Action<ClaimsCurrentActorAccessorOptions>? configure = null)
    {
        services.AddHttpContextAccessor();
        services.AddOptions<ClaimsCurrentActorAccessorOptions>();
        if (configure is not null)
        {
            services.Configure(configure);
        }

        services.AddScoped<ClaimsCurrentActorAccessor>(sp =>
            new ClaimsCurrentActorAccessor(
                sp.GetRequiredService<IHttpContextAccessor>(),
                sp.GetRequiredService<IOptions<ClaimsCurrentActorAccessorOptions>>().Value));
        services.AddScoped<ICurrentActorAccessor>(sp => sp.GetRequiredService<ClaimsCurrentActorAccessor>());
        return services;
    }

    public static IServiceCollection AddOntogonyServiceIdentityActorContext(
        this IServiceCollection services,
        Action<ServiceIdentityOptions>? configure = null)
    {
        services.AddHttpContextAccessor();
        services.AddOptions<ServiceIdentityOptions>();
        if (configure is not null)
        {
            services.Configure(configure);
        }

        services.TryAddSingleton<Ontogony.Primitives.IClock, Ontogony.Primitives.SystemClock>();

        services.TryAddSingleton<IRequestBodyHashProvider>(sp =>
            new Sha256RequestBodyHashProvider(sp.GetRequiredService<IOptions<ServiceIdentityOptions>>()));

        services.AddScoped<ServiceIdentityCurrentActorAccessor>(sp =>
            new ServiceIdentityCurrentActorAccessor(
                sp.GetRequiredService<IHttpContextAccessor>(),
                sp.GetRequiredService<IOptions<ServiceIdentityOptions>>().Value,
                sp.GetService<IServiceSecretResolver>(),
                sp.GetService<INonceReplayStore>(),
                sp.GetService<IRequestBodyHashProvider>(),
                sp.GetRequiredService<Ontogony.Primitives.IClock>()));
        services.AddScoped<ICurrentActorAccessor>(sp => sp.GetRequiredService<ServiceIdentityCurrentActorAccessor>());
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
