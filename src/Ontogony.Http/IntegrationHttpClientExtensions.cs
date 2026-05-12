using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Ontogony.Primitives;

namespace Ontogony.Http;

/// <summary>
/// Registers a named resilient <see cref="HttpClient"/> with correlation and retry handlers.
/// </summary>
public static class IntegrationHttpClientExtensions
{
    /// <summary>
    /// Adds a named HTTP client with Ontogony correlation, resilience, and per-client options from <paramref name="resolveOptions"/>.
    /// </summary>
    public static IHttpClientBuilder AddOntogonyIntegrationHttpClient(
        this IServiceCollection services,
        string clientName,
        Func<IServiceProvider, HttpIntegrationOptions> resolveOptions)
    {
        services.AddOptions<TransportResilienceOptions>();
        services.TryAddTransient<IRetryClassifier, DefaultRetryClassifier>();
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton(sp => new TransportResilienceRegistry(sp.GetRequiredService<IClock>()));
        services.AddTransient<CorrelationHeadersDelegatingHandler>();

        return services.AddHttpClient(clientName)
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
            .AddHttpMessageHandler<CorrelationHeadersDelegatingHandler>()
            .AddHttpMessageHandler(sp => new ResilientIntegrationDelegatingHandler(
                clientName,
                sp.GetRequiredService<TransportResilienceRegistry>(),
                sp.GetRequiredService<IOptions<TransportResilienceOptions>>(),
                sp.GetRequiredService<IClock>(),
                sp.GetService<IRetryClassifier>()));
    }
}
