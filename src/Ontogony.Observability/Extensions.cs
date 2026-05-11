using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ontogony.Observability;

public static class OntogonyObservabilityExtensions
{
    public static IServiceCollection AddOntogonyObservability(
        this IServiceCollection services,
        Action<OntogonyObservabilityOptions>? configure = null)
    {
        if (configure is not null)
        {
            services.Configure(configure);
        }
        else
        {
            services.AddOptions<OntogonyObservabilityOptions>();
        }

        services.AddSingleton<IValidateOptions<OntogonyObservabilityOptions>, OntogonyObservabilityOptionsValidator>();
        return services;
    }

    public static IApplicationBuilder UseOntogonyRequestTracing(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestTracingMiddleware>();
    }
}

public sealed class OntogonyObservabilityOptionsValidator : IValidateOptions<OntogonyObservabilityOptions>
{
    public ValidateOptionsResult Validate(string? name, OntogonyObservabilityOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ServiceName))
        {
            return ValidateOptionsResult.Fail("Observability service name is required.");
        }

        if (string.IsNullOrWhiteSpace(options.TraceHeaderName))
        {
            return ValidateOptionsResult.Fail("Trace header name is required.");
        }

        return ValidateOptionsResult.Success;
    }
}
