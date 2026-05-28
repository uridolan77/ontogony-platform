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
    /// OpenTelemetry SDK metric export interval (milliseconds). Default export is 60s; local stacks use 5s.
    /// </summary>
    public const string MetricExportIntervalEnvironmentVariable = "OTEL_METRIC_EXPORT_INTERVAL";

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
                .AddOtlpExporter(ConfigureMetricOtlpExporter));

        return services;
    }

    /// <summary>Applies <see cref="MetricExportIntervalEnvironmentVariable"/> to the periodic metric reader.</summary>
    public static void ConfigureMetricOtlpExporter(
        OpenTelemetry.Exporter.OtlpExporterOptions _,
        MetricReaderOptions metricReaderOptions)
    {
        metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds =
            ResolveMetricExportIntervalMilliseconds();
    }

    /// <summary>Reads <see cref="MetricExportIntervalEnvironmentVariable"/> or returns the SDK default (60s).</summary>
    public static int ResolveMetricExportIntervalMilliseconds()
    {
        var raw = Environment.GetEnvironmentVariable(MetricExportIntervalEnvironmentVariable);
        if (!string.IsNullOrWhiteSpace(raw) && int.TryParse(raw, out var parsed) && parsed > 0)
        {
            return parsed;
        }

        return 60_000;
    }
}
