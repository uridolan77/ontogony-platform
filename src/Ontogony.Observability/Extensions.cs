using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ontogony.Observability;

/// <summary>
/// ASP.NET Core registration and middleware helpers for Ontogony request tracing.
/// </summary>
public static class OntogonyObservabilityExtensions
{
    /// <summary>Registers <see cref="OntogonyObservabilityOptions"/> and startup validation.</summary>
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

    /// <summary>Installs <see cref="RequestTracingMiddleware"/> in the pipeline.</summary>
    public static IApplicationBuilder UseOntogonyRequestTracing(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestTracingMiddleware>();
    }
}

/// <summary>
/// Validates required fields on <see cref="OntogonyObservabilityOptions"/>.
/// </summary>
public sealed class OntogonyObservabilityOptionsValidator : IValidateOptions<OntogonyObservabilityOptions>
{
    /// <inheritdoc />
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

        if (string.IsNullOrWhiteSpace(options.TraceParentHeaderName))
        {
            return ValidateOptionsResult.Fail("Traceparent header name is required.");
        }

        if (string.IsNullOrWhiteSpace(options.TraceStateHeaderName))
        {
            return ValidateOptionsResult.Fail("Tracestate header name is required.");
        }

        return ValidateOptionsResult.Success;
    }
}
