using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ontogony.Errors;
using Ontogony.Observability;
using Ontogony.Security;

namespace Ontogony.Hosting;

public static class OntogonyHostingExtensions
{
    public static IServiceCollection AddOntogonyServiceDefaults(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<OntogonyServiceDefaultsOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var resolved = new OntogonyServiceDefaultsOptions();
        configuration.GetSection(OntogonyServiceDefaultsOptions.SectionName).Bind(resolved);
        configure?.Invoke(resolved);

        services.Configure<OntogonyServiceDefaultsOptions>(options => CopyOptions(options, resolved));
        services.AddHealthChecks();
        services.Configure<JsonOptions>(options => options.SerializerOptions.WriteIndented = false);

        if (resolved.AddObservability)
        {
            services.AddOntogonyObservability(options =>
            {
                if (!string.IsNullOrWhiteSpace(resolved.ServiceName))
                {
                    options.ServiceName = resolved.ServiceName;
                }

                if (!string.IsNullOrWhiteSpace(resolved.ServiceVersion))
                {
                    options.ServiceVersion = resolved.ServiceVersion;
                }
            });
        }

        if (resolved.AddErrors)
        {
            services.AddOntogonyErrors();
        }

        return services;
    }

    public static IApplicationBuilder UseOntogonyServiceDefaults(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var options = app.ApplicationServices.GetRequiredService<IOptions<OntogonyServiceDefaultsOptions>>().Value;

        if (options.UseRequestTracing)
        {
            app.UseOntogonyRequestTracing();
        }

        if (options.UseExceptionHandling)
        {
            app.UseOntogonyExceptionHandling();
        }

        if (options.UseServiceIdentityBodyHashPreload)
        {
            app.UseOntogonyServiceIdentityBodyHashPreload();
        }

        return app;
    }

    public static IEndpointRouteBuilder MapOntogonyHealthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var options = endpoints.ServiceProvider.GetRequiredService<IOptions<OntogonyServiceDefaultsOptions>>().Value;
        if (!options.MapHealthEndpoints)
        {
            return endpoints;
        }

        endpoints.MapHealthChecks(options.HealthPath);
        endpoints.MapHealthChecks(
            options.ReadinessPath,
            new HealthCheckOptions { Predicate = static registration => registration.Tags.Contains("ready") });

        return endpoints;
    }

    private static void CopyOptions(OntogonyServiceDefaultsOptions destination, OntogonyServiceDefaultsOptions source)
    {
        destination.ServiceName = source.ServiceName;
        destination.ServiceVersion = source.ServiceVersion;
        destination.AddObservability = source.AddObservability;
        destination.AddErrors = source.AddErrors;
        destination.UseRequestTracing = source.UseRequestTracing;
        destination.UseExceptionHandling = source.UseExceptionHandling;
        destination.UseServiceIdentityBodyHashPreload = source.UseServiceIdentityBodyHashPreload;
        destination.MapHealthEndpoints = source.MapHealthEndpoints;
        destination.HealthPath = source.HealthPath;
        destination.ReadinessPath = source.ReadinessPath;
    }
}
