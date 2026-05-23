using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ontogony.Errors;
using Ontogony.Hosting;
using Ontogony.Observability;
using Ontogony.Security;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class OntogonyHostingDefaultsTests
{
    [Fact]
    public void AddOntogonyServiceDefaults_Registers_Observability_And_Errors_By_Default()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddOntogonyServiceDefaults(configuration);

        using var provider = services.BuildServiceProvider();
        var observability = provider.GetService<IOptions<OntogonyObservabilityOptions>>();
        var errors = provider.GetService<IOptions<OntogonyExceptionMappingOptions>>();

        Assert.NotNull(observability);
        Assert.NotNull(errors);
    }

    [Fact]
    public async Task UseOntogonyServiceDefaults_Preserves_TraceId_In_Error_Payload_When_Tracing_Precedes_Errors()
    {
        await using var app = await BuildAndStartAppAsync(services =>
        {
            services.AddOntogonyServiceDefaults(new ConfigurationBuilder().Build(), options =>
            {
                options.ServiceName = "hosting-tests";
            });
        }, app =>
        {
            app.UseOntogonyServiceDefaults();
            app.MapGet("/boom", () =>
            {
                throw new InvalidOperationException("boom");
            });
        });

        var client = app.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Ontogony-Trace-Id", "trace-hosting-order");

        var response = await client.GetAsync("/boom");
        var payload = await response.Content.ReadFromJsonAsync<ApiError>();

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal("trace-hosting-order", payload!.TraceId);
    }

    [Fact]
    public async Task ServiceIdentityBodyHashPreload_Is_Added_Only_When_Opted_In()
    {
        var withoutPreload = await InvokePreloadProbeAsync(usePreload: false);
        var withPreload = await InvokePreloadProbeAsync(usePreload: true);

        Assert.False(withoutPreload);
        Assert.True(withPreload);
    }

    [Fact]
    public async Task MapOntogonyHealthEndpoints_Returns_HealthV1_And_ReadyV1_Payloads()
    {
        await using var app = await BuildAndStartAppAsync(services =>
        {
            services.AddOntogonyServiceDefaults(new ConfigurationBuilder().Build(), options =>
            {
                options.ServiceName = "conexus-api";
                options.ServiceVersion = "0.1.0-alpha.local";
                options.SystemBaseline = "SYSTEM-ALPHA-006";
            });
            services.AddHealthChecks()
                .AddCheck("sample_ready", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("ok"),
                    tags: ["ready", "check-id:sample.ready"]);
        }, app =>
        {
            app.UseOntogonyServiceDefaults();
            app.MapOntogonyHealthEndpoints();
        });

        var client = app.GetTestClient();

        var healthJson = await client.GetStringAsync("/health");
        using var healthDoc = System.Text.Json.JsonDocument.Parse(healthJson);
        Assert.Equal("health.v1", healthDoc.RootElement.GetProperty("schemaVersion").GetString());
        Assert.Equal("conexus", healthDoc.RootElement.GetProperty("service").GetString());
        Assert.Equal("0.1.0-alpha.local", healthDoc.RootElement.GetProperty("version").GetString());

        var ready = await client.GetAsync("/ready");
        var readyJson = await ready.Content.ReadAsStringAsync();
        using var readyDoc = System.Text.Json.JsonDocument.Parse(readyJson);
        Assert.Equal("ready.v1", readyDoc.RootElement.GetProperty("schemaVersion").GetString());
        Assert.Equal("ready", readyDoc.RootElement.GetProperty("status").GetString());
        Assert.True(readyDoc.RootElement.GetProperty("checks").GetArrayLength() >= 1);
    }

    [Fact]
    public async Task MapOntogonyHealthEndpoints_Maps_Health_And_Readiness_By_Default()
    {
        await using var app = await BuildAndStartAppAsync(services =>
        {
            services.AddOntogonyServiceDefaults(new ConfigurationBuilder().Build());
        }, app =>
        {
            app.UseOntogonyServiceDefaults();
            app.MapOntogonyHealthEndpoints();
        });

        var client = app.GetTestClient();

        var health = await client.GetAsync("/health");
        var ready = await client.GetAsync("/ready");

        Assert.Equal(HttpStatusCode.OK, health.StatusCode);
        Assert.Equal(HttpStatusCode.OK, ready.StatusCode);
    }

    [Fact]
    public async Task MapOntogonyHealthEndpoints_Uses_Custom_Health_And_Readiness_Paths()
    {
        await using var app = await BuildAndStartAppAsync(services =>
        {
            services.AddOntogonyServiceDefaults(new ConfigurationBuilder().Build(), options =>
            {
                options.HealthPath = "/healthz";
                options.ReadinessPath = "/readyz";
            });
        }, app =>
        {
            app.UseOntogonyServiceDefaults();
            app.MapOntogonyHealthEndpoints();
        });

        var client = app.GetTestClient();

        var customHealth = await client.GetAsync("/healthz");
        var customReady = await client.GetAsync("/readyz");
        var defaultHealth = await client.GetAsync("/health");
        var defaultReady = await client.GetAsync("/ready");

        Assert.Equal(HttpStatusCode.OK, customHealth.StatusCode);
        Assert.Equal(HttpStatusCode.OK, customReady.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, defaultHealth.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, defaultReady.StatusCode);
    }

    [Fact]
    public void AddOntogonyServiceDefaults_Can_Disable_Observability_And_Errors()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddOntogonyServiceDefaults(configuration, options =>
        {
            options.AddObservability = false;
            options.AddErrors = false;
        });

        Assert.DoesNotContain(
            services,
            descriptor => descriptor.ServiceType == typeof(IValidateOptions<OntogonyObservabilityOptions>));

        Assert.DoesNotContain(
            services,
            descriptor => descriptor.ServiceType == typeof(IConfigureOptions<OntogonyExceptionMappingOptions>));
    }

    [Fact]
    public void AddOntogonyServiceDefaults_Allows_Host_To_Override_Json_Options()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddOntogonyServiceDefaults(configuration);
        services.Configure<JsonOptions>(options => options.SerializerOptions.WriteIndented = true);

        using var provider = services.BuildServiceProvider();
        var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

        Assert.True(jsonOptions.SerializerOptions.WriteIndented);
    }

    private static async Task<bool> InvokePreloadProbeAsync(bool usePreload)
    {
        await using var app = await BuildAndStartAppAsync(services =>
        {
            services.AddOntogonyServiceIdentityActorContext(options =>
            {
                options.RequireHmacSignature = true;
                options.MaxSignedBodyBytes = 1024;
            });

            services.AddOntogonyServiceDefaults(new ConfigurationBuilder().Build(), options =>
            {
                options.ServiceName = "hosting-tests";
                options.UseServiceIdentityBodyHashPreload = usePreload;
            });
        }, app =>
        {
            app.UseOntogonyServiceDefaults();
            app.MapPost("/preload", (HttpContext context) =>
            {
                var hasPreload = context.Items.ContainsKey(ServiceIdentityBodyHashContext.HttpContextItemKey);
                return Results.Ok(new { hasPreload });
            });
        });

        var client = app.GetTestClient();
        using var request = new HttpRequestMessage(HttpMethod.Post, "/preload")
        {
            Content = new StringContent("hello", Encoding.UTF8, "text/plain")
        };
        request.Headers.Add(OntogonyServiceIdentityHeaders.ServiceId, "svc-A");

        var response = await client.SendAsync(request);
        var payload = await response.Content.ReadFromJsonAsync<PreloadProbeResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(payload);
        return payload!.HasPreload;
    }

    private static async Task<WebApplication> BuildAndStartAppAsync(
        Action<IServiceCollection> configureServices,
        Action<WebApplication> configureApp)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = Environments.Development
        });
        builder.WebHost.UseTestServer();

        configureServices(builder.Services);

        var app = builder.Build();
        configureApp(app);
        await app.StartAsync();
        return app;
    }

    private sealed class PreloadProbeResponse
    {
        public bool HasPreload { get; set; }
    }
}
