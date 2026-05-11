using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ontogony.Observability;

namespace Ontogony.Http;

public static class IntegrationHttpClientExtensions
{
    public static IHttpClientBuilder AddOntogonyIntegrationHttpClient(
        this IServiceCollection services,
        string clientName,
        Func<IServiceProvider, HttpIntegrationOptions> resolveOptions)
    {
        services.AddOptions<TransportResilienceOptions>();
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
                sp.GetRequiredService<IOptions<TransportResilienceOptions>>()));
    }
}
