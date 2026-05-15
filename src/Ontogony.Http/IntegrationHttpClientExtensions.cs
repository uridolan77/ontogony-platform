using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Ontogony.Primitives;

namespace Ontogony.Http;

/// <summary>
/// Registers resilient <see cref="HttpClient"/> instances with Ontogony integration conventions.
/// </summary>
public static class IntegrationHttpClientExtensions
{
    /// <summary>
    /// Adds a named HTTP client with Ontogony correlation, actor propagation, resilience, and per-client options.
    /// </summary>
    public static IHttpClientBuilder AddOntogonyIntegrationHttpClient(
        this IServiceCollection services,
        string clientName,
        Func<IServiceProvider, HttpIntegrationOptions> resolveOptions)
    {
        AddOntogonyIntegrationCoreServices(services);

        return services.AddHttpClient(clientName)
            .ApplyOntogonyIntegrationHandlers(clientName, resolveOptions);
    }

    /// <summary>
    /// Adds a typed HTTP client with Ontogony integration conventions.
    /// </summary>
    public static IHttpClientBuilder AddOntogonyIntegrationHttpClient<TClient, TImplementation>(
        this IServiceCollection services,
        string clientName,
        Func<IServiceProvider, HttpIntegrationOptions> resolveOptions)
        where TClient : class
        where TImplementation : class, TClient
    {
        AddOntogonyIntegrationCoreServices(services);

        return services.AddHttpClient<TClient, TImplementation>(clientName)
            .ApplyOntogonyIntegrationHandlers(clientName, resolveOptions);
    }

    internal static void AddOntogonyIntegrationCoreServices(IServiceCollection services)
    {
        services.AddOptions<TransportResilienceOptions>();
        services.TryAddTransient<IRetryClassifier, DefaultRetryClassifier>();
        services.TryAddSingleton<IClock, SystemClock>();
        services.TryAddSingleton(sp => new TransportResilienceRegistry(sp.GetRequiredService<IClock>()));
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IOutboundActorPropagator, CorrelationOutboundActorPropagator>());
        services.TryAddTransient<IntegrationHeadersDelegatingHandler>();
    }

    private static IHttpClientBuilder ApplyOntogonyIntegrationHandlers(
        this IHttpClientBuilder builder,
        string clientName,
        Func<IServiceProvider, HttpIntegrationOptions> resolveOptions)
    {
        return builder
            .ConfigureHttpClient((sp, client) =>
            {
                var options = resolveOptions(sp);
                if (!string.IsNullOrWhiteSpace(options.BaseUrl))
                {
                    client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/", UriKind.Absolute);
                }

                client.Timeout = TimeSpan.FromSeconds(Math.Clamp(options.TimeoutSeconds, 1, 600));
                foreach (var header in options.DefaultHeaders)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            })
            .AddHttpMessageHandler<IntegrationHeadersDelegatingHandler>()
            .AddHttpMessageHandler(sp => new ResilientIntegrationDelegatingHandler(
                clientName,
                sp.GetRequiredService<TransportResilienceRegistry>(),
                sp.GetRequiredService<IOptions<TransportResilienceOptions>>(),
                sp.GetRequiredService<IClock>(),
                sp.GetService<IRetryClassifier>()));
    }
}
