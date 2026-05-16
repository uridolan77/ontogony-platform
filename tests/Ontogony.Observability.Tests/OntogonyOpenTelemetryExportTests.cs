using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using Xunit;

namespace Ontogony.Observability.Tests;

public sealed class OntogonyOpenTelemetryExportTests
{
    [Fact]
    public void AddOntogonyOpenTelemetryExport_WithoutEndpoint_IsNoOp()
    {
        var previous = Environment.GetEnvironmentVariable(OntogonyOpenTelemetryExtensions.OtlpEndpointEnvironmentVariable);
        try
        {
            Environment.SetEnvironmentVariable(OntogonyOpenTelemetryExtensions.OtlpEndpointEnvironmentVariable, null);

            var services = new ServiceCollection();
            services.AddOntogonyObservability(options => options.ServiceName = "test-service");

            var result = services.AddOntogonyOpenTelemetryExport("test-service");

            Assert.Same(services, result);
            using var provider = services.BuildServiceProvider();
            Assert.Null(provider.GetService<TracerProvider>());
        }
        finally
        {
            Environment.SetEnvironmentVariable(
                OntogonyOpenTelemetryExtensions.OtlpEndpointEnvironmentVariable,
                previous);
        }
    }

    [Fact]
    public void AddOntogonyOpenTelemetryExport_WithEndpoint_RegistersTracerProvider()
    {
        var previousEndpoint = Environment.GetEnvironmentVariable(OntogonyOpenTelemetryExtensions.OtlpEndpointEnvironmentVariable);
        var previousService = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME");
        try
        {
            Environment.SetEnvironmentVariable(
                OntogonyOpenTelemetryExtensions.OtlpEndpointEnvironmentVariable,
                "http://localhost:4317");
            Environment.SetEnvironmentVariable("OTEL_SERVICE_NAME", "otel-test-service");

            var services = new ServiceCollection();
            services.AddOntogonyObservability(options => options.ServiceName = "ignored-when-otel-service-name-set");
            services.AddOntogonyOpenTelemetryExport("ignored", "1.0.0");

            using var provider = services.BuildServiceProvider();
            Assert.NotNull(provider.GetService<TracerProvider>());
        }
        finally
        {
            Environment.SetEnvironmentVariable(
                OntogonyOpenTelemetryExtensions.OtlpEndpointEnvironmentVariable,
                previousEndpoint);
            Environment.SetEnvironmentVariable("OTEL_SERVICE_NAME", previousService);
        }
    }
}
