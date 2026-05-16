using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Ontogony.Observability;

/// <summary>
/// Optional OpenTelemetry OTLP export registration for ASP.NET Core hosts.
/// </summary>
public static class OntogonyOpenTelemetryExtensions
{
    /// <summary>
    /// Environment variable that enables OTLP export when non-empty.
    /// </summary>
    public const string OtlpEndpointEnvironmentVariable = "OTEL_EXPORTER_OTLP_ENDPOINT";

    /// <summary>
    /// Registers ASP.NET Core, HTTP client, and Ontogony activity/meter export when
    /// <see cref="OtlpEndpointEnvironmentVariable"/> is set. No-op when the endpoint is unset
    /// so hosts start normally without a collector.
    /// </summary>
    public static IServiceCollection AddOntogonyOpenTelemetryExport(
        this IServiceCollection services,
        string serviceName,
        string? serviceVersion = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(OtlpEndpointEnvironmentVariable)))
        {
            return services;
        }

        var resolvedServiceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME");
        if (!string.IsNullOrWhiteSpace(resolvedServiceName))
        {
            serviceName = resolvedServiceName;
        }

        var version = serviceVersion ?? "0.1.0";

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName: serviceName, serviceVersion: version))
            .WithTracing(tracing => tracing
                .AddSource(OntogonyDiagnostics.DefaultActivitySourceName)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .AddMeter(OntogonyDiagnostics.DefaultActivitySourceName)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter());

        return services;
    }
}
